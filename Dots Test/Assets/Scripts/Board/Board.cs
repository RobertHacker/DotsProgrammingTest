using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    //Variables
    [Header("Prefabs")]
    [SerializeField]
    private GameObject mNodePrefab;
    
    private List<List<Node>> mNodes;

    //Events
    public delegate Dot OnRequestDot();
    public event OnRequestDot OnRequestDotEvent;

    //Public Functions
    public void InitBoard()
    {
        InitNodes();
        ConnectNodes();
        PositionNodes();
    }

    public void DropExistingDots()
    {
        //work from bottom up to keep pulling existing dots down
        for (int r = BoardSettings.Rows - 1; r >= 0; r--)
        {
            for (int c = BoardSettings.Columns - 1; c >= 0; c--)
            {
                Node curNode = mNodes[r][c];
                if(curNode.Contents == null)
                {
                    GrabAboveDot(r, c, ref curNode);
                }
            }
            
        }
    }

    public void RequestDots()
    {
        //The number of dots current lined up to fall in the given column
        int dotsFalling;
        //From bottom to top so that the highest dots spawn the farthest up
        for (int r = BoardSettings.Rows - 1; r >= 0; r--)
        {
            dotsFalling = 0;
            for (int c = BoardSettings.Columns - 1; c >= 0; c--)
            {
                Node curNode = mNodes[r][c];
                if (curNode.Contents == null)
                {
                    Dot nodeContents = null;
                    if (OnRequestDotEvent != null)
                        nodeContents = OnRequestDotEvent();
                    curNode.Contents = nodeContents;
                    dotsFalling++;
                    curNode.Contents.transform.position = new Vector3(curNode.transform.position.x, Screen.height + 1.5f * BoardSettings.DOT_DIAMETER * dotsFalling);
                }
                    

            }
        }
    }

    //Private Functions
    private void ConnectNodes()
    {
        for (int r = 0; r < BoardSettings.Rows; r++)
        {
            for (int c = 0; c < BoardSettings.Columns; c++)
            {
                Node curNode = mNodes[r][c];

                if (c != 0)
                    curNode.Above = mNodes[r][c - 1];

                if (c != BoardSettings.Columns - 1)
                    curNode.Below = mNodes[r][c + 1];

                if (r != BoardSettings.Rows - 1)
                    curNode.Right = mNodes[r + 1][c];

                if (r != 0)
                    curNode.Left = mNodes[r - 1][c];
            }
        }
    }

    private void GrabAboveDot(int row, int column, ref Node startNode)
    {
        //Work up from the row to see if there are any dots to pull down
        for (int c = column; c >= 0; c--)
        {
            Node toCheck = mNodes[row][c];
            //If a dot is found then pull it down and break
            if(toCheck.Contents != null)
            {
                startNode.Contents = toCheck.Contents;
                toCheck.Contents = null;
                break;
            }
        }
    }

    private void InitNodes()
    {
        mNodes = new List<List<Node>>();
        for (int r = 0; r < BoardSettings.Rows; r++)
        {
            List<Node> row = new List<Node>();
            for (int c = 0; c < BoardSettings.Columns; c++)
            {
                GameObject obj = GameObject.Instantiate(mNodePrefab, this.transform);
                Node newNode = obj.GetComponent<Node>();
                row.Add(newNode);
            }
            mNodes.Add(row);
        }
    }

    private void PositionNodes()
    {
        //Based on how the nodes are connected (0,0) is the top left
        Vector3 nodePosShift = new Vector3((Screen.width / 2) + (-BoardSettings.DOT_DIAMETER * 0.75f * BoardSettings.Rows), (Screen.height / 2) + (BoardSettings.DOT_DIAMETER * 0.75f * BoardSettings.Rows));

        //Adjust by a half step if either rows or columns are equal
        if (BoardSettings.Rows % 2 == 0)
            nodePosShift.y -= (BoardSettings.DOT_DIAMETER * 0.75f);

        if (BoardSettings.Columns % 2 == 0)
            nodePosShift.x += (BoardSettings.DOT_DIAMETER * 0.75f);

        for (int r = 0; r < BoardSettings.Rows; r++)
        {
            for (int c = 0; c < BoardSettings.Columns; c++)
            {
                mNodes[r][c].transform.position = nodePosShift + new Vector3(r * BoardSettings.DOT_DIAMETER * 1.5f, c * -BoardSettings.DOT_DIAMETER * 1.5f);
            }
        }
    }
}
