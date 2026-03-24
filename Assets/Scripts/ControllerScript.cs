using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.Input;
 
/// <summary>
/// Attach this script to the cube GameObject.
/// Requires: Meta XR SDK, a Rigidbody (optional), and OVRCameraRig in the scene.
/// In the Inspector, assign LeftHandAnchor and RightHandAnchor from
/// OVRCameraRig > TrackingSpace > LeftHandAnchor / RightHandAnchor.
/// </summary>
public class HandProximityColorChanger : MonoBehaviour
{
    [Header("Hand Anchors")]
    [Tooltip("Assign OVRCameraRig > TrackingSpace > LeftHandAnchor")]
    public Transform leftHandAnchor;
 
    [Tooltip("Assign OVRCameraRig > TrackingSpace > RightHandAnchor")]
    public Transform rightHandAnchor;
 
    [Header("Proximity Settings")]
    [Tooltip("Distance in meters at which the color changes")]
    public float proximityThreshold = 0.25f;
 
    [Header("Colors")]
    public Color nearColor  = Color.blue;
    public Color farColor   = Color.white;
 
    // ── Internal state ──────────────────────────────────────────────────────────
    private Renderer  _renderer;
    private Vector3   _initialPosition;
    private Color     _originalColor;
    private bool      _isNear;
 
    // ── Lifecycle ────────────────────────────────────────────────────────────────
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        if (_renderer == null)
        {
            Debug.LogError("[HandProximityColorChanger] No Renderer found on " + gameObject.name);
            enabled = false;
            return;
        }
 
        // Record the cube's world-space position at startup
        _initialPosition = transform.position;
        _originalColor   = _renderer.material.color;
 
        Debug.Log($"[HandProximityColorChanger] Initial position recorded: {_initialPosition}");
    }
 
    void Update()
    {
        if (leftHandAnchor == null || rightHandAnchor == null)
        {
            Debug.LogWarning("[HandProximityColorChanger] Hand anchors not assigned.");
            return;
        }
 
        float distLeft  = Vector3.Distance(transform.position, leftHandAnchor.position);
        float distRight = Vector3.Distance(transform.position, rightHandAnchor.position);
        float closestDist = Mathf.Min(distLeft, distRight);
 
        bool handIsNear = closestDist < proximityThreshold;
 
        // Only update material when the state actually changes (avoid per-frame GC)
        if (handIsNear != _isNear)
        {
            _isNear = handIsNear;
            _renderer.material.color = _isNear ? nearColor : _originalColor;
 
            Debug.Log(_isNear
                ? $"[HandProximityColorChanger] Hand near! dist={closestDist:F3}m — color → Near"
                : $"[HandProximityColorChanger] Hand far.   dist={closestDist:F3}m — color → Original");
        }
    }
 
    // ── Public helpers ───────────────────────────────────────────────────────────
 
    /// <summary>Returns the cube's recorded start position.</summary>
    public Vector3 GetInitialPosition() => _initialPosition;
 
    /// <summary>Teleports the cube back to its recorded start position.</summary>
    public void ResetToInitialPosition()
    {
        transform.position = _initialPosition;
        Debug.Log("[HandProximityColorChanger] Cube reset to initial position.");
    }
 
#if UNITY_EDITOR
    // Draw a wire sphere in the Scene view so you can see the threshold radius
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.3f, 0.3f, 0.35f);
        Gizmos.DrawSphere(transform.position, proximityThreshold);
        Gizmos.color = new Color(1f, 0.3f, 0.3f, 0.85f);
        Gizmos.DrawWireSphere(transform.position, proximityThreshold);
    }
#endif
}