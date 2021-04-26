using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RockMinigame : Minigame {
    [SerializeField] private GameObject rockPrefab;
    [SerializeField] private float inputDelay = 0.5f;

    private GameManager manager;

    private GameObject rock;
    private Vector3 rockTargetPosition;

    public override string Description => "Hit it at the same time!";

    public override ButtonType? DiggerButton => diggerButton;
    public override ButtonType? DiggurButton => diggurButton;
    private ButtonType? diggerButton, diggurButton;

    private int hitsLeft;

    private float diggerCorrect;
    private float diggurCorrect;

    private void Awake() {
        manager = FindObjectOfType<GameManager>();
    }

    private void Start() {
        rock = Instantiate(rockPrefab, manager.Floor.transform.position + Vector3.down, Quaternion.identity, manager.Floor.transform);
        rockTargetPosition = manager.Floor.transform.position;
        hitsLeft = Random.Range(3, 6);

        diggerButton = Util.RandomButton;
        diggurButton = Util.RandomButton;
        manager.Players.onInput.AddListener(InputListener);
    }

    private void Update() {
        if (rock)
            rock.transform.position = Vector3.Lerp(rock.transform.position, rockTargetPosition, 0.05f);

        if (diggerCorrect > 0 && diggurCorrect > 0) {
            diggerCorrect = 0;
            diggurCorrect = 0;
            hitsLeft--;
            rockTargetPosition += Vector3.down / 10;

            manager.Players.SwingDigger();
            manager.Players.SwingDiggur();

            if (hitsLeft <= 0)
                manager.EndMinigame(true);
        }

        if (DiggerButton == null && diggerCorrect <= 0)
            diggerButton = Util.RandomButton;

        if (DiggurButton == null && diggurCorrect <= 0)
            diggurButton = Util.RandomButton;

        diggerCorrect -= Time.deltaTime;
        diggurCorrect -= Time.deltaTime;
    }

    private void InputListener(bool isDigger, ButtonType button) {
        if (isDigger) {
            if (button == DiggerButton) {
                diggerCorrect = inputDelay;
                diggerButton = null;
            } else
                manager.Players.Stun(2);
        } else {
            if (button == DiggurButton) {
                diggurCorrect = inputDelay;
                diggurButton = null;
            } else
                manager.Players.Stun(2);
        }
    }

    private void OnDestroy() {
        Destroy(rock);
        manager.Players.onInput.RemoveListener(InputListener);
    }
}
