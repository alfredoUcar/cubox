using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public enum Player {
    Player1,
    Player2,
}

[CreateAssetMenu(fileName = "GameState", menuName = "States/GameState")]
public class GameState : ScriptableObject
{
    public int currentPlayer = 1;
    public Color playerOneColor = new Color(255, 0, 0);
    public Color playerTwoColor = new Color(0, 0, 255);
    public Color defaultColor = new Color(255, 255, 255);

    public const int dimensions = 3;
    public int cellSize = 1;
    public float cellMargin = 0.2f;
    
    private Cell[,,] cells;

    public Color getCurrentColor()
    {
        if (currentPlayer == 1) {
            return playerOneColor;
        } else {
            return playerTwoColor;
        }
    }

    public void endTurn()
    {
        if (currentPlayer == 1)
        {
            currentPlayer = 2;
        } else {
            currentPlayer = 1;
        }
    }

    public void createCell(int x, int y, int z)
    {
        cells.SetValue(new Cell(), x, y, z);
    }

    public void setCellOwner(Player owner, int x, int y, int z)
    {
        // var cell = cells.GetValue(x, y, z)
    }
}
