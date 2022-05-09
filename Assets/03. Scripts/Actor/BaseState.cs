using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
	protected StateMachine stateMachine;
	protected Actor actor;

	public BaseState(StateMachine newStateMachine, Actor newActor)
	{
		stateMachine = newStateMachine;
		actor = newActor;
	}

	public abstract void Enter();

	public abstract void Exit();

}

public class IdleState : BaseState
{
	public IdleState(StateMachine newStateMachine, Actor actor)
		: base(newStateMachine, actor) { }

	public override void Enter()
	{
		actor.SetAnimMoveSpeed(true);
		actor.SetAnimation(stateMachine.IdleStateHash, false);
	}
	public override void Exit()
	{
		actor.ResetAnimation(stateMachine.IdleStateHash, false);
	}
}

public class MoveState : BaseState
{
	public MoveState(StateMachine newStateMachine, Actor actor)
		   : base(newStateMachine, actor) { }

	public override void Enter()
	{
		actor.Move(false, stateMachine.MoveDir);
		actor.SetAnimMoveSpeed();
	}
	public override void Exit()
	{
		actor.SetAnimation(stateMachine.IdleStateHash, false);
	}
}

public class DashState : BaseState
{
	public DashState(StateMachine newStateMachine, Actor actor)
		   : base(newStateMachine, actor) { }

	public override void Enter()
	{
		actor.Move(true, stateMachine.MoveDir);
		actor.SetAnimMoveSpeed();
	}
	public override void Exit()
	{
		actor.SetAnimMoveSpeed();
	}
}

public class JumpState : BaseState
{
	public JumpState(StateMachine newStateMachine, Actor actor)
		   : base(newStateMachine, actor) { }

	public override void Enter()
	{
		actor.Jump();
		actor.SetAnimation(stateMachine.JumpStateHash, true);
	}
	public override void Exit()
	{
		actor.ResetAnimation(stateMachine.JumpStateHash, true);
	}
}

public class DeadState : BaseState
{
	public DeadState(StateMachine newStateMachine, Actor actor)
		   : base(newStateMachine, actor) { }

	public override void Enter()
	{
		actor.SetAnimation(stateMachine.DeadStateHash, true);
	}
	public override void Exit()
	{
		actor.ResetAnimation(stateMachine.DeadStateHash, true);
	}
}

public class AttackState : BaseState
{
	public AttackState(StateMachine newStateMachine, Actor actor)
		   : base(newStateMachine, actor) { }

	public override void Enter()
	{
		actor.SetAnimation(stateMachine.GetAttackHash(), true);
	}
	public override void Exit()
	{
		actor.ResetAnimation(stateMachine.GetAttackHash(), true);
	}
}

public class SkillState : BaseState
{
	public SkillState(StateMachine newStateMachine, Actor actor)
		   : base(newStateMachine, actor) { }

	public override void Enter()
	{
		actor.SetAnimation(stateMachine.GetAttackHash(), true);
	}
	public override void Exit()
	{
		actor.ResetAnimation(stateMachine.GetAttackHash(), true);
	}
}