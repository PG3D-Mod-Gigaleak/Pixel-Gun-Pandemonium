using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
	private Camera billboardCamera;

	private void Start()
	{
		billboardCamera = CameraFollow.Instance.GetComponent<Camera>();
		Update();
	}
	
	private void Update()
	{
		transform.LookAt(billboardCamera.transform);
	}
}
