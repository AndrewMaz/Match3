﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUIManager4 : MonoBehaviour
{
  public GameObject board;
  public GameObject background;
  public GameObject backgroundGray;
  public GameObject gameOverPanel;
  public GameObject completedLevelPanel;
  public GameObject pauseButton;
  public GameObject levelInfo;
  public GameObject scoreManager;
  public GameObject scoreBoard;
  public GameObject stepManager;
  public GameObject stepBoard;
  public Text scoreGameOver;
  public Text scoreLevelComplete;
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
    scoreGameOver.text = scoreManager.GetComponent<ScoreManager>().scoreText.text;
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
    scoreLevelComplete.text = (10 - stepManager.GetComponent<StepManager>().stepCounter).ToString();
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
    if (scoreManager.activeSelf && !isGameOver)
    {
      if (scoreManager.GetComponent<ScoreManager>().score >= 1000)
      {
        isLevelCompleted = true;
        LevelCompleted();
      }
    }

    if (stepManager.activeSelf && !isLevelCompleted)
    {
      if (stepManager.GetComponent<StepManager>().stepCounter <= 0 && scoreManager.GetComponent<ScoreManager>().score < 1000)
      {
        isGameOver = true;
        GameOver();
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
    background.SetActive(false);
    pauseButton.SetActive(false);
    levelInfo.SetActive(false);
    stepBoard.SetActive(false);
    scoreBoard.SetActive(false);
    backgroundGray.SetActive(true);
  }
}
