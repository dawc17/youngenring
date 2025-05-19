using UnityEngine;
using System;
using System.IO;

namespace DKC
{
    public class WriteSaveData
    {
        public string saveDataPath = "";
        public string saveFileName = "";

        
        // before creating save, check if one exists in this slot (max 5 slots)
        public bool CheckIfFileExists()
        {
            if (File.Exists(Path.Combine(saveDataPath, saveFileName)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // used to delete character save files
        public void DeleteSaveFile()
        {
            File.Delete(Path.Combine(saveDataPath,saveFileName));
        }

        
        // creates a save file when starting new game
        public void CreateNewCharacterSaveFile(CharacterSaveData characterData)
        {
            // make a path to sace file (location on pc)
            string savePath = Path.Combine(saveDataPath, saveFileName);

            try
            {
                // create the directory the save will be written to, if it does not already exist
                Directory.CreateDirectory(Path.GetDirectoryName(savePath) ?? string.Empty);
                Debug.Log("Creating save file, at save path: " + savePath);

                // serialize the c# game data into json
                string dataToStore = JsonUtility.ToJson(characterData, true);

                // write the file to our system
                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error while trying to save character data, game not saved." + savePath + "\n" + ex);
            }
        }

        // used to load save file upon loading previous game
        public CharacterSaveData LoadSaveFile()
        {
            CharacterSaveData characterSaveData = null;
            string loadPath = Path.Combine(saveDataPath, saveFileName);

            if (File.Exists(loadPath))
            {
                try
                {
                    string dataToLoad = "";

                    using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    // deserialize the data from json back to unity
                    characterSaveData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
                }
                catch (Exception ex)
                {
                    
                }
            }

            return characterSaveData;
        }
    }
}