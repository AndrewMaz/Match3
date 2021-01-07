using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
  public Slider slider;
  public Gradient gradient;
  public Image fill;
  private Board board;

  private void Start()
  {
    board = FindObjectOfType<Board>();
    SetMaxHealth(board.healthBoss);
  }
  public void SetMaxHealth(int health)
  {
    slider.maxValue = health;
    slider.value = health;

    fill.color = gradient.Evaluate(40f);
  }

  public void SetHealth(int health)
  {
    if (SceneManager.GetActiveScene().name != "lvl25")
    {
      if (health >= 40)
      {
        health = 40;
      }
    }
    else
    {
      if (health >= 80)
      {
        health = 80;
      }
    }

    slider.value = health;
    fill.color = gradient.Evaluate(slider.normalizedValue);
  }

  public float GetHealth()
  {
    return slider.value;
  }

  private void Update()
  {
    SetHealth(board.healthBoss);
  }
}
