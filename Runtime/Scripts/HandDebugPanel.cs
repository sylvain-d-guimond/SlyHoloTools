using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;

public class HandDebugPanel : MonoBehaviour
{
    public static HandDebugPanel Instance;

    public TextMeshProUGUI LeftHandTracked;
    public TextMeshProUGUI LeftHandIndexAngle;
    public TextMeshProUGUI LeftHandMiddleAngle;
    public TextMeshProUGUI LeftHandRingAngle;
    public TextMeshProUGUI LeftHandPinkyAngle;
    public TextMeshProUGUI LeftHandThumbAngle;
    public TextMeshProUGUI RightHandTracked;
    public TextMeshProUGUI RightHandIndexAngle;
    public TextMeshProUGUI RightHandMiddleAngle;
    public TextMeshProUGUI RightHandRingAngle;
    public TextMeshProUGUI RightHandPinkyAngle;
    public TextMeshProUGUI RightHandThumbAngle;
    public TextMeshProUGUI FingerDistance;
    public TextMeshProUGUI AvgDist;
    public TextMeshProUGUI LeftAngle;
    public TextMeshProUGUI RightAngle;

    public Rigidbody Rigidbody;
    public TextMeshProUGUI MaxVelocityText;
    public TextMeshProUGUI Velocity;
    public float MaxVelocity;

    private void Awake()
    {
        Instance = this;
    }

    private void LateUpdate()
    {
        if (HandManager.Instance != null)
        {
            LeftHandTracked.text = HandManager.Instance.IsHandTracked(Handedness.Left).ToString();
            RightHandTracked.text = HandManager.Instance.IsHandTracked(Handedness.Right).ToString();

            LeftHandIndexAngle.text = HandManager.Instance.FingerAngle(Handedness.Left, Fingers.Index).ToString();
            RightHandIndexAngle.text = HandManager.Instance.FingerAngle(Handedness.Right, Fingers.Index).ToString();

            LeftHandMiddleAngle.text = HandManager.Instance.FingerAngle(Handedness.Left, Fingers.Middle).ToString();
            RightHandMiddleAngle.text = HandManager.Instance.FingerAngle(Handedness.Right, Fingers.Middle).ToString();

            LeftHandRingAngle.text = HandManager.Instance.FingerAngle(Handedness.Left, Fingers.Ring).ToString();
            RightHandRingAngle.text = HandManager.Instance.FingerAngle(Handedness.Right, Fingers.Ring).ToString();

            LeftHandPinkyAngle.text = HandManager.Instance.FingerAngle(Handedness.Left, Fingers.Pinky).ToString();
            RightHandPinkyAngle.text = HandManager.Instance.FingerAngle(Handedness.Right, Fingers.Pinky).ToString();

            LeftHandThumbAngle.text = HandManager.Instance.FingerAngle(Handedness.Left, Fingers.Thumb).ToString();
            RightHandThumbAngle.text = HandManager.Instance.FingerAngle(Handedness.Right, Fingers.Thumb).ToString();

            LeftAngle.text = HandManager.Instance.FingerAngle(Handedness.Left, (Fingers)15).ToString();
            RightAngle.text = HandManager.Instance.FingerAngle(Handedness.Right, (Fingers)15).ToString();
        }
        //if (MultiDistanceTrigger.Instance != null) FingerDistance.text = MultiDistanceTrigger.Instance.MaxDistance.ToString();
        //if (AvgDistance.Instance != null) AvgDist.text = AvgDistance.Instance.Distance.ToString();
        //if (Rigidbody != null) MaxVelocityText.text = Rigidbody.velocity.ToString();
        //else MaxVelocityText.text = MaxVelocity.ToString();
    }
}
