using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

public class BlockType
{
    //public Color colorClass;
    public int row;
    public int col;
    public int z;
    public List<List<List<int>>> origShape;
    public List<List<List<int>>> shape;//3d list
    public Texture2D texture;
    public int pieceNum;


    public BlockType(List<List<List<int>>> _shape, Texture2D _texture, int _pieceNum)//Color _colorClass
    {
        row = 0;
        col = 0;
        z = 0;
        origShape = _shape;
        //colorClass = _colorClass;

        texture = _texture;
        pieceNum = _pieceNum;

    }

    public void reset()
    {
        row = 0;
        col = 0;
        z = 0;
        shape = Utility.copyList(origShape.Count, origShape);
    }
}