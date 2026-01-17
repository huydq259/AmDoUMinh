using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Gắn script này vào Credits_Menu panel để chuyển đổi thành Leaderboard
/// Script sẽ tự động tìm và sử dụng các UI elements có sẵn trong Credits panel
/// </summary>
public class CreditsToLeaderboard : MonoBehaviour
{
    [Header("UI Elements (Auto-find hoặc kéo thả)")]
    [SerializeField] private Text titleText; // Text header "CREDITS" -> "LEADERBOARD"
    [SerializeField] private Text contentText; // Text nội dung credits -> danh sách điểm cao

    private void Start()
    {
        // Tự động tìm UI elements nếu chưa được gán
        FindUIElements();
        
        // Cập nhật tiêu đề
        UpdateTitle();
        
        // Hiển thị Leaderboard
        RefreshLeaderboard();
    }

    private void OnEnable()
    {
        // Refresh mỗi khi panel được hiển thị
        RefreshLeaderboard();
    }

    /// <summary>
    /// Tự động tìm các UI elements trong panel
    /// </summary>
    private void FindUIElements()
    {
        if (titleText == null)
        {
            // Tìm Header/TXT (có text "CREDITS")
            Transform header = transform.Find("BG/Header/TXT");
            if (header != null)
            {
                titleText = header.GetComponent<Text>();
            }
        }

        if (contentText == null)
        {
            // Tìm Text (Legacy) chứa nội dung credits
            Transform content = transform.Find("BG/Text (Legacy)");
            if (content != null)
            {
                contentText = content.GetComponent<Text>();
            }
        }
    }

    /// <summary>
    /// Changes the title from "CREDITS" to "LEADERBOARD"
    /// </summary>
    private void UpdateTitle()
    {
        if (titleText != null)
        {
            titleText.text = "LEADERBOARD";
        }
    }

    /// <summary>
    /// Updates the displayed Leaderboard content
    /// </summary>
    public void RefreshLeaderboard()
    {
        if (contentText == null)
        {
            Debug.LogWarning("CreditsToLeaderboard: Content text not found!");
            return;
        }

        if (LeaderboardManager.instance == null)
        {
            contentText.text = "Loading...";
            return;
        }

        List<LeaderboardEntry> entries = LeaderboardManager.instance.GetLeaderboard();

        if (entries.Count == 0)
        {
            contentText.text = "No scores yet!\n\nComplete the game to\nadd your name to the board!";
            return;
        }

        // Format the high score list
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < entries.Count && i < 10; i++)
        {
            string rank = (i + 1).ToString();
            string name = entries[i].playerName;
            string score = entries[i].score.ToString();

            // Giới hạn độ dài tên để hiển thị đẹp
            if (name.Length > 12)
            {
                name = name.Substring(0, 12) + "...";
            }

            sb.AppendLine($"#{rank}  {name}  -  {score} pts");
        }

        contentText.text = sb.ToString();
        
        // Điều chỉnh font size và alignment cho phù hợp
        contentText.fontSize = 40;
        contentText.alignment = TextAnchor.UpperCenter;
        contentText.lineSpacing = 1.5f;
    }
}
