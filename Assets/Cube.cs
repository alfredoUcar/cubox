using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Threading;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
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
    if (!hit.collider.transform.name.StartsWith("Cell "))
    {
      return;
    }

    hit.collider.transform.GetComponent<Renderer>().material.color = gameState.getCurrentColor();
    gameState.endTurn();
    
  }

  void Update()
    {
        CheckClickOnCell();   
        HandleWholeCubeRotation();
    }
  
  private void HandleWholeCubeRotation()
  {

    Vector3 rotationToAdd = new Vector3(0, 0, 0);

    if (Input.GetKeyDown(KeyCode.LeftArrow))
    {
      rotationToAdd = new Vector3(-90, 0, 0);
    } 
    else if (Input.GetKeyDown(KeyCode.RightArrow))
    {
      rotationToAdd = new Vector3(90, 0, 0);
    }
    else if (Input.GetKeyDown(KeyCode.UpArrow))
    {
      rotationToAdd = new Vector3(0, 90, 0);
    }
    else if (Input.GetKeyDown(KeyCode.DownArrow))
    {
      rotationToAdd = new Vector3(0, -90, 0);
    }

    Rigidbody rigidBody = gameObject.GetComponent<Rigidbody>();
    Quaternion deltaRotation = Quaternion.Euler(rotationToAdd);
    rigidBody.MoveRotation(rigidBody.rotation * deltaRotation);
  }
}