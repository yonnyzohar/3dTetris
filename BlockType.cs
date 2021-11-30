
using System.Collections.Generic;
using UnityEngine;

public class BlockType
{
    public Color colorClass;
    public int row;
    public int col;
    public int z;
    public List<List<List<int>>> shape;//3d list


    public BlockType(List<List<List<int>>> _shape, Color _colorClass)
    {
        row = 0;
        col = 0;
        z = 0;
        shape = _shape;
        colorClass = _colorClass;

    }
}