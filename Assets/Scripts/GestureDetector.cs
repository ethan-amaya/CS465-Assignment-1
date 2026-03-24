using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;
using TMPro;

public class GestureDetector : MonoBehaviour
{
    public bool isRightHand = true;
    public GestureUI gestureUI;

    private XRHandSubsystem handSubsystem;

    void Start()
    {
        var subsystems = new List<XRHandSubsystem>();
        SubsystemManager.GetSubsystems(subsystems);
        if (subsystems.Count > 0)
            handSubsystem = subsystems[0];
    }

    void Update()
    {
        if (handSubsystem == null) return;

        XRHand hand = isRightHand ? handSubsystem.rightHand : handSubsystem.leftHand;
        string side = isRightHand ? "Right" : "Left";

        if (!hand.isTracked) return;

        if (IsThumbsUp(hand))
            gestureUI.ShowMessage($"{side} hand thumbs up!");
        else if (IsFist(hand))
            gestureUI.ShowMessage($"{side} hand fist!");
        else if (IsPeaceSign(hand))
            gestureUI.ShowMessage($"{side}-hand Peace Sign!");
        else
            gestureUI.ClearMessage();
    }

    bool IsThumbsUp(XRHand hand)
    {
        // Thumb up, all other fingers curled
        return IsFingerCurled(hand, XRHandFingerID.Index) &&
               IsFingerCurled(hand, XRHandFingerID.Middle) &&
               IsFingerCurled(hand, XRHandFingerID.Ring) &&
               IsFingerCurled(hand, XRHandFingerID.Little) &&
               !IsFingerCurled(hand, XRHandFingerID.Thumb);
    }

    bool IsFist(XRHand hand)
    {
        // All fingers curled
        return IsFingerCurled(hand, XRHandFingerID.Thumb) &&
               IsFingerCurled(hand, XRHandFingerID.Index) &&
               IsFingerCurled(hand, XRHandFingerID.Middle) &&
               IsFingerCurled(hand, XRHandFingerID.Ring) &&
               IsFingerCurled(hand, XRHandFingerID.Little);
    }

    bool IsPeaceSign(XRHand hand)
    {
        // Index and middle extended, others curled
        return !IsFingerCurled(hand, XRHandFingerID.Index) &&
               !IsFingerCurled(hand, XRHandFingerID.Middle) &&
               IsFingerCurled(hand, XRHandFingerID.Ring) &&
               IsFingerCurled(hand, XRHandFingerID.Little);
    }

    bool IsFingerCurled(XRHand hand, XRHandFingerID fingerID)
    {
        // Get tip and proximal joints to determine curl
        XRHandJointID tipID = GetTipJoint(fingerID);
        XRHandJointID proxID = GetProximalJoint(fingerID);

        var tipJoint = hand.GetJoint(tipID);
        var proxJoint = hand.GetJoint(proxID);

        if (!tipJoint.TryGetPose(out Pose tipPose) || !proxJoint.TryGetPose(out Pose proxPose))
            return false;

        // For thumb, use a different threshold
        float threshold = (fingerID == XRHandFingerID.Thumb) ? 0.04f : 0.06f;

        // If tip is close to the palm (proximal), finger is curled
        float distance = Vector3.Distance(tipPose.position, proxPose.position);
        return distance < threshold;
    }

    XRHandJointID GetTipJoint(XRHandFingerID finger)
    {
        switch (finger)
        {
            case XRHandFingerID.Thumb: return XRHandJointID.ThumbTip;
            case XRHandFingerID.Index: return XRHandJointID.IndexTip;
            case XRHandFingerID.Middle: return XRHandJointID.MiddleTip;
            case XRHandFingerID.Ring: return XRHandJointID.RingTip;
            case XRHandFingerID.Little: return XRHandJointID.LittleTip;
            default: return XRHandJointID.IndexTip;
        }
    }

    XRHandJointID GetProximalJoint(XRHandFingerID finger)
    {
        switch (finger)
        {
            case XRHandFingerID.Thumb: return XRHandJointID.ThumbProximal;
            case XRHandFingerID.Index: return XRHandJointID.IndexProximal;
            case XRHandFingerID.Middle: return XRHandJointID.MiddleProximal;
            case XRHandFingerID.Ring: return XRHandJointID.RingProximal;
            case XRHandFingerID.Little: return XRHandJointID.LittleProximal;
            default: return XRHandJointID.IndexProximal;
        }
    }
}
