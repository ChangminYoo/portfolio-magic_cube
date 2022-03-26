using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class BaseClass : MonoBehaviour
{
	Transform effectSpawn;
	protected Transform[] target;

	public abstract void Init();

	protected abstract void OnAnimDamage();

	protected abstract void OnAnimAttackEffect();

	protected abstract Transform FindEffectSpawnObject();

	public Transform FindRightEquipWeaponBone()
	{
		Transform root = transform.Find("RigPelvis");
		return root.Find("RigSpine1/RigSpine2/RigRibcage/RigRArm1/RigRArm2/RigRArmPalm/Dummy Prop Right");
	}

	public Transform FindLeftEquipWeaponBone()
	{
		Transform root = transform.Find("RigPelvis");
		return root.Find("RigSpine1/RigSpine2/RigRibcage/RigLArm1/RigLArm2/RigLArmPalm/Dummy Prop Left");
	}

	public void ChangeWeapon(Transform bone, Transform weapon)
	{
		Destroy(bone.GetChild(0).gameObject);

		weapon.SetParent(bone);
		weapon.localPosition = Vector3.zero;
		weapon.localRotation = Quaternion.identity;
	}

	protected Transform[] CheckTarget(float dist, int viewAngle)
	{
		target = new Transform[5];
		int maskLayer = 1 << LayerMask.NameToLayer("Monster");
		Collider[] playerInRange = Physics.OverlapSphere(transform.position, dist, maskLayer);

		int index = 0;
		if (playerInRange.Length > 0)
		{
			for (int i = 0; i < playerInRange.Length; i++)
			{
				Transform monster = playerInRange[i].transform;
				Actor targetActor = monster.GetComponent<Actor>();

				if (targetActor != null && targetActor.IsDead) break; // 이미 죽은 상태

				Vector3 dir = (monster.position - transform.position).normalized;

				if (Vector3.Angle(transform.forward, dir) <= viewAngle * 0.5f) // 시야각 내에 있으면
				{
					if (index < target.Length - 1)
					{
						target[index] = monster;
						index++;
					}
				}
			}
		}
		return target;
	}
}