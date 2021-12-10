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

    public List<Vector3> blocksActual; 
    public int blocksCount;

    public Texture2D texture;
    public int pieceNum;
    Model model;
    DuplicationRes d;


    public BlockType(Model _model, List<List<List<int>>> _shape, Texture2D _texture, int _pieceNum)//Color _colorClass
    {
        model = _model;
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
        d = Utility.copyList(origShape.Count, origShape);
        shape = d.list;
        blocksCount = d.len;
        blocksActual  = new List<Vector3>();
        for (int i = 0; i < blocksCount; i++) blocksActual.Add(new Vector3(0,0,0));
        flatten();

    }
    //we dont want to iterate over a 4 X 4 X4 grid. just a small list. this extracts the blocks out of it
    //this function is called every time the 4 X 4 X4 grid is changed
    public void flatten()
    {
        int count = 0;
        for (int z = 0; z < shape.Count; z++)
        {
            List<List<int>> currShape = shape[z];
            for (var row = 0; row < currShape.Count; row++)
            {
                for (var col = 0; col < currShape[row].Count; col++)
                {
                    if (currShape[row][col] == 1)
                    {
                        Vector3 v = blocksActual[count];
                        v.x = col;
                        v.y = row;
                        v.z = z;
                        blocksActual[count] = v;
                        count++;
                    }
                }
            }
        }
    }

    public void rotateX()
    {
        List<List<List<int>>> tempList = Utility.rotateX(this.shape);
        var colission = Utility.isColCollision(model, this.z, this.row, this.col, tempList);
        if (!colission)
        {
            this.shape = tempList;
            flatten();

        }
    }



    public void rotateY()
    {
        
        List<List<List<int>>> tempList = Utility.rotateY(this.shape);
        var colission = Utility.isColCollision(model, this.z, this.row, this.col, tempList);
        if (!colission)
        {
            this.shape = tempList;
            flatten();

        }
    }

    public void rotateZ()
    {
        DuplicationRes d = Utility.copyList(this.shape.Count, this.shape);
        List<List<List<int>>> tempList = d.list;
        Utility.rotateZ(tempList);

        bool colission = Utility.isColCollision(model, this.z, this.row, this.col, tempList);

        if (!colission)
        {
            this.shape = tempList;
            flatten();

        }
    }

    

    public void drawPiece()
    {
        Block b;
       
       for (int i = 0; i < blocksActual.Count; i++)
       {
           Vector3 v = blocksActual[i];
           b = model.grid[this.z + (int)v.z][this.row + (int)v.y][this.col + (int)v.x];
           b.pieceNum = this.pieceNum;
       }
    }
}