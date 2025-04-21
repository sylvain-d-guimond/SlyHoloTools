using MixedReality.Toolkit;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AngleTrigger : MonoBehaviour, ICondition, IHandedComponent
{
    public Fingers Finger;
    public Operator Operator;
    public float Value;
    public bool Active;
    public bool DeactivateOnTrigger = true;

    public UnityEvent<bool> OnTrigger;

    public string DebugMessage;
    public Handedness Hand { get => hand; set => hand = value; }

    private HandManager _handMgr;
    [SerializeField]
    private Handedness hand;

    public bool Met
    {
        get => _met;
        set
        {
            if (_met != value)
            {
                _met = value;
                OnConditionChanged.Invoke(value);
                if (DebugMode.instance.DebugLevel <= DebugLevels.Debug) Debug.Log($"Condition {gameObject.name} Finger {Finger} {Value}: {DebugMessage} is {value}");
            }
        }
    }

    public UnityEvent<bool> OnConditionChanged { get; set; } = new UnityEvent<bool>();

    [SerializeField, ReadOnly]
    private bool _met;

    private void Start()
    {
        if (DebugMode.instance.DebugLevel <= DebugLevels.Debug) Debug.Log($"Angle trigger on for {Operator} {Value}");
        _handMgr = HandManager.Instance;
    }

    public void SetActive(bool value)
    {
        if (DebugMode.instance.DebugLevel <= DebugLevels.Debug) Debug.Log($"{gameObject.name} sets angle trigger for {Operator} {Value} active: {value}");
        Active = value;
    }

    void Update()
    {
        if (Active && HandManager.Instance.IsHandTracked(hand))
        {
            var angle = 0f;
            angle = _handMgr.FingerAngle(hand, Finger);
            switch (Operator)
            {
                case Operator.LessThan:
                    if (angle < Value)
                    {
                        Call($"Finger {Finger} Angle {angle} < {Value}: {DebugMessage}");
                    } 
                    else { Met = false; }
                    break;
                case Operator.Equals:
                    if (angle == Value)
                    {
                        Call($"Finger {Finger} Angle {angle} = {Value}: {DebugMessage}");
                    }
                    else { Met = false; }
                    break;
                case Operator.MoreThan:
                    if (angle > Value)
                    {
                        Call($"Finger {Finger} Angle {angle} < {Value}: {DebugMessage}");
                    }
                    else { Met = false; }
                    break;
            }
        }
        else { Met = false; }
    }

    private void Call(string debugMessage = "")
    {
        //if (debugMessage != string.Empty) Debug.Log(debugMessage);
        OnTrigger.Invoke(true);
        if (DeactivateOnTrigger) Active = false;
        Met = true;
    }
}

public enum Operator
{
    LessThan,
    Equals,
    MoreThan,
}