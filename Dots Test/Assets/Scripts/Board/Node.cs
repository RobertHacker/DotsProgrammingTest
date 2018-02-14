using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    //Variables
    private Node mAbove = null;
    public Node Above
    {
        get { return mAbove; }
        set { mAbove = value; }
    }
    private Node mBelow = null;
    public Node Below
    {
        get { return mBelow; }
        set { mBelow = value; }
    }
    private Node mLeft = null;
    public Node Left
    {
        get { return mLeft; }
        set { mLeft = value; }
    }
    private Node mRight = null;
    public Node Right
    {
        get { return mRight; }
        set { mRight = value; }
    }
    
    private Dot mContents = null;
    public Dot Contents
    {
        get { return mContents; }
        set
        {
            mContents = value;
            

            if (mContents != null)
            {
                mContents.CurrentNode = this;
                mContents.transform.SetParent(this.transform);
            }
            
            //Update will handle reconnecting the body but it must be disconnected while dropping.
            mDotAnchor.connectedBody = null;

        }
    }

    [SerializeField] 
    private SpringJoint2D mDotAnchor;

    //Public functions
    public bool DotIDAdjacent(int ID)
    {
        bool adjacent = false;

        if (Above != null && Above.Contents.ID == ID)
            adjacent = true;
        else if (Below != null && Below.Contents.ID == ID)
            adjacent = true;
        else if (Right != null && Right.Contents.ID == ID)
            adjacent = true;
        else if (Left != null && Left.Contents.ID == ID)
            adjacent = true;

        return adjacent;
    }

    //Private Functions
    void Update()
    {
        //When the dot moves past the node connect the spring
        if(Contents != null && mDotAnchor.connectedBody == null && Contents.transform.position.y < this.transform.position.y)
        {
            mDotAnchor.connectedBody = Contents.gameObject.GetComponent<Rigidbody2D>();
        }
            
    }
}
