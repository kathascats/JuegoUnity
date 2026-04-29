using System;
using UnityEngine;

public class AutoSaveProgress : MonoBehaviour
{
    [Serializable]
    public class GameProgressData
    {
        public int playerLevel = 1;
        public int coins = 0;
        public float playerX;
        public float playerY;
        public float playerZ;
        public string lastSavedAtUtc;
    }

    [Header("Auto Save")]
    [SerializeField] private bool enableAutoSave = true;
    [SerializeField] private float autoSaveIntervalSeconds = 20f;
    [SerializeField] private string progressKey = "game.progress";
    [SerializeField] private Transform playerTransform;

    private float autoSaveTimer;
    private GameProgressData currentData;

    public GameProgressData CurrentData => currentData;

    private void Awake()
    {
        currentData = SaveSystem.Load(progressKey, BuildDefaultData());
        ApplyLoadedData();
    }

    private void OnEnable()
    {
        PlayerProfileManager.OnProfileChanged += HandleProfileChanged;
    }

    private void OnDisable()
    {
        PlayerProfileManager.OnProfileChanged -= HandleProfileChanged;
    }

    private void Update()
    {
        if (!enableAutoSave)
        {
            return;
        }

        autoSaveTimer += Time.deltaTime;
        if (autoSaveTimer >= autoSaveIntervalSeconds)
        {
            autoSaveTimer = 0f;
            SaveNow();
        }
    }

    private void OnApplicationQuit()
    {
        SaveNow();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveNow();
        }
    }

    public void AddCoins(int amount)
    {
        currentData.coins += amount;
        SaveNow();
    }

    public void SetLevel(int level)
    {
        currentData.playerLevel = Mathf.Max(1, level);
        SaveNow();
    }

    public void SaveNow()
    {
        CaptureRuntimeState();
        SaveSystem.Save(progressKey, currentData);
    }

    public void ResetCurrentProfileProgress()
    {
        currentData = BuildDefaultData();
        SaveNow();
        ApplyLoadedData();
    }

    private void HandleProfileChanged(string _)
    {
        currentData = SaveSystem.Load(progressKey, BuildDefaultData());
        ApplyLoadedData();
    }

    private GameProgressData BuildDefaultData()
    {
        return new GameProgressData
        {
            playerLevel = 1,
            coins = 0,
            playerX = 0f,
            playerY = 0f,
            playerZ = 0f,
            lastSavedAtUtc = DateTime.UtcNow.ToString("o")
        };
    }

    private void CaptureRuntimeState()
    {
        if (playerTransform != null)
        {
            Vector3 pos = playerTransform.position;
            currentData.playerX = pos.x;
            currentData.playerY = pos.y;
            currentData.playerZ = pos.z;
        }

        currentData.lastSavedAtUtc = DateTime.UtcNow.ToString("o");
    }

    private void ApplyLoadedData()
    {
        if (playerTransform != null)
        {
            playerTransform.position = new Vector3(
                currentData.playerX,
                currentData.playerY,
                currentData.playerZ
            );
        }
    }
}
