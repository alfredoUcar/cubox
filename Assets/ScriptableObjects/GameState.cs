using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Animations;

public enum Player {
    Player1,
    Player2,
}

[CreateAssetMenu(fileName = "GameState", menuName = "States/GameState")]
public class GameState : ScriptableObject
{
    public Player currentPlayer = Player.Player1;
    public Color playerOneColor = new Color(255, 0, 0);
    public Color playerTwoColor = new Color(0, 0, 255);
    public Color defaultColor = new Color(255, 255, 255);

    public const int dimensions = 3;
    
    private Cell[,,] cells;

    public GameState()
    {
        cells = new Cell[3, 3, 3];
    }

    public Color getCurrentColor()
    {
        if (currentPlayer == Player.Player1) {
            return playerOneColor;
        } else {
            return playerTwoColor;
        }
    }

    public void endTurn()
    {
        if (currentPlayer == Player.Player1)
        {
            currentPlayer = Player.Player2;
        } else {
            currentPlayer = Player.Player1;
        }
    }

    public void createCell(int x, int y, int z)
    {
        cells.SetValue(new Cell(), x, y, z);
    }

    public void selectCell(int x, int y, int z)
    {
        Cell cell = cells[x, y, z];
        cell.assignOwner(currentPlayer);
    }

    public bool isCellAvailable(int x, int y, int z)
    {
        return cells[x, y, z].getOwner() == null;
    }
}
