using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIBase : Enemy
{
	protected Transform target;

	protected Rigidbody mBody;

	protected enum EnemyState
	{
		Idle, Chasing, Attacking, Dead
	}

	protected EnemyState state;

	protected Material enemySkin;

	protected Renderer enemyRenderer;

	protected AudioSource mSource;

	public Corpse enemyCorpse;

	public AudioClip hurt, die;

	protected bool HasTarget
	{
		get
		{
			return target != null;
		}
		set
		{
			if (!value)
			{
				target = null;
			}
		}
	}

	protected override void Start()
	{
		base.Start();
		
		mBody = GetComponent<Rigidbody>();
		mSource = GetComponent<AudioSource>();

		//as a test
		state = EnemyState.Chasing;

		enemyRenderer = GetComponentInChildren<Renderer>();
		enemySkin = enemyRenderer.sharedMaterial;
	}

	protected virtual void SetTarget(Transform target)
	{
		if (TargetDistance() < TargetDistance(target))
		{
			this.target = target;
		}
	}

	protected virtual float TargetDistance(Transform target = null)
	{
		if (target == null)
		{
			target = this.target;
		}
		
		return Vector3.Distance(transform.position, target.position);
	}

	public virtual void TakeDamage(float damage, float force, Vector3 origin)
	{
		health -= damage;

		if (health <= 0f)
		{
			Destroy(gameObject);
			
			Corpse corpse = Instantiate(enemyCorpse, transform.position, transform.rotation);
			
			corpse.force = force;
			corpse.origin = origin;

			new TempAudioSource(transform.position, die);

			return;
		}

		new TempAudioSource(transform.position, hurt);

		mBody.AddForce((transform.position - origin).normalized * force / knockbackResistance, ForceMode.VelocityChange);
		Flash();

		healthBar.localScale = new Vector3(health / maxHealth, 1f, 1f);
	}

	private void Flash()
	{
		CancelInvoke("FinishFlash");
		//too lazy to cache it for now
		enemyRenderer.sharedMaterial = Resources.Load<Material>("RedFlash");
		Invoke("FinishFlash", 0.05f);
	}

	private void FinishFlash()
	{
		enemyRenderer.sharedMaterial = enemySkin;
	}
}
