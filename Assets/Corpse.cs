using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : MonoBehaviour
{
	public float force { get; set; }

	public Vector3 origin;

	private float flashTimer = -1f, flashInterval = 3f;

	private GameObject flash;

	void Start()
	{
		flash = transform.GetChild(0).gameObject;

		foreach (Rigidbody body in GetComponentsInChildren<Rigidbody>())
		{
			body.AddForce(body.transform.position + ((transform.position - origin) * UnityEngine.Random.Range(force * 0.5f, force)), ForceMode.VelocityChange);
		}
	}

	void Update()
	{
		flashTimer += Time.deltaTime;
		flashInterval -= Time.deltaTime * 0.5f;

		if (flashInterval <= 0f)
		{
			Destroy(gameObject);
		}

		if (flashTimer >= flashInterval)
		{
			CancelInvoke("UnFlash");
			flash.SetActive(false);
			Invoke("UnFlash", 0.2f);

			flashTimer = 0f;
		}
	}

	void UnFlash()
	{
		flash.SetActive(true);
	}
}
