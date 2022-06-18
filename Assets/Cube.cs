using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameState gameState;
    private GameObject[,,] cells = new GameObject[3,3,3];

  void Start()
  {
    CreateCells();
  }

  public void CreateCells()
  {
    for (int x = 0; x < GameState.dimensions; x++)
    {
      for (int y = 0; y < GameState.dimensions; y++)
      {
        for (int z = 0; z < GameState.dimensions; z++)
        {
          Vector3 position = new Vector3(x + x * gameState.cellMargin, y + y * gameState.cellMargin, z + z * gameState.cellMargin);
          GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity) as GameObject;
          cell.name = $"Cell {x}-{y}-{z}";
          cell.transform.SetParent(this.gameObject.transform);
          cells[x, y, z] = cell; 

          // gameState.createCell(x, y, z);
        }
      }

    }
  }

  private void CheckClickOnCell()
  {
    bool isLeftButtonPressed = Input.GetMouseButtonDown(0);
    if (!isLeftButtonPressed)
    {
        return;
    }

    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit hit;
    bool isObjectClicked = Physics.Raycast(ray, out hit);
    if (!isObjectClicked || !hit.transform.name.StartsWith("Cell "))
    {
        return;
    }

    hit.transform.GetComponent<Renderer>().material.color = gameState.getCurrentColor();
    gameState.endTurn();
    
  }

  void Update()
    {
        CheckClickOnCell();   
    }
}
