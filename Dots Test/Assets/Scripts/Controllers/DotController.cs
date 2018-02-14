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
    private void BeginChain(Dot startDot)
    {
        mChain.Add(startDot);
        mConnections.Add(mConnectionFactory.ConnectionFromPool());
        startDot.Selected = true;
        Debug.Log("Chain started at: " + startDot.ID);
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

                    for (int i = 0; i < mConnections.Count; i++)
                    {
                        mConnectionFactory.AddConnectionToPool(mConnections[i]);
                    }

                    if (OnChainScoredEvent != null)
                        OnChainScoredEvent();
                }
                else
                {
                    mChain[0].Selected = false;
                }
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
                    }
                    else
                    {
                        //If the dot is not the previous dot and was selected then a square has been made
                        Debug.Log("Square");
                    }
                }
                else
                {
                    //The dot has not been selected and should be added to the chain
                    mChain.Add(touchedDot);
                    touchedDot.Selected = true;
                }

                //The new dot has been selected and the old selected dot becomes the previous dot
                Debug.Log("Current Dot ID: " + mChain[mChain.Count - 1].ID + " Chain Length: " + mChain.Count);
                
            }
        }
    }

    private void PositionConnection()
    {
        //Only use connection if there is a connection
        if(mConnections.Count > 0)
        {

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
