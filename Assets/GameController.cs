using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	void Awake()
	{
		Instantiate(Resources.Load<GameObject>("Player"));
		Instantiate(Resources.Load<GameObject>("FollowCamera"));
	}
}
