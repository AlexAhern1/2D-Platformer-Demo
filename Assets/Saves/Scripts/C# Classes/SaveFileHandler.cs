using System.IO;
using System.Linq;
using UnityEngine;

namespace Game.SaveData
{
    /// <summary>
    /// <para>this class reads files and passes the resulting json string to classes that need it</para>
    /// <para>this class also saves a string of text into a json file</para>
    /// </summary>
    public static class SaveFileHandler<T> where T : new()
    {
        private const string SAVES_FOLDER = "Saves";
        private const string PROFILE_NAME = "Profile";
        private const string JSON_EXTENSION = "json";

        //path on my device -> C:\Users\Adam_\AppData\LocalLow\DefaultCompany\OBA_prototype\
        private static string _saveDataFolderPath = $"{Application.persistentDataPath}/{SAVES_FOLDER}";

        public static bool PRETTY = true;

        public static T CreateNewProfile(int profileNumber)
        {
            //create a saves folder if it doesn't exist
            CreateSavesFolderIfNotExists();

            //checking if the save profile exists
            if (!SaveProfileExists(profileNumber))
            {
                //create a new instance of the main save data class with default data.
                T newData = new();

                //writing default data to json file (file will be automatically created)
                WriteDataToJsonFile(profileNumber, newData);
                Debug.Log($"<color=#c5ff5c>Created save profile {profileNumber}</color>");
                return newData;
            }

            Debug.Log($"<color=#ffffc5>Save profile {profileNumber} already exists.</color>");
            T existingProfile = LoadExistingProfile(profileNumber);
            return existingProfile;
        }

        public static void DeleteProfile(int profileNumber)
        {
            CreateSavesFolderIfNotExists();
            if (!SaveProfileExists(profileNumber))
            {
                Debug.LogWarning($"Failed to delete profile {profileNumber} as it does not exist!");
                return;
            }
            DeleteJsonFile(profileNumber);
            Debug.Log($"<color=#ff5c5c>Deleted save profile {profileNumber}</color>");
        }

        public static void OverwriteExistingProfile(int profileNumber, T saveData)
        {
            //perform a check to see if the data formatting is the same.
            if (!SaveProfileExists(profileNumber))
            {
                Debug.LogWarning($"Failed to overwrite profile {profileNumber} as it does not exist!");
                return;
            }
            WriteDataToJsonFile(profileNumber, saveData);
            Debug.Log($"<color=#5cffa3>Overwriting save profile {profileNumber}</color>");
        }

        public static void UpdateAllProfiles()
        {
            CreateSavesFolderIfNotExists();

            string[] savedProfiles = Directory.GetFiles(_saveDataFolderPath, "*.json");
            for (int i = 0; i < savedProfiles.Length; i++)
            {
                string savedProfile = savedProfiles[i];
                string jsonData = ReadJsonFile(savedProfile);

                string updatedData = SaveDataHelper.UpdateSaveDataFormatting<T>(jsonData);
                    
                T data = JsonUtility.FromJson<T>(updatedData);
                string newData = JsonUtility.ToJson(data, PRETTY);
                WriteToJsonFile(savedProfile, newData);
            }
        }

        public static T LoadExistingProfile(int profileNumber)
        {
            //check if the saved profile exists
            if (!SaveProfileExists(profileNumber))
            {
                Debug.LogWarning($"Profile {profileNumber} does not exist!");
                return default;
            }

            //get the string representation of the json file data
            string jsonString = ReadJsonFile(profileNumber);

            //get the string version of the file
            Debug.Log($"<color=#5cd9ff>Loading save profile {profileNumber}</color>");
            return JsonUtility.FromJson<T>(jsonString);
        }

        public static string GetJsonData(int profileNumber)
        {
            if (SaveProfileExists(profileNumber))
            {
                return ReadJsonFile(profileNumber);
            }
            return default;
        }

        public static string GetDefaultJsonData()
        {
            T newData = new();
            return JsonUtility.ToJson(newData);
        }

        private static void CreateSavesFolderIfNotExists()
        {
            if (SavesFolderExists()) return;
            Directory.CreateDirectory(_saveDataFolderPath);
        }

        private static void WriteDataToJsonFile(int profileNumber, T data)
        {
            string jsonString = JsonUtility.ToJson(data, PRETTY);
            WriteToJsonFile(profileNumber, jsonString);
        }

        private static void WriteToJsonFile(int profileNumber, string jsonString)
        {
            string filePath = GetProfilePath(profileNumber);
            WriteToJsonFile(filePath, jsonString);
        }

        private static void WriteToJsonFile(string filePath, string jsonString)
        {
            File.WriteAllText(filePath, jsonString);
        }

        private static void DeleteJsonFile(int profileNumber)
        {
            string filePath = GetProfilePath(profileNumber);
            File.Delete(filePath);
        }

        private static bool SavesFolderExists()
        {
            return Directory.Exists(_saveDataFolderPath);
        }

        private static bool SaveProfileExists(int profileNumber)
        {
            string filePath = GetProfilePath(profileNumber);
            return File.Exists(filePath);
        }

        private static string ReadJsonFile(int profileNumber)
        {
            string filePath = GetProfilePath(profileNumber);
            return ReadJsonFile(filePath);
        }

        private static string ReadJsonFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++) { lines[i] = lines[i].TrimStart(' ').Replace(": ", ":"); }
            string fullText = string.Join("", lines);

            return fullText;
        }

        private static string GetProfilePath(int profileNumber)
        {
            return $"{_saveDataFolderPath}/{PROFILE_NAME}{profileNumber}.{JSON_EXTENSION}";
        }
    }
}