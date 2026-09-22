using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    [SerializeField] private string initState;
    [SerializeField] private FSMState[] states;

    public FSMState CurrentState { get; set; }
    public Transform Player { get; set; }

    public string InitState => initState;

    public bool IsBoss;

    private void Start()
    {
        ChangeState(initState);
    }

    private void Update()
    {
        CurrentState?.UpdateState(this);
    }

    public void ChangeState(string newStateID)
    {
        FSMState newState = GetState(newStateID);
        if (newState == null) return;
        CurrentState = newState;
    }

    private FSMState GetState(string newStateID)
    {
        for(int i = 0; i < states.Length; i++)
        {
            if(states[i].id == newStateID)
            {
                return states[i];
            }
        }
        return null;
    }
}
