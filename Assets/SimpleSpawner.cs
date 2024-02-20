using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSpawner : MonoBehaviour
{
	public GameObject enemy;

	public float interval = 1f;

	private float timer;

	private void Update()
	{
		timer -= Time.deltaTime;

		if (timer <= 0f)
		{
			Instantiate(enemy, transform.position, transform.rotation);
			timer = interval;
		}
	}
}
