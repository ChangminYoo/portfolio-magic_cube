using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Monster : Actor
{
    enum EnState
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        KnockBack
    }

    [SerializeField]
    EnState state = EnState.Idle;

    NavMeshAgent navAgent;
    WayPoint waypoint;

    CanvasMonsterHP hpBarCanvas;
    
    int wayPointIndex = 0;
    float checkRadius = 5f; // 플레이어 탐지 반지름
    float viewAngle = 120f; // 시야각

    [SerializeField]
    float arriveDist = 1.5f;
    [SerializeField]
    float attackDist = 1.7f;

    float damagePower = 10f;

    protected override void Start()
    {
        base.Start();
        rigidbd = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        navAgent = gameObject.AddComponent<NavMeshAgent>();
        waypoint = GameObject.Find("WayPoint")?.GetComponent<WayPoint>();
        hpBarCanvas = transform.Find("MonsterHpbarCanvas").GetComponent<CanvasMonsterHP>();
        stateMachine = new StateMachine(this);

        CurrentClass = ClassType.Monster;
        runSpeed = 2f;

        GameEventManager.Instance.EventChangeHP += OnEventChangeHp;
        //
        MaxHp = Hp = IngameManager.Instance.Monster1HP;
    }

    void OnDestroy()
    {
        stateMachine = null;
        GameEventManager.Instance.EventChangeHP -= OnEventChangeHp;
    }

    protected override void Update()
    {
        if (IsDead) return;

        switch (state)
        {
            case EnState.Idle:
                Idle();
                break;
            case EnState.Patrol:
                Patrol();
                break;
            case EnState.Chase:
                Chase();
                break;
            case EnState.Attack:
                Attack();
                break;
        }
        CheckTargetInOverlap();
    }

    public override void KnockBack(Vector3 dir, float power = 0.5f)
    {
        state = EnState.KnockBack;
        StartCoroutine(StartKnockBack(dir, power));
    }

    public void InitalizeMonster(int hp, int damage, int radius)
    {
        Hp = hp;
        damagePower = damage;
        checkRadius = radius;
    }

    #region State
    void Idle()
    {
        if (target != null)
        {
            StopMove();
            state = EnState.Chase;
        }
    }

    void Patrol()
    {
        if (waypoint == null) return;

        Vector3 destination = waypoint.WayPoints[wayPointIndex].position;
        float distFromNextPos = Vector3.Distance(transform.position, destination);
        float changeSpeed = currSpeed;

        if (distFromNextPos <= arriveDist)
        {
            changeSpeed = Mathf.Lerp(runSpeed, walkSpeed, 0.5f);
            wayPointIndex = (wayPointIndex + 1) % waypoint.WayPoints.Count;
        }
        else
        {
            changeSpeed = runSpeed;
            SetDestination(destination);
        }
        ChangeSpeed(changeSpeed);
        SetAnimMoveSpeed();
    }

    void Chase()
    {
        if (target != null)
        {
            if (CheckAttackDist())
            {
                state = EnState.Attack;
            }
            else
            {
                //float distToPlayer = Vector3.Distance(transform.position, target.position);
                //if (distToPlayer > checkRadius)
                //{
                //    state = EnState.Patrol;
                //    target = null;
                //    return;
                //}

                SetDestination(target.position);
                ChangeSpeed(runSpeed);
                SetAnimMoveSpeed();
            }
        }
    }

    void Attack()
    {
        StopMove();

        if (target != null)
        {
            transform.LookAt(target);
            stateMachine.ChangeState(stateMachine.AttackState);
        }
    }

    IEnumerator StartKnockBack(Vector3 dir, float force)
	{
        float t = 0f;
        float speed = 2f;
        Vector3 des = transform.position + force * dir;
        while (t < 1f)
		{
            transform.position = Vector3.Lerp(transform.position, des, t);

            t += Time.deltaTime * speed;
            yield return null;
		}

        state = EnState.Idle;
	}

	void Dead()
    {
        navAgent.enabled = false;
        collider.enabled = false;
        rigidbd.isKinematic = true;
    }
	#endregion

	#region Anim
	void OnAnimAttackEffect()
    {
    }

    void OnAnimDamage()
    {
        if (target != null)
        {
            if (CheckAttackDist())
            {
                GameEventManager.Instance.OnEventChangeHP(target, damagePower);
            }
        }

        state = EnState.Idle;
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    void OnAnimDead()
    {
        Destroy(gameObject);
    }
    #endregion

    void SetDestination(Vector3 target)
    {   
        stateMachine.ChangeState(stateMachine.MoveState);
        if (navAgent.isOnNavMesh)
        {
            navAgent.SetDestination(target);
        }
    }

    void ChangeSpeed(float speed)
    {
        currSpeed = speed;
        navAgent.speed = speed;
        SetAnimMoveSpeed();
    }

    void StopMove()
    {
        ChangeSpeed(0);
        if (navAgent.isOnNavMesh)
        {
            navAgent.ResetPath();
        }
    }

    void CheckTargetInOverlap()
    {
        if (target == null)
        {
            int maskLayer = 1 << LayerMask.NameToLayer("Player");
            Collider[] playerInRange = Physics.OverlapSphere(transform.position, checkRadius, maskLayer);

            if (playerInRange.Length > 0)
            {
                for (int i = 0; i < playerInRange.Length; i++)
                {
                    Transform player = playerInRange[i].transform;
                    Actor targetActor = player.GetComponent<Actor>();

                    if (targetActor != null && targetActor.IsDead) break;

                    Vector3 dir = (player.position - transform.position).normalized;

                    //if (Vector3.Angle(transform.forward, dir) <= viewAngle / 2) // 시야각 내에 있으면
                    //{
                    //    state = EnState.Chase;
                    //}

                    if (state != EnState.Attack) // 공격후 idle로 바뀐후 chase하도록
                    {
                        state = EnState.Chase;
                    }

                    target = player;
                }
            }
        }
    }

    bool CheckAttackDist()
    {
        return Vector3.Distance(transform.position, target.position) <= attackDist;
    }

    public override void OnEventChangeHp(Transform target, float lostHp)
    {
        if (target == transform)
        {
            Hp -= lostHp;
            Hp = Mathf.Clamp(Hp, 0, MaxHp);

            hpBarCanvas.TakeDamage(Hp, lostHp);

            if (Hp <= 0)
            {
                if (!IsDead)
                {
                    stateMachine.ChangeState(stateMachine.DeadState);
                    Dead();
                }
            }
        }   
    }
}
