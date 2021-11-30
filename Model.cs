using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Model
{
    public float cameraDistance = 20;
    public int regStepSpeed = 100;
    public int fastStepSpeed = 1;
    public int stepCount = 50;
    
    public int numRows = 20;
    public int numCols = 7;
    public int numZ = 7;

    public Object prefab;
    public Camera m_MainCamera;
    public GameObject container;
    public Transform centerPoint;

    public int count = 0;
    
    public  List<BlockType> objects = new List<BlockType>();
    public int currentObjI = 0;
    
    public BlockType currentO;
    public List<List<List<Block>>> grid;

    public Model()
    {
        grid = new List<List<List<Block>>>();
        //
        BlockType b;

        List<List<List<int>>> outer;
        List<List<int>> inner;

        ///////////--shape 1---////////////
        outer = new List<List<List<int>>>();
        inner = new List<List<int>>();
        //slice 1
        inner = new List<List<int>>();
        inner.Add(new List<int> { 0, 0, 0 });
        inner.Add(new List<int> { 0, 0, 0 });
        inner.Add(new List<int> { 0, 0, 0 });
        outer.Add(inner);

        //slice 2
        inner = new List<List<int>>();
        inner.Add(new List<int> { 0, 1, 0 });
        inner.Add(new List<int> { 1, 1, 1 });
        inner.Add(new List<int> { 0, 0, 0 });
        outer.Add(inner);

        //slice 3
        inner = new List<List<int>>();
        inner.Add(new List<int> { 0, 0, 0 });
        inner.Add(new List<int> { 0, 0, 0 });
        inner.Add(new List<int> { 0, 0, 0 });
        outer.Add(inner);

        b = new BlockType(outer, Color.green);
        objects.Add(b);
        ////////////////////////////////////////////////
        
        
        outer = new List<List<List<int>>>();

        //slice 1
        inner = new List<List<int>>();
        inner.Add(new List<int> { 0, 0, 0 });
        inner.Add(new List<int> { 0, 0, 0 });
        inner.Add(new List<int> { 0, 0, 0 });
        outer.Add(inner);

        //slice 2
        inner = new List<List<int>>();
        inner.Add(new List<int> { 0, 1, 1 });
        inner.Add(new List<int> { 1, 1, 0 });
        inner.Add(new List<int> { 0, 0, 0 });
        outer.Add(inner);

        //slice 3
        inner = new List<List<int>>();
        inner.Add(new List<int> { 0, 0, 0 });
        inner.Add(new List<int> { 0, 0, 0 });
        inner.Add(new List<int> { 0, 0, 0 });
        outer.Add(inner);

        b = new BlockType(outer, Color.red);
        objects.Add(b);


        /////////////////////////////////

        outer = new List<List<List<int>>>();

        //slice 1
        inner = new List<List<int>>();
        inner.Add(new List<int> { 0, 0, 0 });
        inner.Add(new List<int> { 0, 0, 0 });
        inner.Add(new List<int> { 0, 0, 0 });
        outer.Add(inner);

        //slice 2
        inner = new List<List<int>>();
        inner.Add(new List<int> { 1, 1, 0 });
        inner.Add(new List<int> { 0, 1, 1 });
        inner.Add(new List<int> { 0, 0, 0 });
        outer.Add(inner);

        //slice 3
        inner = new List<List<int>>();
        inner.Add(new List<int> { 0, 0, 0 });
        inner.Add(new List<int> { 0, 0, 0 });
        inner.Add(new List<int> { 0, 0, 0 });
        outer.Add(inner);

        Color c = new Color();
        c.r = 0.2f;
        c.g = 0.2f;
        c.b = 0.2f;
        c.a = 1;

        b = new BlockType(outer, c);
        objects.Add(b);

        ////////////////////////////////
        

        outer = new List<List<List<int>>>();

        //slice 1
        inner = new List<List<int>>();
        inner.Add(new List<int> { 0, 0, 0 });
        inner.Add(new List<int> { 0, 0, 0 });
        inner.Add(new List<int> { 0, 0, 0 });
        outer.Add(inner);

        //slice 2
        inner = new List<List<int>>();
        inner.Add(new List<int> { 1, 0, 0 });
        inner.Add(new List<int> { 1, 0, 0 });
        inner.Add(new List<int> { 1, 1, 0 });
        outer.Add(inner);

        //slice 3
        inner = new List<List<int>>();
        inner.Add(new List<int> { 0, 0, 0 });
        inner.Add(new List<int> { 0, 0, 0 });
        inner.Add(new List<int> { 0, 0, 0 });
        outer.Add(inner);

        b = new BlockType(outer, Color.yellow);
        objects.Add(b);
        //////////////////////////

        outer = new List<List<List<int>>>();

        //slice 1
        inner = new List<List<int>>();
        inner.Add(new List<int> { 0, 0, 0 });
        inner.Add(new List<int> { 0, 0, 0 });
        inner.Add(new List<int> { 0, 0, 0 });
        outer.Add(inner);

        //slice 2
        inner = new List<List<int>>();
        inner.Add(new List<int> { 0, 0, 1 });
        inner.Add(new List<int> { 0, 0, 1 });
        inner.Add(new List<int> { 0, 1, 1 });
        outer.Add(inner);

        //slice 3
        inner = new List<List<int>>();
        inner.Add(new List<int> { 0, 0, 0 });
        inner.Add(new List<int> { 0, 0, 0 });
        inner.Add(new List<int> { 0, 0, 0 });
        outer.Add(inner);

        b = new BlockType(outer, Color.blue);
        objects.Add(b);


        ////////////////////////////

        outer = new List<List<List<int>>>();

        //slice 1
        inner = new List<List<int>>();
        inner.Add(new List<int> { 1, 1});
        inner.Add(new List<int> { 1, 1});
        outer.Add(inner);

        //slice 2
        inner = new List<List<int>>();
        inner.Add(new List<int> { 1, 1 });
        inner.Add(new List<int> { 1, 1 });
        outer.Add(inner);

     

        b = new BlockType(outer, Color.grey);
        objects.Add(b);

        //////////////////////////////////////
        outer = new List<List<List<int>>>();

        //slice 1
        inner = new List<List<int>>();
        inner.Add(new List<int> { 0, 0,0,0 });
        inner.Add(new List<int> { 0, 0, 0, 0 });
        inner.Add(new List<int> { 0, 0, 0, 0 });
        inner.Add(new List<int> { 0, 0, 0, 0 });
        outer.Add(inner);

        //slice 2
        inner = new List<List<int>>();
        inner.Add(new List<int> { 0, 1, 0, 0 });
        inner.Add(new List<int> { 0, 1, 0, 0 });
        inner.Add(new List<int> { 0, 1, 0, 0 });
        inner.Add(new List<int> { 0, 1, 0, 0 });
        outer.Add(inner);

        //slice 3
        inner = new List<List<int>>();
        inner.Add(new List<int> { 0, 0, 0, 0 });
        inner.Add(new List<int> { 0, 0, 0, 0 });
        inner.Add(new List<int> { 0, 0, 0, 0 });
        inner.Add(new List<int> { 0, 0, 0, 0 });
        outer.Add(inner);

        //slice 4
        inner = new List<List<int>>();
        inner.Add(new List<int> { 0, 0, 0, 0 });
        inner.Add(new List<int> { 0, 0, 0, 0 });
        inner.Add(new List<int> { 0, 0, 0, 0 });
        inner.Add(new List<int> { 0, 0, 0, 0 });
        outer.Add(inner);

        b = new BlockType(outer, Color.cyan);
        objects.Add(b);
    }


    
}
