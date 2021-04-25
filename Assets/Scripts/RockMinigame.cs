using UnityEngine;

public class RockMinigame : MonoBehaviour {
    [SerializeField] private GameObject rockPrefab;
    [SerializeField] private float inputDelay = 0.5f;

    private GameManager manager;

    private GameObject rock;
    private Vector3 rockTargetPosition;

    public ButtonType? DiggerButton { get; private set; }
    public ButtonType? DieggerButton { get; private set; }

    private int hitsLeft;

    private float diggerCorrect;
    private float dieggerCorrect;


    private void Awake() {
        manager = FindObjectOfType<GameManager>();
    }

    private void Start() {
        rock = Instantiate(rockPrefab, manager.Floor.transform.position + Vector3.down, Quaternion.identity, manager.Floor.transform);
        rockTargetPosition = manager.Floor.transform.position;
        hitsLeft = Random.Range(3, 5);

        DiggerButton = Util.RandomButton;
        DieggerButton = Util.RandomButton;
        manager.Players.onInput.AddListener(InputListener);
    }

    private void Update() {
        if (rock)
            rock.transform.position = Vector3.Lerp(rock.transform.position, rockTargetPosition, 0.05f);

        if (diggerCorrect > 0 && dieggerCorrect > 0) {
            diggerCorrect = 0;
            dieggerCorrect = 0;
            hitsLeft--;
            rockTargetPosition += Vector3.down / 10;

            if (hitsLeft <= 0) {
                Destroy(rock);
                manager.Players.onInput.RemoveListener(InputListener);
                manager.EndMinigame(true);
            }
        }

        if (DiggerButton == null && diggerCorrect <= 0)
            DiggerButton = Util.RandomButton;

        if (DieggerButton == null && dieggerCorrect <= 0)
            DieggerButton = Util.RandomButton;

        diggerCorrect -= Time.deltaTime;
        dieggerCorrect -= Time.deltaTime;
    }

    private void InputListener(bool isDigger, ButtonType button) {
        if (isDigger) {
            if (button == DiggerButton) {
                diggerCorrect = inputDelay;
                DiggerButton = null;
            } else
                manager.Players.Stun();
        } else {
            if (button == DieggerButton) {
                dieggerCorrect = inputDelay;
                DieggerButton = null;
            } else
                manager.Players.Stun();
        }
    }
}
