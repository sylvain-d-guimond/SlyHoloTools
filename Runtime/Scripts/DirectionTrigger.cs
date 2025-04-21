using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class DirectionTrigger : MonoBehaviour, ICondition
{
    public float AngleTolerance = 30f;
    public Vector3 ReferenceDirection;
    public Transform ReferencePointFrom;
    public Transform ReferencePointTo;

    public UnityEvent OnMet;
    public UnityEvent OnUnMet;

    public Transform To { set => ReferencePointTo = value; }

    [SerializeField, ReadOnly]
    private float angle;

    public bool Met
    {
        get => _met;
        set
        {
            if (_met != value)
            {
                _met = value;
                OnConditionChanged.Invoke(value);
                if (_met) { OnMet.Invoke(); }
                else { OnUnMet.Invoke(); }
            }
        }
    }
    public UnityEvent<bool> OnConditionChanged { get; set; } = new UnityEvent<bool>();

    [SerializeField, ReadOnly]
    private bool _met;

    private void Update()
    {
        var vector1 = ReferencePointTo.rotation * ReferenceDirection;
        var vector2 = ReferencePointTo.position - ReferencePointFrom.position;

        Met = AngleTolerance > (angle = Vector3.Angle(vector1, vector2));
    }
}
