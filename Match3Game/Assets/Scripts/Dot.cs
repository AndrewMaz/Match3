using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dot : MonoBehaviour
{
  public int column;
  public int row;
  public int previousColumn;
  public int previousRow;
  public int targetX;
  public int targetY;
  //public bool isColorChanged = false;
  public bool isMatched = false;
  public bool isUsed;
  //public Color prevColor;

  public float moveSpeed = 0.6f;
  private StepManager stepManager;
  private FindMatches findMatches;
  private Board board;
  private GameObject otherDot;
  private Vector2 firstTouchPosition;
  private Vector2 finalTouchPosition;
  private Vector2 tempPosition;

  public float swipeAngle = 0;
  public float swipeResist = 0.3f;

  void Start()
  {
    board = FindObjectOfType<Board>();
    findMatches = FindObjectOfType<FindMatches>();

    if (SceneManager.GetActiveScene().name == "lvl04" || SceneManager.GetActiveScene().name == "lvl09" ||
      SceneManager.GetActiveScene().name == "lvl14" || SceneManager.GetActiveScene().name == "lvl19" ||
      SceneManager.GetActiveScene().name == "lvl24")
    {
      stepManager = FindObjectOfType<StepManager>();
    }
  }

  void Update()
  {
    if (!board.isActiveAndEnabled)
    {
      return;
    }

    if (isMatched)
    {
      SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
      if ((SceneManager.GetActiveScene().name == "lvl10" || SceneManager.GetActiveScene().name == "lvl25") && this.gameObject.tag == board.poisonousTile.tag)
      {
        mySprite.color = new Color(0.5f, 0.5f, 0.5f, 1);
      }
      else
      {
        mySprite.sprite = board.destroyTile.GetComponent<SpriteRenderer>().sprite;
      }
    }

    targetX = column;
    targetY = row;
    if (Mathf.Abs(targetX - transform.position.x) > .1)
    {
      tempPosition = new Vector2(targetX, transform.position.y);
      transform.position = Vector2.Lerp(transform.position, tempPosition, moveSpeed);
      if (board.allDots[column, row] != this.gameObject)
      {
        board.allDots[column, row] = this.gameObject;
      }
      findMatches.FindAllMatches();
    }
    else
    {
      tempPosition = new Vector2(targetX, transform.position.y);
      transform.position = tempPosition;
    }
    if (Mathf.Abs(targetY - transform.position.y) > .1)
    {
      tempPosition = new Vector2(transform.position.x, targetY);
      transform.position = Vector2.Lerp(transform.position, tempPosition, moveSpeed);
      if (board.allDots[column, row] != this.gameObject)
      {
        board.allDots[column, row] = this.gameObject;
      }
      findMatches.FindAllMatches();
    }
    else
    {
      tempPosition = new Vector2(transform.position.x, targetY);
      transform.position = tempPosition;
    }
  }

  public IEnumerator CheckMoveCo()
  {
    SoundManager.SwitchSound();
    yield return new WaitForSeconds(.5f);
    if (otherDot != null)
    { 
      if (!isMatched && !otherDot.GetComponent<Dot>().isMatched)
      {
        otherDot.GetComponent<Dot>().row = row;
        otherDot.GetComponent<Dot>().column = column;
        row = previousRow;
        column = previousColumn;
        //yield return new WaitForSeconds(0.5f);
        board.currentState = GameState.move;
        SoundManager.SwitchSound();
      }
      else
      {
        board.DestroyMathes();
      }
      otherDot = null;
    }

  }

  private void OnMouseDown()
  {
    if (board.currentState == GameState.move && tag != board.coldTile.tag)
    {
      firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
  }

  private void OnMouseUp()
  {
    if (board.currentState == GameState.move && tag != board.coldTile.tag)
    {
      finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      CalculateAngle();
    }
  }

  private void CalculateAngle()
  {
    if (Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist || Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist)
    {
      board.currentState = GameState.wait;
      swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
      MovePieces();
    }
    else
    {
      board.currentState = GameState.move;
    }
  }

  private void MovePieces()
  {
    if ((swipeAngle >= -45 && swipeAngle < 45) && column < board.width - 1)
    {
      otherDot = board.allDots[column + 1, row];
      if (otherDot != null && otherDot.tag != board.coldTile.tag)
      {
        previousColumn = column;
        previousRow = row;
        otherDot.GetComponent<Dot>().column -= 1;
        column += 1;
        if (SceneManager.GetActiveScene().name == "lvl04" || SceneManager.GetActiveScene().name == "lvl09" ||
      SceneManager.GetActiveScene().name == "lvl14" || SceneManager.GetActiveScene().name == "lvl19" ||
      SceneManager.GetActiveScene().name == "lvl24")
        {
          stepManager.DecreaseCounter(1);
        }
        StartCoroutine(CheckMoveCo());
      }
      else
      {
        board.currentState = GameState.move;
      }
    }
    else if ((swipeAngle >= 45 && swipeAngle < 135) && row < board.height - 1)
    {
      otherDot = board.allDots[column, row + 1];
      if (otherDot != null && otherDot.tag != board.coldTile.tag)
      {
        previousColumn = column;
        previousRow = row;
        otherDot.GetComponent<Dot>().row -= 1;
        row += 1;
        if (SceneManager.GetActiveScene().name == "lvl04" || SceneManager.GetActiveScene().name == "lvl09" ||
      SceneManager.GetActiveScene().name == "lvl14" || SceneManager.GetActiveScene().name == "lvl19" ||
      SceneManager.GetActiveScene().name == "lvl24")
        {
          stepManager.DecreaseCounter(1);
        }
        StartCoroutine(CheckMoveCo());
      }
      else
      {
        board.currentState = GameState.move;
      }
    }
    else if ((swipeAngle >= 135 || swipeAngle < -135) && column > 0)
    {
      otherDot = board.allDots[column - 1, row];
      if (otherDot != null && otherDot.tag != board.coldTile.tag)
      {
        previousColumn = column;
        previousRow = row;
        otherDot.GetComponent<Dot>().column += 1;
        column -= 1;
        if (SceneManager.GetActiveScene().name == "lvl04" || SceneManager.GetActiveScene().name == "lvl09" ||
      SceneManager.GetActiveScene().name == "lvl14" || SceneManager.GetActiveScene().name == "lvl19" ||
      SceneManager.GetActiveScene().name == "lvl24")
        {
          stepManager.DecreaseCounter(1);
        }
        StartCoroutine(CheckMoveCo());
      }
      else
      {
        board.currentState = GameState.move;
      }
    }
    else if ((swipeAngle < -45 && swipeAngle >= -135) && row > 0)
    {
      otherDot = board.allDots[column, row - 1];
      if (otherDot != null && otherDot.tag != board.coldTile.tag)
      {
        previousColumn = column;
        previousRow = row;
        otherDot.GetComponent<Dot>().row += 1;
        row -= 1;
        if (SceneManager.GetActiveScene().name == "lvl04" || SceneManager.GetActiveScene().name == "lvl09" ||
      SceneManager.GetActiveScene().name == "lvl14" || SceneManager.GetActiveScene().name == "lvl19" ||
      SceneManager.GetActiveScene().name == "lvl24")
        {
          stepManager.DecreaseCounter(1);
        }
        StartCoroutine(CheckMoveCo());
      }
      else
      {
        board.currentState = GameState.move;
      }
    }
    else
    {
      board.currentState = GameState.move;
    }
  }

  void FindMatches()
  {
    if (column > 0 && column < board.width - 1)
    {
      GameObject leftDot = board.allDots[column - 1, row];
      GameObject rightDot = board.allDots[column + 1, row];
      if (leftDot != null && rightDot != null && leftDot.tag == this.gameObject.tag && rightDot.tag == this.gameObject.tag)
      {
        leftDot.GetComponent<Dot>().isMatched = true;
        rightDot.GetComponent<Dot>().isMatched = true;
        isMatched = true;
      }
    }
    if (row > 0 && row < board.height - 1)
    {
      GameObject upDot = board.allDots[column, row + 1];
      GameObject downDot = board.allDots[column, row - 1];
      if (upDot != null && downDot != null && upDot.tag == this.gameObject.tag && downDot.tag == this.gameObject.tag)
      {
        upDot.GetComponent<Dot>().isMatched = true;
        downDot.GetComponent<Dot>().isMatched = true;
        isMatched = true;
      }
    }
  }
}
