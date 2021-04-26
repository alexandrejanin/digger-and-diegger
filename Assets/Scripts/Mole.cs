using UnityEngine;

public class Mole : MonoBehaviour {
    [SerializeField] private InputText inputTextPrefab;

    public bool isDigger;
    public ButtonType Button { get; private set; }

    public bool Alive { get; private set; } = true;

    private InputText inputText;

    private void Awake() {
        Button = Util.RandomButton;
    }

    private void Start() {
        inputText = Instantiate(inputTextPrefab, FindObjectOfType<Canvas>().transform);
        inputText.isDigger = isDigger;
        inputText.target = gameObject;
        inputText.Prompt(Button);
    }

    private void OnCollisionEnter(Collision other) {
        var players = other.gameObject.GetComponent<Players>();
        if (players) {
            players.Stun(1000);
            Die();
        }
    }

    public void Die() {
        Alive = false;
        Destroy(inputText.gameObject);
        Destroy(gameObject);
    }
}
