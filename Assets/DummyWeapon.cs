using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyWeapon : MonoBehaviour
{
	public AudioClip shoot, reload;

	public new Animation animation;

	public float damage, force;

	public int ammo { get; set; }

	public int ammoInClip;

	public bool IsMelee
	{
		get
		{
			return GetBulletSpawn() == null;
		}
	}

	public Transform GetBulletSpawn()
	{
		return transform.Find("BulletSpawnPoint");
	}

	public Transform GetGunFlash()
	{
		return GetBulletSpawn().GetChild(0);
	}

	public void GunFlash()
	{
		CancelInvoke("DisableGunFlash");
		GetGunFlash().gameObject.SetActive(true);
		Invoke("DisableGunFlash", 0.1f);
	}

	private void DisableGunFlash()
	{
		GetGunFlash().gameObject.SetActive(false);
	}
}
