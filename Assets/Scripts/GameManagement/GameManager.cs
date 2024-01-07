using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError(typeof(GameManager).ToString() + " is NULL");

            return _instance;
        }
    }

    private ActionState _currentActionState;

    public ActionState CurrentActionState => _currentActionState;

    private void Awake()
    {
        _instance = this;
    }
    public void SetActionState(ActionState newState)
    {
        _currentActionState = newState;
    }
}
public enum ActionState
{
    None,
    Browsing,
    Buy,
    Sell,
    Inventory
}