
using UnityEngine;

public class Block
{
    public GameObject entity;
    public bool clearTile;
    public int pieceNum;

    public Block(bool _clearTile)
    {
        clearTile = _clearTile;
        pieceNum = 0;
    }
}
