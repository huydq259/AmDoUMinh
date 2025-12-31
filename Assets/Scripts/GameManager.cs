using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverUIBG;

    [Header("Victory UI")]
    [SerializeField] private GameObject victoryUIBG;

    public int key;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(this);
        }
    }

    private void Start() {
        key = 0;

        gameOverUIBG.transform.localPosition = new Vector3(0f, -1200f, 0f);
    }

    public void TriggerGameOverUI() {
        AudioManager.instance.PlaySound("Game Over");
        gameOverUIBG.LeanMoveLocalY(0f, .8f).setEaseOutBounce();
    }

    public void TriggerVictoryUI() {
        victoryUIBG.LeanMoveLocalY(0f, .8f).setEaseInOutBack();
    }
}
