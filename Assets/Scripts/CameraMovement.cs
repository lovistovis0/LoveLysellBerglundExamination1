using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Handles camera movement.
/// </summary>
public class SmoothCameraFollow : MonoBehaviour
{
    // Target camera moves to
    [Header("Essentials")]
    [SerializeField, Tooltip("The target of the camera.")] private Transform target;
	[SerializeField, Tooltip("The damp time of the camera movement. Less is more responsive.")] private float dampTime;

	[Header("Camera Prediction")]
	[SerializeField, Tooltip("Enables camera prediction.")] private bool doCameraPrediction;
	[SerializeField, Tooltip("The value passed to the lerp between the target position and the mouse position."), Range(0, 1)] private float tValueMouseToCamera;

	[FormerlySerializedAs("doZoom")]
	[Header("Zoom")]
	[SerializeField] private bool enableZoom;
	[SerializeField, Tooltip("The added zoom to the camera for each unity of velocity of the target.")] private float zoomAmountPerVelocity;
	[SerializeField, Tooltip("The t value passed to the lerp between the current zoom and the wanted zoom."), Range(0, 1)] private float zoomTValue;

	[FormerlySerializedAs("enableBounds")]
	[Header("Bounds")]
	[SerializeField] private bool enableRectBounds;
	[SerializeField] private Vector2 cameraBoundsMin;
	[SerializeField] private Vector2 cameraBoundsMax;
	[SerializeField] private bool enableCircleBounds;
	[FormerlySerializedAs("boundsRadius")] [SerializeField] private float circleBoundsRadius;

	private Vector3 velocity = Vector3.zero;
	private Rigidbody2D targetRigdbody;
	private float originalZoom;


	// Start is called before the first frame update
	private void Start()
	{
		originalZoom = UnityEngine.Camera.main.orthographicSize;

		target.TryGetComponent<Rigidbody2D>(out targetRigdbody);

		if (targetRigdbody == null && enableZoom)
		{
			Debug.LogWarning("The target needs a Rigidbody2D to use doZoom.");
		}
    }

	// Update is called once per frame
	private void LateUpdate()
	{
		// Get player follow position
		Vector3 targetPosition;

		// If camera prediction is enabled get point
		// in between target.position and mouse position
		if (doCameraPrediction)
		{
			Vector3 mousePositionWorldSpace = UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition);
			targetPosition = Vector3.Lerp(target.position, mousePositionWorldSpace, tValueMouseToCamera);
		}
		else
		{
			targetPosition = target.position;
		}
		
		if (enableRectBounds)
		{
			targetPosition = new Vector3(
				Mathf.Clamp(targetPosition.x, cameraBoundsMin.x, cameraBoundsMax.x),
				Mathf.Clamp(targetPosition.y, cameraBoundsMin.y, cameraBoundsMax.y),
				targetPosition.z
			);
		}
		else if (enableCircleBounds)
		{
			Vector2 clampedPos = Vector2.ClampMagnitude(targetPosition, circleBoundsRadius);
			targetPosition = new Vector3(clampedPos.x, clampedPos.y, transform.position.z);
		}

		Vector3 point = UnityEngine.Camera.main.WorldToViewportPoint(targetPosition);
		Vector3 delta = targetPosition - UnityEngine.Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
		Vector3 destination = transform.position + delta;
		transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);

		if (targetRigdbody != null && enableZoom)
		{
			float targetZoom = originalZoom + targetRigdbody.velocity.magnitude * zoomAmountPerVelocity;
			UnityEngine.Camera.main.orthographicSize = Mathf.Lerp(UnityEngine.Camera.main.orthographicSize, targetZoom, zoomTValue);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		if (enableRectBounds)
		{
			Gizmos.DrawLine(new Vector2(cameraBoundsMin.x, cameraBoundsMin.y), new Vector2(cameraBoundsMin.x, cameraBoundsMax.y));
			Gizmos.DrawLine(new Vector2(cameraBoundsMin.x, cameraBoundsMin.y), new Vector2(cameraBoundsMax.x, cameraBoundsMin.y));
			Gizmos.DrawLine(new Vector2(cameraBoundsMax.x, cameraBoundsMin.y), new Vector2(cameraBoundsMax.x, cameraBoundsMax.y));
			Gizmos.DrawLine(new Vector2(cameraBoundsMin.x, cameraBoundsMax.y), new Vector2(cameraBoundsMax.x, cameraBoundsMax.y));
		}
		else if (enableCircleBounds)
		{
			Gizmos.DrawWireSphere(transform.position, circleBoundsRadius);
		}
	}
}
