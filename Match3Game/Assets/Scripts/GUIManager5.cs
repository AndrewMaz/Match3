using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUIManager5 : MonoBehaviour
{
  public GameObject board;
  public GameObject background;
  public GameObject backgroundGray;
  public GameObject gameOverPanel;
  public GameObject completedLevelPanel;
  public GameObject pauseButton;
  public GameObject levelInfo;
  public GameObject healthBar;
  public List<GameObject> GameLife;

  private bool isGameOver = false;
  private bool isLevelCompleted = false;
  private float firstBossTime = 0f;
  private float thirdBossTime = 0f;
  private float countTimeBoss1 = 0.25f;
  private float countTimeBoss3 = 0.25f;
  private bool isSecondBossLife = false;
  private bool isPoisonousSetted = false;
  private bool isColdSetted = false;
  static public bool isDeleteRandom = false;

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
    LevelManager.SetCompletedLevel(SceneManager.GetActiveScene().name);
    StartCoroutine(WaitForGameOver(1f));
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
    StartCoroutine(WaitForLevelCompleted(1f));
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
    if (healthBar.activeSelf && !isGameOver)
    {
      if (healthBar.GetComponent<HealthBar>().GetHealth() <= 0)
      {
        isLevelCompleted = true;
        LevelCompleted();
      }
    }

    if (board.activeSelf && !isLevelCompleted && !isGameOver && board.GetComponent<Board>().isBoardSetted)
    {
      if (board.GetComponent<Board>().currentState == GameState.move && board.GetComponent<Board>().IsDeadlock() &&
        !board.GetComponent<Board>().isMatchesOnBoard)
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
      }
    }

    if ((SceneManager.GetActiveScene().name == "lvl05") && board.activeSelf && !isLevelCompleted && !isGameOver)
    {
      firstBossTime += Time.deltaTime;

      if ((int)firstBossTime >= countTimeBoss1 && board.GetComponent<Board>().currentState == GameState.move)
      {
        board.GetComponent<Board>().currentState = GameState.wait;
        board.GetComponent<Board>().ShuffleBoard();
        countTimeBoss1 += 7;

        board.GetComponent<Board>().currentState = GameState.move;
      }
    }

    if ((SceneManager.GetActiveScene().name == "lvl10") && board.activeSelf && !isLevelCompleted && !isGameOver)
    {
      firstBossTime += Time.deltaTime;

      if ((int)firstBossTime >= countTimeBoss1 && board.GetComponent<Board>().currentState == GameState.move)
      {
        board.GetComponent<Board>().currentState = GameState.wait;
        board.GetComponent<Board>().SetPoisonousTiles();
        countTimeBoss1 += 20;

        board.GetComponent<Board>().currentState = GameState.move;
      }
    }

    if ((SceneManager.GetActiveScene().name == "lvl15") && board.activeSelf && !isLevelCompleted && !isGameOver)
    {
      firstBossTime += Time.deltaTime;

      if ((int)firstBossTime >= countTimeBoss1 && board.GetComponent<Board>().currentState == GameState.move)
      {
        board.GetComponent<Board>().currentState = GameState.wait;
        isDeleteRandom = true;
        board.GetComponent<Board>().DeleteRandomColorCall();
        countTimeBoss1 += 10;
      }
    }

    if ((SceneManager.GetActiveScene().name == "lvl20") && board.activeSelf && !isLevelCompleted && !isGameOver)
    {
      firstBossTime += Time.deltaTime;

      if ((int)firstBossTime >= countTimeBoss1 && board.GetComponent<Board>().currentState == GameState.move)
      {
        board.GetComponent<Board>().currentState = GameState.wait;
        board.GetComponent<Board>().SetColdTiles();
        countTimeBoss1 += 100;

        board.GetComponent<Board>().currentState = GameState.move;
      }
    }

    if ((SceneManager.GetActiveScene().name == "lvl25") && board.activeSelf && !isLevelCompleted && !isGameOver && board.GetComponent<Board>().isBoardSetted)
    {
      if (board.GetComponent<Board>().healthBoss > 60 && board.GetComponent<Board>().healthBoss <= 80)
      {
        firstBossTime += Time.deltaTime;

        if ((int)firstBossTime >= countTimeBoss1 && board.GetComponent<Board>().currentState == GameState.move)
        {
          board.GetComponent<Board>().currentState = GameState.wait;
          board.GetComponent<Board>().ShuffleBoard();
          countTimeBoss1 += 10;

          board.GetComponent<Board>().currentState = GameState.move;
        }
      }
      else if (board.GetComponent<Board>().healthBoss <= 60 && board.GetComponent<Board>().healthBoss > 40 && !isPoisonousSetted)
      {
        if (board.GetComponent<Board>().currentState == GameState.move)
        {
          board.GetComponent<Board>().currentState = GameState.wait;
          isPoisonousSetted = true;
          board.GetComponent<Board>().SetPoisonousTiles();

          board.GetComponent<Board>().currentState = GameState.move;
        }
      }
      else if (board.GetComponent<Board>().healthBoss > 20 && board.GetComponent<Board>().healthBoss <= 40)
      {
        thirdBossTime += Time.deltaTime;

        if ((int)thirdBossTime >= countTimeBoss3 && board.GetComponent<Board>().currentState == GameState.move)
        {
          board.GetComponent<Board>().currentState = GameState.wait;
          isDeleteRandom = true;
          board.GetComponent<Board>().DeleteRandomColorCall();
          countTimeBoss3 += 10;

          board.GetComponent<Board>().currentState = GameState.move;
        }
      }
      else if (board.GetComponent<Board>().healthBoss <= 20 && board.GetComponent<Board>().healthBoss > 0 && !isColdSetted)
      {
        if (board.GetComponent<Board>().currentState == GameState.move)
        {
          board.GetComponent<Board>().currentState = GameState.wait;
          isColdSetted = true;
          board.GetComponent<Board>().SetColdTiles();
 
          board.GetComponent<Board>().currentState = GameState.move;
        }
      }
      else if (board.GetComponent<Board>().healthBoss <= 0 && !isSecondBossLife)
      {
        board.GetComponent<Board>().healthBoss = 80;
        SoundManager.RegenBossSound();
        isSecondBossLife = true;
        isColdSetted = false;
        isPoisonousSetted = false;
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
    board.SetActive(false);
    healthBar.SetActive(false);
    background.SetActive(false);
    pauseButton.SetActive(false);
    levelInfo.SetActive(false);
    backgroundGray.SetActive(true);
  }
}