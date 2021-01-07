using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUIManager2 : MonoBehaviour
{
  public GameObject board;
  public GameObject background;
  public GameObject backgroundGray;
  public GameObject gameOverPanel;
  public GameObject completedLevelPanel;
  public GameObject pauseButton;
  public GameObject levelInfo;
  public GameObject nutCounterManager;
  public GameObject scoreBoard;
  public Text scoreGameOver;
  public GameObject timePanel;
  public GameObject timeManager;
  public List<GameObject> GameLife;

  private bool isGameOver = false;
  private bool isLevelCompleted = false;

  public void OnPause()
  {
    Time.timeScale = 0;
  }

  public void OffPause()
  {
    Time.timeScale = 1;
  }

  private void GameOver()
  {
    board.GetComponent<Board>().currentState = GameState.wait;
    StartCoroutine(WaitForGameOver(2f));
  }

  private IEnumerator WaitForGameOver(float timeDelay)
  {
    yield return new WaitForSeconds(timeDelay);
    gameOverPanel.SetActive(true);
    ChangePanel();
  }

  private void LevelCompleted()
  {
    board.GetComponent<Board>().currentState = GameState.wait;
    LevelManager.SetCompletedLevel(SceneManager.GetActiveScene().name);
    StartCoroutine(WaitForLevelCompleted(2f));
  }

  private IEnumerator WaitForLevelCompleted(float timeDelay)
  {
    yield return new WaitForSeconds(timeDelay);
    completedLevelPanel.SetActive(true);
    ChangePanel();
  }

  private void Start()
  {

  }

  private void Update()
  {
    if (nutCounterManager.activeSelf && !isGameOver)
    {
      if (nutCounterManager.GetComponent<NutCounterManager>().nutCounter <= 0)
      {
        nutCounterManager.GetComponent<NutCounterManager>().nutCounter = 0;
        isLevelCompleted = true;
        LevelCompleted();
      }
    }

    if (timeManager.activeSelf && !isLevelCompleted)
    {
      if (timeManager.GetComponent<TimeManager>().levelTime <= 0f)
      {
        timeManager.GetComponent<TimeManager>().levelTime = 0;
        isGameOver = true;
        GameOver();
        scoreGameOver.text = nutCounterManager.GetComponent<NutCounterManager>().nutCounter.ToString();
      }
    }

    if (board.activeSelf && !isLevelCompleted && !isGameOver)
    {
      if (!board.GetComponent<Board>().isMatchesOnBoard)
      {
        board.GetComponent<Board>().ChangeBoard();
        Destroy(GameLife[GameLife.Count - 1]);

        if (GameLife.Count != 1)
        {
          GameLife.RemoveAt(GameLife.Count - 1);
        }
        else
        {
          isGameOver = true;
          GameOver();
        }

        scoreGameOver.text = nutCounterManager.GetComponent<NutCounterManager>().nutCounter.ToString();
      }
    }
  }

  private void ChangePanel()
  {
    foreach (var i in GameLife)
    {
      if (i != null)
      {
        i.SetActive(false);
      }
    }
    timePanel.SetActive(false);
    board.SetActive(false);
    background.SetActive(false);
    pauseButton.SetActive(false);
    levelInfo.SetActive(false);
    scoreBoard.SetActive(false);
    backgroundGray.SetActive(true);
  }
}
