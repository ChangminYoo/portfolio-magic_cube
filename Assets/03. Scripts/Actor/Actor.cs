using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
	// Component
	protected Rigidbody rigidbd;
	protected Animator animator;
	protected StateMachine stateMachine;
	protected CapsuleCollider collider;

	protected Vector3 initPos = new Vector3(0, 10, 0);
	protected Transform target = null;

	// Actor move Params
	[Space]
	[SerializeField]
	protected float walkSpeed = 1f;
	protected float runSpeed = 3f;
	protected float dashSpeed = 6f;
	protected float currSpeed = 0f;
	
	protected float turnSmoothVelocity = 1f;

	public float MaxHp { get; protected set; }
	public float Hp { get; protected set; }
	public bool IsGround { get; protected set; }
	public ClassType CurrentClass { get; set; }
	public bool IsDead { get { return stateMachine != null && stateMachine.CurrentState == stateMachine.DeadState; } }

	protected virtual void Start()
	{
		rigidbd = GetComponent<Rigidbody>();
		collider = GetComponent<CapsuleCollider>();

		animator = GetComponent<Animator>();
		
		stateMachine = new StateMachine(this);
	}
	protected virtual void Update()
	{
	}
	#region Movement
	public virtual void Move(bool isDash, Vector2 inputDir) { }

	public virtual void Jump() { }

	protected void LookTarget(Transform tr)
	{
		transform.LookAt(tr);
	}

	public void SetAnimMoveSpeed(bool bReset = false)
	{
		if (bReset)
		{
			currSpeed = 0;
		}
		animator.SetFloat(stateMachine.MoveStateHash, currSpeed);
	}
	#endregion

	#region Anim
	public virtual void SetAnimation(int hashId, bool isTrigger)
	{
		if (isTrigger)
		{
			animator.ResetTrigger(hashId);
			animator.SetTrigger(hashId);
		}
		else
		{
			animator.SetBool(hashId, true);
		}
	}	

	public  void ResetAnimation(int hashId, bool isTrigger)
	{
		if (isTrigger)
		{
			animator.ResetTrigger(hashId);
		}
		else
		{
			animator.SetBool(hashId, false);
		}
	}
	#endregion

	#region Status
	public virtual void OnEventChangeHp(Transform target, float lostHp)
	{}
	#endregion
}
