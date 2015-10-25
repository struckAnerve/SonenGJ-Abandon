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
        Debug.Log("Clip played");
    }

    public void RandomizeSFX(params AudioClip[] clips) {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(.95f, 1.05f);
        FXSource.pitch = randomPitch;
        FXSource.clip = clips[randomIndex];
        FXSource.Play();
    }
}
