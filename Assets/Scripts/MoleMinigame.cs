using System.Collections.Generic;
using UnityEngine;

public class MoleMinigame : Minigame {
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private Mole molePrefab;
    [SerializeField] private float diggerX = -0.9f;
    [SerializeField] private float diggurX = 0.9f;
    [SerializeField] private float validHeight = 4;
    [SerializeField] private float validHeightDiff = 0.5f;
    [SerializeField] private float tickDuration = 0.5f;

    public override string Description => "Hit them at the right time!";
    public override ButtonType? DiggerButton => null;
    public override ButtonType? DiggurButton => null;

    private GameManager manager;

    private readonly List<Mole> moles = new List<Mole>();

    private GameObject line;

    private int[] diggerDelays;
    private int[] diggurDelays;

    private int diggerIndex;
    private int diggurIndex;

    private float elapsedTime;

    private float spawnY;

    private void Awake() {
        manager = FindObjectOfType<GameManager>();
        spawnY = manager.Ceiling.transform.position.y;

        line = Instantiate(linePrefab, FindObjectOfType<Canvas>().transform);

        diggerDelays = new int[Random.Range(1, 5)];
        diggurDelays = new int[Random.Range(1, 5)];

        var t = 0;
        for (var i = 0; i < diggerDelays.Length; i++) {
            t += Random.value < 0.5f ? 1 : 2;
            diggerDelays[i] = t;
        }

        t = 0;
        for (var i = 0; i < diggurDelays.Length; i++) {
            t += Random.value < 0.5f ? 1 : 2;
            diggurDelays[i] = t;
        }

        manager.Players.onInput.AddListener(InputListener);
    }

    private void Update() {
        elapsedTime += Time.deltaTime;

        if (diggerIndex < diggerDelays.Length && elapsedTime > tickDuration * diggerDelays[diggerIndex]) {
            diggerIndex++;
            SpawnMole(true);
        }

        if (diggurIndex < diggurDelays.Length && elapsedTime > tickDuration * diggurDelays[diggurIndex]) {
            diggurIndex++;
            SpawnMole(false);
        }

        line.transform.position = Camera.main.WorldToScreenPoint(manager.Players.transform.position + validHeight * Vector3.up);
    }

    private void SpawnMole(bool isDigger) {
        var x = isDigger ? diggerX : diggurX;
        var mole = Instantiate(molePrefab, new Vector3(x, spawnY, 0), Quaternion.identity);
        mole.isDigger = isDigger;
        moles.Add(mole);
    }

    private void InputListener(bool isDigger, ButtonType button) {
        foreach (var mole in moles) {
            if (!mole || mole.isDigger != isDigger || mole.Button != button)
                continue;

            var dist = Mathf.Abs(mole.transform.position.y - (manager.Players.transform.position.y + validHeight));
            Debug.Log(dist);
            if (dist < validHeightDiff)
                mole.Die();
        }

        moles.RemoveAll(m => !m.Alive);

        if (moles.Count == 0 && diggerIndex >= diggerDelays.Length && diggurIndex >= diggurDelays.Length)
            manager.EndMinigame(true);
    }

    private void OnDestroy() {
        if (line)
            Destroy(line);
        manager.Players.onInput.RemoveListener(InputListener);
    }
}
