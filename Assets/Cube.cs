using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class Cube : MonoBehaviour
{
  private const int CUBE_SIZE = 11;
  private const float RELATIVE_CELL_SIZE = 3f;
  private const float RELATIVE_CELL_MARGIN = 1f;
  private const float CELL_SIZE = RELATIVE_CELL_SIZE / CUBE_SIZE;
  private const float ROTATION_TIME_SECONDS = 1.5f;
  public GameObject cellPrefab;
  public GameState gameState;
  private GameObject[,,] cells = new GameObject[3,3,3];

  void Start()
  {
    // InitializeCube();
    // CreateCells();
    // gameState.calculateAdjacency();
  }

  private void InitializeCube()
  {
    gameObject.transform.localScale = Vector3.one * CUBE_SIZE;
    gameObject.transform.position = new Vector3(0, 0, 0);
  }

  public void CreateCells()
  {
    float relativePosition = RELATIVE_CELL_SIZE + RELATIVE_CELL_MARGIN;
    for (int x = -1; x < GameState.CUBE_SIZE - 1; x++)
    {
      for (int y = -1; y < GameState.CUBE_SIZE - 1; y++)
      {
        for (int z = -1; z < GameState.CUBE_SIZE -1; z++)
        {
          Vector3 position = new Vector3(x * relativePosition, y * relativePosition, z * relativePosition);
          GameObject cell = Instantiate(cellPrefab, gameObject.transform, true) as GameObject;
          cell.name = $"Cell ({x+1}, {y+1}, {z+1})";
          cell.transform.position = position;
          cell.transform.localScale = Vector3.one * CELL_SIZE;
          cells[x + 1, y + 1, z + 1] = cell;

          gameState.createCell(x+1, y+1, z+1);
        }
      }

    }
  }

  private void CheckClickOnCell()
  {
    bool isLeftButtonPressed = Input.GetMouseButtonDown((int)MouseButton.Left);
    if (!isLeftButtonPressed)
    {
        return;
    }

    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit hit;
    bool isObjectClicked = Physics.Raycast(ray, out hit);
    if (!isObjectClicked)
    {
      return;
    }

    // Get transform component through the cell collider because first transform
    // detected by the raycast would belong to parent container.
    var name = hit.collider.transform.name;
    Regex regex = new Regex(@"Cell \((\d), (\d), (\d)\)");
    Match match = regex.Match(name);
    if (!match.Success)
    {
      return;
    }

    int x = int.Parse(match.Groups[1].Value);
    int y = int.Parse(match.Groups[2].Value);
    int z = int.Parse(match.Groups[3].Value);

    if (!gameState.isCellAvailable(x, y, z))
    {
      return;
    }

    hit.collider.transform.GetComponent<Renderer>().material.color = gameState.getCurrentColor();
    gameState.selectCell(x, y, z);
    gameState.isLineCompleteAt(x, y, z);
    gameState.endTurn();

  }

  void Update()
    {
        CheckClickOnCell();
        HandleWholeCubeRotation();
    }

  private void HandleWholeCubeRotation()
  {
    if (Input.GetKeyDown(KeyCode.LeftArrow))
    {
      iTween.RotateAdd(gameObject, Vector3.left * 90, ROTATION_TIME_SECONDS);
    }
    else if (Input.GetKeyDown(KeyCode.RightArrow))
    {
      iTween.RotateAdd(gameObject, Vector3.right * 90, ROTATION_TIME_SECONDS);
    }
    else if (Input.GetKeyDown(KeyCode.UpArrow))
    {
      iTween.RotateAdd(gameObject, Vector3.up * 90, ROTATION_TIME_SECONDS);
    }
    else if (Input.GetKeyDown(KeyCode.DownArrow))
    {
      iTween.RotateAdd(gameObject, Vector3.down * 90, ROTATION_TIME_SECONDS);
    }
  }
}