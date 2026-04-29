using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfileManager : MonoBehaviour
{
    [Serializable]
    private class ProfileListData
    {
        public List<string> profileIds = new List<string>();
    }

    public static PlayerProfileManager Instance { get; private set; }

    public static event Action<string> OnProfileChanged;

    private const string LastProfileKey = "auth.lastProfile";
    private const string ProfileListKey = "auth.profileList";

    [SerializeField] private bool autoLoginLastProfile = true;
    [SerializeField] private string defaultProfileId = "Player1";

    public string CurrentProfileId { get; private set; }
    public bool IsLoggedIn => !string.IsNullOrWhiteSpace(CurrentProfileId);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (autoLoginLastProfile)
        {
            string lastProfile = PlayerPrefs.GetString(LastProfileKey, defaultProfileId);
            Login(lastProfile);
        }
    }

    public bool Login(string profileId)
    {
        string sanitized = SanitizeProfileId(profileId);
        if (string.IsNullOrEmpty(sanitized))
        {
            return false;
        }

        CurrentProfileId = sanitized;
        RegisterProfile(CurrentProfileId);
        PlayerPrefs.SetString(LastProfileKey, CurrentProfileId);
        PlayerPrefs.Save();
        OnProfileChanged?.Invoke(CurrentProfileId);
        return true;
    }

    public void Logout()
    {
        CurrentProfileId = string.Empty;
        OnProfileChanged?.Invoke(CurrentProfileId);
    }

    public List<string> GetAllProfiles()
    {
        string json = PlayerPrefs.GetString(ProfileListKey, string.Empty);
        if (string.IsNullOrEmpty(json))
        {
            return new List<string>();
        }

        ProfileListData data = JsonUtility.FromJson<ProfileListData>(json);
        if (data == null || data.profileIds == null)
        {
            return new List<string>();
        }

        return data.profileIds;
    }

    public string BuildScopedKey(string baseKey)
    {
        string profile = IsLoggedIn ? CurrentProfileId : "Guest";
        return profile + "::" + baseKey;
    }

    private void RegisterProfile(string profileId)
    {
        List<string> profiles = GetAllProfiles();
        if (profiles.Contains(profileId))
        {
            return;
        }

        profiles.Add(profileId);
        ProfileListData data = new ProfileListData { profileIds = profiles };
        PlayerPrefs.SetString(ProfileListKey, JsonUtility.ToJson(data));
    }

    private string SanitizeProfileId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        return value.Trim();
    }
}
