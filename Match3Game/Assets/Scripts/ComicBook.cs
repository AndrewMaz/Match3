using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicBook : MonoBehaviour
{
  private static bool isComicWasLoaded;
  public static bool isComicFirstLoaded;
  public static bool isComicSecondLoaded;

  private static void Update()
  {
    if (isComicFirstLoaded && isComicSecondLoaded)
    {
      IsComicWasLoaded = true;
    }
  }

  public static bool IsComicWasLoaded
  {
    get
    {
      return isComicWasLoaded;
    }
    set
    {
      isComicWasLoaded = value;
    }
  }
}
