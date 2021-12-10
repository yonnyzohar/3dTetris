using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DuplicationRes
{
    public int len;
    public List<List<List<int>>> list;

}

public class Utility
{
    public Utility()
    {

    }

    public static GameObject createBlock(int row, int col, int z, Color color, Transform parent, bool fromGrid = false)
    {
        Material mat = new Material(Shader.Find("Standard"));

        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.DisableKeyword("_ALPHABLEND_ON");
        mat.EnableKeyword("_ALPHAPREMULTIPLY_ON");

        mat.renderQueue = 3000;

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Renderer cubeRenderer = cube.GetComponent<Renderer>();
        cubeRenderer.material = mat;
        cubeRenderer.material.SetColor("_Color", color);
        Vector3 pos = new Vector3(col, row, z);
        cube.transform.position = pos;
        return cube;
    }


    public static bool isColCollision(Model model, int _offsetZ, int _offsetRow, int _offsetCol, List<List<List<int>>> shape)
    {
        bool colission = false;
        Block b;

        for (int z = 0; z < shape.Count; z++)
        {
            List<List<int>> currShape = shape[z];
            for (var row = 0; row < currShape.Count; row++)
            {
                for (var col = 0; col < currShape[row].Count; col++)
                {
                    if (currShape[row][col] == 1)
                    {
                        bool inBounds = Utility.tileInBounds(model, z + _offsetZ, row + _offsetRow, col + _offsetCol);
                        if (inBounds)
                        {
                            b = model.grid[z + _offsetZ][row + _offsetRow][col + _offsetCol];
                            if (b.clearTile == false)
                            {
                                colission = true;
                                break;
                            }
                            else
                            {
                                //all is good
                            }
                        }
                        else
                        {
                            colission = true;
                            break;
                        }
                    }

                }
                if (colission)
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

        if (z > (model.grid.Count - 1)      ||
            col > (model.grid[z][0].Count - 1))
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

    public static DuplicationRes copyList(int count, List<List<List<int>>> l = null)
    {
        DuplicationRes d = new DuplicationRes();
        d.len = 0;

        d.list = new List<List<List<int>>>();
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
                        int num = l[z][row][col];
                        if(num == 1)
                        {
                            d.len++;
                        }
                        inner[row].Add(num);
                    }
                    else
                    {
                        inner[row].Add(0);
                    }
                    
                }
            }
            d.list.Add(inner);
        }
        return d;
    }

    public static List<List<List<int>>> rotateX(List<List<List<int>>> l)
    {
        int count = l.Count;
        DuplicationRes d = copyList(count, l);
        List<List<List<int>>> val = d.list;

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
        int count = l.Count;
        DuplicationRes d = copyList(count, l);
        List<List<List<int>>> val = d.list;

        for (int currRow = count - 1; currRow >= 0; currRow--)
        {
            //go over slice
            for (int z = 0; z < count; z++)
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
