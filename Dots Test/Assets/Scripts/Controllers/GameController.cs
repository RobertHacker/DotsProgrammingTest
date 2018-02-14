using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour, IController {
    
    //Variables
    [Header("Controllers")]
    [SerializeField]
    private BoardController mBoardController;
    [SerializeField]
    private DotController mDotController;

    private List<IController> mControllers;

	//Private Functions
	void Start () {

        mControllers = new List<IController>();
        mControllers.Add(mBoardController);
        mControllers.Add(mDotController);

        //Initialize Controllers
        Init();

        //Attach Events
        mBoardController.OnRequestDotEvent += mDotController.GetNewDot;
        mDotController.OnChainScoredEvent += mBoardController.RequestDots;

        //Begin Game
        StartGame();
	}
	
	void Update () {
		
	}

    private void StartGame()
    {
        Debug.Log("Starting Game");
        mBoardController.RequestDots();
    }

    //IController implementation
    public void Init()
    {
        foreach (IController controller in mControllers)
        {
            controller.Init();
        }
    }

    
}
