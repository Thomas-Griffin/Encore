using System.Collections.Generic;
using System.IO;
using Encore.Abstractions.Interfaces;
using UnityEngine;

namespace Encore.Systems.Save
{
    public sealed class JsonlFileSaveService : ISaveService
    {
        private const string SaveFolderName = "Saves";
        private const string SaveFileName = "encore_autosave";
        private const string SaveFileExtension = ".jsonl";

        private readonly string _saveDirectory =
            Path.Combine(Application.persistentDataPath, SaveFolderName);

        private readonly string _saveFilePath =
            Path.Combine(Application.persistentDataPath, SaveFolderName, SaveFileName + SaveFileExtension);

        public void Save(SavedGame savedGame, IEnumerable<EventSnapshot> events)
        {
            EnsureSaveFileExists();

            using StreamWriter writer = new(_saveFilePath, append: false);

            foreach (EventSnapshot e in events)
                writer.WriteLine(JsonUtility.ToJson(e));

            string savedGameJson = JsonUtility.ToJson(savedGame);
            writer.WriteLine(savedGameJson);
        }

        public SavedGame Load()
        {
            if (!File.Exists(_saveFilePath)) return null;

            string[] allLines = File.ReadAllLines(_saveFilePath);
            return allLines.Length == 0 ? null : JsonUtility.FromJson<SavedGame>(allLines[^1]);
        }

        public void DeleteSave()
        {
            if (File.Exists(_saveFilePath))
                File.Delete(_saveFilePath);
        }

        private void EnsureSaveFileExists()
        {
            if (!Directory.Exists(_saveDirectory))
                Directory.CreateDirectory(_saveDirectory);

            if (!File.Exists(_saveFilePath))
                File.Create(_saveFilePath).Close();
        }
    }
}