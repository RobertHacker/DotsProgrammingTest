using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GameObject ConnectionFromPool()
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
