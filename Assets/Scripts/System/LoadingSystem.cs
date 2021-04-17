using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSystem : GameSystem
{
    [SerializeField] private EGameState nextState;
    private void Start()
    {
        StartCoroutine(ChangeState());
    }
    IEnumerator ChangeState()
    {
        yield return new WaitForSeconds(3f);
        StateController.Instance.ChangeState(nextState);
    }
}
