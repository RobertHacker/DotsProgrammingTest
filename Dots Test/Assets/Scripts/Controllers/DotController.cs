using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotController : MonoBehaviour, IController {

    //Variables
    [SerializeField]
    DotFactory mDotFactory;
    
    [SerializeField]
    ConnectionFactory mConnectionFactory;

    private List<Dot> mChain;
    private List<GameObject> mConnections;
    private bool mSquareMade;

    //Events
    public delegate void OnChainScored();
    public event OnChainScored OnChainScoredEvent;
    
    //Public Functions
    public Dot GetNewDot()
    {

        Dot newDot = mDotFactory.DotFromPool();
        RandomizeColor(ref newDot);
        newDot.OnDotClickedEvent += BeginChain;
        newDot.OnDotEnteredEvent += ContinueChain;
        return newDot;
    }

    public void RandomizeColor(ref Dot toColor, Dot.DotColor colorRestriction = Dot.DotColor.Blank)
    {
        int min = 1;
        int max = (int)Dot.DotColor.NumColors;
        //Will continue rolling colors till a non-restricted color is found
        do
        {
            toColor.CurrentColor = (Dot.DotColor)Random.Range(min, max);
        } while (toColor.CurrentColor == colorRestriction);
        
    }

    //Private Functions
    private void AddConnection(ref Dot connectedDot)
    {
        mConnections.Add(mConnectionFactory.ConnectionFromPool(connectedDot.CurrentColor));
        mConnections[mConnections.Count - 1].transform.SetParent(connectedDot.CurrentNode.transform);
    }

    private void BeginChain(Dot startDot)
    {
        mChain.Add(startDot);
        AddConnection(ref startDot);
        startDot.Selected = true;
        Debug.Log("Chain started at: " + startDot.ID);
    }

    private void ConnectDots(ref Dot startDot, ref Dot endDot, ref GameObject connection)
    {
        float angleBetweenDots = Mathf.Atan2(endDot.transform.position.y - startDot.transform.position.y, endDot.transform.position.x - startDot.transform.position.x);
        float distBetweenDots = Mathf.Sqrt(Mathf.Pow(endDot.transform.position.x - startDot.transform.position.x, 2) + Mathf.Pow(endDot.transform.position.y - startDot.transform.position.y, 2));
        connection.transform.eulerAngles = new Vector3(0, 0, angleBetweenDots * 180 / Mathf.PI);
        connection.transform.position = new Vector3(startDot.transform.position.x + Mathf.Cos(angleBetweenDots) * distBetweenDots / 2, startDot.transform.position.y + Mathf.Sin(angleBetweenDots) * distBetweenDots / 2);
        connection.GetComponent<RectTransform>().sizeDelta = new Vector2(distBetweenDots, connection.GetComponent<RectTransform>().sizeDelta.y);
    }

    private void CheckForChainEnd()
    {
        //Check if the chain needs to be scored
        if (Input.GetMouseButtonUp(0))
        {
            if (mChain.Count > 0)
            {
                Debug.Log("Chain Ended");
                //If there is a previous dot then score the chain 
                if (mChain.Count > 1)
                {
                    Debug.Log("Chain Scored");
                    for (int i = 0; i < mChain.Count; i++)
                    { 
                        
                        mChain[i].DotScored();
                        ReturnDotToPool(mChain[i]);
                    }

                    if (OnChainScoredEvent != null)
                        OnChainScoredEvent();
                }
                else
                {
                    mChain[0].Selected = false;
                }

                for (int i = 0; i < mConnections.Count; i++)
                {
                    mConnectionFactory.AddConnectionToPool(mConnections[i]);
                }

                mSquareMade = false;

                mChain.Clear();
                mConnections.Clear();
            }
        }
    }

    private void ContinueChain(Dot touchedDot)
    {
        //If we are chaining check if the dot is valid and not the current dot
        if (mChain.Count > 0 && mChain[mChain.Count - 1].ID != touchedDot.ID)
        {
            //Check if the dot is adjacent and if the colors match
            if (mChain[mChain.Count - 1].CurrentNode.DotIDAdjacent(touchedDot.ID) && mChain[mChain.Count - 1].CurrentColor == touchedDot.CurrentColor)
            {
                //Check if the new Dot has already been selected
                if(touchedDot.Selected)
                {
                    if(touchedDot.ID == mChain[mChain.Count - 2].ID)
                    {
                        //If the dot is the previous dot selected then move back one step in the chain
                        mChain[mChain.Count - 1].Selected = false;
                        mChain.RemoveAt(mChain.Count - 1);
                        mConnectionFactory.AddConnectionToPool(mConnections[mConnections.Count - 1]);
                        mConnections.RemoveAt(mConnections.Count - 1);
                    }
                    else
                    {
                        //If the dot is not the previous dot and was selected then a square has been made
                        Debug.Log("Square");
                        mSquareMade = true;
                        Dot start = mChain[mChain.Count - 2];
                        Dot end = mChain[mChain.Count - 1];
                        GameObject connection = mConnections[mConnections.Count - 1];
                        ConnectDots(ref start, ref end, ref connection);

                    }
                }
                else
                {
                    //The dot has not been selected and should be added to the chain
                    mChain.Add(touchedDot);
                    touchedDot.Selected = true;
                    Dot start = mChain[mChain.Count - 2];
                    Dot end = mChain[mChain.Count - 1];
                    GameObject connection = mConnections[mConnections.Count - 1];
                    ConnectDots(ref start, ref end, ref connection);
                    AddConnection(ref end);
                }

                //The new dot has been selected and the old selected dot becomes the previous dot
                Debug.Log("Current Dot ID: " + mChain[mChain.Count - 1].ID + " Chain Length: " + mChain.Count);
                
            }
        }
    }

    private void PositionConnection()
    {
        //Only use connection if there is a connection
        if(mConnections.Count > 0 && !mSquareMade)
        {
            GameObject connection = mConnections[mConnections.Count - 1];
            Dot currentDot = mChain[mChain.Count - 1];
            float angleToMouse = Mathf.Atan2(Input.mousePosition.y - currentDot.transform.position.y, Input.mousePosition.x - currentDot.transform.position.x);
            float distToMouse = Mathf.Sqrt(Mathf.Pow(Input.mousePosition.x - currentDot.transform.position.x, 2) + Mathf.Pow(Input.mousePosition.y - currentDot.transform.position.y, 2));
            connection.transform.eulerAngles = new Vector3(0, 0, angleToMouse * 180 / Mathf.PI);
            connection.transform.position = new Vector3(currentDot.transform.position.x + Mathf.Cos(angleToMouse) * distToMouse / 2, currentDot.transform.position.y + Mathf.Sin(angleToMouse) * distToMouse / 2);
            connection.GetComponent<RectTransform>().sizeDelta = new Vector2(distToMouse, connection.GetComponent<RectTransform>().sizeDelta.y);
        }
    }

    private void ReturnDotToPool(Dot toReturn)
    {
        toReturn.OnDotClickedEvent -= BeginChain;
        toReturn.OnDotEnteredEvent -= ContinueChain;
        mDotFactory.AddDotToPool(toReturn);

    }

    void Update()
    {
        PositionConnection();
        CheckForChainEnd();    
    }

    //IController Implementation
    public void Init()
    {
        mDotFactory.Init(BoardSettings.Rows * BoardSettings.Columns);
        mConnectionFactory.Init(BoardSettings.Rows * BoardSettings.Columns);
        mChain = new List<Dot>();
        mConnections = new List<GameObject>();
    }

    
}
