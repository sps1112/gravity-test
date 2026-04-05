using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool canInput = true;

    private bool toCount = false;

    [Header("Timer")]

    [Tooltip("Time to be mentioned in seconds")]
    [SerializeField] private int timeLimit = 120;

    private float timeLeft;

    [Header("UI References")]

    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private GameObject gameOverUI;

    [SerializeField] private GameObject gameWinUI;

    [Header("Collectible List")]

    [Tooltip("Reference to the gameobject of every collectible cube")]
    [SerializeField] private List<GameObject> collectibles = new List<GameObject>();

    void Start()
    {
        canInput = true;
        toCount = true;
        timeLeft = timeLimit;
        UpdateTimeUI(timeLeft);
    }

    // Updates the Timer UI in Minutes:Seconds format
    private void UpdateTimeUI(float timeToUse)
    {
        float timeLeftMinutes = (int)Mathf.Floor(timeToUse / 60.0f);
        float timeLeftSeconds = (int)(timeToUse - timeLeftMinutes * 60.0f);

        string minuteString = (timeLeftMinutes < 10) ? ("0" + timeLeftMinutes.ToString()) : timeLeftMinutes.ToString();
        string secondsString = (timeLeftSeconds < 10) ? ("0" + timeLeftSeconds.ToString()) : timeLeftSeconds.ToString();
        timerText.text = minuteString + ":" + secondsString;
    }

    // Stops the Timer and Disables the UI at game over/win
    private void StopTimerWithUI()
    {
        toCount = false;
        canInput = false;
        timerText.transform.parent.gameObject.SetActive(false);
    }

    // GameOver at Time exceeded or Free fall
    public void GameOver()
    {
        StopTimerWithUI();
        gameOverUI.SetActive(true);
    }

    // Updatese the score for collectible
    public void UpdateScore(GameObject cube)
    {
        if (collectibles.Contains(cube))
        {
            collectibles.Remove(cube);
            collectibles.TrimExcess();
        }
        if (collectibles.Count == 0)
        {
            StopTimerWithUI();
            gameWinUI.SetActive(true);
        }
    }

    // Replay function which reloads the scene. Called by UI buttons
    public void Replay()
    {
        SceneManager.LoadScene(0);
    }

    void Update()
    {
        if (toCount)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimeUI(timeLeft + 1.0f);
            if (timeLeft <= 0.0f)
            {
                timeLeft = 0.0f;
                UpdateTimeUI(timeLeft);
                GameOver();
            }
        }
    }
}
