using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NutCounterManager : MonoBehaviour
{
  public Text nutCounterText;
  public int nutCounter;

  void Start()
  {

  }

  void Update()
  {
    nutCounterText.text = "" + nutCounter;
  }

  public void DecreaseCounter(int amountToIncrease)
  {
    nutCounter -= amountToIncrease;
  }
}
