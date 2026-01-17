using UnityEngine;

/// <summary>
/// Lưu trữ dữ liệu game xuyên suốt các scene.
/// Chỉ reset khi bấm Play từ Menu.
/// </summary>
public class GameData : MonoBehaviour
{
    public static GameData instance;

    // Giá trị mặc định
    private const int DEFAULT_HEALTH = 5;
    private const int DEFAULT_DIAMONDS = 0;

    // Dữ liệu game
    [HideInInspector] public int currentHealth;
    [HideInInspector] public int currentDiamonds;
    [HideInInspector] public bool isNewGame = true; // Kiểm tra xem có phải game mới không
    
    // Leaderboard flags
    [HideInInspector] public bool isGameCompleted = false; // Đánh dấu đã phá đảo
    [HideInInspector] public bool showNameInput = false; // Hiển thị popup nhập tên khi về Menu

    private void Awake()
    {
        // Singleton pattern với DontDestroyOnLoad
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            ResetToDefault(); // Khởi tạo giá trị mặc định
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Reset tất cả dữ liệu về mặc định (gọi khi bấm Play từ Menu)
    /// </summary>
    public void ResetToDefault()
    {
        currentHealth = DEFAULT_HEALTH;
        currentDiamonds = DEFAULT_DIAMONDS;
        isNewGame = true;
        isGameCompleted = false;
        showNameInput = false;
        Debug.Log("GameData: Reset to default - Health: " + currentHealth + ", Diamonds: " + currentDiamonds);
    }

    /// <summary>
    /// Lưu dữ liệu hiện tại từ Player
    /// </summary>
    public void SavePlayerData(int health, int diamonds)
    {
        currentHealth = health;
        currentDiamonds = diamonds;
        isNewGame = false;
        Debug.Log("GameData: Saved - Health: " + currentHealth + ", Diamonds: " + currentDiamonds);
    }

    /// <summary>
    /// Kiểm tra xem có dữ liệu đã lưu không
    /// </summary>
    public bool HasSavedData()
    {
        return !isNewGame;
    }

    /// <summary>
    /// Tính điểm cuối cùng: 1 heart = 2 pts, 1 diamond = 1 pt
    /// </summary>
    public int CalculateFinalScore()
    {
        int heartScore = currentHealth * 2;
        int diamondScore = currentDiamonds * 1;
        int totalScore = heartScore + diamondScore;
        
        Debug.Log($"GameData: Final Score = {totalScore} (Hearts: {currentHealth} x 2 = {heartScore}, Diamonds: {currentDiamonds} x 1 = {diamondScore})");
        
        return totalScore;
    }
}
