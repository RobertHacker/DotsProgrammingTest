using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour, IController {
    
    //Variables
    [SerializeField]
    private Board mBoard;

    //Events
    public delegate Dot OnRequestDot();
    public event OnRequestDot OnRequestDotEvent;
    
    //Public Functions
    public void RequestDots()
    {
        mBoard.DropExistingDots();
        mBoard.RequestDots();
    }

    //Private Functions
    private Dot RequestDot()
    {
        Dot requestedDot = null;
        if (OnRequestDotEvent != null)
            requestedDot = OnRequestDotEvent();
        return requestedDot;
    }

    //IController Implementation
    public void Init()
    {
        mBoard.InitBoard();
        mBoard.OnRequestDotEvent += RequestDot;
    }
}
