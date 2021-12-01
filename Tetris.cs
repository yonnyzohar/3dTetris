using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Tetris : MonoBehaviour
{
    private Model model;
    
    // Start is called before the first frame update
    void Start()
    {
        model = new Model();
        model.stepCount = model.regStepSpeed;
        model.container = GameObject.Find("cont");

        GameObject mainLight = GameObject.Find("Directional Light");
        Vector3 rot = new Vector3(-38, -24, 49);
        mainLight.transform.eulerAngles = rot;

        //pos.transform.position = new Vector3(numCols/2, numRows / 2, 0);


        model.m_MainCamera = Camera.main;
        //model.prefab = AssetDatabase.LoadAssetAtPath("Assets/prefabs/Cube.prefab", typeof(GameObject));
        
        createGrid();
        getNewTetrinome();
        

    }

    void showHologram(BlockType o)
    {
        int hologramRow = 0;

        for(int row = model.numRows -1; row >= 0; row--)
        {
            bool foundEmptyArea = true;

            for (int z = 0; z < o.shape.Count; z++)
            {
                for (int i = 0; i < o.shape.Count; i++)
                {
                    for (int j = 0; j < o.shape.Count; j++)
                    {
                        if (o.shape[z][i][j] == 1)
                        {
                            bool inBounds = Utility.tileInBounds(model, z + o.z, row + i, j + o.col);
                            if (!inBounds)
                            {
                                foundEmptyArea = false;
                                break;
                            }
                            bool clearTile = model.grid[z + o.z][row + i][j + o.col].clearTile;
                            if (!clearTile)
                            {
                                foundEmptyArea = false;
                                break;
                            }
                        }
                    }
                    if (!foundEmptyArea)
                    {
                        break;
                    }
                }
                if (!foundEmptyArea)
                {
                    break;
                }
            }

            
            if(foundEmptyArea)
            {
                hologramRow = row;
                break;
            }
        }

        if (hologramRow != 0)
        {
            for (int z = 0; z < o.shape.Count; z++)
            {
                for (int i = 0; i < o.shape.Count; i++)
                {
                    for (int j = 0; j < o.shape.Count; j++)
                    {
                        if (o.shape[z][i][j] == 1)
                        {
                            GameObject obj = model.grid[z + o.z][i + hologramRow][j + o.col].entity;
                            obj.active = true;
                            Renderer cubeRenderer = obj.GetComponent<Renderer>();
                            Color c = Color.yellow;// Utility.getColor(o.colorClass);
                            c.a = 0.3f;
                            cubeRenderer.material.SetColor("_Color", c);
                        }
                    }
                }
            }
            
        }
    }

    bool isTransparent(int z, int row, int col)
    {
        bool isTrans = true;
        if (
            //row == 0 ||
            (col == 0 && z == 0) ||
            (col == 0 && z == model.numZ - 1 ) ||
            row == model.numRows-1
        //z == 0 //||
        // row == model.grid[z][row].Count, 
        )
        {
            isTrans = false;
        }
        
        return isTrans;
    }

    void createGrid()
    {
        Vector3 pos = model.container.transform.position;
        pos.x -= model.numCols / 2;
        pos.z -= model.numZ / 2;

        model.container.transform.position = pos;
        // List<List<List<Block>>>()

        for (int z = 0; z < model.numZ; z++)
        {
            List<List<Block>> slice = new List<List<Block>>();
            for (int row = 0; row < model.numRows; row++)
            {
                slice.Add(new List<Block>());

                for (int col = 0; col < model.numCols; col++)
                {
                    Block b = new Block(true);
                    Color c = Color.white;// getGridColor(z, row, col); 
                    c.a = 0.1f;
                    GameObject obj = createBlock(row, col, z, c, model.container.transform);
                    //if(isTransparent(z, row, col))
                   // {
                      //  obj.active = false;
                    //}
                 
                    b.entity = obj;

                    if (z == model.numZ-1 && row == model.numRows-1 && col == model.numCols / 2)
                    {
                        model.centerPoint = obj.transform;
                    }

                    if(row == model.numRows - 1)
                    {
                        GameObject frame = createBlock(row+1, col, z, Color.white, model.container.transform);
                    }


                    slice[row].Add(b);

                    //setCheckeredTile(row, col, b);

                }
            }

            model.grid.Add(slice);
        }


    }

    void Update()
    {
        
        model.cameraDistance -= Input.mouseScrollDelta.y;
        Vector3 mousePos = Input.mousePosition;
        float xPer = mousePos.x / Screen.width;
        float yPer = mousePos.y / Screen.height;

        int degree = (int)(360 * xPer);
        float rad = degree * Mathf.PI / 180;
        float x = Mathf.Cos(rad) * model.cameraDistance + model.centerPoint.transform.position.x;
        float z = Mathf.Sin(rad) * model.cameraDistance + model.centerPoint.transform.position.z;

        model.m_MainCamera.transform.position = new Vector3(x, (model.cameraDistance * yPer) * -1, z);
        model.m_MainCamera.transform.LookAt(model.centerPoint, Vector3.down );

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            onDownPressed();
            showHologram(model.currentO);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            onUpPressed();
            showHologram(model.currentO);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            onRightPressed();
            showHologram(model.currentO);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {

            onLeftPressed();
            showHologram(model.currentO);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            
            rotateX();
            showHologram(model.currentO);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            rotateY();
            showHologram(model.currentO);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            rotateZ();
            showHologram(model.currentO);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            model.stepCount = model.fastStepSpeed;
        }


        if (model.count % model.stepCount == 0)
        {
            
            bool getNew = drawObject(model.currentO);
            if(getNew)
            {
                getNewTetrinome();
            }
            showHologram(model.currentO);

            model.currentO.row++;
            checkRow();
        }
        model.count++;

    }

    

    //determins next tetrinome or moves next one
    void getNewTetrinome()
    {
        BlockType obj = model.objects[model.currentObjI];
        model.currentO = new BlockType(obj.shape, obj.texture);
        model.currentO.col = 0;
        int num = Mathf.FloorToInt(Random.Range(0.0f, 1.0f) * model.objects.Count);
        model.currentObjI = num;

        //miniGrid.init(model.objects[currentObjI]);
       
    }

    //cleans the entire grid
    void clearGrid()
    {
        for (int z = 0; z < model.grid.Count; z++)
        {
            List<List<Block>> slice = model.grid[z];
            for (var row = 0; row < slice.Count; row++)
            {
                for (var col = 0; col < slice[row].Count; col++)
                {
                    Block b = slice[row][col];
                    GameObject o = slice[row][col].entity;
                    if (b.clearTile)
                    {
                        o.active = false;
                        //if (isTransparent(z, row, col))
                        // {
                        // 
                        //}

                        //Renderer cubeRenderer = o.GetComponent<Renderer>();
                        //Color c = getGridColor(z, row, col);
                        //cubeRenderer.material.SetColor("_Color", c);
                    }
                    else
                    {
                        o.active = true;
                    }

                }
            }
        }
        
    }
    /* */
    // Update is called once per frame
    

    

    GameObject createBlock(int row, int col, int z ,Color color, Transform parent, bool fromGrid = false)
    {
        Material mat = new Material(Shader.Find("Standard"));
        
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.DisableKeyword("_ALPHABLEND_ON");
        mat.EnableKeyword("_ALPHAPREMULTIPLY_ON");

        


        mat.renderQueue = 3000;
        /* 

         */

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Renderer cubeRenderer = cube.GetComponent<Renderer>();
        cubeRenderer.material = mat;
        cubeRenderer.material.SetColor("_Color", color);
        Vector3 pos = new Vector3(col, row, z);
        cube.transform.position = pos;
        return cube;
    }

    void imprintElementOnGrid(BlockType o)
    {
        for(int z = 0; z < o.shape.Count; z++)
        {
            List<List<int>> currShape = o.shape[z];

            for (int row = 0; row < currShape.Count; row++)
            {
                for (int col = 0; col < currShape[row].Count; col++)
                {
                    if (currShape[row][col] == 1)
                    {
                        if (Utility.tileInBounds(model, z + o.z, row + o.row, col + o.col))
                        {
                            //GameObject obj =  createBlock(i + o.row, j + o.col, o.colorClass, model.container.transform);
                            GameObject obj = model.grid[z + o.z][row + o.row][col + o.col].entity;
                            obj.active = true;
                            Renderer cubeRenderer = obj.GetComponent<Renderer>();
                            cubeRenderer.material.SetTexture("_MainTex", o.texture);
                            Color c = Color.white;// cubeRenderer.material.GetColor("_Color");
                            c.a = 1f;
                            cubeRenderer.material.SetColor("_Color", c);
                            //cubeRenderer.material.SetColor("_Color", o.colorClass);
                            model.grid[z + o.z][row + o.row][col + o.col].clearTile = false;
                        }
                    }
                }
            }
        }
        
    }


    bool drawObject(BlockType o)
    {
        bool getNewElement = false;
        clearGrid();
        bool doBreak = false;
        for (int z = 0; z < o.shape.Count; z++)
        {
            List<List<int>> currShape = o.shape[z];
            for (int i = 0; i < currShape.Count; i++)
            {
                for (int j = 0; j < currShape.Count; j++)
                {
                    if (currShape[i][j] == 1)
                    {
                        if (Utility.tileInBounds(model, z + o.z, i + o.row, j + o.col))
                        {
                            //if there is currently something there
                            //we have hit an existing piece!
                            if (model.grid[z + o.z][i + o.row][j + o.col].clearTile == false)
                            {
                                o.row--;

                                imprintElementOnGrid(o);
                                clearGrid();
                                getNewElement = true;
                                model.stepCount = model.regStepSpeed;
                                doBreak = true;
                                break;

                            }
                            else
                            {
                                //this tile is clear, draw a block here!

                                GameObject obj = model.grid[z + o.z][i + o.row][j + o.col].entity;
                                Renderer cubeRenderer = obj.GetComponent<Renderer>();
                                cubeRenderer.material.SetTexture("_MainTex", o.texture);
                                Color c = Color.white;// cubeRenderer.material.GetColor("_Color");
                                c.a = 1f;
                                cubeRenderer.material.SetColor("_Color", c);
                                obj.active = true;
                            }
                        }
                        else
                        {
                            //check to see that we're not lower than the screen
                            //if we are the we've hit ground!
                            if ((o.row + i) >= model.grid[z].Count - 1)
                            {
                                o.row--;
                                imprintElementOnGrid(o);
                                clearGrid();
                                getNewElement = true;
                                model.stepCount = model.regStepSpeed;
                                doBreak = true;
                                break;
                            }
                            /*
                             //dont understand what this means
                            if ((o.col + i) <= 0)
                            {
                                o.col++;
                                getNewElement = drawObject(model.currentO);
                                doBreak = true;
                                break;
                            }

                            if ((o.col + i) >= model.grid[z][i].Count - 1)
                            {
                                o.col--;
                                getNewElement = drawObject(model.currentO);
                                doBreak = true;
                                break;

                            }
                            */
                        }
                    }

                    if (doBreak)
                    {
                        break;
                    }

                }
                if (doBreak)
                {
                    break;
                }
            }
            if (doBreak)
            {
                break;
            }
        }

        

        return getNewElement;
    }

    void checkRow()
    {


        for (int currRow = model.grid[0].Count - 1; currRow >= 0; currRow--)
        {
            bool breakLoop = false;

            //go over slice
            for (int z = 0; z < model.grid.Count; z++)
            {
                List<List<Block>> slice = model.grid[z];

                //go over last row - > all columns in slice
                for (int col = slice[currRow].Count - 1; col >= 0; col--)
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
                break;
            }
        }
    }


    void removeFilledRows(int _row)
    {
        //first remove the entire row, then drop the rest down
        for (int z = 0; z < model.grid.Count; z++)
        {
            List<List<Block>> slice = model.grid[z];
            for (int col = slice[_row].Count - 1; col >= 0; col--)
            {
                slice[_row][col].clearTile = true;
            }
        }
        
        clearGrid();
        for (int z = 0; z < model.grid.Count; z++)
        {
            List<List<Block>> slice = model.grid[z];
            for (int row = _row; row >= 0; row--)
            {
                for (int col = slice[row].Count - 1; col >= 0; col--)
                {
                    if (Utility.tileInBounds(model, z, row - 1, col))
                    {
                        GameObject upperEntity = slice[row - 1][col].entity;
                        Renderer topRenderer = upperEntity.GetComponent<Renderer>();

                        GameObject currEntity = slice[row][col].entity;
                        Renderer currRenderer = currEntity.GetComponent<Renderer>();

                        //currRenderer.material.SetColor("_Color", topRenderer.material.color);
                        currRenderer.material.SetTexture("_MainTex", topRenderer.material.GetTexture("_MainTex"));
                        slice[row][col].clearTile = slice[row - 1][col].clearTile;

                        Color c = Color.white;
                        topRenderer.material.SetColor("_Color", c);

                    }
                }
            }
        }
        
        checkRow();
    }

    ///------- <Inputs> ------///

    void onLeftPressed()
    {
        if (model.stepCount == model.fastStepSpeed) return;
        if (Utility.isColCollision(model,0, -1, model.currentO.shape) == false)
        {
            model.currentO.col--;
            drawObject(model.currentO);
        }
    }

    void onRightPressed()
    {
        if (model.stepCount == model.fastStepSpeed) return;
        if (Utility.isColCollision(model,0, 1, model.currentO.shape) == false)
        {
            model.currentO.col++;
            drawObject(model.currentO);
        }
    }

    void onDownPressed()
    {
        if (model.stepCount == model.fastStepSpeed) return;
        if (Utility.isColCollision(model, 1, 0, model.currentO.shape) == false)
        {
            model.currentO.z++;
            drawObject(model.currentO);
        }

       
        //
    }

    void onUpPressed()
    {
        if (model.stepCount == model.fastStepSpeed) return;
        if (Utility.isColCollision(model, -1, 0, model.currentO.shape) == false)
        {
            model.currentO.z--;
            drawObject(model.currentO);
        }
    }

    void rotateX()
    {
        List<List<List<int>>> tempList = Utility.rotateX(model.currentO.shape);
        var colission = Utility.isColCollision(model, 0, 0, tempList);
        if (!colission)
        {
            model.currentO.shape = tempList;
            drawObject(model.currentO);
        }
    }



    void rotateY()
    {
        List<List<List<int>>> tempList = Utility.rotateY(model.currentO.shape);
        var colission = Utility.isColCollision(model, 0, 0, tempList);
        if (!colission)
        {
            model.currentO.shape = tempList;
            drawObject(model.currentO);
        }
    }

    void rotateZ()
    {
        List<List<List<int>>> tempList = Utility.copyList(model.currentO.shape.Count, model.currentO.shape);
        Utility.rotateZ(tempList);
        
        var colission = Utility.isColCollision(model,0,0, tempList);

        if (!colission)
        {
            model.currentO.shape = tempList;
            drawObject(model.currentO);
        }
    }
}


/*
 function Game()
{

    var miniGrid = new MiniGrid();


	function setCheckeredTile(row, col,  b)
	{
		if(row % 2 == 0)
		{
			if (col % 2 == 0)
			{
				 b.entity = "block";
			}
			else
			{
				b.entity = "block2";
			}
		}
		else
		{
			if ( col % 2 == 0)
			{
				 b.entity = "block2";
			}
			else
			{
				b.entity = "block";
			}
		}
	}

}
*/