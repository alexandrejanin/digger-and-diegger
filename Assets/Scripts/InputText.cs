using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class InputText : MonoBehaviour {
    [SerializeField] private Vector3 worldOffset, screenOffset;
    [SerializeField] private Sprite emptyKey, upKey, downKey, leftKey, rightKey, a, b, x, y;

    [HideInInspector] public GameObject target;
    public bool isDigger;

    private Text text;
    private Image background;

    private void Awake() {
        text = GetComponent<Text>();
    }

    private void Update() {
        if (target)
            transform.position = Camera.main.WorldToScreenPoint(target.transform.position + worldOffset) + screenOffset;
    }

    public void Prompt(ButtonType? button) {
        gameObject.SetActive(button != null);
        if (button == null)
            return;
        if (isDigger) {
            background.sprite = emptyKey;
            text.text = ButtonText(button);
        } else {
            background.sprite = ButtonSprite(button);
        }
    }

    private static string ButtonText(ButtonType? button) => button switch {
        ButtonType.Up => "W",
        ButtonType.Down => "S",
        ButtonType.Left => "A",
        ButtonType.Right => "D",
        _ => null,
    };

    private Sprite ButtonSprite(ButtonType? button) => button switch {
        ButtonType.Up => upKey,
        ButtonType.Down => downKey,
        ButtonType.Left => leftKey,
        ButtonType.Right => rightKey,
        _ => null,
    };
}
