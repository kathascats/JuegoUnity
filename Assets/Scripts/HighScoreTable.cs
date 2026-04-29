using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreTable : MonoBehaviour
{
    [Serializable]
    private class EntryUiPaths
    {
        [Header("Text fields in template row")]
        public string posTextPath = "posText";
        public string scoreTextPath = "scoreText";
        public string nameTextPath = "nameText";

        [Header("Optional visual fields in template row")]
        public string backgroundPath = "background";
        public string firstPlaceIconPath = "firstPlaceIcon";
        public string secondPlaceIconPath = "secondPlaceIcon";
        public string thirdPlaceIconPath = "thirdPlaceIcon";
    }

    [Serializable]
    private class HighScoreEntry
    {
        public int score;
        public string name;
    }

    [Serializable]
    private class HighScores
    {
        public List<HighScoreEntry> highScoreEntryList;
    }

    [Header("UI References")]
    [SerializeField] private Transform entryContainer;
    [SerializeField] private Transform entryTemplate;
    [SerializeField] private int maxEntriesToDisplay = 10;
    [SerializeField] private float entryHeight = 30f;
    [SerializeField] private EntryUiPaths uiPaths = new EntryUiPaths();

    private const string PlayerPrefsKey = "highScoreTable";
    private List<Transform> highScoreEntryTransformList;
    private HighScores highScores;

    private void Awake()
    {
        InitializeDataIfNeeded();
        entryTemplate.gameObject.SetActive(false);
        RefreshHighScoreTable();
    }

    /// <summary>
    /// Adds a new score entry, sorts descending, trims to top N, and persists.
    /// </summary>
    public void AddHighScoreEntry(int score, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            name = "ANON";
        }

        HighScoreEntry highScoreEntry = new HighScoreEntry
        {
            score = score,
            name = name.Trim()
        };

        highScores.highScoreEntryList.Add(highScoreEntry);
        SortAndTrimEntries();
        SaveHighScoreTable();
        RefreshHighScoreTable();
    }

    public void SaveHighScoreTable()
    {
        string json = JsonUtility.ToJson(highScores);
        PlayerPrefs.SetString(PlayerPrefsKey, json);
        PlayerPrefs.Save();
    }

    public void LoadHighScoreTable()
    {
        string jsonString = PlayerPrefs.GetString(PlayerPrefsKey, string.Empty);

        if (string.IsNullOrEmpty(jsonString))
        {
            highScores = new HighScores
            {
                highScoreEntryList = new List<HighScoreEntry>()
            };
            return;
        }

        highScores = JsonUtility.FromJson<HighScores>(jsonString);

        if (highScores == null || highScores.highScoreEntryList == null)
        {
            highScores = new HighScores
            {
                highScoreEntryList = new List<HighScoreEntry>()
            };
        }
    }

    private void InitializeDataIfNeeded()
    {
        LoadHighScoreTable();
        SortAndTrimEntries();
        SaveHighScoreTable();
    }

    private void SortAndTrimEntries()
    {
        highScores.highScoreEntryList.Sort((a, b) => b.score.CompareTo(a.score));

        if (highScores.highScoreEntryList.Count > maxEntriesToDisplay)
        {
            highScores.highScoreEntryList.RemoveRange(
                maxEntriesToDisplay,
                highScores.highScoreEntryList.Count - maxEntriesToDisplay
            );
        }
    }

    private void RefreshHighScoreTable()
    {
        if (highScoreEntryTransformList == null)
        {
            highScoreEntryTransformList = new List<Transform>();
        }
        else
        {
            foreach (Transform entry in highScoreEntryTransformList)
            {
                Destroy(entry.gameObject);
            }
            highScoreEntryTransformList.Clear();
        }

        for (int i = 0; i < highScores.highScoreEntryList.Count; i++)
        {
            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0f, -entryHeight * i);
            entryTransform.gameObject.SetActive(true);

            int rank = i + 1;
            SetupEntryVisual(entryTransform, highScores.highScoreEntryList[i], rank, i);
            highScoreEntryTransformList.Add(entryTransform);
        }
    }

    private void SetupEntryVisual(Transform entryTransform, HighScoreEntry entry, int rank, int index)
    {
        TextMeshProUGUI posText = FindRequiredText(entryTransform, uiPaths.posTextPath, "position/rank");
        TextMeshProUGUI scoreText = FindRequiredText(entryTransform, uiPaths.scoreTextPath, "score");
        TextMeshProUGUI nameText = FindRequiredText(entryTransform, uiPaths.nameTextPath, "name");

        posText.text = GetRankString(rank);
        scoreText.text = entry.score.ToString();
        nameText.text = entry.name;

        GameObject background = FindOptionalObject(entryTransform, uiPaths.backgroundPath);
        if (background != null)
        {
            Image bgImage = background.GetComponent<Image>();
            if (bgImage != null)
            {
                bool isOddRow = index % 2 == 1;
                bgImage.color = isOddRow
                    ? new Color32(0, 0, 0, 90)
                    : new Color32(0, 0, 0, 45);
            }
        }

        GameObject firstIcon = FindOptionalObject(entryTransform, uiPaths.firstPlaceIconPath);
        GameObject secondIcon = FindOptionalObject(entryTransform, uiPaths.secondPlaceIconPath);
        GameObject thirdIcon = FindOptionalObject(entryTransform, uiPaths.thirdPlaceIconPath);

        if (firstIcon != null) firstIcon.SetActive(rank == 1);
        if (secondIcon != null) secondIcon.SetActive(rank == 2);
        if (thirdIcon != null) thirdIcon.SetActive(rank == 3);
    }

    private TextMeshProUGUI FindRequiredText(Transform root, string path, string fieldLabel)
    {
        if (!string.IsNullOrWhiteSpace(path))
        {
            Transform explicitTarget = root.Find(path);
            if (explicitTarget != null)
            {
                TextMeshProUGUI explicitText = explicitTarget.GetComponent<TextMeshProUGUI>();
                if (explicitText != null)
                {
                    return explicitText;
                }
            }
        }

        throw new MissingReferenceException(
            "Could not find TextMeshProUGUI for " + fieldLabel + ". Check UI Paths in the HighScoreTable inspector."
        );
    }

    private GameObject FindOptionalObject(Transform root, string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return null;
        }

        Transform target = root.Find(path);
        return target != null ? target.gameObject : null;
    }

    private string GetRankString(int rank)
    {
        switch (rank)
        {
            case 1: return "1ST";
            case 2: return "2ND";
            case 3: return "3RD";
            default: return rank + "TH";
        }
    }

    [ContextMenu("Add Random Test Entry")]
    private void AddRandomTestEntry()
    {
        string testName = "P" + UnityEngine.Random.Range(10, 99);
        int testScore = UnityEngine.Random.Range(0, 10000);
        AddHighScoreEntry(testScore, testName);
    }
}
