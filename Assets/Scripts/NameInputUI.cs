using UnityEngine;
using UnityEngine.UI;
using TMPro; // Thêm namespace TextMesh Pro

/// <summary>
/// Popup nhập tên sau khi phá đảo game
/// Hỗ trợ nhập tiếng Việt Unicode và TextMeshPro
/// </summary>
public class NameInputUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject nameInputPanel;
    
    [Header("Input Field Settings")]
    [SerializeField] private InputField standardInputField;
    
    // Đổi thành GameObject để dễ kéo thả, script sẽ tự lấy component
    [Tooltip("Kéo object chứa TMP InputField vào đây")]
    [SerializeField] private GameObject tmpInputFieldObject; 
    
    private TMP_InputField tmpInputField; // Biến nội bộ để dùng

    [Header("Text References")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text congratsText;
    // Hỗ trợ hiển thị bằng TMP nếu muốn (tùy chọn)
    [SerializeField] private TextMeshProUGUI scoreTextTMP;
    [SerializeField] private TextMeshProUGUI congratsTextTMP;
    
    [SerializeField] private Button submitButton;

    private int finalScore;

    private void Start()
    {
        // Tự động lấy component từ GameObject nếu có gán
        if (tmpInputFieldObject != null)
        {
            tmpInputField = tmpInputFieldObject.GetComponent<TMP_InputField>();
            if (tmpInputField == null)
            {
                Debug.LogWarning("NameInputUI: GameObject gán vào không chứa TMP_InputField!");
            }
        }

        if (submitButton != null)
        {
            submitButton.onClick.AddListener(OnSubmitClicked);
        }

        // Kiểm tra nếu cần hiện popup
        CheckShowNameInput();
    }

    /// <summary>
    /// Kiểm tra xem có cần hiển thị popup nhập tên không
    /// </summary>
    public void CheckShowNameInput()
    {
        if (GameData.instance != null && GameData.instance.showNameInput)
        {
            int finalScore = GameData.instance.CalculateFinalScore();
            ShowNameInput(finalScore);
            GameData.instance.showNameInput = false; // Reset flag
        }
        else
        {
            HideNameInput();
        }
    }

    /// <summary>
    /// Hiển thị popup nhập tên
    /// </summary>
    public void ShowNameInput(int score)
    {
        finalScore = score;

        if (nameInputPanel != null)
        {
            nameInputPanel.SetActive(true);
        }

        string scoreStr = "Score: " + score.ToString();
        string congratsStr = "Congratulations!";

        // Cập nhật text standard
        if (scoreText != null) scoreText.text = scoreStr;
        if (congratsText != null) congratsText.text = congratsStr;

        // Cập nhật text TMP (nếu dùng)
        if (scoreTextTMP != null) scoreTextTMP.text = scoreStr;
        if (congratsTextTMP != null) congratsTextTMP.text = congratsStr;

        // Reset input field
        if (standardInputField != null)
        {
            standardInputField.text = "";
            standardInputField.Select();
            standardInputField.ActivateInputField();
        }
        
        if (tmpInputField != null)
        {
            tmpInputField.text = "";
            tmpInputField.Select();
            tmpInputField.ActivateInputField();
        }

        Debug.Log("NameInputUI: Showing name input with score " + score);
    }

    /// <summary>
    /// Ẩn popup nhập tên
    /// </summary>
    public void HideNameInput()
    {
        if (nameInputPanel != null)
        {
            nameInputPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Xử lý khi người chơi bấm Submit
    /// </summary>
    public void OnSubmitClicked()
    {
        string playerName = "";

        // Lấy tên từ input field nào đang được gán
        if (tmpInputField != null)
        {
            playerName = tmpInputField.text.Trim();
        }
        else if (standardInputField != null)
        {
            playerName = standardInputField.text.Trim();
        }
        else 
        {
            Debug.LogError("NameInputUI: Chưa gán InputField (Standard hoặc TMP)!");
            return;
        }

        // Kiểm tra tên hợp lệ
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Player";
        }

        // Giới hạn độ dài tên
        if (playerName.Length > 20)
        {
            playerName = playerName.Substring(0, 20);
        }

        // Lưu vào Leaderboard
        if (LeaderboardManager.instance != null)
        {
            LeaderboardManager.instance.AddScore(playerName, finalScore);
        }

        AudioManager.instance?.PlaySound("Click");

        // Ẩn popup
        HideNameInput();

        // Reset GameData
        if (GameData.instance != null)
        {
            GameData.instance.isGameCompleted = false;
            GameData.instance.ResetToDefault();
        }

        Debug.Log("NameInputUI: Saved score - " + playerName + ": " + finalScore);
    }
}
