using System.Collections;
using System.Collections.Generic;
using TapticPlugin;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("GameManager").AddComponent<GameManager>();
            }

            return instance;
        }
    }

    public PlayerController playerController;

    public int currentLevel = 1;
    int MaxLevelNumber = 10;
    public bool isGameStarted, isGameOver;

    #region UI Elements
    public GameObject WinPanel, LosePanel, InGamePanel;
    public Button TapToStartButton;
    public Text LevelText, CountText;
    public GameObject PlayText, ContinueText;
    public GameObject Tutorial1, Tutorial2;
    #endregion

    private void Awake()
    {
        Application.targetFrameRate = 60;

        PlayerPrefs.SetInt("FromMenu", 1);
        if (PlayerPrefs.GetInt("FromMenu") == 1)
        {
            ContinueText.SetActive(false);
            PlayerPrefs.SetInt("FromMenu", 0);
        }
        else
        {
            PlayText.SetActive(false);
        }
        currentLevel = PlayerPrefs.GetInt("LevelId");
        LevelText.text = "LEVEL " + currentLevel.ToString();
        CountText.text = burgerCount.ToString();
    }

    private void OnEnable()
    {
        instance = this;
    }

    public IEnumerator WaitAndGameWin()
    {
        Debug.Log("Win");
        isGameOver = true;
        SoundManager.Instance.StopAllSounds();
        SoundManager.Instance.playSound(SoundManager.GameSounds.Win);

        yield return new WaitForSeconds(1f);

        if (PlayerPrefs.GetInt("VIBRATION") == 1)
            TapticManager.Impact(ImpactFeedback.Light);

        currentLevel++;
        PlayerPrefs.SetInt("LevelId", currentLevel);
        WinPanel.SetActive(true);
    }

    public IEnumerator WaitAndGameLose()
    {
        Debug.Log("Lose");
        isGameOver = true;
        SoundManager.Instance.playSound(SoundManager.GameSounds.Lose);

        yield return new WaitForSeconds(1f);

        if (PlayerPrefs.GetInt("VIBRATION") == 1)
            TapticManager.Impact(ImpactFeedback.Medium);

        LosePanel.SetActive(true);
    }

    public void TapToNextButtonClick()
    {
        if (currentLevel > MaxLevelNumber)
        {
            int rand = Random.Range(1, MaxLevelNumber);
            if (rand == PlayerPrefs.GetInt("LastRandomLevel"))
            {
                rand = Random.Range(1, MaxLevelNumber);
            }
            else
            {
                PlayerPrefs.SetInt("LastRandomLevel", rand);
            }
            SceneManager.LoadScene("Level" + rand);
        }
        else
        {
            SceneManager.LoadScene("Level" + currentLevel);
        }
    }

    public void TapToTryAgainButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TapToStartButtonClick()
    {
        isGameStarted = true;
        TapToStartButton.gameObject.SetActive(false);
        playerController.animator.SetBool("isTurning", false);
        if (currentLevel == 1)
        {
            Tutorial1.SetActive(true);
        }
    }

    public int burgerCount;
    public void AddBurger()
    {
        burgerCount++;
        CountText.text = burgerCount.ToString();
    }
}
