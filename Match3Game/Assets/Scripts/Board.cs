using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
  wait,
  move
}

public enum TileKind
{
  Breakable,
  Blank,
  Normal
}

[System.Serializable]
public class TileType
{
  public int x;
  public int y;
  public TileKind tileKind;
}

public class Board : MonoBehaviour
{
  public GameState currentState = GameState.move;
  public int width;
  public int height;
  public int offSet;
  public bool isMatchesOnBoard = true;
  public GameObject tilePrefab;
  public GameObject destroyTile;
  public GameObject poisonousTile;
  public GameObject coldTile;
  public GameObject wall;
  public GameObject[] dots;
  public TileType[] boardLayout;
  private bool[,] blankSpaces;
  private bool isGeneratedBlankSpaces = false;
  public GameObject[,] allDots;
  private FindMatches findMatches;

  public int basePieceValue = 20;
  private int streakValue = 1;
  public int healthBoss = 40;
  public bool isBoardSetted = false;
  private ScoreManager scoreManager;
  private NutCounterManager nutCounterManager;

  public float refillDelay = 0.5f;  

  void Start()
  {
    scoreManager = FindObjectOfType<ScoreManager>();
    nutCounterManager = FindObjectOfType<NutCounterManager>();
    findMatches = FindObjectOfType<FindMatches>();
    blankSpaces = new bool[width, height];
    allDots = new GameObject[width, height];

    if (SceneManager.GetActiveScene().name == "lvl05" || SceneManager.GetActiveScene().name == "lvl10" ||
      SceneManager.GetActiveScene().name == "lvl15" || SceneManager.GetActiveScene().name == "lvl20")
    {
      healthBoss = 40;
    }
    
    if (SceneManager.GetActiveScene().name == "lvl25")
    {
      healthBoss = 80;
    }

    SetUp();
  }

  public void GenerateBlankSpaces()
  {
    for (int i = 0; i < boardLayout.Length; i += 2)
    {
      boardLayout[i].x = Random.Range(0, width - 1);
      boardLayout[i].y = Random.Range(0, height - 1);
      boardLayout[i].tileKind = TileKind.Blank;

      Vector2 currentWay = SelectVector(new Vector2(boardLayout[i].x, boardLayout[i].y));

      boardLayout[i + 1].x = (int)currentWay.x;
      boardLayout[i + 1].y = (int)currentWay.y;
      boardLayout[i + 1].tileKind = TileKind.Blank;

      if (!blankSpaces[boardLayout[i].x, boardLayout[i].y] && !blankSpaces[boardLayout[i + 1].x, boardLayout[i + 1].y])
      {
        blankSpaces[boardLayout[i].x, boardLayout[i].y] = true;
        blankSpaces[boardLayout[i + 1].x, boardLayout[i + 1].y] = true;

        Vector2 wallPosition1 = new Vector2(boardLayout[i].x, boardLayout[i].y);
        Vector2 wallPosition2 = new Vector2(boardLayout[i + 1].x, boardLayout[i + 1].y);
        GameObject currentWall1 = Instantiate(wall, wallPosition1, Quaternion.identity);
        GameObject currentWall2 = Instantiate(wall, wallPosition2, Quaternion.identity);
      }
    }
  }

  private void SetUp()
  {
    if ((SceneManager.GetActiveScene().name == "lvl03" || SceneManager.GetActiveScene().name == "lvl08" || 
      SceneManager.GetActiveScene().name == "lvl13" || SceneManager.GetActiveScene().name == "lvl18" ||
      SceneManager.GetActiveScene().name == "lvl23") && !isGeneratedBlankSpaces)
    {
      GenerateBlankSpaces();
      isGeneratedBlankSpaces = true;
    }

    for (int i = 0; i < width; i++)
    {
      for (int j = 0; j < height; j++)
      {
        if (!blankSpaces[i, j])
        {
          Vector2 tempPosition = new Vector2(i, j + offSet);
          GameObject backgroundTile = Instantiate(tilePrefab, tempPosition, Quaternion.identity) as GameObject;
          backgroundTile.transform.parent = this.transform;
          backgroundTile.name = "( " + i + ";" + j + " )";
          int dotToUse = Random.Range(0, dots.Length);
          int maxIterators = 0;
          while (MatchesAt(i, j, dots[dotToUse]) && maxIterators < 100)
          {
            dotToUse = Random.Range(0, dots.Length);
            maxIterators++;
          }
          maxIterators = 0;

          GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
          dot.GetComponent<Dot>().row = j;
          dot.GetComponent<Dot>().column = i;
          dot.transform.parent = this.transform;
          dot.name = "( " + i + ";" + j + " )";

          dot.GetComponent<Dot>().isUsed = true;
          allDots[i, j] = dot;
        }
      }
    }

    isBoardSetted = true;
  }

