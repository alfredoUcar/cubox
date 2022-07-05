using System;
using UnityEngine;

public class CellAlreadyAssigned : Exception
{
    public CellAlreadyAssigned() {}
    public CellAlreadyAssigned(string message) : base(message) {}
    public CellAlreadyAssigned(string message, Exception innerException) : base(message, innerException) {}
}

public class Cell
{
    private Player? owner;
    private int _x;
    private int _y;
    private int _z;

    public int x { get => _x; }
    public int y { get => _y; }
    public int z { get => _z; }

    public int index { get => (int) ((x * Math.Pow(3, 2)) + (y * Math.Pow(3, 1)) + (z * Math.Pow(3, 0))); }


  public Cell(int x, int y, int z)
    {
        this._x = x;
        this._y = y;
        this._z = z;
    }

    public void assignOwner(Player newOwner)
    {
        if (this.owner != null)
        {
            throw new CellAlreadyAssigned("Cell already has an owner");
        }

        this.owner = newOwner;
    }

    public Player? getOwner()
    {
        return owner;
    }

    public static Vector3 getPosition(int index)
    {
        int quotient;

        int z = index % 3;
        quotient = index / 3;

        int y = quotient % 3;
        quotient /= 3;

        int x = quotient % 3;

        return new Vector3(x, y, z);
    }

    public bool isAlignedTo(Cell cell1, Cell cell2) 
    {
        Vector3 a = new Vector3(this.x, this.y, this.z);
        Vector3 b = new Vector3(cell1.x, cell1.y, cell1.z);
        Vector3 c = new Vector3(cell2.x, cell2.y, cell2.z);
        
        Vector3 ab = b - a;
        Vector3 bc = c - b;

        Vector3 crossProduct = Vector3.Cross(ab, bc);
        return crossProduct == Vector3.zero;
    }
}
