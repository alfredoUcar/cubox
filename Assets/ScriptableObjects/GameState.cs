using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public const int CUBE_SIZE = 3;
    public const int DIMENSIONS = 3;
    private Cell[,,] cells;

    private Boolean[,] cellAdjacency;

    public GameState()
    {
        cells = new Cell[CUBE_SIZE, CUBE_SIZE, CUBE_SIZE];
        cellAdjacency = new Boolean[TotalCells, TotalCells];
    }
  public int TotalCells { get => (int)Math.Pow(CUBE_SIZE, DIMENSIONS); }

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
        cells.SetValue(new Cell(x, y, z), x, y, z);
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

    public bool isLineCompleteAt(int x, int y, int z)
    {
        Cell firstCell = cells[x, y, z];

        Player? player = firstCell.getOwner();
        if (player == null)
        {
            return false;
        }

        Cell[] playerAdjacentCells = getAdjacentCells(firstCell).Where(cell => cell.getOwner() == player).ToArray();

        foreach (Cell secondCell in playerAdjacentCells) {
             Cell[] playerNextAdjacentCells = getAdjacentCells(secondCell).Where(cell => cell.getOwner() == player && cell.index != firstCell.index).ToArray();
             foreach (Cell thirdCell in playerNextAdjacentCells) {
                if (firstCell.isAlignedTo(secondCell, thirdCell)) {
                    return true;
                }
             }
        }

        return false;
    }

    public void calculateAdjacency()
    {
        foreach (Cell cell in cells)
        {
            calculateAdjacentCells(cell);
        }
    }

    private void calculateAdjacentCells(Cell cell)
    {
        int[] offsets = {-1, 0, 1};
        Cell adjacent;

        foreach (int offsetX in offsets)
        {
            foreach (int offsetY in offsets)
            {
                foreach (int offsetZ in offsets)
                {
                    if (offsetX == 0 && offsetY == 0 && offsetZ == 0)
                    {
                        continue; // it's the same cell
                    }

                    try {
                        adjacent = cells[cell.x+offsetX, cell.y+offsetY, cell.z+offsetZ];
                    } catch (IndexOutOfRangeException) {
                        continue; // coordinates are out of cube boundaries
                    }

                    setAdjacency(cell, adjacent);
                }
            }
        }
    }

    private void setAdjacency(Cell a, Cell b)
    {
        cellAdjacency[a.index, b.index] = true;
        cellAdjacency[b.index, a.index] = true;
    }

    private Cell[] getAdjacentCells(Cell cell)
    {
        List<Cell> adjacentCells = new List<Cell>();

        for (int i = 0; i < TotalCells; i++)
        {
            if (cellAdjacency[cell.index, i])
            {
                Vector3 position = Cell.getPosition(i);
                Cell adjacentCell = cells[(int)position.x, (int)position.y, (int)position.z];
                adjacentCells.Add(adjacentCell);
            }
        }
        return adjacentCells.ToArray();
    }
}
