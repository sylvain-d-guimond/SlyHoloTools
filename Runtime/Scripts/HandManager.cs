using System.Collections.Generic;
using UnityEngine;
using System;
using MixedReality.Toolkit.Subsystems;
using MixedReality.Toolkit;
using NaughtyAttributes;
using MixedReality.Toolkit.Input;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance;

    public bool ExcludeMetacarpals = true;

    private Dictionary<Fingers, List<TrackedHandJoint>> _hands;
    private List<TrackedHandJoint> _metacarpals;

    private HandJointPoseSource p1 = new HandJointPoseSource(), p2 = new HandJointPoseSource();
    private Pose pose1, pose2;

    private void Awake()
    {
        Instance = this;

        _hands = new Dictionary<Fingers, List<TrackedHandJoint>>();

        var index = new List<TrackedHandJoint>();
        index.Add(TrackedHandJoint.IndexMetacarpal);
        index.Add(TrackedHandJoint.IndexProximal);
        index.Add(TrackedHandJoint.IndexIntermediate);
        index.Add(TrackedHandJoint.IndexDistal);
        index.Add(TrackedHandJoint.IndexTip);
        _hands.Add(Fingers.Index, index);

        var middle = new List<TrackedHandJoint>();
        middle.Add(TrackedHandJoint.MiddleMetacarpal);
        middle.Add(TrackedHandJoint.MiddleProximal);
        middle.Add(TrackedHandJoint.MiddleIntermediate);
        middle.Add(TrackedHandJoint.MiddleDistal);
        middle.Add(TrackedHandJoint.MiddleTip);
        _hands.Add(Fingers.Middle, middle);

        var ring = new List<TrackedHandJoint>();
        ring.Add(TrackedHandJoint.RingMetacarpal);
        ring.Add(TrackedHandJoint.RingProximal);
        ring.Add(TrackedHandJoint.RingIntermediate);
        ring.Add(TrackedHandJoint.RingDistal);
        ring.Add(TrackedHandJoint.RingTip);
        _hands.Add(Fingers.Ring, ring);

        var pinky = new List<TrackedHandJoint>();
       pinky.Add(TrackedHandJoint.LittleMetacarpal);
       pinky.Add(TrackedHandJoint.LittleProximal);
       pinky.Add(TrackedHandJoint.LittleIntermediate);
       pinky.Add(TrackedHandJoint.LittleDistal);
       pinky.Add(TrackedHandJoint.LittleTip);
        _hands.Add(Fingers.Pinky, pinky);

        var thumb = new List<TrackedHandJoint>();
        thumb.Add(TrackedHandJoint.ThumbMetacarpal);
        thumb.Add(TrackedHandJoint.ThumbProximal);
        thumb.Add(TrackedHandJoint.ThumbDistal);
        thumb.Add(TrackedHandJoint.ThumbTip);
        _hands.Add(Fingers.Thumb, thumb);

        _metacarpals = new List<TrackedHandJoint>();
        _metacarpals.Add(TrackedHandJoint.IndexMetacarpal);
        _metacarpals.Add(TrackedHandJoint.MiddleMetacarpal);
        _metacarpals.Add(TrackedHandJoint.RingMetacarpal);
        _metacarpals.Add(TrackedHandJoint.LittleMetacarpal);
        _metacarpals.Add(TrackedHandJoint.ThumbMetacarpal);
    }

    float AddAngle(Handedness hand, List<TrackedHandJoint> joints)
    {
        var angle = 0f;

        for (int i=0; i<joints.Count-2; i++)
        {
            if (ExcludeMetacarpals && _metacarpals.Contains(_hands[Fingers.Index][i])) continue;
            p1.Hand = p2.Hand = hand; 
            p1.Joint = _hands[Fingers.Index][i];
            p2.Joint = _hands[Fingers.Index][i+1];
            p1.TryGetPose(out pose1);
            p2.TryGetPose(out pose2);
            angle += Vector3.Angle(pose1.forward, pose2.forward);
        }

        return angle;
    }

    public float FingerAngle(Handedness hand, Fingers finger)
    {
        var angle = 0f;
        var count = 0;

        if ((finger & Fingers.Index) == Fingers.Index)
        {
            angle += AddAngle(hand, _hands[Fingers.Index]);
        }

        if ((finger & Fingers.Middle) == Fingers.Middle)
        {
            angle += AddAngle(hand, _hands[Fingers.Middle]);
        }

        if ((finger & Fingers.Ring) == Fingers.Ring)
        {
            angle += AddAngle(hand, _hands[Fingers.Ring]);
        }

        if ((finger & Fingers.Pinky) == Fingers.Pinky)
        {
            angle += AddAngle(hand, _hands[Fingers.Pinky]);
        }

        if ((finger & Fingers.Thumb) == Fingers.Thumb)
        {
            angle += AddAngle(hand, _hands[Fingers.Thumb]);
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
