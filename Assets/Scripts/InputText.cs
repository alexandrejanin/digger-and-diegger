using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class InputText : MonoBehaviour {
    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 worldOffset, screenOffset;

    private Text text;

    private void Awake() {
        text = GetComponent<Text>();
    }

    private void Update() {
        transform.position = Camera.main.WorldToScreenPoint(target.transform.position + worldOffset) + screenOffset;
    }

    public void Prompt(bool isDigger, ButtonType? button) {
        var inputText = InputPrompt(isDigger, button);
        gameObject.SetActive(inputText != null);
        text.text = inputText;
    }

    private string InputPrompt(bool isDigger, ButtonType? button) => button switch {
        null => null,
        ButtonType.Up => isDigger ? "W" : "Up",
        ButtonType.Down => isDigger ? "S" : "Down",
        ButtonType.Left => isDigger ? "A" : "Left",
        ButtonType.Right => isDigger ? "D" : "Right",
        _ => "<ERR_BUTTON>",
    };
}
