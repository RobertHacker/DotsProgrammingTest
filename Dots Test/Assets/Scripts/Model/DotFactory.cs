using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotFactory : MonoBehaviour{

    //Variables
    [Header("Prefabs")]
    [SerializeField]
    private GameObject DotPrefab;

    private List<Dot> mDotPool;

    private Dot.DotColor restrictedColor = Dot.DotColor.Blank;

    private int mDotsCreated;

    //Public Functions
    public void Init(int numDots)
    {
        ProduceDots(numDots);
    }

    public Dot DotFromPool()
    {
        Dot toReturn;
        //Returns a dot from the pool if one exists otherwise adds creates a new dot
        if (mDotPool.Count > 0)
        {
            toReturn = mDotPool[0];
            mDotPool.RemoveAt(0);
        }
        else
        {
            toReturn = ProduceDot();
            Debug.LogWarning("Not enough dots were originally created");
        }
        
        return toReturn;
    }

    public void AddDotToPool(Dot toReturn)
    {
        toReturn.gameObject.transform.position = new Vector2(BoardSettings.STORAGE_AREA_X, BoardSettings.STORAGE_AREA_Y);
        toReturn.transform.SetParent(this.transform);
        mDotPool.Add(toReturn);
    }

    //Private Functions
    void Start()
    {
        mDotPool = new List<Dot>();
    }

    private void ProduceDots(int numDots)
    {
        mDotPool = new List<Dot>();
        for (int i = 0; i < numDots; i++)
        {
            mDotPool.Add(ProduceDot());
        }
    }
    
    private Dot ProduceDot()
    {
        GameObject newDot = GameObject.Instantiate(DotPrefab, this.transform);
        Dot dotComp = newDot.GetComponent<Dot>();
        dotComp.transform.position = new Vector3(BoardSettings.STORAGE_AREA_X, BoardSettings.STORAGE_AREA_Y);
        dotComp.InitDot(++mDotsCreated);
        return dotComp;
    }
    
}
