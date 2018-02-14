using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionFactory : MonoBehaviour {

    //Variables
    [Header("Prefabs")]
    [SerializeField]
    private GameObject ConnectionPrefab;

    private List<GameObject> mConnectionPool;

    private int mConnectionsCreated;

    //Public Functions
    public void Init(int numConnections)
    {
        ProduceConnections(numConnections);
    }

    public GameObject ConnectionFromPool(Dot.DotColor dotColor)
    {
        GameObject toReturn;
        //Returns a Connection from the pool if one exists otherwise adds creates a new Connection
        if (mConnectionPool.Count > 0)
        {
            toReturn = mConnectionPool[0];
            mConnectionPool.RemoveAt(0);
        }
        else
        {
            toReturn = ProduceConnection();
            Debug.LogWarning("Not enough Connections were originally created");
        }

        ChangeConnectionColor(dotColor, ref toReturn);

        return toReturn;
    }

    public void AddConnectionToPool(GameObject toReturn)
    {
        toReturn.gameObject.transform.position = new Vector2(BoardSettings.STORAGE_AREA_X, BoardSettings.STORAGE_AREA_Y);
        toReturn.transform.SetParent(this.transform);
        mConnectionPool.Add(toReturn);
    }

    //Private Functions
    void Start()
    {
        mConnectionPool = new List<GameObject>();
    }

    private void ChangeConnectionColor(Dot.DotColor dotColor, ref GameObject connection)
    {
        Image image = connection.GetComponent<Image>();
        switch (dotColor)
        {
            case Dot.DotColor.Red:
                image.color = Color.red;
                break;
            case Dot.DotColor.Blue:
                image.color = Color.blue;
                break;
            case Dot.DotColor.Yellow:
                image.color = Color.yellow;
                break;
            case Dot.DotColor.Green:
                image.color = Color.green;
                break;
            case Dot.DotColor.Purple:
                image.color = Color.magenta;
                break;
            default:
                image.color = Color.white;
                break;
        }
    }

    private void ProduceConnections(int numConnections)
    {
        mConnectionPool = new List<GameObject>();
        for (int i = 0; i < numConnections; i++)
        {
            mConnectionPool.Add(ProduceConnection());
        }
    }

    private GameObject ProduceConnection()
    {
        GameObject newConnection = GameObject.Instantiate(ConnectionPrefab, this.transform);
        newConnection.transform.position = new Vector3(BoardSettings.STORAGE_AREA_X, BoardSettings.STORAGE_AREA_Y);
        return newConnection;
    }
}
