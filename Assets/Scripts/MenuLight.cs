using UnityEngine;

[RequireComponent(typeof(Light))]
public class MenuLight : MonoBehaviour {
    [SerializeField] private float fadeTime = 1f;
    private new Light light;
    private GameManager manager;

    private float startIntensity;

    private void Awake() {
        light = GetComponent<Light>();
        manager = FindObjectOfType<GameManager>();
        startIntensity = light.intensity;
    }

    private void Update() {
        if (manager.Players)
            light.intensity = Mathf.Max(0, light.intensity - Time.deltaTime * startIntensity / fadeTime);
    }
}
