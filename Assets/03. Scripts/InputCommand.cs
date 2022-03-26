using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
	void Execute(StateMachine stateMachine);
}

public class IdleCommand : ICommand
{
	public void Execute(StateMachine stateMachine)
	{
		stateMachine.ChangeState(stateMachine.IdleState);
	}
}

public class MoveCommand : ICommand
{
	public void Execute(StateMachine stateMachine)
	{
		stateMachine.ChangeState(stateMachine.MoveState);
	}
}

public class DashCommand : ICommand
{
	public void Execute(StateMachine stateMachine)
	{	
		stateMachine.ChangeState(stateMachine.DashState);
	}
}

public class JumpCommand : ICommand
{
	public void Execute(StateMachine stateMachine)
	{
		if (stateMachine.Owner.IsGround)
		{
			stateMachine.ChangeState(stateMachine.JumpState);		
		}
	}
}

public class AttackCommand : ICommand
{
	public float NextAttackTime { get; set; }
	public float CoolTime { get; private set; }
	public AttackCommand()
	{
		NextAttackTime = 0f;
		CoolTime = 1f;
	}

	public void Execute(StateMachine stateMachine)
	{
		if (CoolTime < Time.time - NextAttackTime)
		{
			NextAttackTime = Time.time;
			stateMachine.ChangeState(stateMachine.AttackState);
		}
	}
}