  Vector2 SelectVector(Vector2 currentPos)
  {
    if (currentPos.x > 0 && currentPos.x < width - 1 && currentPos.y > 0 && currentPos.y < height - 1)
    {
      Vector2[] arrayVec = { Vector2.up, Vector2.down, Vector2.right, Vector2.left };
      Vector2 currentWay = arrayVec[Random.Range(0, arrayVec.Length)];
      return new Vector2(currentPos.x + currentWay.x, currentPos.y + currentWay.y);
    }
    else if (currentPos.x > 0 && currentPos.x < width - 1 && currentPos.y == 0)
    {
      Vector2[] arrayVec = { Vector2.up, Vector2.right, Vector2.left };
      Vector2 currentWay = arrayVec[Random.Range(0, arrayVec.Length)];
      return new Vector2(currentPos.x + currentWay.x, currentPos.y + currentWay.y);
    }
    else if (currentPos.x > 0 && currentPos.x < width - 1 && currentPos.y == height - 1)
    {
      Vector2[] arrayVec = { Vector2.down, Vector2.right, Vector2.left };
      Vector2 currentWay = arrayVec[Random.Range(0, arrayVec.Length)];
      return new Vector2(currentPos.x + currentWay.x, currentPos.y + currentWay.y);
    }
    else if (currentPos.y > 0 && currentPos.y < height - 1 && currentPos.x == 0)
    {
      Vector2[] arrayVec = { Vector2.up, Vector2.down, Vector2.right };
      Vector2 currentWay = arrayVec[Random.Range(0, arrayVec.Length)];
      return new Vector2(currentPos.x + currentWay.x, currentPos.y + currentWay.y);
    }
    else if (currentPos.y > 0 && currentPos.y < height - 1 && currentPos.x == width - 1)
    {
      Vector2[] arrayVec = { Vector2.up, Vector2.down, Vector2.left };
      Vector2 currentWay = arrayVec[Random.Range(0, arrayVec.Length)];
      return new Vector2(currentPos.x + currentWay.x, currentPos.y + currentWay.y);
    }
    else if (currentPos.x == 0 && currentPos.y == 0)
    {
      Vector2[] arrayVec = { Vector2.up, Vector2.right };
      Vector2 currentWay = arrayVec[Random.Range(0, arrayVec.Length)];
      return new Vector2(currentPos.x + currentWay.x, currentPos.y + currentWay.y);
    }
    else if (currentPos.x == 0 && currentPos.y == height - 1)
    {
      Vector2[] arrayVec = { Vector2.down, Vector2.right };
      Vector2 currentWay = arrayVec[Random.Range(0, arrayVec.Length)];
      return new Vector2(currentPos.x + currentWay.x, currentPos.y + currentWay.y);
    }
    else if (currentPos.x == width - 1 && currentPos.y == 0)
    {
      Vector2[] arrayVec = { Vector2.up, Vector2.left };
      Vector2 currentWay = arrayVec[Random.Range(0, arrayVec.Length)];
      return new Vector2(currentPos.x + currentWay.x, currentPos.y + currentWay.y);
    }
    else if (currentPos.x == width - 1 && currentPos.y == height - 1)
    {
      Vector2[] arrayVec = { Vector2.down, Vector2.left };
      Vector2 currentWay = arrayVec[Random.Range(0, arrayVec.Length)];
      return new Vector2(currentPos.x + currentWay.x, currentPos.y + currentWay.y);
    }

    return new Vector2(-1, -1);
  }

  public void ChangeBoard()
  {    
    while (IsDeadlock())
    {
      DeleteAllDots();
      SetUp();
    }

    if (SceneManager.GetActiveScene().name == "lvl20" && isSettedCold)
    {
      isSettedCold = false;
      SetColdTiles();
    }

    if (SceneManager.GetActiveScene().name == "lvl10" && isSettedPoisonous)
    {
      isSettedPoisonous = false;
      SetPoisonousTiles();
    }

    isBoardSetted = true;
    isMatchesOnBoard = true;
  }

  private void DeleteAllDots()
  {
    for (int i = 0; i < width; i++)
    {
      for (int j = 0; j < height; j++)
      {
        if (allDots[i, j] != null)
        {
          Destroy(allDots[i, j]);
        }
      }
    }
  }

