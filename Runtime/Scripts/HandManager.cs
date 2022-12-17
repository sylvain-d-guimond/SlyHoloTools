using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance;

    public bool ExcludeMetacarpals = true;

    private IMixedRealityHandJointService _handJointService;
    private Dictionary<Handedness, Dictionary<TrackedHandJoint, Transform>> _joints = new Dictionary<Handedness, Dictionary<TrackedHandJoint, Transform>>();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>();

        _joints.Add(Handedness.Left, new Dictionary<TrackedHandJoint, Transform>());
        _joints.Add(Handedness.Right, new Dictionary<TrackedHandJoint, Transform>());

        foreach (TrackedHandJoint trackedJoint in Enum.GetValues(typeof(TrackedHandJoint)))
        {
            _joints[Handedness.Left].Add(trackedJoint, _handJointService.RequestJointTransform(trackedJoint, Handedness.Left));
            _joints[Handedness.Right].Add(trackedJoint, _handJointService.RequestJointTransform(trackedJoint, Handedness.Right));
        }
    }

    public Transform Get(Handedness hand, TrackedHandJoint joint)
    {
        return _joints[hand][joint];
    }

    public bool IsHandTracked(Handedness hand)
    {
        return _handJointService.IsHandTracked(hand);
    }

    public float FingerAngle(Handedness hand, Fingers finger)
    {
        var angle = 0f;
        var count = 0;

        if ((finger & Fingers.Index) == Fingers.Index) {
            //Angle between metacarpal and proximal phalanges
            if (!ExcludeMetacarpals) angle += Vector3.Angle(_joints[hand][TrackedHandJoint.IndexKnuckle].position - _joints[hand][TrackedHandJoint.IndexMetacarpal].position,
                _joints[hand][TrackedHandJoint.IndexMiddleJoint].position - _joints[hand][TrackedHandJoint.IndexKnuckle].position);
            //Angle between proximal and intermediate phalanges
            angle += Vector3.Angle(_joints[hand][TrackedHandJoint.IndexMiddleJoint].position - _joints[hand][TrackedHandJoint.IndexKnuckle].position,
                _joints[hand][TrackedHandJoint.IndexDistalJoint].position - _joints[hand][TrackedHandJoint.IndexMiddleJoint].position);
            //Angle between intermediate and distal phalanges
            angle += Vector3.Angle(_joints[hand][TrackedHandJoint.IndexDistalJoint].position - _joints[hand][TrackedHandJoint.IndexMiddleJoint].position,
                _joints[hand][TrackedHandJoint.IndexTip].position - _joints[hand][TrackedHandJoint.IndexDistalJoint].position);
            count++;
        }

        if ((finger & Fingers.Middle) == Fingers.Middle) {
            //Angle between metacarpal and proximal phalanges
            if (!ExcludeMetacarpals) angle += Vector3.Angle(_joints[hand][TrackedHandJoint.MiddleKnuckle].position - _joints[hand][TrackedHandJoint.MiddleMetacarpal].position,
                _joints[hand][TrackedHandJoint.MiddleMiddleJoint].position - _joints[hand][TrackedHandJoint.MiddleKnuckle].position);
            //Angle between proximal and intermediate phalanges
            angle += Vector3.Angle(_joints[hand][TrackedHandJoint.MiddleMiddleJoint].position - _joints[hand][TrackedHandJoint.MiddleKnuckle].position,
                _joints[hand][TrackedHandJoint.MiddleDistalJoint].position - _joints[hand][TrackedHandJoint.MiddleMiddleJoint].position);
            //Angle between intermediate and distal phalanges
            angle += Vector3.Angle(_joints[hand][TrackedHandJoint.MiddleDistalJoint].position - _joints[hand][TrackedHandJoint.MiddleMiddleJoint].position,
                _joints[hand][TrackedHandJoint.MiddleTip].position - _joints[hand][TrackedHandJoint.MiddleDistalJoint].position);
            count++;
        }

        if ((finger &  Fingers.Ring) == Fingers.Ring) {
            //Angle between metacarpal and proximal phalanges
            if (!ExcludeMetacarpals) angle += Vector3.Angle(_joints[hand][TrackedHandJoint.RingKnuckle].position - _joints[hand][TrackedHandJoint.RingMetacarpal].position,
                _joints[hand][TrackedHandJoint.RingMiddleJoint].position - _joints[hand][TrackedHandJoint.RingKnuckle].position);
            //Angle between proximal and intermediate phalanges
            angle += Vector3.Angle(_joints[hand][TrackedHandJoint.RingMiddleJoint].position - _joints[hand][TrackedHandJoint.RingKnuckle].position,
                _joints[hand][TrackedHandJoint.RingDistalJoint].position - _joints[hand][TrackedHandJoint.RingMiddleJoint].position);
            //Angle between intermediate and distal phalanges
            angle += Vector3.Angle(_joints[hand][TrackedHandJoint.RingDistalJoint].position - _joints[hand][TrackedHandJoint.RingMiddleJoint].position,
                _joints[hand][TrackedHandJoint.RingTip].position - _joints[hand][TrackedHandJoint.RingDistalJoint].position);
            count++;
        }

        if ((finger & Fingers.Pinky) == Fingers.Pinky) {
            //Angle between metacarpal and proximal phalanges
            if (!ExcludeMetacarpals) angle += Vector3.Angle(_joints[hand][TrackedHandJoint.PinkyKnuckle].position - _joints[hand][TrackedHandJoint.PinkyMetacarpal].position,
                _joints[hand][TrackedHandJoint.PinkyMiddleJoint].position - _joints[hand][TrackedHandJoint.PinkyKnuckle].position);
            //Angle between proximal and intermediate phalanges
            angle += Vector3.Angle(_joints[hand][TrackedHandJoint.PinkyMiddleJoint].position - _joints[hand][TrackedHandJoint.PinkyKnuckle].position,
                _joints[hand][TrackedHandJoint.PinkyDistalJoint].position - _joints[hand][TrackedHandJoint.PinkyMiddleJoint].position);
            //Angle between intermediate and distal phalanges
            angle += Vector3.Angle(_joints[hand][TrackedHandJoint.PinkyDistalJoint].position - _joints[hand][TrackedHandJoint.PinkyMiddleJoint].position,
                _joints[hand][TrackedHandJoint.PinkyTip].position - _joints[hand][TrackedHandJoint.PinkyDistalJoint].position);
            count++;
        }

        if ((finger & Fingers.Thumb) == Fingers.Thumb) {
            //Angle between metacarpal and proximal phalanges
            angle += Vector3.Angle(_joints[hand][TrackedHandJoint.ThumbProximalJoint].position - _joints[hand][TrackedHandJoint.PinkyMetacarpal].position,
                _joints[hand][TrackedHandJoint.ThumbDistalJoint].position - _joints[hand][TrackedHandJoint.ThumbProximalJoint].position);
            //Angle between proximal and distal phalanges
            angle += Vector3.Angle(_joints[hand][TrackedHandJoint.ThumbDistalJoint].position - _joints[hand][TrackedHandJoint.ThumbProximalJoint].position,
                _joints[hand][TrackedHandJoint.ThumbTip].position - _joints[hand][TrackedHandJoint.ThumbDistalJoint].position);
            count++;
        }

        if (count > 0)
        {
            angle /= count;
        }

        return angle;
    }
}

[Flags]
public enum Fingers : int
{
    None = 0,
    Index = 1, 
    Middle = 2,
    Ring = 4,
    Pinky = 8,
    Thumb = 16,
    All = 31,
}
