using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Dot : MonoBehaviour {
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

    [SerializeField]
    private Button mButton;

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

    //Events
    public delegate void OnDotClicked(Dot thisDot);
    public event OnDotClicked OnDotClickedEvent;

    //Public Functions
    public void InitDot(int DotID)
    {
        ID = DotID;
        CurrentColor = DotColor.Blank;
        CurrentType = DotType.Default;
        mNode = null;
        mButton.onClick.AddListener(DotScored);
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

    private void DotScored()
    {
        if(mNode != null)
        {
            mNode.Contents = null;
        }

        if(OnDotClickedEvent != null)
            OnDotClickedEvent(this);
    }
}
