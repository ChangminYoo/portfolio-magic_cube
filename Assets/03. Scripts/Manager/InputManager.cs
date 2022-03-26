using UnityEngine;
using UnityEngine.UI;

public class InputManager
{
	StateMachine stateMachine;

	ICommand command = null;
	IdleCommand commandIdle;
	MoveCommand commandMove;
	DashCommand commandDash;
	JumpCommand commandJump;
	AttackCommand commandAttack;

	JoyStick joystick;
	InputButtonEvent attackButton;
	InputButtonEvent jumpButton;
	InputButtonEvent dashButton;
	InputButtonEvent skillButton;

	bool isPressedAttack = false;
	bool isPressedJump = false;
	bool isPressedSkill = false;

	public void SetCommand(StateMachine newStateMachine)
	{
		stateMachine = newStateMachine;

		commandMove = new MoveCommand();
		commandDash = new DashCommand();
		commandJump = new JumpCommand();
		commandAttack = new AttackCommand();
		commandIdle = new IdleCommand();

		SetInputCanvas();
	}

	public void Destroy()
	{
		commandMove = null;
		commandDash = null;
		commandJump = null;
		commandAttack = null;
		commandIdle = null;

		attackButton.onClick.RemoveAllListeners();
		jumpButton.onClick.RemoveAllListeners();
		skillButton.onClick.RemoveAllListeners();
	}

	public void InputHandler()
	{
		if (joystick.IsJoyStickPressed)
		{
			if (dashButton.IsPressed)
			{
				stateMachine.MoveDir = joystick.InputDir;
				command = commandDash;
			}
			else
			{
				stateMachine.MoveDir = joystick.InputDir;
				command = commandMove;
			}
		}
		else
		{
			command = commandIdle;
		}

		if (isPressedAttack)
		{
			command = commandAttack;
		}
		if (isPressedSkill)
		{
			
		}
		if (isPressedJump)
		{
			command = commandJump;
		}

		command.Execute(stateMachine);

		ResetComand();
	}

	void SetInputCanvas()
	{
		Transform inputCanvas = GameObject.Find("InputCanvas").transform;
		joystick = inputCanvas.Find("JoyStick").GetComponent<JoyStick>();

		attackButton = inputCanvas.Find("AttackButton").GetComponent<InputButtonEvent>();
		jumpButton = inputCanvas.Find("JumpButton").GetComponent<InputButtonEvent>();
		dashButton = inputCanvas.Find("DashButton").GetComponent<InputButtonEvent>();
		skillButton = inputCanvas.Find("SkillButton").GetComponent<InputButtonEvent>();

		attackButton.onClick.AddListener(() => isPressedAttack = true);
		jumpButton.onClick.AddListener(() => isPressedJump = true);
		skillButton.onClick.AddListener(() => isPressedSkill = true); ;
	}


	void ResetComand()
	{
		command = null;
		isPressedAttack = false;
		isPressedJump = false;
		isPressedSkill = false;
	}
}