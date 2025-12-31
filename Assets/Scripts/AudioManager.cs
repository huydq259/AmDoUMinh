using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;
    public Sound[] sounds;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else {
            Destroy(this.gameObject);
        }
    }

    private void Start() {
        foreach (Sound s in sounds) {
            s.source = this.gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.name = s.name;
            s.source.loop = s.loop;
            s.source.volume = s.volume;
        }
        //PlaySound("Theme");
    }

    public void PlaySound(string name) {
        foreach (Sound s in sounds) {
            if (s.name == name) {
                s.source.Play();
            }
        }
    }
}
