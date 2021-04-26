using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour {
    [SerializeField] private AudioMixer mixer;

    private void Awake() {
        var slider = GetComponent<Slider>();
        mixer.SetFloat(gameObject.name, slider.value < 0.01f ? -80 : 20 * Mathf.Log10(slider.value));
    }

    public void SetFloat(float value) => mixer.SetFloat(gameObject.name, value < 0.01f ? -80 : 20 * Mathf.Log10(value));
}
