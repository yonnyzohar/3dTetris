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
    private float time = 0;
    


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
        

        time += Time.deltaTime;

        if (time >= model.stepCount)
        {
            grid.clearGrid();
            model.currTetrinom.row++;
            bool colission = Utility.isColCollision(model, model.currTetrinom.z, model.currTetrinom.row, model.currTetrinom.col, model.currTetrinom.shape);
            if(!colission)
            {
                model.currTetrinom.drawPiece();
            }
            else
            {
                model.currTetrinom.row--;
                grid.imprint();
                if (model.stepCount == model.fastStepSpeed)
                {
                    model.shake = true;
                }
                model.stepCount = model.regStepSpeed;
                getNewTetrinome();
            }

            getHologramPos();

            time -= model.stepCount;
            
        }

        render();

    }

    ////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////--------LOGIC-----------///////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////

    void logic()
    {
        model.shake = false;
        
        

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
            grid.clearGrid();
            model.currTetrinom.rotateX();
            bool colission = Utility.isColCollision(model, model.currTetrinom.z, model.currTetrinom.row, model.currTetrinom.col, model.currTetrinom.shape);
            if(!colission)
            {
                model.currTetrinom.drawPiece();
                getHologramPos();
            }

        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            grid.clearGrid();
            model.currTetrinom.rotateY();
            bool colission = Utility.isColCollision(model, model.currTetrinom.z, model.currTetrinom.row, model.currTetrinom.col, model.currTetrinom.shape);
            if (!colission)
            {
                model.currTetrinom.drawPiece();
                getHologramPos();
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            grid.clearGrid();
            model.currTetrinom.rotateZ();
            bool colission = Utility.isColCollision(model, model.currTetrinom.z, model.currTetrinom.row, model.currTetrinom.col, model.currTetrinom.shape);
            if (!colission)
            {
                model.currTetrinom.drawPiece();
                getHologramPos();
            }
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
        BlockType obj = model.objects[model.currTetrinombjI];
        if(model.hologram == null)
        {
            model.hologram = new BlockType(model, obj.origShape, obj.texture, 7);
        }
        else
        {
            model.hologram.shape = obj.origShape;
        }
        model.hologram.reset();
        obj.reset();//resetting only copies the array. fix that as well
        model.currTetrinom = obj;// new BlockType(obj.shape, obj.texture, obj.pieceNum);
        model.currTetrinom.col = model.numCols/2;
        model.currTetrinom.z = model.numZ / 2;

        int num = Mathf.FloorToInt(Random.Range(0.0f, 1.0f) * model.objects.Count);
        model.currTetrinombjI = num;

        //miniGrid.init(model.objects[currTetrinombjI]);
    }

    //this on counts from 0 to end of grid!
    //is the exact same function as moving pieces - refactor!
    void getHologramPos()
    {
        BlockType o = model.currTetrinom;
        BlockType h = model.hologram;
        h.z = o.z;
        h.col = o.col;
        h.shape = o.shape;
        h.blocksActual = o.blocksActual;

        for (int row = o.row; row < model.numRows; row++)
        {
            h.row = row;
            bool colission = Utility.isColCollision(model, h.z, h.row, h.col, h.shape);
            if (colission)
            {
                h.row--;
                break;
            }
        }

        if(h.row == 0)
        {
            return;
        }

        if(h.row == o.row)
        {
            return;
        }
        h.drawPiece();

    }



    void onLeftPressed()
    {
        if (model.stepCount == model.fastStepSpeed) return;
        BlockType t = model.currTetrinom;
        bool colission = Utility.isColCollision(model, t.z, t.row, t.col - 1, t.shape);
        if (!colission)
        {
            grid.clearGrid();
            model.currTetrinom.col--;
            model.currTetrinom.drawPiece();
            getHologramPos();
        }
    }

    void onRightPressed()
    {
        if (model.stepCount == model.fastStepSpeed) return;
        BlockType t = model.currTetrinom;
        bool colission = Utility.isColCollision(model, t.z, t.row, t.col + 1, t.shape);
        if (!colission)
        {
            grid.clearGrid();
            model.currTetrinom.col++;
            model.currTetrinom.drawPiece();
            getHologramPos();
        }
    }

    void onDownPressed()
    {
        if (model.stepCount == model.fastStepSpeed) return;
        BlockType t = model.currTetrinom;
        bool colission = Utility.isColCollision(model, t.z + 1, t.row, t.col, t.shape);
        if (!colission)
        {
            grid.clearGrid();
            model.currTetrinom.z++;
            model.currTetrinom.drawPiece();
            getHologramPos();
        }


        //
    }

    void onUpPressed()
    {
        if (model.stepCount == model.fastStepSpeed) return;
        BlockType t = model.currTetrinom;
        bool colission = Utility.isColCollision(model, t.z - 1, t.row, t.col, t.shape);
        if ( !colission)
        {
            grid.clearGrid();
            model.currTetrinom.z--;
            model.currTetrinom.drawPiece();
            getHologramPos();
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
        if(model.shake)
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

        if (hologramRow != 0 && hologramRow != model.currTetrinom.row)
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