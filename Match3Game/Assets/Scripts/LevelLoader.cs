using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
  public Animator transition;

  public float transitionTime = 1f;

  public void LoadMenu()
  {
    StartCoroutine(LoadLevel("main"));
  }

  public void LoadLevelSelector1()
  {
    StartCoroutine(LoadLevel("levelSelector01"));
  }

  public void LoadLevelSelector2()
  {
    StartCoroutine(LoadLevel("levelSelector02"));
  }

  public void LoadLevelSelector3()
  {
    StartCoroutine(LoadLevel("levelSelector03"));
  }

  public void ReloadLevel()
  {
    StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
  }

  public void LoadNextLevel()
  {
    if (SceneManager.GetActiveScene().name == "main" && ComicBook.IsComicWasLoaded)
    {
      StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 3));
    }
    else
    {
      StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }
  }

  public void LoadPreviousLevel()
  {
    if (SceneManager.GetActiveScene().name == "levelSelector01" && ComicBook.IsComicWasLoaded)
    {
      StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 3));
    }
    else
    {
      StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 1));
    }
  }

  public void LoadLevel01()
  {
    StartCoroutine(LoadLevel("lvl01"));
  }

  public void LoadLevel02()
  {
    StartCoroutine(LoadLevel("lvl02"));
  }

  public void LoadLevel03()
  {
    StartCoroutine(LoadLevel("lvl03"));
  }

  public void LoadLevel04()
  {
    StartCoroutine(LoadLevel("lvl04"));
  }

  public void LoadLevel05()
  {
    StartCoroutine(LoadLevel("lvl05"));
  }

  public void LoadLevel06()
  {
    StartCoroutine(LoadLevel("lvl06"));
  }

  public void LoadLevel07()
  {
    StartCoroutine(LoadLevel("lvl07"));
  }

  public void LoadLevel08()
  {
    StartCoroutine(LoadLevel("lvl08"));
  }

  public void LoadLevel09()
  {
    StartCoroutine(LoadLevel("lvl09"));
  }

  public void LoadLevel10()
  {
    StartCoroutine(LoadLevel("lvl10"));
  }

  public void LoadLevel11()
  {
    StartCoroutine(LoadLevel("lvl11"));
  }

  public void LoadLevel12()
  {
    StartCoroutine(LoadLevel("lvl12"));
  }

  public void LoadLevel13()
  {
    StartCoroutine(LoadLevel("lvl13"));
  }

  public void LoadLevel14()
  {
    StartCoroutine(LoadLevel("lvl14"));
  }

  public void LoadLevel15()
  {
    StartCoroutine(LoadLevel("lvl15"));
  }

  public void LoadLevel16()
  {
    StartCoroutine(LoadLevel("lvl16"));
  }

  public void LoadLevel17()
  {
    StartCoroutine(LoadLevel("lvl17"));
  }

  public void LoadLevel18()
  {
    StartCoroutine(LoadLevel("lvl18"));
  }

  public void LoadLevel19()
  {
    StartCoroutine(LoadLevel("lvl19"));
  }

  public void LoadLevel20()
  {
    StartCoroutine(LoadLevel("lvl20"));
  }

  public void LoadLevel21()
  {
    StartCoroutine(LoadLevel("lvl21"));
  }

  public void LoadLevel22()
  {
    StartCoroutine(LoadLevel("lvl22"));
  }

  public void LoadLevel23()
  {
    StartCoroutine(LoadLevel("lvl23"));
  }

  public void LoadLevel24()
  {
    StartCoroutine(LoadLevel("lvl24"));
  }

  public void LoadLevel25()
  {
    StartCoroutine(LoadLevel("lvl25"));
  }

  IEnumerator LoadLevel(int levelIndex)
  {
    transition.SetTrigger("Start");
    yield return new WaitForSeconds(transitionTime);

    SceneManager.LoadScene(levelIndex);
  }

  IEnumerator LoadLevel(string levelIndex)
  {
    transition.SetTrigger("Start");

    yield return new WaitForSeconds(transitionTime);

    SceneManager.LoadScene(levelIndex);
  }

  private void Update()
  {
    if (SceneManager.GetActiveScene().name == "ComicBook1")
    {
      ComicBook.isComicFirstLoaded = true;
    }

    if (SceneManager.GetActiveScene().name == "ComicBook2")
    {
      ComicBook.isComicSecondLoaded = true;
    }

    if (ComicBook.isComicFirstLoaded && ComicBook.isComicSecondLoaded)
    {
      ComicBook.IsComicWasLoaded = true;
    }

    if (Input.GetKey(KeyCode.Escape))
    {
      if (SceneManager.GetActiveScene().name == "levelSelector01" || SceneManager.GetActiveScene().name == "levelSelector02" ||
      SceneManager.GetActiveScene().name == "levelSelector03")
      {
        LoadMenu();
      }
      else if (SceneManager.GetActiveScene().name == "lvl01" || SceneManager.GetActiveScene().name == "lvl02" ||
      SceneManager.GetActiveScene().name == "lvl03" || SceneManager.GetActiveScene().name == "lvl04" ||
      SceneManager.GetActiveScene().name == "lvl05")
      {
        LoadLevelSelector1();
      }
      else if (SceneManager.GetActiveScene().name == "lvl06" || SceneManager.GetActiveScene().name == "lvl07" ||
      SceneManager.GetActiveScene().name == "lvl08" || SceneManager.GetActiveScene().name == "lvl09" ||
      SceneManager.GetActiveScene().name == "lvl10" || SceneManager.GetActiveScene().name == "lvl11" ||
      SceneManager.GetActiveScene().name == "lvl12" || SceneManager.GetActiveScene().name == "lvl13" ||
      SceneManager.GetActiveScene().name == "lvl14" || SceneManager.GetActiveScene().name == "lvl15" ||
      SceneManager.GetActiveScene().name == "lvl16" || SceneManager.GetActiveScene().name == "lvl17" ||
      SceneManager.GetActiveScene().name == "lvl18" || SceneManager.GetActiveScene().name == "lvl19" ||
      SceneManager.GetActiveScene().name == "lvl20")
      {
        LoadLevelSelector2();
      }
      else if (SceneManager.GetActiveScene().name == "lvl21" || SceneManager.GetActiveScene().name == "lvl22" ||
      SceneManager.GetActiveScene().name == "lvl23" || SceneManager.GetActiveScene().name == "lvl24" ||
      SceneManager.GetActiveScene().name == "lvl25")
      {
        LoadLevelSelector3();
      }
    }
  }
}
