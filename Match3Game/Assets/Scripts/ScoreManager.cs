using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
  public Text scoreText;
  public int score;
   
  void Start()
  {
    score = 0;
  }

  void Update()
  {
    scoreText.text = "" + score;
  }

  public void InscreaseScore(int amountToIncrease)
  {
    score += amountToIncrease;
  }
}
