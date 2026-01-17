using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Quản lý Leaderboard - lưu và đọc điểm cao
/// </summary>
[System.Serializable]
public class LeaderboardEntry
{
    public string playerName;
    public int score;

    public LeaderboardEntry(string name, int score)
    {
        this.playerName = name;
        this.score = score;
    }
}

[System.Serializable]
public class LeaderboardData
{
    public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
}

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager instance;

    private const string LEADERBOARD_KEY = "LeaderboardData";
    private const int MAX_ENTRIES = 10;

    private LeaderboardData leaderboardData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadLeaderboard();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Load dữ liệu Leaderboard từ PlayerPrefs
    /// </summary>
    public void LoadLeaderboard()
    {
        string json = PlayerPrefs.GetString(LEADERBOARD_KEY, "");
        if (!string.IsNullOrEmpty(json))
        {
            leaderboardData = JsonUtility.FromJson<LeaderboardData>(json);
        }
        else
        {
            leaderboardData = new LeaderboardData();
        }
        Debug.Log("LeaderboardManager: Loaded " + leaderboardData.entries.Count + " entries");
    }

    /// <summary>
    /// Lưu dữ liệu Leaderboard vào PlayerPrefs
    /// </summary>
    public void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(leaderboardData);
        PlayerPrefs.SetString(LEADERBOARD_KEY, json);
        PlayerPrefs.Save();
        Debug.Log("LeaderboardManager: Saved " + leaderboardData.entries.Count + " entries");
    }

    /// <summary>
    /// Thêm điểm mới vào Leaderboard
    /// </summary>
    public void AddScore(string playerName, int score)
    {
        LeaderboardEntry newEntry = new LeaderboardEntry(playerName, score);
        leaderboardData.entries.Add(newEntry);

        // Sắp xếp theo điểm giảm dần
        leaderboardData.entries.Sort((a, b) => b.score.CompareTo(a.score));

        // Giữ lại top 10
        if (leaderboardData.entries.Count > MAX_ENTRIES)
        {
            leaderboardData.entries.RemoveRange(MAX_ENTRIES, leaderboardData.entries.Count - MAX_ENTRIES);
        }

        SaveLeaderboard();
        Debug.Log("LeaderboardManager: Added score - " + playerName + ": " + score);
    }

    /// <summary>
    /// Lấy danh sách điểm cao
    /// </summary>
    public List<LeaderboardEntry> GetLeaderboard()
    {
        return leaderboardData.entries;
    }

    /// <summary>
    /// Xóa toàn bộ Leaderboard (dùng cho testing)
    /// </summary>
    public void ClearLeaderboard()
    {
        leaderboardData = new LeaderboardData();
        SaveLeaderboard();
        Debug.Log("LeaderboardManager: Cleared all entries");
    }
}
