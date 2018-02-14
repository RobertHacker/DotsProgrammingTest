using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotController : MonoBehaviour, IController {

    //Variables
    [SerializeField]
    DotFactory mFactory;

    //Events
    public delegate void OnChainScored();
    public event OnChainScored OnChainScoredEvent;

    //Public Functions
    public Dot GetNewDot()
    {

        Dot newDot = mFactory.DotFromPool();
        RandomizeColor(ref newDot);
        newDot.OnDotClickedEvent += ReturnDotToPool;
        return newDot;
    }

    public void ReturnDotToPool(Dot toReturn)
    {
        toReturn.OnDotClickedEvent -= ReturnDotToPool;
        mFactory.AddDotToPool(toReturn);
        if(OnChainScoredEvent != null)
            OnChainScoredEvent();
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

    //IController Implementation
    public void Init()
    {
        mFactory.Init(BoardSettings.Rows * BoardSettings.Columns);
    }

    
}
