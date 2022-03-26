using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillProjectile : MonoBehaviour
{
    float velocity = 5f;
    float acceleration = 3f;

    float lifeTime = 5f;
    float currentTime = 0;
    float damage = 30f;

    protected virtual void Update()
    {
        if (Time.time - currentTime > lifeTime)
        {
            ObjectPoolManager.Instance.ReleaseObject(gameObject);
        }

        Move(ref velocity, acceleration);
    }

    protected void Move(ref float vel, float acc)
    {
        vel += acc * Time.deltaTime;
        transform.Translate(transform.forward * vel * Time.deltaTime, Space.World);
    }

    public virtual void Initialize(Transform owner, Transform target)
    {
        currentTime = Time.time;

        if (target ==  null)
        {
            transform.forward = owner.forward;
        }
        else
        {
            Vector3 dir = target.position - owner.position;
            transform.forward = dir.normalized;
        }
        //int targetRange = 100;
        //var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        //Vector3 targetDir = (ray.GetPoint(targetRange) - owner.position).normalized;
        //transform.forward = targetDir;
    }

    protected void CollsionEffect(ObjectPoolManager.EnEffectType effectType)
    {
        ObjectPoolManager.Instance.GetEffect(effectType, transform, transform);
        ObjectPoolManager.Instance.ReleaseObject(gameObject);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag != "Player")
        {
            CollsionEffect(ObjectPoolManager.EnEffectType.explosion1);
            if (collision.transform.tag == "Monster")
            {
                GameEventManager.Instance.OnEventChangeHP(collision.transform, damage);
            }
        }
    }
}

public class SkillFireBall : SkillProjectile
{
    float velocity = 7f;
    float acceleration = 3f;

    float lifeTime = 5f;
    float currentTime = 0;

    float damage = 30f;

    protected override void Update()
    {
        if (Time.time - currentTime > lifeTime)
        {
            ObjectPoolManager.Instance.ReleaseObject(gameObject);
        }

        Move(ref velocity, acceleration);
    }

    public override void Initialize(Transform owner, Transform target)
    {
        currentTime = Time.time;

        base.Initialize(owner, target);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag != "Player")
        {
            CollsionEffect(ObjectPoolManager.EnEffectType.explosion2);
            
            if (collision.transform.tag == "Monster")
            {
                GameEventManager.Instance.OnEventChangeHP(collision.transform, damage);
            }
        }
    }
}

public class SkillElectricBall : SkillProjectile
{

}

public class SkillArrow : SkillProjectile
{
    float velocity = 30f;
    float acceleration = 3f;

    float lifeTime = 20f;
    float currentTime = 0;
    float damage = 20;

    float limitRange = 200f;
    
    protected override void Update()
    {
        if (Time.time - currentTime > lifeTime ||
            velocity > limitRange)
        {
            ObjectPoolManager.Instance.ReleaseObject(gameObject);
        }

        Move(ref velocity, acceleration);
    }

    public override void Initialize(Transform owner, Transform target)
    {
        currentTime = Time.time;

        base.Initialize(owner, target);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Monster")
        {
            GameEventManager.Instance.OnEventChangeHP(collision.transform, damage);

            ObjectPoolManager.Instance.ReleaseObject(gameObject);
        }
    }
}