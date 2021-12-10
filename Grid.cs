using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    Model model;

    public Grid(Model _model)
    {
        model = _model;
    }

    public void imprint()
    {
        BlockType o = model.currTetrinom;
        Block b;

        for (int i = 0; i < o.blocksActual.Count; i++)
        {
            Vector3 v = o.blocksActual[i];
            b = model.grid[o.z + (int)v.z][o.row + (int)v.y][o.col + (int)v.x];
            b.clearTile = false;
        }

       
    }

    //cleans the entire grid
    public void clearGrid()
    {
        Block b;
        

        BlockType o = model.currTetrinom;
        BlockType h = model.hologram;

        for (int i = 0; i < o.blocksActual.Count; i++)
        {
            Vector3 v = o.blocksActual[i];
            int currZ = o.z + (int)v.z;
            int currCol = o.col + (int)v.x;

            int pieceRow = o.row + (int)v.y;
            int holoRow = h.row + (int)v.y;


            b = model.grid[currZ][pieceRow][currCol];
            b.pieceNum = -1;

            if (holoRow != 0 && holoRow != pieceRow)
            {
                b = model.grid[currZ][holoRow][currCol];
                b.pieceNum = -1;
            }
        }

    }

    public bool checkRow()
    {
        for (int currRow = model.numRows - 1; currRow >= 0; currRow--)
        {
            bool breakLoop = false;

            //go over slice
            for (int z = 0; z < model.numZ; z++)
            {
                List<List<Block>> slice = model.grid[z];

                //go over last row - > all columns in slice
                for (int col = model.numCols - 1; col >= 0; col--)
                {
                    //this means the column is not full
                    if (slice[currRow][col].clearTile == true)
                    {
                        breakLoop = true;
                        break;
                    }
                }
                if (breakLoop)
                {
                    break;
                }

            }
            //this means the floor is completely full
            if (!breakLoop)
            {
                removeFilledRows(currRow);
                return true;
            }
        }
        return false;
    }

    public void drawGrid()
    {
        Renderer cubeRenderer;
        Color c = Color.white;
        c.a = 1f;

        Color y = Color.yellow;
        y.a = 0.3f;

        for (int z = 0; z < model.grid.Count; z++)
        {
            List<List<Block>> slice = model.grid[z];
            for (var row = 0; row < slice.Count; row++)
            {
                for (var col = 0; col < slice[row].Count; col++)
                {
                    Block b = slice[row][col];

                    /**/
                    GameObject o = b.entity;
                    cubeRenderer = o.GetComponent<Renderer>();

                    if (b.clearTile)
                    {
                        if (b.pieceNum == -1)
                        {
                            o.active = false;
                            cubeRenderer.material.SetTexture("_MainTex", null);
                        }
                        else if (b.pieceNum == 7)
                        {
                            o.active = true;
                            cubeRenderer.material.SetTexture("_MainTex", null);
                            cubeRenderer.material.SetColor("_Color", y);
                        }
                        else
                        {
                            o.active = true;
                            cubeRenderer.material.SetTexture("_MainTex", model.objects[b.pieceNum].texture);
                            cubeRenderer.material.SetColor("_Color", c);
                        }

                    }
                    else
                    {
                        o.active = true;
                        if (b.pieceNum != -1)
                        {
                            //color the piece
                            cubeRenderer.material.SetTexture("_MainTex", model.objects[b.pieceNum].texture);
                            cubeRenderer.material.SetColor("_Color", c);
                        }
                    }
                }
            }
        }
    }

    public void removeFilledRows(int _row)
    {
        Block b;
        Block topB;
        //first remove the entire row, then drop the rest down
        for (int z = 0; z < model.grid.Count; z++)
        {
            List<List<Block>> slice = model.grid[z];

            for (int col = slice[_row].Count - 1; col >= 0; col--)
            {
                b = slice[_row][col];
                b.clearTile = true;
            }
        }

        for (int z = 0; z < model.grid.Count; z++)
        {
            List<List<Block>> slice = model.grid[z];
            for (int row = _row; row >= 0; row--)
            {
                for (int col = slice[row].Count - 1; col >= 0; col--)
                {
                    if (Utility.tileInBounds(model, z, row - 1, col))
                    {
                        b = slice[row][col];
                        topB = slice[row - 1][col];

                        b.clearTile = topB.clearTile;
                        b.pieceNum = topB.pieceNum;

                    }
                }
            }
        }
    }

    public void createGrid()
    {
        Vector3 pos = model.container.transform.position;
        pos.x -= model.numCols / 2;
        pos.z -= model.numZ / 2;

        model.container.transform.position = pos;

        for (int z = 0; z < model.numZ; z++)
        {
            List<List<Block>> slice = new List<List<Block>>();
            for (int row = 0; row < model.numRows; row++)
            {
                slice.Add(new List<Block>());

                for (int col = 0; col < model.numCols; col++)
                {
                    Block b = new Block(true);
                    b.pieceNum = -1;
                    Color c = Color.white;// getGridColor(z, row, col); 
                    c.a = 0.1f;
                    GameObject obj = Utility.createBlock(row, col, z, c, model.container.transform);

                    b.entity = obj;

                    if (z == model.numZ - 1 && row == model.numRows - 1 && col == model.numCols / 2)
                    {
                        model.centerPoint = obj.transform;
                    }

                    if (row == model.numRows - 1)
                    {
                        GameObject frame = Utility.createBlock(row + 1, col, z, Color.white, model.container.transform);
                    }


                    slice[row].Add(b);

                }
            }

            model.grid.Add(slice);
        }


    }

}
