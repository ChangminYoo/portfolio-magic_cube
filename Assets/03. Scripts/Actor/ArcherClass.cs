using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherClass : BaseClass
{
	Transform effectSpawn;
	float attackDistance = 20f;
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
		GameObject bullet = ObjectPoolManager.Instance.GetEffect(ObjectPoolManager.EnEffectType.arrow1, effectSpawn, transform, target[0]);
	}

	protected override Transform FindEffectSpawnObject()
	{
		Transform root = transform.Find("RigPelvis");
		return root.Find("RigSpine1/RigSpine2/RigRibcage/RigLArm1/RigLArm2/RigLArmPalm/EffectSpawn");
	}
}
