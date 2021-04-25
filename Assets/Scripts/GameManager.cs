using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [SerializeField] public InputText diggerInputText, dieggerInputText;
    [SerializeField] private GameObject pauseMenu, defeatMenu;
    [SerializeField, Min(0)] private float minMinigameDelay = 3, maxMinigameDelay = 10;
    [SerializeField] private RockMinigame[] minigames;

    private float minigameDelay;

    public Players Players { get; private set; }
    public Floor Floor { get; private set; }
    public Ceiling Ceiling { get; private set; }

    private RockMinigame minigame;
    public bool InMinigame => minigame != null;

    public bool InDigPhase => minigame == null;

    private void Awake() {
        Players = FindObjectOfType<Players>();
        Floor = FindObjectOfType<Floor>();
        Ceiling = FindObjectOfType<Ceiling>();
        minigameDelay = Random.Range(3f, 6f);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();

        minigameDelay -= Time.deltaTime;
        if (InDigPhase && minigameDelay < 0)
            SpawnMinigame();

        if (minigame != null) {
            diggerInputText.Prompt(true, minigame.DiggerButton);
            dieggerInputText.Prompt(false, minigame.DieggerButton);
        } else {
            diggerInputText.Prompt(true, Players.DiggerButton);
            dieggerInputText.Prompt(false, Players.DieggerButton);
        }
    }

    private void SpawnMinigame() {
        minigame = Instantiate(minigames[Random.Range(0, minigames.Length)]);
    }

    public void EndMinigame(bool won) {
        Destroy(minigame.gameObject);
        minigame = null;
        minigameDelay = Random.Range(minMinigameDelay, maxMinigameDelay);
    }

    public void Pause() {
        pauseMenu.SetActive(true);
    }

    public void Lose() {
        defeatMenu.SetActive(true);
    }

    public void Restart() {
        SceneManager.LoadScene(0);
    }
}
