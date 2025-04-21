using MixedReality.Toolkit;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandednessSwitcher : MonoBehaviour
{
    [SerializeField]
    [ValidateInput("isHanded", "The component must extend IHandedComponent")] 
    List<Component> TrackedJoints;
    public Handedness Handedness
    {
        get { return handedness; }
        set { 
            handedness = value; 
            UpdateJoints(handedness);
        }
    }

    [SerializeField, ReadOnly]
    protected Handedness handedness;

    public void UpdateJoints(Handedness hand)
    {
        foreach (IHandedComponent joint in TrackedJoints)
        {
            joint.Hand = hand;
        }
    }

    [Button("Right")]
    public void SetRight() { Handedness = Handedness.Right; }

    [Button("Left")]
    public void SetLeft() { Handedness = Handedness.Left; }

    private bool IsHanded(List<Component> components)
    {
        bool didRemove = false;
        for (int i = 0; i < components.Count; i++)
        {
            if (components[i] != null && !(components[i] is IHandedComponent))
            {
                components.RemoveAt(i);
                didRemove = true;
            }
        }
        return !didRemove;
    }
}
