using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DistanceRange : MonoBehaviour
{
    [SerializeField] Transform transform1;
    [SerializeField] Transform transform2;

    [SerializeField] float minDistance;
    [SerializeField] float maxDistance;
    [SerializeField] float minValue;
    [SerializeField] float maxValue;

    public UnityEvent<float> OnCall;

    private bool isMinValue;
    private bool isMaxValue;

    private void Update()
    {
        var distance = (transform1.position - transform2.position).magnitude;

        if (distance>minDistance && distance < maxDistance)
        {
            var t = (distance - minDistance) / (maxDistance - minDistance);
            var value = Mathf.Lerp(minValue, maxValue, t);
            OnCall.Invoke(value);
            isMaxValue = false;
            isMinValue = false;
        }

        if (!isMaxValue && distance > maxDistance)
        {
            OnCall.Invoke(maxValue);
            isMaxValue = true;
            isMinValue = false;
        }

        if (!isMinValue && distance < minDistance)
        {
            OnCall.Invoke(minValue);
            isMinValue = true;
            isMaxValue = false;
        }
    }
}
