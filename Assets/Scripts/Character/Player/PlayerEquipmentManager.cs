using UnityEngine;

namespace DKC
{
    public class PlayerEquipmentManager : CharacterEquipmentManager
    {
        PlayerManager player;

        public WeaponModelInstatiationSlot rightHandSlot;
        public WeaponModelInstatiationSlot leftHandSlot;

        [SerializeField] WeaponManager rightWeaponManager;
        [SerializeField] WeaponManager leftWeaponManager;

        public GameObject rightHandWeaponModel;
        public GameObject leftHandWeaponModel;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();

            InitializeWeaponSlots();
        }

        protected override void Start()
        {
            base.Start();

            LoadWeaponOnBothHands();
        }

        private void InitializeWeaponSlots()
        {
            WeaponModelInstatiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstatiationSlot>();

            foreach (var weaponSlot in weaponSlots)
            {
                if (weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
                {
                    rightHandSlot = weaponSlot;
                }
                else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHand)
                {
                    leftHandSlot = weaponSlot;
                }
            }
        }

        public void LoadWeaponOnBothHands()
        {
            LoadLeftWeapon();
            LoadRightWeapon();
        }

        // right weapon

        public void SwitchRightWeapon()
        {
            if (!player.IsOwner)
                return;

            player.playerAnimationManager.PlayTargetActionAnimation("Swap_Right_Weapon_01", false, false, true, true);

            WeaponItem selectedWeapon = null;

            // disable two handing if we are twohanding
            // check weapon index (we have 3 slots)

            // add one to index to switch
            player.playerInventoryManager.rightHandWeaponIndex += 1;

            // reset index if out of bounds
            if (player.playerInventoryManager.rightHandWeaponIndex < 0 || player.playerInventoryManager.rightHandWeaponIndex > 2)
            {
                player.playerInventoryManager.rightHandWeaponIndex = 0;

                // we check if we are holding more than one weapon
                float weaponCount = 0;
                WeaponItem firstWeapon = null;
                int firstWeaponPosition = 0;

                for (int i = 0; i < player.playerInventoryManager.weaponsInRightHandSlots.Length; i++)
                {
                    if (player.playerInventoryManager.weaponsInRightHandSlots[i].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        weaponCount += 1;

                        if (firstWeapon == null)
                        {
                            firstWeapon = player.playerInventoryManager.weaponsInRightHandSlots[i];
                            firstWeaponPosition = i;
                        }
                    }
                }

                if (weaponCount <= 1)
                {
                    player.playerInventoryManager.rightHandWeaponIndex = -1;
                    selectedWeapon = WorldItemDatabase.Instance.unarmedWeapon;
                    player.playerNetworkManager.currentRightHandWeaponID.Value = selectedWeapon.itemID;
                }
                else
                {
                    player.playerInventoryManager.rightHandWeaponIndex = firstWeaponPosition;
                    player.playerNetworkManager.currentRightHandWeaponID.Value = firstWeapon.itemID;
                }

                return;
            }

            foreach (WeaponItem weapon in player.playerInventoryManager.weaponsInRightHandSlots)
            {
                // if weapon does not equal unarmed, proceed
                if (player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                {
                    selectedWeapon = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex];
                    // assign network weapon id so it syncs for everyone
                    player.playerNetworkManager.currentRightHandWeaponID.Value = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID;
                    return;
                }
            }

            if (selectedWeapon == null && player.playerInventoryManager.rightHandWeaponIndex <= 2)
            {
                SwitchRightWeapon();
            }
        }

        public void LoadRightWeapon()
        {
            if (player.playerInventoryManager.currentRightHandWeapon != null)
            {
                // remove the old weapon
                rightHandSlot.UnloadWeapon();

                // bring in new weapon
                rightHandWeaponModel = Instantiate(player.playerInventoryManager.currentRightHandWeapon.weaponModel);
                rightHandSlot.LoadWeapon(rightHandWeaponModel);

                // Add null check here to prevent NullReferenceException
                rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
                if (rightWeaponManager != null)
                {
                    rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
                }
                else
                {
                    Debug.LogError("WeaponManager component not found on the weapon model: " +
                    player.playerInventoryManager.currentRightHandWeapon.name);
                }
            }
        }

