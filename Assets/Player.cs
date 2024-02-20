using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
	public static Player Instance;

	public float speed, gravity = -9.81f;

	private CharacterController mController;

	private AudioSource mSource;

	public AudioSource walkSource;

	public Animation characterAnimation;

	public Transform weaponPoint;

	private DummyWeapon[] weapons;

	private int weaponIndex;

	private DummyWeapon dummyWeapon;

	public Transform ammoBar;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		mController = GetComponent<CharacterController>();
		mSource = GetComponent<AudioSource>();

		DummyWeapon[] weapons = WeaponLoader.Load();
		this.weapons = new DummyWeapon[weapons.Length];

		for (int i = 0; i < weapons.Length; i++)
		{
			this.weapons[i] = Instantiate(weapons[i], weaponPoint);

			if (!this.weapons[i].IsMelee)
			{
				this.weapons[i].GetGunFlash().gameObject.SetActive(false);
				this.weapons[i].ammo = this.weapons[i].ammoInClip;
			}
			
			this.weapons[i].gameObject.SetActive(false);
		}

		this.weapons[weaponIndex].gameObject.SetActive(true);
		dummyWeapon = this.weapons[weaponIndex];
	}

	private void Update()
	{
		CameraFollow.Orientation orientation = CameraFollow.Instance.orientations[CameraFollow.Instance.orientationIndex];
		KeyCode[] movementKeys = orientation.movementKeys;

		if (movementKeys == null)
		{
			return;
		}

		Vector2 move = new Vector2(Input.GetKey(movementKeys[0]) ? -1 : Input.GetKey(movementKeys[2]) ? 1 : 0, Input.GetKey(movementKeys[1]) ? -1 : Input.GetKey(movementKeys[3]) ? 1 : 0);
		
		if (move.magnitude > 1)
		{
			move /= move.magnitude;
		}

		mController.Move(new Vector3(move.x, 0f, move.y) * speed * Time.deltaTime);
		mController.Move(new Vector3(0f, gravity * Time.deltaTime, 0f));

		if (move.x != 0 || move.y != 0)
		{
			if (!Input.GetMouseButton(0))
			{
				Quaternion targetRotation = Quaternion.Euler(0f, Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg, 0f);
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
			}

			if (mController.isGrounded)
			{
				characterAnimation.CrossFade(Vector3.Dot(transform.forward, new Vector3(move.x, 0f, move.y)) < 0 ? "WalkBackwards" : "Walk", 0.15f);
				walkSource.Play();
			}
			else
			{
				walkSource.Stop();
				characterAnimation.CrossFade("Idle", 0.1f);

				if (!dummyWeapon.animation.IsPlaying("Shoot") && !dummyWeapon.animation.IsPlaying("Reload"))
				{
					dummyWeapon.animation.CrossFade("Idle", 0.1f);
				}
			}

			if (!dummyWeapon.animation.IsPlaying("Shoot") && !dummyWeapon.animation.IsPlaying("Reload"))
			{
				dummyWeapon.animation.CrossFade("Walk", 0.05f);
			}
		}
		else 
		{
			walkSource.Stop();
			characterAnimation.CrossFade("Idle", 0.1f);

			if (!dummyWeapon.animation.IsPlaying("Shoot") && !dummyWeapon.animation.IsPlaying("Reload"))
			{
				dummyWeapon.animation.CrossFade("Idle", 0.1f);
			}
		}

		if (Input.GetMouseButton(0))
		{
			transform.rotation = Quaternion.Euler(0f, Mathf.Atan2(Input.mousePosition.x / Screen.width * 2f - 1, Input.mousePosition.y / Screen.height * 2f - 1) * Mathf.Rad2Deg + orientation.rotationOffset.y, 0f);

			if (!dummyWeapon.animation.IsPlaying("Shoot") && !dummyWeapon.animation.IsPlaying("Reload") && (dummyWeapon.IsMelee || dummyWeapon.ammo > 0))
			{
				dummyWeapon.animation.Play("Shoot");
				mSource.PlayOneShot(dummyWeapon.shoot);

				if (!dummyWeapon.IsMelee)
				{
					CameraFollow.Instance.shake = 1f + dummyWeapon.animation["Shoot"].length * 3.5f;
					dummyWeapon.GunFlash();

					//we are NOT putting weapon code in player beyond 1.0, you WILL move this to a REAL weapon class that isn't "dummyweapon".
					//and it WILL be abstract.

					RaycastHit hit;
					if (Physics.Raycast(dummyWeapon.GetBulletSpawn().position, transform.forward, out hit))
					{
						if (hit.transform.tag == "Enemy")
						{
							EnemyAIBase AI = hit.transform.GetComponent<EnemyAIBase>();

							if (AI != null)
							{
								AI.TakeDamage(dummyWeapon.damage, dummyWeapon.force, dummyWeapon.GetBulletSpawn().position);
							}

							//cache it later
							Vector3 normal = Quaternion.LookRotation(hit.normal).eulerAngles;
							Instantiate(Resources.Load<GameObject>("BloodParticle"), hit.point, Quaternion.Euler(normal.x + 90, normal.y, normal.z));
						}
						else
						{
							//again, cache it later
							Vector3 normal = Quaternion.LookRotation(hit.normal).eulerAngles;
							Instantiate(Resources.Load<GameObject>("Bullet_hole"), hit.point, Quaternion.Euler(normal.x + 90, normal.y, normal.z));
						}
						
					}

					dummyWeapon.ammo--;
					RefreshAmmo();
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.R) || (!dummyWeapon.IsMelee && dummyWeapon.ammo == 0))
		{
			if (!dummyWeapon.animation.IsPlaying("Shoot") && !dummyWeapon.animation.IsPlaying("Reload"))
			{
				dummyWeapon.animation.CrossFade("Reload", 0.1f);
				mSource.PlayOneShot(dummyWeapon.reload);

				dummyWeapon.ammo = dummyWeapon.ammoInClip;
				RefreshAmmo();
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			weapons[weaponIndex].gameObject.SetActive(false);

			if (weaponIndex == 0)
			{
				weaponIndex = weapons.Length - 1;
			}
			else
			{
				weaponIndex--;
			}

			weapons[weaponIndex].gameObject.SetActive(true);
			dummyWeapon = weapons[weaponIndex];

			RefreshAmmo();
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			weapons[weaponIndex].gameObject.SetActive(false);

			if (weaponIndex == weapons.Length - 1)
			{
				weaponIndex = 0;
			}
			else
			{
				weaponIndex++;
			}

			weapons[weaponIndex].gameObject.SetActive(true);
			dummyWeapon = weapons[weaponIndex];

			RefreshAmmo();
		}
	}

	public void RefreshAmmo()
	{
		if (dummyWeapon.IsMelee)
		{
			ammoBar.parent.parent.parent.gameObject.SetActive(false);
			return;
		}

		ammoBar.parent.parent.parent.gameObject.SetActive(true);

		ammoBar.localScale = new Vector3(dummyWeapon.ammo / (float)dummyWeapon.ammoInClip, 1f, 1f);
	}
}
