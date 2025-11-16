using Encore.Model.Game;
using Encore.Systems.Core;
using Encore.Systems.GameEvent.Events;
using Encore.Systems.Save;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode
{
    public class GameSaveLoadTests
    {
        [Test]
        public void SaveAndLoad_Roundtrip_PreservesStatsAndEvents()
        {
            GameObject gameObject = new("GameManager");
            GameManager gameManager = gameObject.AddComponent<GameManager>();
            
            gameManager.StartGame(DifficultyLevel.Easy);
            
            gameManager.Instance.SaveFileName = "test_save_roundtrip";
            
            RestEvent rest = new();
            gameManager.Instance.Events.Append(rest);
            PractiseEvent practice = new();
            gameManager.Instance.Events.Append(practice);

            SaveData saveData = SaveData.FromGameInstance(gameManager.Instance, gameManager.Instance.SaveFileName);
            gameManager.SaveManager.SaveToFile(saveData, overwrite: true);

            SaveData loaded = gameManager.SaveManager.LoadFromFile(gameManager.Instance.SaveFileName);
            Assert.NotNull(loaded);

            GameInstance runtime = loaded.ToGameInstance();
            Assert.NotNull(runtime);
            Assert.AreEqual(runtime.Stats.Energy.CurrentValue, gameManager.Instance.Stats.Energy.CurrentValue);
            Assert.AreEqual(runtime.Events.GetAllEvents().Count, gameManager.Instance.Events.GetAllEvents().Count);

            gameManager.SaveManager.DeleteSaveFile(gameManager.Instance.SaveFileName);
            Object.DestroyImmediate(gameObject);
        }
    }
}