        // left weapon

        public void SwitchLeftWeapon()
        {
            if (!player.IsOwner)
                return;

            player.playerAnimationManager.PlayTargetActionAnimation("Swap_Left_Weapon_01", false, false, true, true);

            WeaponItem selectedWeapon = null;

            // disable two handing if we are twohanding
            // check weapon index (we have 3 slots)

            // add one to index to switch
            player.playerInventoryManager.leftHandWeaponIndex += 1;

            // reset index if out of bounds
            if (player.playerInventoryManager.leftHandWeaponIndex < 0 || player.playerInventoryManager.leftHandWeaponIndex > 2)
            {
                player.playerInventoryManager.leftHandWeaponIndex = 0;

                // we check if we are holding more than one weapon
                float weaponCount = 0;
                WeaponItem firstWeapon = null;
                int firstWeaponPosition = 0;

                for (int i = 0; i < player.playerInventoryManager.weaponsInLeftHandSlots.Length; i++)
                {
                    if (player.playerInventoryManager.weaponsInLeftHandSlots[i].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        weaponCount += 1;

                        if (firstWeapon == null)
                        {
                            firstWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[i];
                            firstWeaponPosition = i;
                        }
                    }
                }

                if (weaponCount <= 1)
                {
                    player.playerInventoryManager.leftHandWeaponIndex = -1;
                    selectedWeapon = WorldItemDatabase.Instance.unarmedWeapon;
                    player.playerNetworkManager.currentLeftHandWeaponID.Value = selectedWeapon.itemID;
                }
                else
                {
                    player.playerInventoryManager.leftHandWeaponIndex = firstWeaponPosition;
                    player.playerNetworkManager.currentLeftHandWeaponID.Value = firstWeapon.itemID;
                }

                return;
            }

            foreach (WeaponItem weapon in player.playerInventoryManager.weaponsInLeftHandSlots)
            {
                // if weapon does not equal unarmed, proceed
                if (player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                {
                    selectedWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex];
                    // assign network weapon id so it syncs for everyone
                    player.playerNetworkManager.currentLeftHandWeaponID.Value = player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex].itemID;
                    return;
                }
            }

            if (selectedWeapon == null && player.playerInventoryManager.leftHandWeaponIndex <= 2)
            {
                SwitchLeftWeapon();
            }
        }

        public void LoadLeftWeapon()
        {
            if (player.playerInventoryManager.currentLeftHandWeapon != null)
            {
                leftHandSlot.UnloadWeapon();

                leftHandWeaponModel = Instantiate(player.playerInventoryManager.currentLeftHandWeapon.weaponModel);
                leftHandSlot.LoadWeapon(leftHandWeaponModel);
                leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
                if (leftWeaponManager != null)
                {
                    leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
                }
                else
                {
                    Debug.LogError("WeaponManager component not found on the weapon model: " +
                    player.playerInventoryManager.currentLeftHandWeapon.name);
                }
            }
        }

        // damage colliders

        public void OpenDamageCollider()
        {
            // open right weapon collider
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                rightWeaponManager.meleeDamageCollider.EnableDamageCollider();
                player.characterSFXManager.PlayAttackGrunt();

            }
            // open left weapon collider
            else if (player.playerNetworkManager.isUsingLeftHand.Value)
            {
                leftWeaponManager.meleeDamageCollider.EnableDamageCollider();
                player.characterSFXManager.PlayAttackGrunt();
            }
        }

        public void CloseDamageCollider()
        {
            // close right weapon collider
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                rightWeaponManager.meleeDamageCollider.DisableDamageCollider();
            }
            // close left weapon collider
            else if (player.playerNetworkManager.isUsingLeftHand.Value)
            {
                leftWeaponManager.meleeDamageCollider.DisableDamageCollider();
            }

            // play sfx
        }
    }
}