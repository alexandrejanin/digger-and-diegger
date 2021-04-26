using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InputText : MonoBehaviour {
    [FormerlySerializedAs("background")] [SerializeField]
    private Image image;

    public Vector3 worldOffset, screenOffset;
    [SerializeField] private Sprite emptyKey, upKey, downKey, leftKey, rightKey, a, b, x, y;

    [HideInInspector] public GameObject target;

    private void Update() {
        if (target)
            transform.position = Camera.main.WorldToScreenPoint(target.transform.position + worldOffset) + screenOffset;
    }

    public void Prompt(ButtonType? button) {
        image.enabled = button != null;
        image.sprite = ButtonSprite(button);
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
