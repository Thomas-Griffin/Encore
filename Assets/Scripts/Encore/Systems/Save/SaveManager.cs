using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Encore.Systems.Save
{
    public class SaveManager
    {
        private string CurrentSaveFileName { get; set; }
        private const string SaveFileExtension = ".jsonl";
        private const string SaveFolderName = "Saves";
        private readonly string _saveDirectory = Path.Combine(Application.persistentDataPath, SaveFolderName);

        public void EnsureSaveDirectoryExists()
        {
            if (Directory.Exists(_saveDirectory)) return;
            Directory.CreateDirectory(_saveDirectory);
        }

        public void LoadMostRecentSave()
        {
            SaveData mostRecentSave = GetMostRecentSave();
            if (mostRecentSave != null)
            {
                if (GetAllSaves().Count == 1)
                {
                    CurrentSaveFileName = mostRecentSave.saveName + SaveFileExtension;
                }
                else if (GetAllSaves().Count > 1)
                {
                    CurrentSaveFileName = GetMostRecentSave().saveName;
                }
            }
            else
            {
                CurrentSaveFileName = NewSaveFile().saveName;
            }
        }

        public bool SaveFileExists(string saveFileName)
        {
            string path = Path.Combine(_saveDirectory, saveFileName + SaveFileExtension);
            return File.Exists(path);
        }

        public void DeleteSaveFile(string saveFileName)
        {
            string path = Path.Combine(_saveDirectory, saveFileName + SaveFileExtension);
            if (!File.Exists(path)) return;
            File.Delete(path);
        }

        public SaveData NewSaveFile()
        {
            string newSaveFileName = "save_" + DateTime.UtcNow.ToString("F");
            string fullPath = Path.Combine(_saveDirectory, newSaveFileName + SaveFileExtension);
            if (File.Exists(fullPath))
            {
                return LoadFromFile(newSaveFileName);
            }

            File.Create(fullPath).Dispose();

            return LoadFromFile(newSaveFileName);
        }

        public List<SaveData> GetAllSaves()
        {
            string[] files = Directory.GetFiles(_saveDirectory, "*" + SaveFileExtension);
            return (from file in files
                let baseName = Path.GetFileNameWithoutExtension(file)
                select new SaveData(baseName, GetSaveSlot(file), null, File.GetCreationTime(file))).ToList();
        }

        private int GetSaveSlot(string file)
        {
            using StreamReader reader = new(file);
            string firstLine = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(firstLine)) return -1;
            SaveData saveData = JsonUtility.FromJson<SaveData>(firstLine);
            return saveData?.saveSlot ?? -1;
        }

        private SaveData GetMostRecentSave()
        {
            List<SaveData> saves = GetAllSaves();
            // Sort by timestamp (parse ISO string) falling back to file creation time if parse fails
            saves.Sort((a, b) =>
            {
                if (DateTime.TryParse(a.saveTimeStamp, out DateTime aDateTime) &&
                    DateTime.TryParse(b.saveTimeStamp, out DateTime bDateTime))
                {
                    return bDateTime.CompareTo(aDateTime);
                }

                return 0;
            });

            return saves.Count > 0 ? saves[0] : null;
        }

        public void SaveToFile(SaveData saveData, bool overwrite = false)
        {
            string fullPath = Path.Combine(_saveDirectory, saveData.saveName + SaveFileExtension);
            if (overwrite && File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            using StreamWriter writer = new(fullPath, true);
            writer.WriteLine(JsonUtility.ToJson(saveData));
        }

        public SaveData LoadFromFile(string saveFileName)
        {
            string fullPath = Path.Combine(_saveDirectory, saveFileName + SaveFileExtension);
            if (!File.Exists(fullPath)) return null;
            using StreamReader reader = new(fullPath);
            string firstLine = reader.ReadLine();
            return string.IsNullOrWhiteSpace(firstLine) ? null : JsonUtility.FromJson<SaveData>(firstLine);
        }

        public void DeleteAllSaves()
        {
            List<SaveData> saves = GetAllSaves();
            foreach (SaveData save in saves)
            {
                DeleteSaveFile(save.saveName);
            }
        }
    }
}