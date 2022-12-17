using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionTrigger : MonoBehaviour, ICondition
{
    public float AngleTolerance = 30f;
    public Vector3 ReferenceDirection;
    public Transform ReferencePoint;

    public bool Met
    {
        get => _met;
        set
        {
            if (_met != value)
            {
                _met = value;
                OnConditionChanged.Invoke(value);
            }
        }
    }
    public ConditionEvent OnConditionChanged { get; set; } = new ConditionEvent();

    private bool _met;

    private void Update()
    {
        var vector1 = transform.rotation * ReferenceDirection;
        var vector2 = transform.position - ReferencePoint.position;

        Met = AngleTolerance > Vector3.Angle(vector1, vector2);
    }
}
