using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyAI : EnemyAIBase
{
	protected virtual void Update()
	{
		target = GetTargetPlaceholder();

		if (state == EnemyState.Chasing && target != null)
		{
			if (TargetDistance() > minimumDistance)
			{
				mBody.MovePosition(Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed));
				mAnim.CrossFade("Zombie_Walk", 0.1f);
			}
			else
			{
				Vector3 pushDirection = (transform.position - target.position).normalized;
                mBody.MovePosition(Vector3.MoveTowards(transform.position, transform.position + (transform.position - target.position).normalized, Time.smoothDeltaTime));
			}

			mBody.MoveRotation(Quaternion.Slerp(mBody.rotation, Quaternion.Euler(0, Quaternion.LookRotation(transform.position - target.position).eulerAngles.y - 180, 0), Time.deltaTime * 15f));
		}
	}

	protected Transform GetTargetPlaceholder()
	{
		Transform player = Player.Instance.transform;

		if (Vector3.Distance(transform.position, player.position) > range)
		{
			return null;
		}

		return player;
	}

	protected virtual void OnShoot(Transform shotBy)
	{
		SetTarget(shotBy);
	}
}
