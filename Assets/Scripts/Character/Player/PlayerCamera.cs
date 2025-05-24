using System.Collections.Generic;
using UnityEngine;

namespace DKC
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera instance;
        public Camera cameraObject;
        [SerializeField] Transform cameraPivotTransform;

        [HideInInspector] public PlayerManager player;

        [Header("Camera Settings")]
        private float cameraSmoothSpeed = 1;

        [SerializeField] float upAndDownRotationSpeed = 220;
        [SerializeField] float leftAndRightRotationSpeed = 220;
        [SerializeField] float minimumPivot = -30; // lowest look angle
        [SerializeField] float maximumPivot = 60; // highest look angle
        [SerializeField] float cameraCollisionRadius = 0.2f;
        [SerializeField] LayerMask collideWithLayers;

        [Header("Camera Values")]
        private Vector3 cameraVelocity;
        private Vector3 cameraObjectPosition; // camera collisions moves the camera object to this position

        [SerializeField] private float leftAndRightLookAngle;
        [SerializeField] private float upAndDownLookAngle;
        private float cameraZPosition;
        private float targetCameraZPosition;

        [Header("Lock On")]
        [SerializeField] private float lockOnSphereRadius = 20f; // radius to search for targets
        [SerializeField] private float minimumViewableAngle = -50;
        [SerializeField] private float maximumViewableAngle = 50;
        private List<CharacterManager> availableTargets = new List<CharacterManager>();
        public CharacterManager nearestTarget; // the closest target to the player
        [SerializeField] private float lockOnTargetFollowSpeed = 0.2f; // speed at which camera follows the target when locked on

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            cameraZPosition = cameraObject.transform.localPosition.z;
        }

        public void HandleAllCameraActions()
        {
            if (player != null)
            {
                HandleFollowTarget();
                HandleRotations();
                HandleCollisions();
            }
        }

        private void HandleFollowTarget()
        {
            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position,
                ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPosition;
        }

        private void HandleRotations()
        {
            // if locked on, rotate towards target
            if (player.playerNetworkManager.isLockedOn.Value)
            {
                // main player camera object
                Vector3 rotationDirection = player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - transform.position;
                rotationDirection.Normalize();
                rotationDirection.y = 0; // keep the rotation on the y-axis only

                Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lockOnTargetFollowSpeed);

                // this rotates the pivot object
                rotationDirection = player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - cameraPivotTransform.position;
                rotationDirection.Normalize();

                targetRotation = Quaternion.LookRotation(rotationDirection);
                cameraPivotTransform.transform.rotation = Quaternion.Slerp(cameraPivotTransform.rotation, targetRotation, lockOnTargetFollowSpeed);

                // save our rotations to our look angles, so when we unlock it doesnt snap too far
                leftAndRightLookAngle = transform.eulerAngles.y;
                upAndDownLookAngle = transform.eulerAngles.x;
            }
            // if not locked on, rotate based on camera input
            else
            {
                // rotate left and right based on camera input
                leftAndRightLookAngle += PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed * Time.deltaTime;
                // rotate up and down and clamp
                upAndDownLookAngle -= PlayerInputManager.instance.cameraVerticalInput * upAndDownRotationSpeed * Time.deltaTime;
                upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

                Vector3 cameraRotation = Vector3.zero;
                Quaternion targetRotation;

                // rotate left and right
                cameraRotation.y = leftAndRightLookAngle;
                targetRotation = Quaternion.Euler(cameraRotation);
                transform.rotation = targetRotation;

                // rotate pivot up and down
                cameraRotation = Vector3.zero;
                cameraRotation.x = upAndDownLookAngle;
                targetRotation = Quaternion.Euler(cameraRotation);
                cameraPivotTransform.localRotation = targetRotation;
            }
        }

        private void HandleCollisions()
        {
            targetCameraZPosition = cameraZPosition;
            RaycastHit hit;
            // this is the direction
            Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
            direction.Normalize();


            // check if object is in front of camera using a raycast from desired direction
            if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit,
                    Mathf.Abs(cameraZPosition), collideWithLayers))
            {
                // if there is, get distance fropm it
                float distanceFromObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
                // then equate our target z position to the following
                targetCameraZPosition = -(distanceFromObject - cameraCollisionRadius);
            }

            // if our target position is less than our collision radius, we - our collision radius
            if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
            {
                targetCameraZPosition = -cameraCollisionRadius;
            }

            // apply our finals position using a lerp over time
            cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
            cameraObject.transform.localPosition = cameraObjectPosition;
        }

        public void HandleLocatingLockOnTargets()
        {
            float shortDistance = Mathf.Infinity; // used to find the closest target
            float shortDistanceOfRightTarget = Mathf.Infinity; // will be used to find the closest target on the right side of current target
            float shortDistanceOfLeftTarget = -Mathf.Infinity; // will be used to find the closest target on the left side of current target

            Collider[] colliders = Physics.OverlapSphere(player.transform.position, lockOnSphereRadius, WorldUtilityManager.instance.GetCharacterLayers());

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager lockOnTarget = colliders[i].GetComponent<CharacterManager>();

                if (lockOnTarget != null)
                {
                    // check if they are within our fov
                    Vector3 lockOnTargetDirection = lockOnTarget.transform.position - player.transform.position;
                    float distanceFromTarget = Vector3.Distance(player.transform.position, lockOnTarget.transform.position);
                    float viewableAngle = Vector3.Angle(lockOnTargetDirection, cameraObject.transform.forward);

                    if (lockOnTarget.isDead.Value)
                        continue; // skip dead targets

                    if (lockOnTarget.transform.root == player.transform.root)
                        continue; // skip self

                    // lastly if the target is outside of fov or blocked by an obstacle, skip it
                    if (viewableAngle > minimumViewableAngle && viewableAngle < maximumViewableAngle)
                    {
                        RaycastHit hit;

                        // todo add layer mask for env layers only
                        if (Physics.Linecast(
                            player.playerCombatManager.lockOnTransform.position,
                            lockOnTarget.characterCombatManager.lockOnTransform.position,
                            out hit,
                            WorldUtilityManager.instance.GetEnvironmentLayers()))
                        {
                            // cannot lock on to target if there is an obstacle in the way
                            continue;
                        }
                        else
                        {
                            availableTargets.Add(lockOnTarget);
                        }
                    }
                }

                for (int k = 0; k < availableTargets.Count; k++)
                {
                    if (availableTargets[k] != null)
                    {
                        float distanceFromTarget = Vector3.Distance(player.transform.position, availableTargets[k].transform.position);

                        if (distanceFromTarget < shortDistance)
                        {
                            shortDistance = distanceFromTarget;
                            nearestTarget = availableTargets[k];
                        }
                    }
                    else
                    {
                        ClearLockOnTargets();
                        player.playerNetworkManager.isLockedOn.Value = false;
                    }
                }
            }
        }

        public void ClearLockOnTargets()
        {
            nearestTarget = null;
            availableTargets.Clear();
        }
    }
}