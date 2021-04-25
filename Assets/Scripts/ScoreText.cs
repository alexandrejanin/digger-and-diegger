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
        text.text = $"{manager.Floor.transform.position.y:F1}m";
    }
}
