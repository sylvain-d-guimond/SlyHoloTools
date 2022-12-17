using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MultiConditionTrigger : MonoBehaviour, ICondition
{
    public List<ICondition> Conditions = new List<ICondition>();
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
    private bool _met;
    public ConditionEvent OnConditionChanged { get; set; } = new ConditionEvent();

    private void Awake()
    {
        foreach (var condition in GetComponents<AngleTrigger>())
        {
            condition.OnConditionChanged.AddListener(Check);
            Conditions.Add(condition);
        }
        foreach (var condition in GetComponents<DirectionTrigger>())
        {
            condition.OnConditionChanged.AddListener(Check);
            Conditions.Add(condition);
        }
        foreach (var condition in GetComponents<MultiDistanceTrigger>())
        {
            condition.OnConditionChanged.AddListener(Check);
            Conditions.Add(condition);
        }
    }

    public void Check(bool b) {
        if (DebugMode.instance.DebugLevel <= DebugLevels.Debug) Debug.Log($"Check {Conditions.Count} conditions");
        if (!Conditions.Any(condition => !condition.Met))
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
    public ConditionEvent OnConditionChanged { get; set; }
}

[Serializable]
public class ConditionEvent : UnityEvent<bool> { }