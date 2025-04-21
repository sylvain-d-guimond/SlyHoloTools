using MixedReality.Toolkit;
using MixedReality.Toolkit.Input;
using System;
using UnityEngine;


public class TrackJoint : MonoBehaviour, IHandedComponent
{
    [SerializeField]
    [Tooltip("The pose source representing the hand joint this interactor tracks")]
    private HandJointPoseSource jointPoseSource;

    /// <summary>
    /// The pose source representing the hand joint this interactor tracks
    /// </summary>
    public HandJointPoseSource JointPoseSource { 
        get => jointPoseSource; 
        set => jointPoseSource = value; }
    public Handedness Hand { 
        get => JointPoseSource.Hand; 
        set => jointPoseSource.Hand = value; }

    /// <summary>
    /// A Unity event function that is called every frame, if this object is enabled.
    /// </summary>
    private void Update()
    {
        if (JointPoseSource != null && JointPoseSource.TryGetPose(out Pose pose))
        {
            transform.SetPositionAndRotation(pose.position, pose.rotation);
        }
        else
        {
            // If we have no valid tracked joint, reset to local zero.
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }
}