using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit;

public class TrackHand : MonoBehaviour
{
    private Transform _trackedTransform;
    public TrackedHandJoint TrackedJoint;
    public Handedness Handedness;

    public bool Position = true;
    public bool Rotation = false;

    public float PositionDamping = 0.1f;
    public float RotationDamping = 0.1f;

    public Vector3 PositionOffset;
    public Vector3 RotationOffset;

    private void OnEnable()
    {
        Update();
    }

    void Update()
    {
        if (_trackedTransform == null)
        {
            var handJoint = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>();
            if (handJoint != null)
            {
                _trackedTransform = handJoint.RequestJointTransform(TrackedJoint, Handedness);
            }
        }

        if (_trackedTransform != null)
        {
            if (Position) transform.position = 
                    Vector3.Lerp(transform.position, _trackedTransform.position + PositionOffset, Time.deltaTime / PositionDamping);
            if (Rotation) transform.rotation = 
                    Quaternion.Slerp(transform.rotation, _trackedTransform.rotation*Quaternion.Euler(RotationOffset), Time.deltaTime/RotationDamping);
        }
    }

    public void SetLeft()
    {
        Handedness = Handedness.Left;
        _trackedTransform = null;
        Update();
    }

    public void SetRight()
    {
        Handedness = Handedness.Right;
        _trackedTransform = null;
        Update();
    }
}
