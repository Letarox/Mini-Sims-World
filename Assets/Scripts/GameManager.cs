using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private ActionState _currentActionState;

    public ActionState CurrentActionState => _currentActionState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetActionState(ActionState newState)
    {
        _currentActionState = newState;
    }
}
public enum ActionState
{
    None,
    Buy,
    Sell,
    Inventory
}