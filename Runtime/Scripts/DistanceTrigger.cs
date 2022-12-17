using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DistanceTrigger : MonoBehaviour
{
    public Transform Position1;
    public Transform Position2;

    public float Distance;
    public Comparison Operator;
    public UnityEvent OnTrigger;

    private void Update()
    {
        switch (Operator)
        {
            case Comparison.LessThan:
                if ((Position1.position-Position2.position).sqrMagnitude < Mathf.Pow(Distance, 2f))
                {
                    OnTrigger.Invoke();
                }
                break;
            case Comparison.MoreThan:
                if ((Position1.position - Position2.position).sqrMagnitude > Mathf.Pow(Distance, 2f))
                {
                    OnTrigger.Invoke();
                }
                break;
        }
    }
}

public enum Comparison
{
    LessThan,
    MoreThan,
}
