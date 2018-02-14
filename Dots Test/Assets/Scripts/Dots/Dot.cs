using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Dot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler {
    //Enums
    public enum DotColor
    {
        Blank = 0, //default uncolored <- should not show up
        Red,
        Blue,
        Yellow,
        Green,
        Purple,
        NumColors
    }

    public enum DotType
    {
        Default = 0,
        NumTypes
    }

    //Variables
    [SerializeField]
    private Image mImage;

    private DotColor mColor;
    public DotColor CurrentColor
    {
        get { return mColor; }
        set {
            mColor = value;
            ChangeColor();
        }
    }

    private DotType mType;
    public DotType CurrentType
    {
        get { return mType; }
        set { mType = value; }
    }

    private int mID;
    public int ID
    {
        get { return mID; }
        private set { mID = value; }
    }

    private Node mNode;
    public Node CurrentNode
    {
        get { return mNode; }
        set { mNode = value; }
    }

    private bool mSelected = false;
    public bool Selected
    {
        get { return mSelected; }
        set { mSelected = value; }
    }

    //Events
    public delegate void OnDotClicked(Dot thisDot);
    public event OnDotClicked OnDotClickedEvent;

    public delegate void OnDotEntered(Dot thisDot);
    public event OnDotEntered OnDotEnteredEvent;
    

    //Public Functions
    public void InitDot(int DotID)
    {
        ID = DotID;
        CurrentColor = DotColor.Blank;
        CurrentType = DotType.Default;
        mNode = null;
    }

    public void DotScored()
    {
        if (mNode != null)
        {
            mNode.Contents = null;
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (OnDotClickedEvent != null)
            OnDotClickedEvent(this);
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if (OnDotEnteredEvent != null)
            OnDotEnteredEvent(this);
    }

    //Private Functions
    void Update()
    {
        
    }

    private void ChangeColor()
    {
        switch(mColor)
        {
            case DotColor.Red:
                mImage.color = Color.red;
                break;
            case DotColor.Blue:
                mImage.color = Color.blue;
                break;
            case DotColor.Yellow:
                mImage.color = Color.yellow;
                break;
            case DotColor.Green:
                mImage.color = Color.green;
                break;
            case DotColor.Purple:
                mImage.color = Color.magenta;
                break;
            default:
                mImage.color = Color.white;
                break;
        }
    }
    
}
