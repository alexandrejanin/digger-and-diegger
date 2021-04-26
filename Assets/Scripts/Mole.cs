using UnityEngine;

public class Mole : MonoBehaviour {
    [SerializeField] private InputText inputTextPrefab;

    [HideInInspector] public bool isDigger;
    public ButtonType Button { get; private set; }

    public bool Alive { get; private set; } = true;

    private InputText inputText;

    private void Awake() {
        Button = Util.RandomButton;
    }

    private void Start() {
        inputText = Instantiate(inputTextPrefab, FindObjectOfType<Canvas>().transform);
        inputText.target = gameObject;
        inputText.worldOffset = isDigger ? Vector3.left : Vector3.right;
        inputText.Prompt(Button);
    }

    private void OnCollisionEnter(Collision other) {
        var players = other.gameObject.GetComponent<Players>();
        if (players) {
            FindObjectOfType<GameManager>().EndMinigame(false);
            Die();
        }
    }

    public void Die() {
        Alive = false;
        Destroy(inputText.gameObject);
        GetComponent<Rigidbody>().velocity = new Vector3(
            Random.Range(-10f, 10f),
            Random.Range(0f, 10f),
            -50f
        );
    }
}
