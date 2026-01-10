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
}
