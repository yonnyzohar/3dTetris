using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ChangeObj
{
    public int z;
    public int row;
    public int col;
}

public class Tetris : MonoBehaviour
{
    private Model model;
    bool shake = false;

    //private int numChanges = 0;
    //private List<ChangeObj> currPieceArr;

    // Start is called before the first frame update
    void Start()
    {
        model = new Model();
        model.stepCount = model.regStepSpeed;
        model.container = GameObject.Find("cont");
        //currPieceArr = new List<ChangeObj>();

        GameObject mainLight = GameObject.Find("Directional Light");
        Vector3 rot = new Vector3(-38, -24, 49);
        mainLight.transform.eulerAngles = rot;

        model.m_MainCamera = Camera.main;
        
        createGrid();
        getNewTetrinome();
        

    }

    void createGrid()
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

                    if (z == model.numZ-1 && row == model.numRows-1 && col == model.numCols / 2)
                    {
                        model.centerPoint = obj.transform;
                    }

                    if(row == model.numRows - 1)
                    {
                        GameObject frame = Utility.createBlock(row+1, col, z, Color.white, model.container.transform);
                    }


                    slice[row].Add(b);

                }
            }

            model.grid.Add(slice);
        }


    }



    void Update()
    {
        logic();
        render();

        if (model.count % model.stepCount == 0)
        {
            model.currentO.row++;
        }

        model.count++;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////--------LOGIC-----------///////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////

    void logic()
    {
        shake = false;
        clearGrid();

        bool getNew = moveCurrentPiece(model.currentO);
        if (getNew)
        {
            getNewTetrinome();
        }

        getHologramPos(model.currentO);

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            onDownPressed();

        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            onUpPressed();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            onRightPressed();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            onLeftPressed();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            rotateX();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            rotateY();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            rotateZ();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            model.stepCount = model.fastStepSpeed;
        }
        checkRow();

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
                    b.pieceNum = -1;
                }
            }
        }

    }


    //determins next tetrinome or moves next one
    void getNewTetrinome()
    {
        BlockType obj = model.objects[model.currentObjI];
        model.currentO = new BlockType(obj.shape, obj.texture, obj.pieceNum);
        model.currentO.col = model.numCols/2;
        model.currentO.z = model.numZ / 2;
        int num = Mathf.FloorToInt(Random.Range(0.0f, 1.0f) * model.objects.Count);
        model.currentObjI = num;

        //miniGrid.init(model.objects[currentObjI]);
       
    }

    //this on counts from 0 to end of grid!
    //is the exact same function as moving pieces - refactor!
    void getHologramPos(BlockType o)
    {
        int hologramRow = 0;

        for (int row = 0; row < model.numRows; row++)
        {
            bool hitABlock = false;

            for (int z = 0; z < o.shape.Count; z++)
            {
                for (int i = 0; i < o.shape.Count; i++)
                {
                    for (int j = 0; j < o.shape.Count; j++)
                    {
                        if (o.shape[z][i][j] == 1)
                        {
                            bool inBounds = Utility.tileInBounds(model, z + o.z, row + i, j + o.col);
                            if (inBounds)
                            {
                                bool clearTile = model.grid[z + o.z][row + i][j + o.col].clearTile;
                                //if i've hit a piece
                                if (!clearTile)
                                {
                                    hologramRow = row - 1;
                                    hitABlock = true;
                                    break;
                                }
                                
                            }
                            else
                            {
                                hitABlock = true;
                                hologramRow = row-1;
                                break;
                            }
                        }
                    }
                    if (hitABlock)
                    {
                        break;
                    }
                }
                if (hitABlock)
                {
                    break;
                }
            }


            if (hitABlock)
            {

                break;
            }
        }

        if (hologramRow != 0 && hologramRow != model.currentO.row)//
        {
            for (int z = 0; z < o.shape.Count; z++)
            {
                for (int i = 0; i < o.shape.Count; i++)
                {
                    for (int j = 0; j < o.shape.Count; j++)
                    {
                        if (o.shape[z][i][j] == 1)
                        {
                            Block b = model.grid[z + o.z][i + hologramRow][j + o.col];
                            bool clearTile = b.clearTile;
                            if (clearTile)
                            {
                                b.pieceNum = 7;

                            }
                        }
                    }
                }
            }

        }
    }

    bool moveCurrentPiece(BlockType o)
    {
        bool getNewElement = false;
        Block b;
        for (int z = 0; z < o.shape.Count; z++)
        {
            List<List<int>> currShape = o.shape[z];
            for (int i = 0; i < currShape.Count; i++)
            {
                for (int j = 0; j < currShape.Count; j++)
                {
                    if (currShape[i][j] == 1)
                    {

                        bool inBounds = Utility.tileInBounds(model, z + o.z, i + o.row, j + o.col);
                        if (inBounds)
                        {
                            b = model.grid[z + o.z][i + o.row][j + o.col];
                            bool clearTile = b.clearTile;
                            //if there is currently something there
                            //we have hit an existing piece!
                            if (!clearTile)
                            {
                                o.row--;
                                getNewElement = true;
                                break;

                            }
                            else
                            {
                                //this tile is clear, draw a block here!
                                b.pieceNum = o.pieceNum;

                            }
                        }
                        else
                        {
                            //check to see that we're not lower than the screen
                            //if we are then we've hit ground!
                            if ((o.row + i) >= model.numRows - 1)
                            {
                                o.row--;

                                getNewElement = true;
                                break;
                            }

                        }
                    }

                    if (getNewElement)
                    {
                        break;
                    }

                }
                if (getNewElement)
                {
                    break;
                }
            }
            if (getNewElement)
            {
                break;
            }
        }

        //imprint!
        if(getNewElement)
        {
            if (model.stepCount == model.fastStepSpeed)
            {
                shake = true;
            }
            model.stepCount = model.regStepSpeed;
            for (int z = 0; z < o.shape.Count; z++)
            {
                List<List<int>> currShape = o.shape[z];

                for (int row = 0; row < currShape.Count; row++)
                {
                    for (int col = 0; col < currShape[row].Count; col++)
                    {
                        if (currShape[row][col] == 1)
                        {
                            bool inBounds = Utility.tileInBounds(model, z + o.z, row + o.row, col + o.col);
                            if (inBounds)
                            {
                                b = model.grid[z + o.z][row + o.row][col + o.col];
                                //b.pieceNum = o.pieceNum;
                                b.clearTile = false;

                            }
                        }
                    }
                }
            }
        }



        return getNewElement;
    }


    bool checkRow()
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


    void removeFilledRows(int _row)
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


    void onLeftPressed()
    {
        if (model.stepCount == model.fastStepSpeed) return;
        if (Utility.isColCollision(model, 0, -1, model.currentO.shape) == false)
        {
            model.currentO.col--;
            moveCurrentPiece(model.currentO);
        }
    }

    void onRightPressed()
    {
        if (model.stepCount == model.fastStepSpeed) return;
        if (Utility.isColCollision(model, 0, 1, model.currentO.shape) == false)
        {
            model.currentO.col++;
            moveCurrentPiece(model.currentO);
        }
    }

    void onDownPressed()
    {
        if (model.stepCount == model.fastStepSpeed) return;
        if (Utility.isColCollision(model, 1, 0, model.currentO.shape) == false)
        {
            model.currentO.z++;
            moveCurrentPiece(model.currentO);
        }


        //
    }

    void onUpPressed()
    {
        if (model.stepCount == model.fastStepSpeed) return;
        if (Utility.isColCollision(model, -1, 0, model.currentO.shape) == false)
        {
            model.currentO.z--;
            moveCurrentPiece(model.currentO);
        }
    }

    void rotateX()
    {
        List<List<List<int>>> tempList = Utility.rotateX(model.currentO.shape);
        var colission = Utility.isColCollision(model, 0, 0, tempList);
        if (!colission)
        {
            model.currentO.shape = tempList;
            moveCurrentPiece(model.currentO);
        }
    }



    void rotateY()
    {
        List<List<List<int>>> tempList = Utility.rotateY(model.currentO.shape);
        var colission = Utility.isColCollision(model, 0, 0, tempList);
        if (!colission)
        {
            model.currentO.shape = tempList;
            moveCurrentPiece(model.currentO);
        }
    }

    void rotateZ()
    {
        List<List<List<int>>> tempList = Utility.copyList(model.currentO.shape.Count, model.currentO.shape);
        Utility.rotateZ(tempList);

        var colission = Utility.isColCollision(model, 0, 0, tempList);

        if (!colission)
        {
            model.currentO.shape = tempList;
            moveCurrentPiece(model.currentO);
        }
    }

    



    ////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////--------RENDER-----------///////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////


    void drawGrid()
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
                        if(b.pieceNum == -1)
                        {
                            o.active = false;
                            cubeRenderer.material.SetTexture("_MainTex", null);
                        }
                        else if(b.pieceNum == 7)
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
                        if(b.pieceNum != -1)
                        {
                            cubeRenderer.material.SetTexture("_MainTex", model.objects[b.pieceNum].texture);
                            cubeRenderer.material.SetColor("_Color", c);
                        }
                        
                    }
                    

                }
            }
        }
    }
    IEnumerator shakeCam(float magnitude, float duration)
    {
        Vector3 origPos = model.m_MainCamera.transform.position;
        float elapsed = 0;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            model.m_MainCamera.transform.position = new Vector3(origPos.x + x, origPos.y + y, origPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        model.m_MainCamera.transform.position = origPos;
    }

    void render()
    {
        if(shake)
        {
           StartCoroutine( shakeCam(Random.Range(.1f, .3f), Random.Range(.1f, .3f)));

        }
        moveCamera();
        drawGrid();
    }

    void moveCamera()
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
        model.m_MainCamera.transform.LookAt(model.centerPoint, Vector3.down);


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

/*
 //goes from bottom up - not good!
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

        if (hologramRow != 0 && hologramRow != model.currentO.row)
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
                            cubeRenderer.material.SetTexture("_MainTex", null);
                            Color c = Color.yellow;// Utility.getColor(o.colorClass);
                            c.a = 0.3f;
                            cubeRenderer.material.SetColor("_Color", c);
                        }
                    }
                }
            }
            
        }
    }
    */