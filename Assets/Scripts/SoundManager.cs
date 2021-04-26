using UnityEngine;

public class SoundManager : MonoBehaviour {
    private AudioSource source;

    private void Awake() {
        source = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip, Vector3 position) {
        transform.position = position;
        source.clip = clip;
        source.Play();
    }
}
