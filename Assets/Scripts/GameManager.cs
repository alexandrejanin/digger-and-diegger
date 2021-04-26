using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [SerializeField] public Players playersPrefab;
    [SerializeField] public InputText diggerInputText, diggurInputText;
    [SerializeField] private GameObject mainMenu, pauseMenu, defeatMenu;
    [SerializeField, Min(0)] private float minMinigameDelay = 3, maxMinigameDelay = 10;
    [SerializeField] private Minigame[] minigames;

    private float minigameDelay;

    public Players Players { get; private set; }
    public Floor Floor { get; private set; }
    public Ceiling Ceiling { get; private set; }

    public Minigame Minigame { get; private set; }
    public bool InMinigame => IsPlaying && Minigame != null;

    public bool InDigPhase => IsPlaying && Minigame == null;

    public bool IsPlaying { get; private set; }

    private void Awake() {
        Floor = FindObjectOfType<Floor>(true);
        Ceiling = FindObjectOfType<Ceiling>(true);
        minigameDelay = Random.Range(3f, 6f);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();

        if (!IsPlaying)
            return;

        minigameDelay -= Time.deltaTime;
        if (InDigPhase && minigameDelay < 0)
            SpawnMinigame();

        if (Minigame != null) {
            diggerInputText.Prompt(Minigame.DiggerButton);
            diggurInputText.Prompt(Minigame.DiggurButton);
        } else {
            diggerInputText.Prompt(Players.DiggerButton);
            diggurInputText.Prompt(Players.DiggurButton);
        }
    }

    private void SpawnMinigame() {
        Minigame = Instantiate(minigames[Random.Range(0, minigames.Length)]);
    }

    public void EndMinigame(bool won) {
        Destroy(Minigame.gameObject);
        Minigame = null;
        if (won)
            minigameDelay = Random.Range(minMinigameDelay, maxMinigameDelay);
        else
            minigameDelay = 1000;
    }

    public void Play() {
        Players = Instantiate(playersPrefab, new Vector3(0, 15, 0), Quaternion.identity);
        diggerInputText.target = Players.transform.Find("Digger").gameObject;
        diggurInputText.target = Players.transform.Find("Diggur").gameObject;
        mainMenu.SetActive(false);
    }

    public void PlayersLanded() {
        Ceiling.gameObject.SetActive(true);
        IsPlaying = true;
    }

    public void Pause() {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

    public void Resume() {
        pauseMenu.SetActive(false);
    }

    public void Lose() {
        defeatMenu.SetActive(true);
    }

    public void Restart() {
        SceneManager.LoadScene(0);
    }
}
