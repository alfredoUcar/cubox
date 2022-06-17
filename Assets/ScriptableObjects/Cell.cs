using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : ScriptableObject
{
    private Player? owner;

    public void assignOwner(Player newOwner)
    {
        if (this.owner != null)
        {
            throw new Exception("Cell already has an owner");
        }

        this.owner = newOwner;
    }
}
