using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Currently selected zombie
    public GameObject selectedZombie;

    // All zombies in the scene
    public GameObject[] zombies;

    // Input actions for selecting and pushing zombies
    private InputAction next, previous, push;

    // Index of the currently selected zombie
    private int selectedID = 0;

    // Scale for the selected zombie and force used for pushing upward
    public Vector3 selectedScale, pushForce;

    [Header("Collectibles")]
    // Collectible objects in the scene
    public GameObject[] collectibles;

    // Delay before a collected cube appears again
    public float collectibleRespawnDelay = 2f;

    [Header("Score")]
    public int score = 0;
    public TMP_Text scoreText;

    [Header("Timer")]
    public TMP_Text timerText;
    private float elapsedTime = 0f;

    [Header("Lose State")]
    public GameObject losePanel;
    public TMP_Text loseText;
    public GameObject restartButton;
    public float fallYThreshold = -10f;

    private bool gameOver = false;

    // Sets up input, UI, and the first selected zombie.
    void Start()
    {
        next = InputSystem.actions.FindAction("Player/Next");
        previous = InputSystem.actions.FindAction("Player/Previous");
        push = InputSystem.actions.FindAction("Player/Push");

        if (losePanel != null)
            losePanel.SetActive(false);

        if (restartButton != null)
            restartButton.SetActive(false);

        UpdateScoreUI();
        UpdateTimerUI();

        SelectZombie(0);
    }

    // Selects a zombie by index and updates its visual scale.
    private void SelectZombie(int id)
    {
        selectedID = id;

        if (selectedZombie != null)
            selectedZombie.transform.localScale = Vector3.one;

        selectedZombie = zombies[id];
        selectedZombie.transform.localScale = selectedScale;
    }

    // Handles timer updates, zombie selection, pushing, and lose checking.
    void Update()
    {
        if (gameOver)
            return;

        elapsedTime += Time.deltaTime;
        UpdateTimerUI();
        CheckLoseCondition();

        if (push.WasPressedThisFrame())
        {
            Rigidbody rb = selectedZombie.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddForce(pushForce, ForceMode.Impulse);
        }

        if (next.WasPressedThisFrame())
        {
            selectedID++;
            if (selectedID >= zombies.Length)
                selectedID = 0;
            SelectZombie(selectedID);
        }

        if (previous.WasPressedThisFrame())
        {
            selectedID--;
            if (selectedID < 0)
                selectedID = zombies.Length - 1;
            SelectZombie(selectedID);
        }
    }

    // Adds score when a collectible is collected and hides that collectible temporarily.
    public void CollectCollectible(GameObject collectibleObject, int points)
    {
        if (gameOver)
            return;

        score += points;
        UpdateScoreUI();

        collectibleObject.SetActive(false);
        StartCoroutine(RespawnCollectible(collectibleObject));
    }

    // Waits a bit and shows the collectible again.
    IEnumerator RespawnCollectible(GameObject collectibleObject)
    {
        yield return new WaitForSeconds(collectibleRespawnDelay);

        if (!gameOver && collectibleObject != null)
            collectibleObject.SetActive(true);
    }

    // Refreshes the score text on screen.
    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    // Refreshes the timer text on screen.
    void UpdateTimerUI()
    {
        if (timerText != null)
            timerText.text = "Time: " + elapsedTime.ToString("0.0");
    }

    // Checks whether all zombies have fallen off the slope.
    void CheckLoseCondition()
    {
        bool allFallen = true;

        for (int i = 0; i < zombies.Length; i++)
        {
            if (zombies[i] != null && zombies[i].transform.position.y > fallYThreshold)
            {
                allFallen = false;
                break;
            }
        }

        if (allFallen)
            LoseGame();
    }

    // Activates the lose state and stops gameplay.
    void LoseGame()
    {
        gameOver = true;
        StopAllCoroutines();

        if (losePanel != null)
            losePanel.SetActive(true);

        if (loseText != null)
            loseText.text = "You lose";

        if (restartButton != null)
            restartButton.SetActive(true);
    }

    // Restarts the current scene.
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
