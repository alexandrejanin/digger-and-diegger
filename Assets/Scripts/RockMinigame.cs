using UnityEngine;

public class RockMinigame : Minigame {
    [SerializeField] private AudioSource rockPrefab;
    [SerializeField] private float inputDelay = 0.5f;
    [SerializeField] private AudioClip[] hitClips, breakClips;

    private GameManager manager;

    private AudioSource rock;
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

            rock.clip = hitClips[Random.Range(0, hitClips.Length)];
            rock.Play();

            foreach (var p in rock.GetComponentsInChildren<ParticleSystem>())
                p.Play();

            if (hitsLeft <= 0) {
                manager.EndMinigame(true);
                FindObjectOfType<SoundManager>().Play(breakClips[Random.Range(0, breakClips.Length)], rock.transform.position);
            }
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
        if (rock)
            Destroy(rock.gameObject);
        manager.Players.onInput.RemoveListener(InputListener);
    }
}
