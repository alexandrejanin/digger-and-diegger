using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreText : MonoBehaviour {
    private Text text;
    private GameManager manager;

    private void Awake() {
        text = GetComponent<Text>();
        manager = FindObjectOfType<GameManager>();
    }

    private void Update() {
        transform.parent.GetComponent<Image>().enabled = manager.IsPlaying;
        text.enabled = manager.IsPlaying;
        if (manager.IsPlaying)
            text.text = $"{manager.Score}m";
    }
}
