using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

public class BlockType
{
    //public Color colorClass;
    public int row;
    public int col;
    public int z;
    public List<List<List<int>>> shape;//3d list
    public Texture2D texture;


    public BlockType(List<List<List<int>>> _shape, Texture2D _texture)//Color _colorClass
    {
        row = 0;
        col = 0;
        z = 0;
        shape = _shape;
        //colorClass = _colorClass;

        texture = _texture; 

    }
}