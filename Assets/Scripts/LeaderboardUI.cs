using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Quản lý UI hiển thị Leaderboard
/// </summary>
public class LeaderboardUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private Transform entriesContainer;
    [SerializeField] private GameObject entryPrefab; // Prefab cho mỗi dòng điểm
    [SerializeField] private Button closeButton;

    // Nếu không dùng prefab, có thể dùng Text trực tiếp
    [Header("Alternative: Direct Text References")]
    [SerializeField] private Text[] rankTexts; // Text hiển thị thứ hạng (1-10)
    [SerializeField] private Text[] nameTexts; // Text hiển thị tên
    [SerializeField] private Text[] scoreTexts; // Text hiển thị điểm

    private void Start()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(HideLeaderboard);
        }

        // Mặc định ẩn panel
        if (leaderboardPanel != null)
        {
            leaderboardPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Hiển thị Leaderboard Panel
    /// </summary>
    public void ShowLeaderboard()
    {
        if (leaderboardPanel != null)
        {
            leaderboardPanel.SetActive(true);
            RefreshLeaderboard();
            AudioManager.instance?.PlaySound("Click");
        }
    }

    /// <summary>
    /// Ẩn Leaderboard Panel
    /// </summary>
    public void HideLeaderboard()
    {
        if (leaderboardPanel != null)
        {
            leaderboardPanel.SetActive(false);
            AudioManager.instance?.PlaySound("Click");
        }
    }

    /// <summary>
    /// Cập nhật hiển thị danh sách điểm cao
    /// </summary>
    public void RefreshLeaderboard()
    {
        if (LeaderboardManager.instance == null)
        {
            Debug.LogWarning("LeaderboardUI: LeaderboardManager not found!");
            return;
        }

        List<LeaderboardEntry> entries = LeaderboardManager.instance.GetLeaderboard();

        // Nếu dùng Text trực tiếp
        if (nameTexts != null && nameTexts.Length > 0)
        {
            for (int i = 0; i < nameTexts.Length; i++)
            {
                if (i < entries.Count)
                {
                    if (rankTexts != null && i < rankTexts.Length && rankTexts[i] != null)
                    {
                        rankTexts[i].text = (i + 1).ToString();
                    }
                    if (nameTexts[i] != null)
                    {
                        nameTexts[i].text = entries[i].playerName;
                    }
                    if (scoreTexts != null && i < scoreTexts.Length && scoreTexts[i] != null)
                    {
                        scoreTexts[i].text = entries[i].score.ToString();
                    }
                }
                else
                {
                    // Ô trống
                    if (rankTexts != null && i < rankTexts.Length && rankTexts[i] != null)
                    {
                        rankTexts[i].text = (i + 1).ToString();
                    }
                    if (nameTexts[i] != null)
                    {
                        nameTexts[i].text = "---";
                    }
                    if (scoreTexts != null && i < scoreTexts.Length && scoreTexts[i] != null)
                    {
                        scoreTexts[i].text = "0";
                    }
                }
            }
        }

        // Nếu dùng prefab động
        if (entryPrefab != null && entriesContainer != null)
        {
            // Xóa các entry cũ
            foreach (Transform child in entriesContainer)
            {
                Destroy(child.gameObject);
            }

            // Tạo entry mới
            for (int i = 0; i < entries.Count; i++)
            {
                GameObject entryGO = Instantiate(entryPrefab, entriesContainer);
                Text[] texts = entryGO.GetComponentsInChildren<Text>();
                if (texts.Length >= 3)
                {
                    texts[0].text = (i + 1).ToString(); // Rank
                    texts[1].text = entries[i].playerName; // Name
                    texts[2].text = entries[i].score.ToString(); // Score
                }
            }
        }

        Debug.Log("LeaderboardUI: Refreshed with " + entries.Count + " entries");
    }
}
