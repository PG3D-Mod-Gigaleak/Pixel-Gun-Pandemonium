using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WeaponLoader
{
	public static DummyWeapon[] Load()
	{
		List<DummyWeapon> weapons = new List<DummyWeapon>();

		for (int i = 1;; i++)
		{
			GameObject weapon = Resources.Load<GameObject>("weapons/weapon" + i);

			if (weapon == null)
			{
				break;
			}

			weapons.Add(weapon.GetComponent<DummyWeapon>());
		}

		return weapons.ToArray();
	}
}
