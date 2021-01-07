using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
  public void OnPause()
  {
    Time.timeScale = 0;
  }

  public void OffPause()
  {
    Time.timeScale = 1;
  }

  public void OffPauseAfterFerSeconds()
  {
    StartCoroutine(OffPauseAfterFerSecondsCo());
  }

  IEnumerator OffPauseAfterFerSecondsCo()
  {
    yield return new WaitForSeconds(0.5f);
    OffPause();
  }
}
