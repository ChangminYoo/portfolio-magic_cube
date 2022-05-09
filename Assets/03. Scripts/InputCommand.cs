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
	public virtual void Execute(StateMachine stateMachine)
	{
		stateMachine.ChangeState(stateMachine.AttackState);
	}
}

public class SkillCommand : ICommand
{
	public float NextAttackTime { get; protected set; }
	public float CoolTime { get; protected set; }
	public bool IsStartCoolTime { get; set; }

	public SkillCommand()
	{
		CoolTime = 0f;
		NextAttackTime = IngameManager.Instance.SkillCoolTime;
	}

	public void Execute(StateMachine stateMachine)
	{
		if (CoolTime <= 0f)
		{
			CoolTime = NextAttackTime;
			IsStartCoolTime = true;

			stateMachine.ChangeState(stateMachine.SkillState);
		}
	}

	public void CoolDown()
	{
		if (CoolTime > 0f)
		{
			CoolTime -= Time.deltaTime;
		}
	}
}