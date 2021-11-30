
using UnityEngine;

public class Block
{
    public GameObject entity;
    public bool clearTile;

    public Block(bool _clearTile)
    {
        clearTile = _clearTile;
    }
}
