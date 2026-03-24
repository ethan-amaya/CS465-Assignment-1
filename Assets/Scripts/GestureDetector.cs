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
        if (handSubsystem == null)
        {
            Debug.Log("Hand subsystem is NULL");
            return;
        }

        XRHand hand = isRightHand ? handSubsystem.rightHand : handSubsystem.leftHand;
        string side = isRightHand ? "Right" : "Left";

        Debug.Log($"{side} hand tracked: {hand.isTracked}");

        if (!hand.isTracked) return;

        if (IsThumbsUp(hand))
        {
            gestureUI.ShowMessage($"{side} hand thumbs up!");
            Debug.Log($"{side} hand thumbs up detected!");
        }
        else if (IsFist(hand))
        {
            gestureUI.ShowMessage($"{side} hand fist!");
            Debug.Log($"{side} hand fist detected!");
        }
        else if (IsPeaceSign(hand))
        {
            gestureUI.ShowMessage($"{side}-hand Peace Sign!");
            Debug.Log($"{side} peace sign detected!");
        }
        else
            gestureUI.ClearMessage();
    }

    bool IsThumbsUp(XRHand hand)
    {
        var thumbTip = hand.GetJoint(XRHandJointID.ThumbTip);
        var thumbProx = hand.GetJoint(XRHandJointID.ThumbProximal);

        if (!thumbTip.TryGetPose(out Pose tipPose) || !thumbProx.TryGetPose(out Pose proxPose))
            return false;

        // Thumb must be pointing upward
        Vector3 thumbDir = tipPose.position - proxPose.position;
        bool thumbPointingUp = thumbDir.y > 0.02f;

        return IsFingerCurled(hand, XRHandFingerID.Index) &&
            IsFingerCurled(hand, XRHandFingerID.Middle) &&
            IsFingerCurled(hand, XRHandFingerID.Ring) &&
            IsFingerCurled(hand, XRHandFingerID.Little) &&
            thumbPointingUp;
    }

    bool IsFist(XRHand hand)
    {
        return IsFingerCurled(hand, XRHandFingerID.Index) &&
            IsFingerCurled(hand, XRHandFingerID.Middle) &&
            IsFingerCurled(hand, XRHandFingerID.Ring) &&
            IsFingerCurled(hand, XRHandFingerID.Little) &&
            IsFingerCurled(hand, XRHandFingerID.Thumb);
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
        XRHandJointID tipID = GetTipJoint(fingerID);
        XRHandJointID proxID = GetProximalJoint(fingerID);

        var tipJoint = hand.GetJoint(tipID);
        var proxJoint = hand.GetJoint(proxID);

        if (!tipJoint.TryGetPose(out Pose tipPose) || !proxJoint.TryGetPose(out Pose proxPose))
            return false;

        float threshold = (fingerID == XRHandFingerID.Thumb) ? 0.05f : 0.07f;
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
