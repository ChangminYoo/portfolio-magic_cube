using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbarianClass : BaseClass
{
	Transform effectSpawn;

	float attackDistance = 2f;
	float damage = 30;
	int viewAngle = 150;

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
				target[i].GetComponent<Actor>().KnockBack(target[i].position - transform.position);
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
