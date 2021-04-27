using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Mole : MonoBehaviour {
    [SerializeField] private InputText inputTextPrefab;
    [SerializeField] private AudioClip[] deathClips;
    [SerializeField] private AudioClip thatOneDeathClip;

    [HideInInspector] public bool isDigger;
    public ButtonType Button { get; private set; }

    public bool Alive { get; private set; } = true;

    private InputText inputText;
    private AudioSource audioSource;

    private void Awake() {
        Button = Util.RandomButton;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        inputText = Instantiate(inputTextPrefab, FindObjectOfType<Canvas>().transform);
        inputText.target = gameObject;
        inputText.worldOffset = isDigger ? Vector3.left : Vector3.right;
        inputText.Prompt(Button);
    }

    private void OnCollisionEnter(Collision other) {
        if (!Alive)
            return;

        var players = other.gameObject.GetComponent<Players>();
        if (players) {
            FindObjectOfType<GameManager>().EndMinigame(false);
            Die();
        }
    }

    public void Die() {
        Alive = false;
        Destroy(inputText.gameObject);
        GetComponentInChildren<ParticleSystem>().Play();
        GetComponent<Rigidbody>().velocity = new Vector3(
            Random.Range(-10f, 10f),
            Random.Range(5f, 10f),
            -50f
        );
        audioSource.clip = Random.value < 0.05f
            ? thatOneDeathClip
            : deathClips[Random.Range(0, deathClips.Length)];
        audioSource.Play();
    }

    private void OnDestroy() {
        if (inputText)
            Destroy(inputText.gameObject);
    }
}