  private bool MatchesAt (int column, int row, GameObject piece)
  {
    if (piece.tag == coldTile.tag)
    {
      return false;
    }
    else if (column == 0 && row == 0)
    {
      if (allDots[column + 1, row] != null && allDots[column + 2, row] != null && allDots[column + 1, row].tag != coldTile.tag && allDots[column + 2, row].tag != coldTile.tag)
      {
        if (allDots[column + 1, row].tag == piece.tag && allDots[column + 2, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row + 1] != null && allDots[column, row + 2] != null && allDots[column, row + 1].tag != coldTile.tag && allDots[column, row + 2].tag != coldTile.tag)
      {
        if (allDots[column, row + 1].tag == piece.tag && allDots[column, row + 2].tag == piece.tag)
        {
          return true;
        }
      }
    }
    else if (column == 0 && row == height - 1)
    {
      if (allDots[column + 1, row] != null && allDots[column + 2, row] != null && allDots[column + 1, row].tag != coldTile.tag && allDots[column + 2, row].tag != coldTile.tag)
      {
        if (allDots[column + 1, row].tag == piece.tag && allDots[column + 2, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row - 1] != null && allDots[column, row - 2] != null && allDots[column, row - 1].tag != coldTile.tag && allDots[column, row - 2].tag != coldTile.tag)
      {
        if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
        {
          return true;
        }
      }
    }
    else if (column == width - 1 && row == height - 1)
    {
      if (allDots[column - 1, row] != null && allDots[column - 2, row] != null && allDots[column - 1, row].tag != coldTile.tag && allDots[column - 2, row].tag != coldTile.tag)
      {
        if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row - 1] != null && allDots[column, row - 2] != null && allDots[column, row - 1].tag != coldTile.tag && allDots[column, row - 2].tag != coldTile.tag)
      {
        if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
        {
          return true;
        }
      }
    }
    else if (column == width - 1 && row == 0)
    {
      if (allDots[column - 1, row] != null && allDots[column - 2, row] != null && allDots[column - 1, row].tag != coldTile.tag && allDots[column - 2, row].tag != coldTile.tag)
      {
        if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row + 1] != null && allDots[column, row + 2] != null && allDots[column, row + 1].tag != coldTile.tag && allDots[column, row + 2].tag != coldTile.tag)
      {
        if (allDots[column, row + 1].tag == piece.tag && allDots[column, row + 2].tag == piece.tag)
        {
          return true;
        }
      }
    }
    else if (column == 0 && (row > 1 && row < height - 2))
    {
      if (allDots[column + 1, row] != null && allDots[column + 2, row] != null && allDots[column + 1, row].tag != coldTile.tag && allDots[column + 2, row].tag != coldTile.tag)
      {
        if (allDots[column + 1, row].tag == piece.tag && allDots[column + 2, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row + 1] != null && allDots[column, row + 2] != null && allDots[column, row + 1].tag != coldTile.tag && allDots[column, row + 2].tag != coldTile.tag)
      {
        if (allDots[column, row + 1].tag == piece.tag && allDots[column, row + 2].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row - 1] != null && allDots[column, row - 2] != null && allDots[column, row - 1].tag != coldTile.tag && allDots[column, row - 2].tag != coldTile.tag)
      {
        if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row - 1] != null && allDots[column, row  + 1] != null && allDots[column, row - 1].tag != coldTile.tag && allDots[column, row + 1].tag != coldTile.tag)
      {
        if (allDots[column, row - 1].tag == piece.tag && allDots[column, row + 1].tag == piece.tag)
        {
          return true;
        }
      }
    }
    else if (column == width - 1 && (row > 1 && row < height - 2))
    {
      if (allDots[column - 1, row] != null && allDots[column - 2, row] != null && allDots[column - 1, row].tag != coldTile.tag && allDots[column - 2, row].tag != coldTile.tag)
      {
        if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row + 1] != null && allDots[column, row + 2] != null && allDots[column, row + 1].tag != coldTile.tag && allDots[column, row + 2].tag != coldTile.tag)
      {
        if (allDots[column, row + 1].tag == piece.tag && allDots[column, row + 2].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row - 1] != null && allDots[column, row - 2] != null && allDots[column, row - 1].tag != coldTile.tag && allDots[column, row - 2].tag != coldTile.tag)
      {
        if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row - 1] != null && allDots[column, row + 1] != null && allDots[column, row - 1].tag != coldTile.tag && allDots[column, row + 1].tag != coldTile.tag)
      {
        if (allDots[column, row - 1].tag == piece.tag && allDots[column, row + 1].tag == piece.tag)
        {
          return true;
        }
      }
    }
    else if (row == 0 && (column > 1 && column < width - 2))
    {
      if (allDots[column + 1, row] != null && allDots[column + 2, row] != null && allDots[column + 1, row].tag != coldTile.tag && allDots[column + 2, row].tag != coldTile.tag)
      {
        if (allDots[column + 1, row].tag == piece.tag && allDots[column + 2, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column - 1, row] != null && allDots[column - 2, row] != null && allDots[column - 1, row].tag != coldTile.tag && allDots[column - 2, row].tag != coldTile.tag)
      {
        if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row + 1] != null && allDots[column, row + 2] != null && allDots[column, row + 1].tag != coldTile.tag && allDots[column, row + 2].tag != coldTile.tag)
      {
        if (allDots[column, row + 1].tag == piece.tag && allDots[column, row + 2].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column + 1, row] != null && allDots[column - 1, row] != null && allDots[column + 1, row].tag != coldTile.tag && allDots[column - 1, row].tag != coldTile.tag)
      {
        if (allDots[column + 1, row].tag == piece.tag && allDots[column - 1, row].tag == piece.tag)
        {
          return true;
        }
      }
    }
    else if (row == height - 1 && (column > 1 && column < width - 2))
    {
      if (allDots[column + 1, row] != null && allDots[column + 2, row] != null && allDots[column + 1, row].tag != coldTile.tag && allDots[column + 2, row].tag != coldTile.tag)
      {
        if (allDots[column + 1, row].tag == piece.tag && allDots[column + 2, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column - 1, row] != null && allDots[column - 2, row] != null && allDots[column - 1, row].tag != coldTile.tag && allDots[column - 2, row].tag != coldTile.tag)
      {
        if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row - 1] != null && allDots[column, row - 2] != null && allDots[column, row - 1].tag != coldTile.tag && allDots[column, row - 2].tag != coldTile.tag)
      {
        if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column + 1, row] != null && allDots[column - 1, row] != null && allDots[column + 1, row].tag != coldTile.tag && allDots[column - 1, row].tag != coldTile.tag)
      {
        if (allDots[column + 1, row].tag == piece.tag && allDots[column - 1, row].tag == piece.tag)
        {
          return true;
        }
      }
    }
    else if (row == 0 && column == 1)
    {
      if (allDots[column + 1, row] != null && allDots[column + 2, row] != null && allDots[column + 1, row].tag != coldTile.tag && allDots[column + 2, row].tag != coldTile.tag)
      {
        if (allDots[column + 1, row].tag == piece.tag && allDots[column + 2, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column + 1, row] != null && allDots[column - 1, row] != null && allDots[column + 1, row].tag != coldTile.tag && allDots[column - 1, row].tag != coldTile.tag)
      {
        if (allDots[column + 1, row].tag == piece.tag && allDots[column - 1, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row + 1] != null && allDots[column, row + 2] != null && allDots[column, row + 1].tag != coldTile.tag && allDots[column, row + 2].tag != coldTile.tag)
      {
        if (allDots[column, row + 1].tag == piece.tag && allDots[column, row + 2].tag == piece.tag)
        {
          return true;
        }
      }
    }
    else if (row == 0 && column == width - 2)
    {
      if (allDots[column - 1, row] != null && allDots[column - 2, row] != null && allDots[column - 1, row].tag != coldTile.tag && allDots[column - 2, row].tag != coldTile.tag)
      {
        if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column + 1, row] != null && allDots[column - 1, row] != null && allDots[column + 1, row].tag != coldTile.tag && allDots[column - 1, row].tag != coldTile.tag)
      {
        if (allDots[column + 1, row].tag == piece.tag && allDots[column - 1, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row + 1] != null && allDots[column, row + 2] != null && allDots[column, row + 1].tag != coldTile.tag && allDots[column, row + 2].tag != coldTile.tag)
      {
        if (allDots[column, row + 1].tag == piece.tag && allDots[column, row + 2].tag == piece.tag)
        {
          return true;
        }
      }
    }
    else if (row == height - 1 && column == width - 2)
    {
      if (allDots[column - 1, row] != null && allDots[column - 2, row] != null && allDots[column - 1, row].tag != coldTile.tag && allDots[column - 2, row].tag != coldTile.tag)
      {
        if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column + 1, row] != null && allDots[column - 1, row] != null && allDots[column + 1, row].tag != coldTile.tag && allDots[column - 1, row].tag != coldTile.tag)
      {
        if (allDots[column + 1, row].tag == piece.tag && allDots[column - 1, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row - 1] != null && allDots[column, row - 2] != null && allDots[column, row - 1].tag != coldTile.tag && allDots[column, row - 2].tag != coldTile.tag)
      {
        if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
        {
          return true;
        }
      }
    }
    else if (row == height - 1 && column == 1)
    {
      if (allDots[column + 1, row] != null && allDots[column + 2, row] != null && allDots[column + 1, row].tag != coldTile.tag && allDots[column + 2, row].tag != coldTile.tag)
      {
        if (allDots[column + 1, row].tag == piece.tag && allDots[column + 2, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column + 1, row] != null && allDots[column - 1, row] != null && allDots[column + 1, row].tag != coldTile.tag && allDots[column - 1, row].tag != coldTile.tag)
      {
        if (allDots[column + 1, row].tag == piece.tag && allDots[column - 1, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row - 1] != null && allDots[column, row - 2] != null && allDots[column, row - 1].tag != coldTile.tag && allDots[column, row - 2].tag != coldTile.tag)
      {
        if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
        {
          return true;
        }
      }
    }
    else if (row == height - 2 && column == 0)
    {
      if (allDots[column + 1, row] != null && allDots[column + 2, row] != null && allDots[column + 1, row].tag != coldTile.tag && allDots[column + 2, row].tag != coldTile.tag)
      {
        if (allDots[column + 1, row].tag == piece.tag && allDots[column + 2, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row + 1] != null && allDots[column, row - 1] != null && allDots[column, row + 1].tag != coldTile.tag && allDots[column, row - 1].tag != coldTile.tag)
      {
        if (allDots[column, row + 1].tag == piece.tag && allDots[column, row - 1].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row - 1] != null && allDots[column, row - 2] != null && allDots[column, row - 1].tag != coldTile.tag && allDots[column, row - 2].tag != coldTile.tag)
      {
        if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
        {
          return true;
        }
      }
    }
    else if (row == 1 && column == 0)
    {
      if (allDots[column + 1, row] != null && allDots[column + 2, row] != null && allDots[column + 1, row].tag != coldTile.tag && allDots[column + 2, row].tag != coldTile.tag)
      {
        if (allDots[column + 1, row].tag == piece.tag && allDots[column + 2, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row + 1] != null && allDots[column, row - 1] != null && allDots[column, row + 1].tag != coldTile.tag && allDots[column, row - 1].tag != coldTile.tag)
      {
        if (allDots[column, row + 1].tag == piece.tag && allDots[column, row - 1].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row + 1] != null && allDots[column, row + 2] != null && allDots[column, row + 1].tag != coldTile.tag && allDots[column, row + 2].tag != coldTile.tag)
      {
        if (allDots[column, row + 1].tag == piece.tag && allDots[column, row + 2].tag == piece.tag)
        {
          return true;
        }
      }
    }
    else if (row == 1 && column == width - 1)
    {
      if (allDots[column - 1, row] != null && allDots[column - 2, row] != null && allDots[column - 1, row].tag != coldTile.tag && allDots[column - 2, row].tag != coldTile.tag)
      {
        if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row + 1] != null && allDots[column, row - 1] != null && allDots[column, row + 1].tag != coldTile.tag && allDots[column, row - 1].tag != coldTile.tag)
      {
        if (allDots[column, row + 1].tag == piece.tag && allDots[column, row - 1].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row + 1] != null && allDots[column, row + 2] != null && allDots[column, row + 1].tag != coldTile.tag && allDots[column, row + 2].tag != coldTile.tag)
      {
        if (allDots[column, row + 1].tag == piece.tag && allDots[column, row + 2].tag == piece.tag)
        {
          return true;
        }
      }
    }
    else if (row == height - 2 && column == width - 1)
    {
      if (allDots[column - 1, row] != null && allDots[column - 2, row] != null && allDots[column - 1, row].tag != coldTile.tag && allDots[column - 2, row].tag != coldTile.tag)
      {
        if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row + 1] != null && allDots[column, row - 1] != null && allDots[column, row + 1].tag != coldTile.tag && allDots[column, row - 1].tag != coldTile.tag)
      {
        if (allDots[column, row + 1].tag == piece.tag && allDots[column, row - 1].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row - 1] != null && allDots[column, row - 2] != null && allDots[column, row - 1].tag != coldTile.tag && allDots[column, row - 2].tag != coldTile.tag)
      {
        if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
        {
          return true;
        }
      }
    }
    else if ((row > 1 && row < height - 2) && (column > 1 && column < width - 2))
    {
      if (allDots[column - 1, row] != null && allDots[column - 2, row] != null && allDots[column - 1, row].tag != coldTile.tag && allDots[column - 2, row].tag != coldTile.tag)
      {
        if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column + 1, row] != null && allDots[column + 2, row] != null && allDots[column + 1, row].tag != coldTile.tag && allDots[column + 2, row].tag != coldTile.tag)
      {
        if (allDots[column + 1, row].tag == piece.tag && allDots[column + 2, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row + 1] != null && allDots[column, row + 2] != null && allDots[column, row + 1].tag != coldTile.tag && allDots[column, row + 2].tag != coldTile.tag)
      {
        if (allDots[column, row + 1].tag == piece.tag && allDots[column, row + 2].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row - 1] != null && allDots[column, row - 2] != null && allDots[column, row - 1].tag != coldTile.tag && allDots[column, row - 2].tag != coldTile.tag)
      {
        if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row - 1] != null && allDots[column, row + 1] != null && allDots[column, row - 1].tag != coldTile.tag && allDots[column, row + 1].tag != coldTile.tag)
      {
        if (allDots[column, row - 1].tag == piece.tag && allDots[column, row + 1].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column + 1, row] != null && allDots[column - 1, row] != null && allDots[column + 1, row].tag != coldTile.tag && allDots[column - 1, row].tag != coldTile.tag)
      {
        if (allDots[column + 1, row].tag == piece.tag && allDots[column - 1, row].tag == piece.tag)
        {
          return true;
        }
      }
    }
    else if (row == 1 && column == 1)
    {
      if (allDots[column + 1, row] != null && allDots[column + 2, row] != null && allDots[column + 1, row].tag != coldTile.tag && allDots[column + 2, row].tag != coldTile.tag)
      {
        if (allDots[column + 1, row].tag == piece.tag && allDots[column + 2, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row + 1] != null && allDots[column, row + 2] != null && allDots[column, row + 1].tag != coldTile.tag && allDots[column, row + 2].tag != coldTile.tag)
      {
        if (allDots[column, row + 1].tag == piece.tag && allDots[column, row + 2].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row - 1] != null && allDots[column, row + 1] != null && allDots[column, row - 1].tag != coldTile.tag && allDots[column, row + 1].tag != coldTile.tag)
      {
        if (allDots[column, row - 1].tag == piece.tag && allDots[column, row + 1].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column - 1, row] != null && allDots[column + 1, row] != null && allDots[column - 1, row].tag != coldTile.tag && allDots[column + 1, row].tag != coldTile.tag)
      {
        if (allDots[column - 1, row].tag == piece.tag && allDots[column + 1, row].tag == piece.tag)
        {
          return true;
        }
      }
    }
    else if (row == height - 2 && column == 1)
    {
      if (allDots[column + 1, row] != null && allDots[column + 2, row] != null && allDots[column + 1, row].tag != coldTile.tag && allDots[column + 2, row].tag != coldTile.tag)
      {
        if (allDots[column + 1, row].tag == piece.tag && allDots[column + 2, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row - 1] != null && allDots[column, row - 2] != null && allDots[column, row - 1].tag != coldTile.tag && allDots[column, row - 2].tag != coldTile.tag)
      {
        if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row - 1] != null && allDots[column, row + 1] != null && allDots[column, row - 1].tag != coldTile.tag && allDots[column, row + 1].tag != coldTile.tag)
      {
        if (allDots[column, row - 1].tag == piece.tag && allDots[column, row + 1].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column - 1, row] != null && allDots[column + 1, row] != null && allDots[column - 1, row].tag != coldTile.tag && allDots[column + 1, row].tag != coldTile.tag)
      {
        if (allDots[column - 1, row].tag == piece.tag && allDots[column + 1, row].tag == piece.tag)
        {
          return true;
        }
      }
    }
    else if (row == height - 2 && column == width - 2)
    {
      if (allDots[column - 1, row] != null && allDots[column - 2, row] != null && allDots[column - 1, row].tag != coldTile.tag && allDots[column - 2, row].tag != coldTile.tag)
      {
        if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row - 1] != null && allDots[column, row - 2] != null && allDots[column, row - 1].tag != coldTile.tag && allDots[column, row - 2].tag != coldTile.tag)
      {
        if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row - 1] != null && allDots[column, row + 1] != null && allDots[column, row - 1].tag != coldTile.tag && allDots[column, row + 1].tag != coldTile.tag)
      {
        if (allDots[column, row - 1].tag == piece.tag && allDots[column, row + 1].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column - 1, row] != null && allDots[column + 1, row] != null && allDots[column - 1, row].tag != coldTile.tag && allDots[column + 1, row].tag != coldTile.tag)
      {
        if (allDots[column - 1, row].tag == piece.tag && allDots[column + 1, row].tag == piece.tag)
        {
          return true;
        }
      }
    }
    else if (row == 1 && column == width - 2)
    {
      if (allDots[column - 1, row] != null && allDots[column - 2, row] != null && allDots[column - 1, row].tag != coldTile.tag && allDots[column - 2, row].tag != coldTile.tag)
      {
        if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row + 1] != null && allDots[column, row + 2] != null && allDots[column, row + 1].tag != coldTile.tag && allDots[column, row + 2].tag != coldTile.tag)
      {
        if (allDots[column, row + 1].tag == piece.tag && allDots[column, row + 2].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row - 1] != null && allDots[column, row + 1] != null && allDots[column, row - 1].tag != coldTile.tag && allDots[column, row + 1].tag != coldTile.tag)
      {
        if (allDots[column, row - 1].tag == piece.tag && allDots[column, row + 1].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column - 1, row] != null && allDots[column + 1, row] != null && allDots[column - 1, row].tag != coldTile.tag && allDots[column + 1, row].tag != coldTile.tag)
      {
        if (allDots[column - 1, row].tag == piece.tag && allDots[column + 1, row].tag == piece.tag)
        {
          return true;
        }
      }
    }
    else if (row == 1 && (column > 1 && column < width - 2))
    {
      if (allDots[column - 1, row] != null && allDots[column - 2, row] != null && allDots[column - 1, row].tag != coldTile.tag && allDots[column - 2, row].tag != coldTile.tag)
      {
        if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column + 1, row] != null && allDots[column + 2, row] != null && allDots[column + 1, row].tag != coldTile.tag && allDots[column + 2, row].tag != coldTile.tag)
      {
        if (allDots[column + 1, row].tag == piece.tag && allDots[column + 2, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row + 1] != null && allDots[column, row + 2] != null && allDots[column, row + 1].tag != coldTile.tag && allDots[column, row + 2].tag != coldTile.tag)
      {
        if (allDots[column, row + 1].tag == piece.tag && allDots[column, row + 2].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row - 1] != null && allDots[column, row + 1] != null && allDots[column, row - 1].tag != coldTile.tag && allDots[column, row + 1].tag != coldTile.tag)
      {
        if (allDots[column, row - 1].tag == piece.tag && allDots[column, row + 1].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column - 1, row] != null && allDots[column + 1, row] != null && allDots[column - 1, row].tag != coldTile.tag && allDots[column + 1, row].tag != coldTile.tag)
      {
        if (allDots[column - 1, row].tag == piece.tag && allDots[column + 1, row].tag == piece.tag)
        {
          return true;
        }
      }
    }
    else if (row == height - 2 && (column > 1 && column < width - 2))
    {
      if (allDots[column - 1, row] != null && allDots[column - 2, row] != null && allDots[column - 1, row].tag != coldTile.tag && allDots[column - 2, row].tag != coldTile.tag)
      {
        if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column + 1, row] != null && allDots[column + 2, row] != null && allDots[column + 1, row].tag != coldTile.tag && allDots[column + 2, row].tag != coldTile.tag)
      {
        if (allDots[column + 1, row].tag == piece.tag && allDots[column + 2, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row - 1] != null && allDots[column, row - 2] != null && allDots[column, row - 1].tag != coldTile.tag && allDots[column, row - 2].tag != coldTile.tag)
      {
        if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row - 1] != null && allDots[column, row + 1] != null && allDots[column, row - 1].tag != coldTile.tag && allDots[column, row + 1].tag != coldTile.tag)
      {
        if (allDots[column, row - 1].tag == piece.tag && allDots[column, row + 1].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column - 1, row] != null && allDots[column + 1, row] != null && allDots[column - 1, row].tag != coldTile.tag && allDots[column + 1, row].tag != coldTile.tag)
      {
        if (allDots[column - 1, row].tag == piece.tag && allDots[column + 1, row].tag == piece.tag)
        {
          return true;
        }
      }
    }
    else if (column == 1 && (row > 1 && row < height - 2))
    {
      if (allDots[column, row + 1] != null && allDots[column, row + 2] != null && allDots[column, row + 1].tag != coldTile.tag && allDots[column, row + 2].tag != coldTile.tag)
      {
        if (allDots[column, row + 1].tag == piece.tag && allDots[column, row + 2].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row - 1] != null && allDots[column, row - 2] != null && allDots[column, row - 1].tag != coldTile.tag && allDots[column, row - 2].tag != coldTile.tag)
      {
        if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column + 1, row] != null && allDots[column + 2, row] != null && allDots[column + 1, row].tag != coldTile.tag && allDots[column + 2, row].tag != coldTile.tag)
      {
        if (allDots[column + 1, row].tag == piece.tag && allDots[column + 2, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row - 1] != null && allDots[column, row + 1] != null && allDots[column, row - 1].tag != coldTile.tag && allDots[column, row + 1].tag != coldTile.tag)
      {
        if (allDots[column, row - 1].tag == piece.tag && allDots[column, row + 1].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column - 1, row] != null && allDots[column + 1, row] != null && allDots[column - 1, row].tag != coldTile.tag && allDots[column + 1, row].tag != coldTile.tag)
      {
        if (allDots[column - 1, row].tag == piece.tag && allDots[column + 1, row].tag == piece.tag)
        {
          return true;
        }
      }
    }
    else if (column == width - 2 && (row > 1 && row < height - 2))
    {
      if (allDots[column, row + 1] != null && allDots[column, row + 2] != null && allDots[column, row + 1].tag != coldTile.tag && allDots[column, row + 2].tag != coldTile.tag)
      {
        if (allDots[column, row + 1].tag == piece.tag && allDots[column, row + 2].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row - 1] != null && allDots[column, row - 2] != null && allDots[column, row - 1].tag != coldTile.tag && allDots[column, row - 2].tag != coldTile.tag)
      {
        if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column - 1, row] != null && allDots[column - 2, row] != null && allDots[column - 1, row].tag != coldTile.tag && allDots[column - 2, row].tag != coldTile.tag)
      {
        if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column, row - 1] != null && allDots[column, row + 1] != null && allDots[column, row - 1].tag != coldTile.tag && allDots[column, row + 1].tag != coldTile.tag)
      {
        if (allDots[column, row - 1].tag == piece.tag && allDots[column, row + 1].tag == piece.tag)
        {
          return true;
        }
      }
      if (allDots[column - 1, row] != null && allDots[column + 1, row] != null && allDots[column - 1, row].tag != coldTile.tag && allDots[column + 1, row].tag != coldTile.tag)
      {
        if (allDots[column - 1, row].tag == piece.tag && allDots[column + 1, row].tag == piece.tag)
        {
          return true;
        }
      }
    }

    return false;
  }

  private void DestroyMathesAt(int column, int row)
  {
    if (allDots[column, row].GetComponent<Dot>().isMatched)
    {
      findMatches.currentMatches.Remove(allDots[column, row]);
      Destroy(allDots[column, row]);
      if (SceneManager.GetActiveScene().name == "lvl01" || SceneManager.GetActiveScene().name == "lvl03" ||
        SceneManager.GetActiveScene().name == "lvl04" || SceneManager.GetActiveScene().name == "lvl06" ||
        SceneManager.GetActiveScene().name == "lvl08" || SceneManager.GetActiveScene().name == "lvl09" ||
        SceneManager.GetActiveScene().name == "lvl11" || SceneManager.GetActiveScene().name == "lvl13" ||
        SceneManager.GetActiveScene().name == "lvl14" || SceneManager.GetActiveScene().name == "lvl16" ||
        SceneManager.GetActiveScene().name == "lvl18" || SceneManager.GetActiveScene().name == "lvl19" ||
        SceneManager.GetActiveScene().name == "lvl21" || SceneManager.GetActiveScene().name == "lvl23" ||
        SceneManager.GetActiveScene().name == "lvl24")
      {
        scoreManager.InscreaseScore(basePieceValue * streakValue);
      }
      else if (SceneManager.GetActiveScene().name == "lvl02" || SceneManager.GetActiveScene().name == "lvl07" ||
        SceneManager.GetActiveScene().name == "lvl12" || SceneManager.GetActiveScene().name == "lvl17" ||
          SceneManager.GetActiveScene().name == "lvl22")
      {
        nutCounterManager.DecreaseCounter(1);
      }
      else if (SceneManager.GetActiveScene().name == "lvl05" || SceneManager.GetActiveScene().name == "lvl20")
      {
        healthBoss -= 1;
      }
      else if (SceneManager.GetActiveScene().name == "lvl10")
      {
        if (allDots[column, row].GetComponent<SpriteRenderer>().tag == poisonousTile.GetComponent<SpriteRenderer>().tag)
        {
          healthBoss += 3;
        }
        else
        {
          healthBoss -= 1;
        }
      }
      else if (SceneManager.GetActiveScene().name == "lvl15")
      {
        if (!GUIManager5.isDeleteRandom)
        {
          healthBoss -= 1;
        }
      }
      else if (SceneManager.GetActiveScene().name == "lvl25")
      {
        if (allDots[column, row].GetComponent<SpriteRenderer>().tag == poisonousTile.GetComponent<SpriteRenderer>().tag)
        {
          healthBoss += 3;
        }
        else if (!GUIManager5.isDeleteRandom)
        {
          healthBoss -= 1;
        }
      }
      allDots[column, row] = null;
    }
  }

  public void DestroyMathes()
  {
    for (int i = 0; i < width; i++)
    {
      for (int j = 0; j < height; j++)
      {
        if (allDots[i, j] != null)
        {
          DestroyMathesAt(i, j);
        }
      }
    }

    SoundManager.CrackSound();
    StartCoroutine(DecreaseRowCo2());
  }

  private IEnumerator DecreaseRowCo2()
  {
    for (int i = 0; i < width; i++)
    {
      for (int j = 0; j < height; j++)
      {
        if (allDots[i, j] == null && !blankSpaces[i, j])
        {
          for (int k = j + 1; k < height; k++)
          {
            if (allDots[i, k] != null)
            {
              allDots[i, k].GetComponent<Dot>().row = j;
              allDots[i, k] = null;
              break;
            }
          }
        }
      }
    }

    yield return new WaitForSeconds(refillDelay * 0.5f);//0.4f;
    StartCoroutine(FillBoardCo());
  }

  private void RefillBoard()
  {
    for (int i = 0; i < width; i++)
    {
      for (int j = 0; j < height; j++)
      {
        if (allDots[i, j] == null && !blankSpaces[i, j])
        {
          Vector2 tempPosition = new Vector2(i, j + offSet);
          int dotToUse = Random.Range(0, dots.Length);
          int maxIterations = 0;
          while (MatchesAt(i, j, dots[dotToUse]) && maxIterations < 100)
          {
            maxIterations++;
            dotToUse = Random.Range(0, dots.Length);
          }
          maxIterations = 0;

          GameObject piece = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
          allDots[i, j] = piece;
          piece.GetComponent<Dot>().row = j;
          piece.GetComponent<Dot>().column = i;
        }
      }
    }
  }

  private bool MatchesOnBoard()
  {
    for (int i = 0; i < width; i++)
    {
      for (int j = 0; j < height; j++)
      {
        if (allDots[i, j] != null)
        {
          if (allDots[i, j].GetComponent<Dot>().isMatched)
          {
            return true;
          }
        }
      }
    }
    return false;
  }

  private void SwitchPieces(int i, int j, Vector2 direction)
  {
    GameObject holderDot = allDots[i + (int)direction.x, j + (int)direction.y];
    allDots[i + (int)direction.x, j + (int)direction.y] = allDots[i, j];
    allDots[i, j] = holderDot;
  }

  public bool CheckMatches()
  {
    for (int i = 0; i < width; i++)
    {
      for (int j = 0; j < height; j++)
      {
        if (allDots[i, j] != null && allDots[i, j].tag != coldTile.tag)
        {
          if (i < width - 2 && allDots[i + 1, j] != null && allDots[i + 2, j] != null && 
            allDots[i + 1, j].tag != coldTile.tag && allDots[i + 2, j].tag != coldTile.tag)
          {
            if (allDots[i, j].tag == allDots[i + 1, j].tag && allDots[i, j].tag == allDots[i + 2, j].tag)
            {
              return true;
            }
          }

          if (j < height - 2 && allDots[i, j + 1] != null && allDots[i, j + 2] != null &&
            allDots[i, j + 1].tag != coldTile.tag && allDots[i, j + 2].tag != coldTile.tag)
          {
            if (allDots[i, j].tag == allDots[i, j + 1].tag && allDots[i, j].tag == allDots[i, j + 2].tag)
            {
              return true;
            }
          }
        }
      }
    }

    return false;
  }

  private bool SwitchAndCheck(int i, int j, Vector2 direction)
  {
    SwitchPieces(i, j, direction);

    if (CheckMatches())
    {
      SwitchPieces(i, j, direction);
      Debug.Log("" + i + j);
      return true;
    }
    SwitchPieces(i, j, direction);
    return false;
  }

  public bool IsDeadlock()
  {
    for (int i = 0; i < width; i++)
    {
      for (int j = 0; j < height; j++)
      {
        if (allDots[i, j] != null && allDots[i, j].tag != coldTile.tag)
        {
          if (i < width - 1 && allDots[i + 1, j] != null && allDots[i + 1, j].tag != coldTile.tag)
          {
            if (SwitchAndCheck(i, j, Vector2.right))
            {
              return false;
            }
          }

          if (j < height - 1 && allDots[i, j + 1] != null && allDots[i, j + 1].tag != coldTile.tag)
          {
            if (SwitchAndCheck(i, j, Vector2.up))
            {
              return false;
            }
          }
        }
      }
    }

    return true;
  }

  public void ShuffleBoard()
  {
    bool isBadShuffle = false;
    List<GameObject> shuffleBoard = new List<GameObject>();

    for (int i = 0; i < width; i++)
    {
      for (int j = 0; j < height; j++)
      {
        if (allDots[i, j] != null)
        {
          shuffleBoard.Add(allDots[i, j]);
        }
      }
    }

    for (int i = 0; i < width; i++)
    {
      for (int j = 0; j < height; j++)
      {
        int currentSprite = Random.Range(0, shuffleBoard.Count);
        int selectCounter = 0;

        while (MatchesAt(i, j, shuffleBoard[currentSprite]) && selectCounter < 100)
        {
          currentSprite = Random.Range(0, shuffleBoard.Count);
          selectCounter++;
        }

        if (selectCounter >= 100)
        {
          isBadShuffle = true;
        }

        Dot ourSprite = shuffleBoard[currentSprite].GetComponent<Dot>();
        ourSprite.column = i;
        ourSprite.row = j;
        allDots[i, j] = shuffleBoard[currentSprite];
        shuffleBoard.Remove(shuffleBoard[currentSprite]);
      }
    }

    if (isBadShuffle)
    {
      ShuffleBoard();
    }
    else
    {
      SoundManager.ShuffleSound();
    }
  }

  public bool isSettedPoisonous = false;
  public void SetPoisonousTiles()
  {
    GameObject poisonousDot = dots[Random.Range(0, dots.Length)];
    int counter = 0;

    for (int i = 0; i < width; i++)
    {
      for (int j = 0; j < height; j++)
      {
        if (allDots[i, j] != null && allDots[i,j].tag == poisonousDot.tag && !MatchesAt(i, j, poisonousTile))
        {
          counter++;
        }
      }
    }

    if (counter <= 6 && !isSettedPoisonous)
    {
      SetPoisonousTiles();
    }
    else
    {
      isSettedPoisonous = true; 
      SoundManager.PoisonSound();

      for (int i = 0; i < width; i++)
      {
        for (int j = 0; j < height; j++)
        {
          if (allDots[i, j] != null && allDots[i, j].tag == poisonousDot.tag && !MatchesAt(i, j, poisonousTile))
          {
            allDots[i, j].tag = poisonousTile.tag;
            allDots[i, j].GetComponent<SpriteRenderer>().sprite = poisonousTile.GetComponent<SpriteRenderer>().sprite;
            allDots[i, j].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
          }
        }
      }
    }
  }

  public bool isSettedCold = false;
  public void SetColdTiles()
  {
    GameObject coldDot = dots[Random.Range(0, dots.Length)];
    int counter = 0;

    for (int i = 0; i < width; i++)
    {
      for (int j = 0; j < height; j++)
      {
        if (allDots[i, j] != null && allDots[i, j].tag == coldDot.tag)
        {
          counter++;
        }
      }
    }

    if (counter <= 6 && !isSettedCold)
    {
      SetColdTiles();
    }
    else
    {
      isSettedCold = true;
      SoundManager.FreezeSound();

      for (int i = 0; i < width; i++)
      {
        for (int j = 0; j < height; j++)
        {
          if (allDots[i, j] != null && allDots[i, j].tag == coldDot.tag)
          {
            allDots[i, j].tag = coldTile.tag;
            allDots[i, j].GetComponent<SpriteRenderer>().sprite = coldTile.GetComponent<SpriteRenderer>().sprite;
            allDots[i, j].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
          }
        }
      }
    }

    if (IsDeadlock())
    {
      ChangeBoard();
    }
  }

  public void DeleteRandomColorCall()
  {
    StartCoroutine(DeleteRandomColorTilesCo());
  }

  private IEnumerator DeleteRandomColorTilesCo()
  {
    DeleteRandomColorTiles();

    yield return new WaitForSeconds(0.5f);
    DestroyMathes();
    SoundManager.CrashSound();

    yield return new WaitForSeconds(1f);
    currentState = GameState.move;
  }
  public void DeleteRandomColorTiles()
  {
    GameObject deleteDot = dots[Random.Range(0, dots.Length)];
    int counter = 0;

    for (int i = 0; i < width; i++)
    {
      for (int j = 0; j < height; j++)
      {
        if (allDots[i, j] != null && allDots[i, j].tag == deleteDot.tag)
        {
          counter++;
        }
      }
    }

    if (counter <= 6)
    {
      DeleteRandomColorTiles();
    }
    else
    {
      for (int i = 0; i < width; i++)
      {
        for (int j = 0; j < height; j++)
        {
          if (allDots[i, j] != null && allDots[i, j].tag == deleteDot.tag)
          {
            allDots[i, j].GetComponent<Dot>().isMatched = true;
          }
        }
      }
    }
  }

  public void SetMoveSpeed(float currentSpeed)
  {
    for (int i = 0; i < width; i++)
    {
      for (int j = 0; j < height; j++)
      {
        if (allDots[i, j] != null)
        {
          allDots[i, j].GetComponent<Dot>().moveSpeed = currentSpeed;
        }
      }
    }
  }

  private IEnumerator FillBoardCo()
  {
    RefillBoard();
    yield return new WaitForSeconds(refillDelay);

    while (MatchesOnBoard())
    {
      streakValue++;
      DestroyMathes();
      yield return new WaitForSeconds(refillDelay * 2);
    }
    //yield return new WaitForSeconds(refillDelay);
    streakValue = 1;

    if (IsDeadlock())
    {
      Debug.Log("Game Over");
      isMatchesOnBoard = false;
    }

    //yield return new WaitForSeconds(refillDelay);
    GUIManager5.isDeleteRandom = false;
    currentState = GameState.move;
  }
}