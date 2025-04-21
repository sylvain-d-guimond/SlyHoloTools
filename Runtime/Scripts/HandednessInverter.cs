using MixedReality.Toolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandednessInverter : MonoBehaviour, IHandedComponent
{
    public Component HandedComponent;
    public Handedness Hand
    {
        get { return ((IHandedComponent)HandedComponent).Hand == Handedness.Left? Handedness.Right : Handedness.Left; }
        set
        {
            if (value == Handedness.Left)
                ((IHandedComponent)HandedComponent).Hand = Handedness.Right;
            else if (value == Handedness.Right)
                ((IHandedComponent)HandedComponent).Hand = Handedness.Left;
        }
    }

    private void Start()
    {
        if (!(HandedComponent is IHandedComponent)) Debug.LogError($"Component {HandedComponent.name} is not IHandedComponent!");
    }
}
