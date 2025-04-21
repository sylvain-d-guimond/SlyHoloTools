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

        var index = new List<TrackedHandJoint>
        {
            TrackedHandJoint.IndexMetacarpal,
            TrackedHandJoint.IndexProximal,
            TrackedHandJoint.IndexIntermediate,
            TrackedHandJoint.IndexDistal,
            TrackedHandJoint.IndexTip
        };
        _hands.Add(Fingers.Index, index);

        var middle = new List<TrackedHandJoint>
        {
            TrackedHandJoint.MiddleMetacarpal,
            TrackedHandJoint.MiddleProximal,
            TrackedHandJoint.MiddleIntermediate,
            TrackedHandJoint.MiddleDistal,
            TrackedHandJoint.MiddleTip
        };
        _hands.Add(Fingers.Middle, middle);

        var ring = new List<TrackedHandJoint>
        {
            TrackedHandJoint.RingMetacarpal,
            TrackedHandJoint.RingProximal,
            TrackedHandJoint.RingIntermediate,
            TrackedHandJoint.RingDistal,
            TrackedHandJoint.RingTip
        };
        _hands.Add(Fingers.Ring, ring);

        var pinky = new List<TrackedHandJoint>
        {
            TrackedHandJoint.LittleMetacarpal,
            TrackedHandJoint.LittleProximal,
            TrackedHandJoint.LittleIntermediate,
            TrackedHandJoint.LittleDistal,
            TrackedHandJoint.LittleTip
        };
        _hands.Add(Fingers.Pinky, pinky);

        var thumb = new List<TrackedHandJoint>
        {
            TrackedHandJoint.ThumbMetacarpal,
            TrackedHandJoint.ThumbProximal,
            TrackedHandJoint.ThumbDistal,
            TrackedHandJoint.ThumbTip
        };
        _hands.Add(Fingers.Thumb, thumb);

        _metacarpals = new List<TrackedHandJoint>
        {
            TrackedHandJoint.IndexMetacarpal,
            TrackedHandJoint.MiddleMetacarpal,
            TrackedHandJoint.RingMetacarpal,
            TrackedHandJoint.LittleMetacarpal,
            TrackedHandJoint.ThumbMetacarpal
        };
    }

    public bool IsHandTracked(Handedness handedness)
    {
        if (handedness == Handedness.None) return true;

        p1.Hand = handedness;
        p1.Joint = TrackedHandJoint.IndexTip;

        var tracked = p1.TryGetPose(out pose1);

        return tracked;
    }

    float AddAngle(Handedness hand, List<TrackedHandJoint> joints)
    {
        var angle = 0f;

        for (int i=0; i<joints.Count-2; i++)
        {
            if (ExcludeMetacarpals && _metacarpals.Contains(joints[i])) continue;
            p1.Hand = p2.Hand = hand; 
            p1.Joint = joints[i];
            p2.Joint = joints[i+1];
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
            count++;
        }

        if ((finger & Fingers.Middle) == Fingers.Middle)
        {
            angle += AddAngle(hand, _hands[Fingers.Middle]);
            count++;
        }

        if ((finger & Fingers.Ring) == Fingers.Ring)
        {
            angle += AddAngle(hand, _hands[Fingers.Ring]);
            count++;
        }

        if ((finger & Fingers.Pinky) == Fingers.Pinky)
        {
            angle += AddAngle(hand, _hands[Fingers.Pinky]);
            count++;
        }

        if ((finger & Fingers.Thumb) == Fingers.Thumb)
        {
            angle += AddAngle(hand, _hands[Fingers.Thumb]);
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
