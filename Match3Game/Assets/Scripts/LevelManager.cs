using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
  private static Dictionary<string, bool> levelManager = new Dictionary<string, bool>();

  private static void GetLevelManager()
  {
    if (levelManager.Count != 0)
    {
      return;
    }

    levelManager.Add("lvl01", true);
    levelManager.Add("lvl02", false);
    levelManager.Add("lvl03", false);
    levelManager.Add("lvl04", false);
    levelManager.Add("lvl05", false);
    levelManager.Add("lvl06", false);
    levelManager.Add("lvl07", false);
    levelManager.Add("lvl08", false);
    levelManager.Add("lvl09", false);
    levelManager.Add("lvl10", false);
    levelManager.Add("lvl11", false);
    levelManager.Add("lvl12", false);
    levelManager.Add("lvl13", false);
    levelManager.Add("lvl14", false);
    levelManager.Add("lvl15", false);
    levelManager.Add("lvl16", false);
    levelManager.Add("lvl17", false);
    levelManager.Add("lvl18", false);
    levelManager.Add("lvl19", false);
    levelManager.Add("lvl20", false);
    levelManager.Add("lvl21", false);
    levelManager.Add("lvl22", false);
    levelManager.Add("lvl23", false);
    levelManager.Add("lvl24", false);
    levelManager.Add("lvl25", false);
  }

  private void Start()
  {
    GetLevelManager();
  }

  private void Update()
  {
    if (levelManager[tag] == false)
    {
      this.GetComponent<Button>().enabled = false;
      this.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
    }
  }

  public static void SetCompletedLevel(string levelName)
  {
    switch (levelName)
    {
      case "lvl01":
        levelManager["lvl02"] = true;
        break;
      case "lvl02":
        levelManager["lvl03"] = true;
        break;
      case "lvl03":
        levelManager["lvl04"] = true;
        break;
      case "lvl04":
        levelManager["lvl05"] = true;
        break;
      case "lvl05":
        levelManager["lvl06"] = true;
        break;
      case "lvl06":
        levelManager["lvl07"] = true;
        break;
      case "lvl07":
        levelManager["lvl08"] = true;
        break;
      case "lvl08":
        levelManager["lvl09"] = true;
        break;
      case "lvl09":
        levelManager["lvl10"] = true;
        break;
      case "lvl10":
        levelManager["lvl11"] = true;
        break;
      case "lvl11":
        levelManager["lvl12"] = true;
        break;
      case "lvl12":
        levelManager["lvl13"] = true;
        break;
      case "lvl13":
        levelManager["lvl14"] = true;
        break;
      case "lvl14":
        levelManager["lvl15"] = true;
        break;
      case "lvl15":
        levelManager["lvl16"] = true;
        break;
      case "lvl16":
        levelManager["lvl17"] = true;
        break;
      case "lvl17":
        levelManager["lvl18"] = true;
        break;
      case "lvl18":
        levelManager["lvl19"] = true;
        break;
      case "lvl19":
        levelManager["lvl20"] = true;
        break;
      case "lvl20":
        levelManager["lvl21"] = true;
        break;
      case "lvl21":
        levelManager["lvl22"] = true;
        break;
      case "lvl22":
        levelManager["lvl23"] = true;
        break;
      case "lvl23":
        levelManager["lvl24"] = true;
        break;
      case "lvl24":
        levelManager["lvl25"] = true;
        break;
    }
  }
}
