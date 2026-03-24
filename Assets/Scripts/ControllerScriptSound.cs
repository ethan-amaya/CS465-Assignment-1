using UnityEngine;
 

[RequireComponent(typeof(AudioSource))]
public class HandProximitySoundTrigger : MonoBehaviour
{
    [Header("Hand Anchors")]
    [Tooltip("Assign OVRCameraRig > TrackingSpace > LeftHandAnchor")]
    public Transform leftHandAnchor;
 
    [Tooltip("Assign OVRCameraRig > TrackingSpace > RightHandAnchor")]
    public Transform rightHandAnchor;
 
    [Header("Proximity Settings")]
    [Tooltip("Distance in meters at which the sound triggers")]
    public float proximityThreshold = 0.25f;
 
    [Header("Audio")]
    [Tooltip("The sound clip to play when a hand enters the proximity zone")]
    public AudioClip proximitySound;
 
    [Tooltip("Volume of the proximity sound (0–1)")]
    [Range(0f, 1f)]
    public float volume = 1f;
 
    [Tooltip("Loop the sound while the hand remains near, or play it once on enter")]
    public bool loopWhileNear = false;
 
    // ── Internal state ───────────────────────────────────────────────────────────
    private AudioSource _audioSource;
    private Vector3     _initialPosition;
    private bool        _isNear;
 
    // ── Lifecycle ────────────────────────────────────────────────────────────────
    void Start()
    {
        // Record the cube's world-space position at startup
        _initialPosition = transform.position;
        Debug.Log($"[HandProximitySoundTrigger] Initial position recorded: {_initialPosition}");
 
        // Grab (or auto-create) the AudioSource
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip        = proximitySound;
        _audioSource.volume      = volume;
        _audioSource.loop        = loopWhileNear;
        _audioSource.playOnAwake = false;
        _audioSource.spatialBlend = 1f; // full 3-D positional audio in VR
 
        if (proximitySound == null)
            Debug.LogWarning("[HandProximitySoundTrigger] No AudioClip assigned — assign one in the Inspector.");
    }
 
    void Update()
    {
        if (leftHandAnchor == null || rightHandAnchor == null)
        {
            Debug.LogWarning("[HandProximitySoundTrigger] Hand anchors not assigned.");
            return;
        }
 
        float distLeft    = Vector3.Distance(transform.position, leftHandAnchor.position);
        float distRight   = Vector3.Distance(transform.position, rightHandAnchor.position);
        float closestDist = Mathf.Min(distLeft, distRight);
 
        bool handIsNear = closestDist < proximityThreshold;
 
        if (handIsNear != _isNear)
        {
            _isNear = handIsNear;
 
            if (_isNear)
            {
                OnHandEnter(closestDist);
            }
            else
            {
                OnHandExit(closestDist);
            }
        }
    }
 
    // ── Proximity events ─────────────────────────────────────────────────────────
 
    void OnHandEnter(float dist)
    {
        Debug.Log($"[HandProximitySoundTrigger] Hand entered zone at dist={dist:F3}m — playing sound.");
 
        if (proximitySound == null) return;
 
        if (loopWhileNear)
        {
            // Start looping audio
            _audioSource.clip = proximitySound;
            _audioSource.loop = true;
            _audioSource.Play();
        }
        else
        {
            // One-shot — won't restart if already playing
            _audioSource.PlayOneShot(proximitySound, volume);
        }
    }
 
    void OnHandExit(float dist)
    {
        Debug.Log($"[HandProximitySoundTrigger] Hand left zone at dist={dist:F3}m — stopping sound.");
 
        if (loopWhileNear)
        {
            _audioSource.Stop();
        }
        // If not looping, the one-shot plays to completion naturally — nothing to stop
    }
 
    // ── Public helpers ───────────────────────────────────────────────────────────
 
    /// <summary>Returns the cube's recorded start position.</summary>
    public Vector3 GetInitialPosition() => _initialPosition;
 
    /// <summary>Teleports the cube back to its recorded start position.</summary>
    public void ResetToInitialPosition()
    {
        transform.position = _initialPosition;
        Debug.Log("[HandProximitySoundTrigger] Cube reset to initial position.");
    }
 
#if UNITY_EDITOR
    // Draw a wire sphere in the Scene view to visualise the threshold radius
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.3f, 0.7f, 1f, 0.25f);
        Gizmos.DrawSphere(transform.position, proximityThreshold);
        Gizmos.color = new Color(0.3f, 0.7f, 1f, 0.85f);
        Gizmos.DrawWireSphere(transform.position, proximityThreshold);
    }
#endif
}
