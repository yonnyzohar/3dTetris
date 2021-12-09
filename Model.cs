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

        int num = 0;

        grid = new List<List<List<Block>>>();
        //
        BlockType b;

        List<List<List<int>>> outer;
        List<List<int>> inner;

        /**/
        ///////////--shape 1---////////////
        outer = new List<List<List<int>>>();

        /**/

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

        b = new BlockType(outer, (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/textures/1.png", typeof(Texture2D)), num);//Color.green
        objects.Add(b);
        num++;

        ///////////--shape 2---////////////
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

        b = new BlockType(outer, (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/textures/2.png", typeof(Texture2D)), num);//Color.red
        objects.Add(b);
        num++;

        ///////////--shape3---//////////////

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

        b = new BlockType(outer, (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/textures/3.png", typeof(Texture2D)), num);//c
        objects.Add(b);
        num++;

        ////////////--shape4--////////////
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

        b = new BlockType(outer, (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/textures/4.png", typeof(Texture2D)), num) ;//Color.yellow
        objects.Add(b);
        num++;

        //////////--shape5--//////////

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

        b = new BlockType(outer, (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/textures/5.png", typeof(Texture2D)),num);//Color.blue
        objects.Add(b);
        num++;
        
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
        b = new BlockType(outer, (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/textures/6.png", typeof(Texture2D)),num);//Color.grey
        objects.Add(b);
        num++;

        ////////////----shape7----///////////////
        outer = new List<List<List<int>>>();

        //slice 1
        inner = new List<List<int>>();
        inner.Add(new List<int> { 0, 0, 0,0 });
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

        b = new BlockType(outer, (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/textures/7.png", typeof(Texture2D)),num);//Color.cyan
        objects.Add(b);
        num++;
        
        //////////--shape6--//////////



    }



}
