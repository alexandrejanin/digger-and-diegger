using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [SerializeField] private AudioClip music, musicLoop;
    [SerializeField] private Walls wallsPrefab;
    [SerializeField] public Players playersPrefab;
    [SerializeField] public InputText diggerInputText, diggurInputText;
    [SerializeField] private GameObject mainMenu, optionsMenu, pauseMenu, defeatMenu;
    [SerializeField, Min(0)] private float minMinigameDelay = 3, maxMinigameDelay = 10;
    [SerializeField] private Minigame[] minigames;

    [SerializeField] private float moleMinDepth = 30;
    [SerializeField] private float moleChance = 0.4f;

    private float minigameDelay;

    public Players Players { get; private set; }
    public Floor Floor { get; private set; }
    public Ceiling Ceiling { get; private set; }

    public int Score => -Mathf.FloorToInt(Floor.transform.position.y);

    public Minigame Minigame { get; private set; }
    public bool InMinigame => IsPlaying && Minigame != null;

    public bool InDigPhase => IsPlaying && Minigame == null;

    public bool IsPlaying { get; private set; }

    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        Floor = FindObjectOfType<Floor>(true);
        Ceiling = FindObjectOfType<Ceiling>(true);
        minigameDelay = Random.Range(3f, 6f);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();

        Time.timeScale = pauseMenu.activeInHierarchy ? 0 : 1;

        if (!IsPlaying)
            return;

        minigameDelay -= Time.deltaTime;
        if (InDigPhase && minigameDelay < 0)
            SpawnMinigame();

        if (!IsPlaying || Players.IsStunned) {
            diggerInputText.Prompt(null);
            diggurInputText.Prompt(null);
        } else if (Minigame != null) {
            diggerInputText.Prompt(Minigame.DiggerButton);
            diggurInputText.Prompt(Minigame.DiggurButton);
        } else {
            diggerInputText.Prompt(Players.DiggerButton);
            diggurInputText.Prompt(Players.DiggurButton);
        }
    }

    private void SpawnMinigame() {
        Minigame = Instantiate(Score < moleMinDepth
            ? minigames[0]
            : Random.value < moleChance
                ? minigames[1]
                : minigames[0]
        );
    }

    public void EndMinigame(bool won) {
        if (Minigame)
            Destroy(Minigame.gameObject);
        Minigame = null;
        if (won)
            minigameDelay = Random.Range(minMinigameDelay, maxMinigameDelay);
        else
            StartLose();
    }

    public void Play() {
        if (audioSource.clip != music) {
            audioSource.Stop();
            audioSource.clip = music;
            audioSource.Play();
        }

        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        defeatMenu.SetActive(false);
        Players = Instantiate(playersPrefab, new Vector3(0, 15, 0), Quaternion.identity);
        diggerInputText.target = Players.transform.Find("Digger").gameObject;
        diggurInputText.target = Players.transform.Find("Diggur").gameObject;
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

    public void StartLose() {
        IsPlaying = false;
        Ceiling.Fall();
        Players.Die();
    }

    public void EndLose() {
        Destroy(Players.gameObject);

        if (Minigame)
            Destroy(Minigame.gameObject);

        Minigame = null;
        defeatMenu.SetActive(true);
    }

    public void Restart() {
        Floor.transform.position = Vector3.zero;
        Floor.Depth = 0;
        Ceiling.transform.position = new Vector3(0.5f, 12, 0);
        Ceiling.gameObject.SetActive(false);
        Ceiling.GetComponent<Rumble>().enabled = true;
        Destroy(Ceiling.GetComponent<Rigidbody>());

        foreach (var walls in FindObjectsOfType<Walls>())
            Destroy(walls.gameObject);

        Instantiate(wallsPrefab, 17.03f * Vector3.up, Quaternion.identity);

        FindObjectOfType<Camera>().transform.position = new Vector3(0, 2, -30);
        FindObjectOfType<Camera>().fieldOfView = 35;

        Play();
    }

    public void GoToMainMenu() {
        SceneManager.LoadScene(0);
    }

    public void Quit() {
        Application.Quit();
    }

    public void ToggleOptions() {
        mainMenu.SetActive(!mainMenu.activeSelf);
        optionsMenu.SetActive(!optionsMenu.activeSelf);
    }

    public void OpenItchioPage() {
        Application.OpenURL("https://bad-rng-studios.itch.io/");
    }
}
