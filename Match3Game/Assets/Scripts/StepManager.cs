using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepManager : MonoBehaviour
{
  public Text stepCounterText;
  public int stepCounter;

  void Start()
  {
  
  }

  void Update()
  {
    stepCounterText.text = "" + stepCounter;
  }

  public void DecreaseCounter(int amountToIncrease)
  {
    stepCounter -= amountToIncrease;
  }
}
