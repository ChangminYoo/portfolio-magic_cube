using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputManager
{
	StateMachine stateMachine;

	ICommand command = null;
	IdleCommand commandIdle;
	MoveCommand commandMove;
	DashCommand commandDash;
	JumpCommand commandJump;
	AttackCommand commandAttack;
	SkillCommand commandSkill;

	JoyStick joystick;
	InputButtonEvent attackButton;
	InputButtonEvent jumpButton;
	InputButtonEvent dashButton;
	InputButtonEvent skillButton;

	Image coolTimeImg;

	bool isPressedAttack;
	bool isPressedJump;
	bool isPressedSkill;

	public void SetCommand(StateMachine newStateMachine)
	{
		stateMachine = newStateMachine;

		commandMove = new MoveCommand();
		commandDash = new DashCommand();
		commandJump = new JumpCommand();
		commandAttack = new AttackCommand();
		commandIdle = new IdleCommand();
		commandSkill = new SkillCommand();

		SetInputCanvas();
	}

	public void Destroy()
	{
		commandMove = null;
		commandDash = null;
		commandJump = null;
		commandAttack = null;
		commandIdle = null;
		commandSkill = null;

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
		else if (isPressedSkill)
		{
			command = commandSkill;
		}

		if (isPressedJump)
		{
			command = commandJump;
		}

		command.Execute(stateMachine);
		UpdateCoolTime();
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
		skillButton.onClick.AddListener(() => StartSkillCoolTime());

		coolTimeImg = skillButton.transform.Find("CoolTime").GetComponent<Image>();
	}

	void ResetComand()
	{
		command = null;
		isPressedAttack = false;
		isPressedJump = false;
		isPressedSkill = false;
	}

	void StartSkillCoolTime()
	{
		coolTimeImg.fillAmount = 1f;
		isPressedSkill = true;
	}

	void UpdateCoolTime()
	{
		if (commandAttack == null || commandSkill == null) return;

		if (commandSkill.IsStartCoolTime)
		{
			commandSkill.CoolDown();

			float amount = commandSkill.CoolTime / commandSkill.NextAttackTime;
			if (amount <= 0f)
			{
				amount = 0f;
				commandSkill.IsStartCoolTime = false;
			}

			coolTimeImg.fillAmount = amount;
		}
	}
}