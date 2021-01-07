using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
  public float levelTime;
  public Text TimeForLevel;

  void Start()
  {

  }

  void Update()
  {
    levelTime -= Time.deltaTime;
    TimeForLevel.text = NumberToTime(levelTime);
  }

  private string NumberToTime(float currentTime)
  {
    int currentMin = (int)(currentTime / 60);
    int currentSec = (int)(currentTime - 60 * currentMin);
    if (currentSec >= 10)
    {
      return ("" + currentMin + ":" + currentSec);
    }
    else
    {
      return ("" + currentMin + ":0" + currentSec);
    }
  }
}
