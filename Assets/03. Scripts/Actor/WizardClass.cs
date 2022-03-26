using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardClass : BaseClass
{
	Transform effectSpawn;
	float attackDistance = 15f;
	int viewAngle = 20;

	public override void Init()
	{
		effectSpawn = FindEffectSpawnObject();
	}

	protected override void OnAnimDamage()
	{
	}

	protected override void OnAnimAttackEffect()
	{
		CheckTarget(attackDistance, viewAngle);
		GameObject bullet = ObjectPoolManager.Instance.GetEffect(ObjectPoolManager.EnEffectType.cube, effectSpawn, transform, target[0]);
	}

	protected override Transform FindEffectSpawnObject()
	{
		Transform root = transform.Find("RigPelvis");
		return root.Find("RigSpine1/RigSpine2/RigRibcage/RigRArm1/RigRArm2/RigRArmPalm/EffectSpawn");
	}
}
