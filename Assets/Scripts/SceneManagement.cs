using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour {

    public static SceneManagement instance;

    public void PlayClickSound() {
        AudioManager.instance.PlaySound("Click");
    }

    private void Awake() {

        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    public void LoadLevel(string tenManChoi)
    {
        SceneManager.LoadScene(tenManChoi);
        Time.timeScale = 1f; // Đảm bảo game không bị pause khi sang màn mới
    }
    public void SmartPlayButton()
    {
        // Lấy tên màn đang đứng
        string tenManHienTai = SceneManager.GetActiveScene().name;

        // KIỂM TRA: Nếu đang đứng ở Menu thì vào Màn 1
        if (tenManHienTai == "Menu")
        {
            // Reset dữ liệu game về mặc định khi bắt đầu từ Menu
            if (GameData.instance != null)
            {
                GameData.instance.ResetToDefault();
            }
            SceneManager.LoadScene("man1"); // Nhớ thay "Scene1" đúng tên màn 1 của bạn
        }
        // CÒN LẠI: Nếu đang chơi (ở Màn 1 hoặc Màn 2) thì load lại màn đó
        else
        {
            SceneManager.LoadScene(tenManHienTai);
        }

        // Mở khóa thời gian
        Time.timeScale = 1f;
    }
    public void Retry() {
        Debug.Log("Retry Game!");
        AudioManager.instance.PlaySound("Click");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu() {
        Debug.Log("Menu");
        AudioManager.instance.PlaySound("Click");
        SceneManager.LoadScene("Menu");
    }

    public void ExitGame() {
        Debug.Log("Exit Game");
        AudioManager.instance.PlaySound("Click");
        Application.Quit();
    }

    public void NextLevel() {
        Debug.Log("Next Level");
        AudioManager.instance.PlaySound("Click");
        
        // Lấy tên màn hiện tại
        string currentScene = SceneManager.GetActiveScene().name;
        
        // Xác định màn tiếp theo dựa trên màn hiện tại
        // man1 -> Scene3.1 -> SampleScene
        if (currentScene == "man1") {
            SceneManager.LoadScene("Scene3.1");
        }
        else if (currentScene == "Scene3.1") {
            SceneManager.LoadScene("SampleScene");
        }
        else if (currentScene == "SampleScene") {
            // Đây là màn cuối - người chơi đã phá đảo!
            if (GameData.instance != null)
            {
                GameData.instance.isGameCompleted = true;
                GameData.instance.showNameInput = true;
                Debug.Log("Game Completed! Final score: " + GameData.instance.currentDiamonds);
            }
            // Quay về Menu để hiển thị popup nhập tên
            SceneManager.LoadScene("Menu");
        }
        else {
            // Mặc định: quay về Menu nếu đã hết màn
            SceneManager.LoadScene("Menu");
        }
        
        Time.timeScale = 1f;
    }
}
