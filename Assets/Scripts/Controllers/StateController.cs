using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EGameState
{
    Loading,
    Menu,
    Game
}

public class StateController : MonoBehaviour
{
    [SerializeField] private Transform stateParent;
    [SerializeField] private EGameState currentState;
    private StateSetup[] stateSetups;
    private int statesNum;

    public static StateController Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this as StateController;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            DestroyImmediate(gameObject);
        }


        PrepareStates();
        LoadState();
    }


    private void PrepareStates()
    {
        statesNum = stateParent.childCount;
        stateSetups = new StateSetup[statesNum];
        for (int i = 0; i < statesNum; i++)
        {
            stateSetups[i] = stateParent.GetChild(i).GetComponent<StateSetup>();
        }
    }

    private void LoadState()
    {
        bool wasFound = false;
        for (int i = 0; i < statesNum; i++)
        {   
            if (stateSetups[i].GetState() == currentState)
            {
                stateSetups[i].gameObject.SetActive(true);
                wasFound = true;
            }
            else
            {
                stateSetups[i].gameObject.SetActive(false);
            }
        }
        if (!wasFound)
        {
            Debug.LogError("Choosen state isn't in hierarchy");
        }
    }

    public void ChangeState(EGameState gameState)
    {
        currentState = gameState;
        LoadState();
    }
}
