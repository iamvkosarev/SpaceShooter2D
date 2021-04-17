using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSystem : GameSystem
{
    public EGameState nextState;
    private void Start()
    {
        StartCoroutine(ChangeState());
    }
    IEnumerator ChangeState()
    {
        yield return new WaitForSeconds(0f);
        StateController.Instance.ChangeState(nextState);
    }
}
