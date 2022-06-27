using System;
using System.Collections;
using System.Collections.Generic;
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
}
