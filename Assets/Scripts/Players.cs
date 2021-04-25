using UnityEngine;
using UnityEngine.Events;

public class Players : MonoBehaviour {
    [SerializeField, Min(0)] private float digDistance = 0.5f;

    public UnityEvent<bool, ButtonType> onInput;

    public ButtonType? DiggerButton => waitingForInput && diggerNext ? ButtonType.Down : (ButtonType?) null;
    public ButtonType? DieggerButton => waitingForInput && !diggerNext ? ButtonType.Down : (ButtonType?) null;

    private PlayerStun[] stuns;
    private float stunTimer;

    private bool waitingForInput;
    private bool diggerNext;

    private GameManager manager;

    private void Awake() {
        manager = FindObjectOfType<GameManager>();
        stuns = GetComponentsInChildren<PlayerStun>();

        onInput.AddListener((isDigger, button) => {
            if (!manager.InDigPhase || !waitingForInput) return;

            if (isDigger == diggerNext && button == ButtonType.Down)
                DigDown();
            else
                Stun();
        });
    }

    private void Update() {
        stunTimer -= Time.deltaTime;
        foreach (var stun in stuns)
            stun.active = stunTimer > 0;

        if (stunTimer > 0)
            return;

        if (Input.GetButtonDown("DiggerUp")) onInput?.Invoke(true, ButtonType.Up);
        if (Input.GetButtonDown("DiggerDown")) onInput?.Invoke(true, ButtonType.Down);
        if (Input.GetButtonDown("DiggerLeft")) onInput?.Invoke(true, ButtonType.Left);
        if (Input.GetButtonDown("DiggerRight")) onInput?.Invoke(true, ButtonType.Right);

        if (Input.GetButtonDown("DieggerUp")) onInput?.Invoke(false, ButtonType.Up);
        if (Input.GetButtonDown("DieggerDown")) onInput?.Invoke(false, ButtonType.Down);
        if (Input.GetButtonDown("DieggerLeft")) onInput?.Invoke(false, ButtonType.Left);
        if (Input.GetButtonDown("DieggerRight")) onInput?.Invoke(false, ButtonType.Right);
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Floor"))
            waitingForInput = true;
    }

    private void DigDown() {
        manager.Floor.Depth -= digDistance;
        diggerNext = !diggerNext;
        waitingForInput = false;
    }

    public void Stun(float duration = 1f) {
        stunTimer = duration;
    }
}
