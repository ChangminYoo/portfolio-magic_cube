using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    InputManager inputhandler;
    MainCamera mainCamera;

    [SerializeField]
    float jumpPower = 5f;

	float radius = 0.5f;
	float height = 1.7f;
	float stamina;
	float staminaLost = 10f;
	float maxStamina;

    protected override void Start()
    {
        base.Start();
		SetAnimStateBehaviour();

		inputhandler = new InputManager();
        inputhandler.SetCommand(stateMachine);

        mainCamera = Camera.main.GetComponent<MainCamera>();

		collider.radius = radius;
		collider.height = height;
		collider.center = new Vector3(0, 0.77f, 0);

		CurrentClass = ClassManager.Instance.CurrentClass;
		//
		MaxHp = Hp = IngameManager.Instance.PlayerHP;
		maxStamina = stamina = IngameManager.Instance.PlayerStamina;
		GameEventManager.Instance.EventChangeHP += OnEventChangeHp;
	}

	private void OnDestroy()
    {
		inputhandler.Destroy();
        inputhandler = null;
        stateMachine = null;
		GameEventManager.Instance.EventChangeHP -= OnEventChangeHp;
	}

	protected override void Update()
	{
		if (IsDead) return;

		inputhandler.InputHandler();

		ResetPos();
		StaminaRestore(staminaLost);
	}

	#region Movement
	public override void Move(bool isDash, Vector2 inputDir)
	{
		//float vertical = Input.GetAxis("Vertical");
		//float horizontal = Input.GetAxis("Horizontal");
		Transform tr = mainCamera.transform;
		tr.eulerAngles = new Vector3(0, tr.eulerAngles.y, tr.eulerAngles.z);

		Vector3 forward = inputDir.y * tr.forward;
		Vector3 right = inputDir.x * tr.right;
		Vector3 moveDir = forward + right;

		if (isDash && CheckStamina(staminaLost * Time.deltaTime))
		{
			currSpeed = dashSpeed;
		}
		else
		{
			currSpeed = runSpeed;			
		}

		transform.Translate(moveDir.normalized * currSpeed * Time.deltaTime, Space.World);
		LookRotation(inputDir, tr);
	}

	public override void Jump()
	{
		rigidbd.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
	}

	public void ResetPos()
	{
		if (transform.position.y < -20f)
		{
			transform.position = initPos;
		}
	}

	public void LookRotation(Vector2 inputDir, Transform tr)
	{
		// 마우스입력과 독립적인 회전 (방향으로만 회전시 사용)
		if (inputDir != Vector2.zero)
		{
			//	1번 방식
			//	float angle = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
			//	transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref turnSmoothVelocity, 0.2f);
			//  2번 방식
			float rotateSpeed = 100f;
			Vector3 angle = Vector3.up * Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
			angle.y += tr.eulerAngles.y;
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(angle), Time.deltaTime * rotateSpeed);		
		}

		// 마우스로 카메라 회전시 캐릭터도 같이 회전하는 방식
		// float turnRate = 300f;
		// Vector3 angle = mainCamera.transform.eulerAngles;
		// transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0.0f, angle.y, 0.0f), Time.deltaTime * turnRate);
	}
	#endregion Movement

	#region Anim
	void SetAnimStateBehaviour()
	{
		AnimStateBehaviour[] animStateBehaviours = animator.GetBehaviours<AnimStateBehaviour>();

		foreach (AnimStateBehaviour stateBehaviour in animStateBehaviours)
		{
			if (stateBehaviour.Key == 0)
			{
				stateBehaviour.EventStateEnter -= OnEventIdleEnter;
				stateBehaviour.EventStateEnter += OnEventIdleEnter;
			}
		}
	}

	void OnAnimDead()
	{
		IngameManager.Instance.EndMode();
	}
	#endregion

	#region Collider
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag.CompareTo("Ground") == 0)
		{
			IsGround = true;
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.tag.CompareTo("Ground") == 0)
		{
			IsGround = false;
		}
	}
	#endregion

	#region Event
	void OnEventIdleEnter(AnimatorStateInfo stateInfo)
	{
		stateMachine.ChangeState(stateMachine.IdleState);
	}

	public override void OnEventChangeHp(Transform target, float lostHp)
	{
		if (target == transform)
		{
			Hp -= lostHp;
			Hp = Mathf.Clamp(Hp, 0, MaxHp);

			if (Hp <= 0)
			{
				if (!IsDead)
				{
					stateMachine.ChangeState(stateMachine.DeadState);
				}
			}
		}	
	}
	#endregion

	bool CheckStamina(float lost)
	{
		bool bReturn;
		if (stamina - lost > 0)
		{
			stamina -= lost;
			bReturn = true;
		}
		else
		{
			bReturn = false;
		}

		if (stamina > 0 && stamina < maxStamina)
		{
			GameEventManager.Instance.OnEventChangeStamina(stamina);
		}
		return bReturn;
	}

	void StaminaRestore(float restore)
	{
		if (IngameManager.Instance.GameMode != EnGameMode.WaveDefence) return;

		if (stateMachine.CurrentState != stateMachine.DashState)
		{
			restore *= (0.5f * Time.deltaTime);
			if (stamina < maxStamina)
			{
				stamina += restore;
				stamina = Mathf.Clamp(stamina, 0, maxStamina);
				GameEventManager.Instance.OnEventChangeStamina(stamina);
			}
		}
	}
}
