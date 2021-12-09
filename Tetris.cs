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
    private Grid grid;
    bool shake = false;

    //private int numChanges = 0;
    //private List<ChangeObj> currPieceArr;

    // Start is called before the first frame update
    void Start()
    {
        model = new Model();
        grid = new Grid(model);
        model.stepCount = model.regStepSpeed;
        model.container = GameObject.Find("cont");
        //currPieceArr = new List<ChangeObj>();

        GameObject mainLight = GameObject.Find("Directional Light");
        Vector3 rot = new Vector3(-38, -24, 49);
        mainLight.transform.eulerAngles = rot;

        model.m_MainCamera = Camera.main;

        grid.createGrid();
        getNewTetrinome();
        

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
        grid.clearGrid();

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
        grid.checkRow();

    }

    //determins next tetrinome or moves next one
    void getNewTetrinome()
    {
        BlockType obj = model.objects[model.currentObjI];
        obj.reset();//resetting only copies the array. fix that as well
        model.currentO = obj;// new BlockType(obj.shape, obj.texture, obj.pieceNum);
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

        for (int row = o.row; row < model.numRows; row++)
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
        grid.drawGrid();
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