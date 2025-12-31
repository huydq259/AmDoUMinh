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

    public void PlayGame() {
        SceneManager.LoadScene("SampleScene");
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
