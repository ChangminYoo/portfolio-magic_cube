using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightClass : BaseClass
{
	Transform effectSpawn;

	float attackDistance = 1.5f;
	float damage = 30;
	int viewAngle = 140;

	public override void Init()
	{
		effectSpawn = FindEffectSpawnObject();
	}

	protected override void OnAnimDamage()
	{
		CheckTarget(attackDistance, viewAngle);
		for (int i = 0; i < target.Length; i++)
		{
			if (target[i] != null)
			{
				GameEventManager.Instance.OnEventChangeHP(target[i], damage);
			}
		}
	}

	protected override void OnAnimAttackEffect()
	{		
	}

	protected override Transform FindEffectSpawnObject()
	{
		Transform root = transform.Find("RigPelvis");
		return root.Find("RigSpine1/RigSpine2/RigRibcage/RigRArm1/RigRArm2/RigRArmPalm/EffectSpawn");
	}
}
