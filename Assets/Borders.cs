using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Borders : MonoBehaviour
{
	public Vector2 borders;

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;

		Gizmos.DrawWireCube(transform.position, new Vector3(borders.x, 0f, borders.y));
	}

	public static Borders GetInstance()
	{
		return FindObjectOfType<Borders>();
	}
}
