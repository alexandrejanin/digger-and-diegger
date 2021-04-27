using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class HighScoreText : MonoBehaviour {
    [SerializeField] private bool hideOutOfGame = true;
    private Text text;
    private GameManager manager;

    private void Awake() {
        text = GetComponent<Text>();
        manager = FindObjectOfType<GameManager>();
        UpdateDisplay();
    }

    private void Update() {
        if (hideOutOfGame)
            transform.parent.GetComponent<Image>().enabled = manager.IsPlaying;
        if (hideOutOfGame)
            text.enabled = manager.IsPlaying;
    }

    public void UpdateDisplay() {
        text.text = $"Your deepest:\n-{PlayerPrefs.GetInt("HighScore", 0)}m";
    }
}
