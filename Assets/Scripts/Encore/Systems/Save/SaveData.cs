using System;
using Encore.Model.Game;

namespace Encore.Systems.Save
{
    [Serializable]
    public class SaveData
    {
        public string saveName;
        public int saveSlot;
        public string saveTimeStamp;
        public GameInstanceSnapshot gameInstanceSnapshot;

        public SaveData()
        {
            saveName = string.Empty;
            saveSlot = -1;
            saveTimeStamp = DateTime.UtcNow.ToString("o");
            gameInstanceSnapshot = null;
        }

        public SaveData(
            string saveName,
            int saveSlot,
            GameInstance gameInstance,
            DateTime saveTimeStamp
        )
        {
            this.saveName = saveName;
            this.saveSlot = saveSlot;
            this.saveTimeStamp = saveTimeStamp.ToString("o");
            gameInstanceSnapshot = gameInstance == null ? null : GameInstanceSnapshot.FromGameInstance(gameInstance);
        }

        public GameInstance ToGameInstance()
        {
            return gameInstanceSnapshot?.ToGameInstance();
        }

        public static SaveData FromGameInstance(GameInstance instance, string saveName = null, int slot = 0)
        {
            SaveData data = new()
            {
                saveName = saveName ?? instance?.SaveFileName ?? "encore_autosave",
                saveSlot = slot,
                saveTimeStamp = DateTime.UtcNow.ToString("o"),
                gameInstanceSnapshot = instance == null ? null : GameInstanceSnapshot.FromGameInstance(instance)
            };
            return data;
        }

        public override string ToString()
        {
            return $"{saveName} (Slot {saveSlot}) - Saved on {saveTimeStamp}";
        }

        public string ToDetailedString()
        {
            GameInstance inst = ToGameInstance();
            return $"{ToString()}\nGame State: {inst?.State}, Difficulty: {inst?.Difficulty}";
        }

        public string ToShortString()
        {
            return $"{saveName} (Slot {saveSlot})";
        }

        public void SaveToFile(bool overwrite = false)
        {
        }

        public void Load()
        {
        }

        public void Delete()
        {
        }
    }
}