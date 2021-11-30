using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Utility
{
    public Utility()
    {

    }

    public static Color getColor(Color _c)
    {
        Color c = new Color();
        c.r = _c.r;
        c.g = _c.g;
        c.b = _c.b;
        /*
        if (c == Color.green)
        {
            return Color.green;
        }
        if (c == Color.red)
        {
            return Color.red;
        }
        if (c == Color.magenta)
        {
            return Color.magenta;
        }
        if (c == Color.black)
        {
            return Color.black;
        }
        if (c == Color.blue)
        {
            return Color.blue;
        }
        if (c == Color.grey)
        {
            return Color.grey;
        }
        if (c == Color.cyan)
        {
            return Color.cyan;
        }*/

        return c;

    }

    public static bool isColCollision(Model model, int _z, int _col, List<List<List<int>>> shape)
    {
        bool colission = false;
        bool breakOut = false;
        for (int z = 0; z < shape.Count; z++)
        {
            List<List<int>> currShape = shape[z];
            for (var row = 0; row < currShape.Count; row++)
            {
                for (var col = 0; col < currShape[row].Count; col++)
                {
                    if (currShape[row][col] == 1)
                    {
                        if (Utility.tileInBounds(model, z + model.currentO.z + _z, row + model.currentO.row, col + (model.currentO.col + _col)))
                        {
                            if (model.grid[z + model.currentO.z + _z][row + model.currentO.row][col + (model.currentO.col + _col)].clearTile == false)
                            {
                                colission = true;
                                breakOut = true;
                                break;
                            }
                        }
                        else
                        {
                            colission = true;
                            breakOut = true;
                            break;
                        }
                    }

                }
                if (breakOut)
                {
                    break;
                }
            }
        }


        return colission;
    }

    //just checks that we are not out of screen bounds
    public static bool tileInBounds(Model model, int z, int row, int col)
    {
        bool exists = false;

        if (row < 0 || col < 0 || z < 0) return false;


        if (z > (model.grid.Count - 1) || row > (model.grid[z].Count - 1) || col > (model.grid[z][0].Count - 1))
        {
            return false;
        }

        if (row < model.grid[z].Count)
        {
            if (col < model.grid[z][0].Count)
            {
                exists = true;
            }
        }


        return exists;
    }

    public static List<List<List<int>>> copyList(int count, List<List<List<int>>> l = null)
    {
        List<List<List<int>>> val = new List<List<List<int>>>();
        for (int z = 0; z < count; z++)
        {
            List<List<int>> inner = new List<List<int>>();
            for(int row = 0; row < count; row++)
            {
                inner.Add(new List<int>());
                for (int col = 0; col < count; col++)
                {
                    if(l != null)
                    {
                        inner[row].Add(l[z][row][col]);
                    }
                    else
                    {
                        inner[row].Add(0);
                    }
                    
                }
            }
            val.Add(inner);
        }
        return val;
    }

    public static List<List<List<int>>> rotateX(List<List<List<int>>> l)
    {
        int count = l.Count;
        List<List<List<int>>> val = copyList(count, l);

        for (int z = 0; z < count; z++)
        {
            List<List<int>> slice = l[z];
            //go over slice
            for (int row = 0; row < count; row++)
            {
                //go over last row - > all columns in slice
                //int colCount = slice[currRow].Count - 1;
                for (int col = 0; col < count; col++)
                {
                    val[count - 1 - row][z][col] = l[z][row][col];
                }
            }
        }

        return val;
    }

    //CounterClockWise
    public static List<List<List<int>>> rotateY(List<List<List<int>>> l)
    {
        List<List<List<int>>> val = copyList(l.Count, l);

        for (int currRow = l.Count - 1; currRow >= 0; currRow--)
        {
            //go over slice
            for (int z = 0; z < l.Count; z++)
            {
                List<List<int>> slice = l[z];

                //go over last row - > all columns in slice
                int colCount = slice[currRow].Count-1;
                for (int col =  0; col <= colCount; col++)
                {
                    val[col][currRow][colCount - z] = l[z][currRow][col];
                }
            }
        }

        return val;
    }

    //CounterClockwise
    public static void rotateZ(List<List<List<int>>> l)
    {
        
        for (int z = 0; z < l.Count; z++)
        {
            List<List<int>> a = l[z];
            int n = a.Count;
            for (int i = 0; i < n / 2; i++)
            {
                for (int j = i; j < n - i - 1; j++)
                {
                    int tmp = a[i][j];
                    a[i][j] = a[j][n - i - 1];
                    a[j][n - i - 1] = a[n - i - 1][n - j - 1];
                    a[n - i - 1][n - j - 1] = a[n - j - 1][i];
                    a[n - j - 1][i] = tmp;
                }
            }
        }
        
    }

    /*
    public static void rotateZClockwise(List<List<List<int>>> l)
    {
        for (int z = 0; z < l.Count; z++)
        {
            List<List<int>> a = l[z];
            int n = a.Count;
            for (int i = 0; i < n / 2; i++)
            {
                for (int j = i; j < n - i - 1; j++)
                {
                    int tmp = a[i][j];
                    a[i][j] = a[n - j - 1][i];
                    a[n - j - 1][i] = a[n - i - 1][n - j - 1];
                    a[n - i - 1][n - j - 1] = a[j][n - i - 1];
                    a[j][n - i - 1] = tmp;
                }
            }

        }
        
    }
    */
}
