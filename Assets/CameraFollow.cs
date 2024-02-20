using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	private Camera mCamera;

	private Player mPlayer;

	private Borders mBorders;

	private Vector3 startingPosition;

	public Orientation[] orientations;

	public Vector4 borderLerpBounds;

	public int orientationIndex { get; private set; }

	[System.Serializable]
	public class Orientation
	{
		public Vector3 offset, rotationOffset;

		//doing this the lazy way, screw you.
		public KeyCode[] movementKeys;
	}

	public float shake { private get; set; }

	private bool shakeZero;

	public static CameraFollow Instance;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		mCamera = GetComponent<Camera>();
		mPlayer = Player.Instance;

		mBorders = Borders.GetInstance();

		startingPosition = transform.position;
	}

	private void Update()
	{
		transform.position = startingPosition + mPlayer.transform.position - orientations[orientationIndex].offset;

		Quaternion targetRotation = Quaternion.Euler(orientations[orientationIndex].rotationOffset + new Vector3(Mathf.Lerp(borderLerpBounds.x, borderLerpBounds.y, Mathf.InverseLerp(-mBorders.borders.x, mBorders.borders.x, mPlayer.transform.position.x)), Mathf.Lerp(borderLerpBounds.z, borderLerpBounds.w, Mathf.InverseLerp(-mBorders.borders.y, mBorders.borders.y, mPlayer.transform.position.z)), 0f));
		
		transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * 3f);
		transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, shakeZero ? 0 : Random.Range(-shake, shake));

		shakeZero = !shakeZero;

		if (shake > 0f)
		{
			shake -= Time.deltaTime * 10f;
		}
		
		if (Input.GetKeyDown(KeyCode.E))
		{
			orientationIndex--;

			if (orientationIndex < 0)
			{
				orientationIndex = 3;
			}

			transform.localRotation = Quaternion.Euler(orientations[orientationIndex].rotationOffset + new Vector3(Mathf.Lerp(borderLerpBounds.x, borderLerpBounds.y, Mathf.InverseLerp(-mBorders.borders.x, mBorders.borders.x, mPlayer.transform.position.x)), Mathf.Lerp(borderLerpBounds.z, borderLerpBounds.w, Mathf.InverseLerp(-mBorders.borders.y, mBorders.borders.y, mPlayer.transform.position.z)), 0f));
		}
		else if (Input.GetKeyDown(KeyCode.Q))
		{
			orientationIndex++;

			if (orientationIndex > 3)
			{
				orientationIndex = 0;
			}

			transform.localRotation = Quaternion.Euler(orientations[orientationIndex].rotationOffset + new Vector3(Mathf.Lerp(borderLerpBounds.x, borderLerpBounds.y, Mathf.InverseLerp(-mBorders.borders.x, mBorders.borders.x, mPlayer.transform.position.x)), Mathf.Lerp(borderLerpBounds.z, borderLerpBounds.w, Mathf.InverseLerp(-mBorders.borders.y, mBorders.borders.y, mPlayer.transform.position.z)), 0f));
		}
	}
}
