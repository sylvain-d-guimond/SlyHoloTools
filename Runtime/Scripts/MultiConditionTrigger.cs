using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MultiConditionTrigger : MonoBehaviour, ICondition
{
    public List<Component> Conditions = new List<Component>();
    public string DebugText;

    public UnityEvent OnConditionsMet;
    public UnityEvent OnConditionsUnmet;

    public bool Met { 
        get => _met;
        set {
            if(_met != value)
            {
                _met = value;
                OnConditionChanged.Invoke(value);
                Debug.Log($"Condition {gameObject.name} {DebugText}: {value}");
            }
        }
    }
    [SerializeField, ReadOnly]
    private bool _met;
    public UnityEvent<bool> OnConditionChanged { get; set; } = new UnityEvent<bool>();

    private void Awake()
    {
        foreach (var condition in Conditions)
        {
            if (condition is ICondition iCondition)
            {
                iCondition.OnConditionChanged.AddListener(Check);
            }
        }
    }

    public void Check(bool b) {
        if (DebugMode.instance.DebugLevel <= DebugLevels.Debug) Debug.Log($"Check {Conditions.Count} conditions");
        if (!Conditions.Any(condition => !((ICondition)condition).Met))
        {
            OnConditionsMet.Invoke();
            Met = true;
        }
        else
        {
            OnConditionsUnmet.Invoke();
            Met = false;
        }
    }
}

public interface ICondition
{
    public bool Met { get; set; }
    public UnityEvent<bool> OnConditionChanged { get; set; }
}