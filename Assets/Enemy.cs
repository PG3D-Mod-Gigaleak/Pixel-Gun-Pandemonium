using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public float speed, range, minimumDistance = 1f, knockbackResistance = 1f, health = 10f;

	protected float maxHealth;

	protected Transform healthBar;

	protected Animation mAnim;

	protected virtual void Start()
	{
		maxHealth = health;
		mAnim = transform.GetChild(0).GetComponent<Animation>();

		healthBar = transform.Find("Health_Point").GetChild(0).GetChild(0).GetChild(0);
	}
}
