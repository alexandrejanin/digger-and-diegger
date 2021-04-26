using UnityEngine;
using UnityEngine.Events;

public class Players : MonoBehaviour {
    [SerializeField] private Animator diggerAnimator, diggurAnimator;
    [SerializeField, Min(0)] private float digDistance = 0.5f;

    public UnityEvent<bool, ButtonType> onInput;

    public ButtonType? DiggerButton => waitingForInput && diggerNext ? ButtonType.Down : (ButtonType?) null;
    public ButtonType? DiggurButton => waitingForInput && !diggerNext ? ButtonType.Down : (ButtonType?) null;

    private float stunTimer;
    public bool IsStunned => stunTimer > 0;

    private bool waitingForInput;
    private bool diggerNext = true;

    private GameManager manager;

    private void Awake() {
        manager = FindObjectOfType<GameManager>();

        onInput.AddListener((isDigger, button) => {
            if (!manager.InDigPhase || !waitingForInput) return;

            if (isDigger == diggerNext && button == ButtonType.Down)
                DigDown();
            else
                Stun(1 + manager.Score / 100f);
        });
    }

    private void Update() {
        stunTimer -= Time.deltaTime;

        diggerAnimator.SetBool("Stunned", IsStunned);
        diggurAnimator.SetBool("Stunned", IsStunned);

        if (IsStunned)
            return;

        if (Input.GetButtonDown("DiggerUp")) onInput?.Invoke(true, ButtonType.Up);
        if (Input.GetButtonDown("DiggerDown")) onInput?.Invoke(true, ButtonType.Down);
        if (Input.GetButtonDown("DiggerLeft")) onInput?.Invoke(true, ButtonType.Left);
        if (Input.GetButtonDown("DiggerRight")) onInput?.Invoke(true, ButtonType.Right);

        if (Input.GetButtonDown("DiggurUp")) onInput?.Invoke(false, ButtonType.Up);
        if (Input.GetButtonDown("DiggurDown")) onInput?.Invoke(false, ButtonType.Down);
        if (Input.GetButtonDown("DiggurLeft")) onInput?.Invoke(false, ButtonType.Left);
        if (Input.GetButtonDown("DiggurRight")) onInput?.Invoke(false, ButtonType.Right);
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Floor")) {
            waitingForInput = true;
            if (!manager.IsPlaying)
                manager.PlayersLanded();
        }
    }

    private void DigDown() {
        if (diggerNext)
            SwingDigger();
        else
            SwingDiggur();

        manager.Floor.Depth -= digDistance;
        diggerNext = !diggerNext;
        // waitingForInput = false;
    }

    public void Stun(float duration = 1f) {
        stunTimer = duration;
        diggerAnimator.SetBool("Stunned", true);
        diggerAnimator.Play("Stunned");
        diggurAnimator.Play("Stunned");
    }

    public void SwingDigger() => diggerAnimator.Play("Swing");
    public void SwingDiggur() => diggurAnimator.Play("Swing");

    public void Die() {
        Stun(1000);
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<BoxCollider>());
    }
}
