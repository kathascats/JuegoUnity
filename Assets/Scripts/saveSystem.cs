using UnityEngine;

public static class SaveSystem
{
    public static void Save<T>(string baseKey, T data)
    {
        string key = ResolveKey(baseKey);
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
    }

    public static T Load<T>(string baseKey, T fallback)
    {
        string key = ResolveKey(baseKey);
        string json = PlayerPrefs.GetString(key, string.Empty);

        if (string.IsNullOrEmpty(json))
        {
            return fallback;
        }

        T data = JsonUtility.FromJson<T>(json);
        return data == null ? fallback : data;
    }

    public static bool HasData(string baseKey)
    {
        return PlayerPrefs.HasKey(ResolveKey(baseKey));
    }

    public static void Delete(string baseKey)
    {
        PlayerPrefs.DeleteKey(ResolveKey(baseKey));
        PlayerPrefs.Save();
    }

    private static string ResolveKey(string baseKey)
    {
        if (PlayerProfileManager.Instance == null)
        {
            return "Guest::" + baseKey;
        }

        return PlayerProfileManager.Instance.BuildScopedKey(baseKey);
    }
}
