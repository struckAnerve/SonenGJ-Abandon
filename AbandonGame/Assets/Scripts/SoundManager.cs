using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
    public AudioSource FXSource;
    public static SoundManager instance = null;

	// Use this for initialization
	void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
	}

    public void PlaySingle(AudioClip clip) {
        FXSource.clip = clip;
        FXSource.Play();
    }
	
	// Update is called once per frame
	void Update() {
	
	}
}
