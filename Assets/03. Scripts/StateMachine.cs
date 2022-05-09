using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
	// Animator string Hash
	public int IdleStateHash { get { return Animator.StringToHash("Idle"); } }
	public int JumpStateHash { get { return Animator.StringToHash("Jump"); } }
	public int MoveStateHash { get { return Animator.StringToHash("Move Speed"); } }
	public int WalkBackStateHash { get { return Animator.StringToHash("Walk Backward"); } }	
	public int DeadStateHash { get { return Animator.StringToHash("Die"); } }
	public int ProjectileRightAttackHash { get { return Animator.StringToHash("Projectile Right Attack"); } }
	public int CrossbowShootAttackHash { get { return Animator.StringToHash("Crossbow Shoot Attack"); } }
	public int THSwordMeleeAttack1Hash { get { return Animator.StringToHash("TH Sword Melee Attack 1"); } }
	public int THSwordMeleeAttack2Hash { get { return Animator.StringToHash("TH Sword Melee Attack 2"); } }
	public int MeleeRightAttack1Hash { get { return Animator.StringToHash("Melee Right Attack 1"); } }
	public int MeleeRightAttack2Hash { get { return Animator.StringToHash("Melee Right Attack 2"); } }
	public int CrossbowShootSkillHash { get { return Animator.StringToHash("Crossbow Shoot Skill"); } }
	public int SpinSkillHash { get { return Animator.StringToHash("Spin Skill"); } }

	// State
	public IdleState IdleState { get; private set; }
	public MoveState MoveState { get; private set; }
	public DashState DashState { get; private set; }
	public JumpState JumpState { get; private set; }
	public AttackState AttackState { get; private set; }
	public SkillState SkillState { get; private set; }
	public DeadState DeadState { get; private set; }
	public BaseState CurrentState { get; private set; }

	public Actor Owner { get; private set; }
	public Monster Monster { get; private set; }

	public Vector2 MoveDir { get; set; }

	public StateMachine(Actor newActor)
	{
		IdleState = new IdleState(this, newActor);
		MoveState = new MoveState(this, newActor);
		DashState = new DashState(this, newActor);
		AttackState = new AttackState(this, newActor);
		SkillState = new SkillState(this, newActor);
		JumpState = new JumpState(this, newActor);
		DeadState = new DeadState(this, newActor);

		Owner = newActor;
		CurrentState = IdleState;
	}

	public void ChangeState(BaseState newState)
	{
		 //Debug.Log("[StateMachine] ChangeState : " + newState);

		if (CurrentState != newState)
		{ 
			CurrentState.Exit();
			CurrentState = newState;
		}
		CurrentState.Enter();
	}

	// 직업 별 공격모션 분기처리
	public int GetAttackHash()
	{
		int val = MeleeRightAttack1Hash;
		int rand = Random.Range(0, 2);
		switch (Owner.CurrentClass)
		{
			case ClassType.Human:
				val = ProjectileRightAttackHash;
				break;
			case ClassType.Barbarian:
				if (rand == 0)
				{
					val = THSwordMeleeAttack1Hash; 
				}
				else
				{
					val = THSwordMeleeAttack2Hash;
				}
				break;
			case ClassType.Knight:
				if (rand == 0)
				{
					val = MeleeRightAttack1Hash;
				}
				else
				{
					val = MeleeRightAttack2Hash;
				}
				break;
			case ClassType.Wizard:
				val = ProjectileRightAttackHash;
				break;
			case ClassType.Archer:
				val = CrossbowShootAttackHash;
				break;
			default:
				break;
		}

		return val;
	}

	public int GetSkillHash()
	{
		int val = ProjectileRightAttackHash;
		switch (Owner.CurrentClass)
		{
			case ClassType.Barbarian:
				
				break;
			case ClassType.Knight:
				
				break;
			case ClassType.Wizard:
				
				break;
			case ClassType.Archer:
				
				break;
			default:
				break;
		}

		return val;
	}
}