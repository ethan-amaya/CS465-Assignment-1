using UnityEngine;
 

public class HandProximityInvisible : MonoBehaviour
{
    [Header("Hand Anchors")]
    [Tooltip("OVRCameraRig > TrackingSpace > LeftHandAnchor")]
    public Transform leftHandAnchor;
 
    [Tooltip("OVRCameraRig > TrackingSpace > RightHandAnchor")]
    public Transform rightHandAnchor;
 
    [Header("Proximity Settings")]
    [Tooltip("Distance in meters at which the cube becomes invisible")]
    public float proximityThreshold = 0.25f;
 
    // ── Internal state ───────────────────────────────────────────────────────────
    private Renderer _renderer;
    private Vector3  _initialPosition;
    private bool     _isNear;
 
    // ── Lifecycle ────────────────────────────────────────────────────────────────
    void Start()
    {
        _initialPosition = transform.position;
        Debug.Log($"[HandProximityInvisible] Initial position recorded: {_initialPosition}");
 
        _renderer = GetComponent<Renderer>();
        if (_renderer == null)
        {
            Debug.LogError("[HandProximityInvisible] No Renderer found on " + gameObject.name);
            enabled = false;
        }
    }
 
    void Update()
    {
        if (leftHandAnchor == null || rightHandAnchor == null)
        {
            Debug.LogWarning("[HandProximityInvisible] Hand anchors not assigned.");
            return;
        }
 
        float distLeft    = Vector3.Distance(transform.position, leftHandAnchor.position);
        float distRight   = Vector3.Distance(transform.position, rightHandAnchor.position);
        float closestDist = Mathf.Min(distLeft, distRight);
        bool  handIsNear  = closestDist < proximityThreshold;
 
        if (handIsNear != _isNear)
        {
            _isNear = handIsNear;
            _renderer.enabled = !_isNear;
 
            Debug.Log(_isNear
                ? $"[HandProximityInvisible] Hand entered at dist={closestDist:F3}m — cube hidden"
                : $"[HandProximityInvisible] Hand left   at dist={closestDist:F3}m — cube visible");
        }
    }
 
    // ── Public helpers ───────────────────────────────────────────────────────────
 
    public Vector3 GetInitialPosition() => _initialPosition;
 
    public void ResetToInitialPosition()
    {
        transform.position = _initialPosition;
        Debug.Log("[HandProximityInvisible] Cube reset to initial position.");
    }
 
#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 1f, 1f, 0.15f);
        Gizmos.DrawSphere(transform.position, proximityThreshold);
        Gizmos.color = new Color(1f, 1f, 1f, 0.8f);
        Gizmos.DrawWireSphere(transform.position, proximityThreshold);
    }
#endif
}
