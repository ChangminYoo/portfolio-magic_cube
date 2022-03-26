using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public enum EnEffectType
    {
        cube = 0,
        fireTrail,
        fireBall,
        fireBall2,
        eletricBall,
        electricBall2,
        arrow1,
        explosion1,
        explosion2,
        explosion3
    }

    // EffectPool
    [Header("Effect Pool")]
    [SerializeField]
    GameObject cube;
    [Space]

    [SerializeField]
    GameObject fireTrail;
    [SerializeField]
    GameObject fireball;
    [SerializeField]
    GameObject fireball2;

    [Space]
    [SerializeField]
    GameObject electricBall;
    [SerializeField]
    GameObject electricBall2;

    [Space]
    [SerializeField]
    GameObject arrow1;

    [Space]
    [SerializeField]
    GameObject explosion1;
    [SerializeField]
    GameObject explosion2;
    [SerializeField]
    GameObject explosion3;

    const int max_pool_size = 50;
    Vector3 initPos = new Vector3(-1000, 1000, -1000);
    List<KeyValuePair<EnEffectType, GameObject>> effectPool = new List<KeyValuePair<EnEffectType, GameObject>>();

    public static ObjectPoolManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private GameObject Create(EnEffectType effectType)
    {
        GameObject gameObject = null;
        
        switch (effectType)
        {
            case EnEffectType.cube:
                gameObject = Instantiate(cube, initPos, Quaternion.identity);
                gameObject.AddComponent<SkillProjectile>();
                break;
            case EnEffectType.fireTrail:
                gameObject = Instantiate(fireTrail, initPos, Quaternion.identity);
                gameObject.AddComponent<SkillProjectile>();
                break;
            case EnEffectType.fireBall:
                gameObject = Instantiate(fireball, initPos, Quaternion.identity);
                gameObject.AddComponent<SkillProjectile>();
                break;
            case EnEffectType.fireBall2:
                gameObject = Instantiate(fireball2, initPos, Quaternion.identity);
                gameObject.AddComponent<SkillProjectile>();
                break;
            case EnEffectType.eletricBall:
                gameObject = Instantiate(electricBall, initPos, Quaternion.identity);
                gameObject.AddComponent<SkillProjectile>();
                break;
            case EnEffectType.electricBall2:
                gameObject = Instantiate(electricBall2, initPos, Quaternion.identity);
                gameObject.AddComponent<SkillProjectile>();
                break;
            case EnEffectType.arrow1:
                gameObject = Instantiate(arrow1, initPos, Quaternion.identity);
                gameObject.AddComponent<SkillArrow>();
                break;
            case EnEffectType.explosion1:
                gameObject = Instantiate(explosion1, initPos, Quaternion.identity);
                gameObject.AddComponent<ExplosionEffect>();
                break;
            case EnEffectType.explosion2:
                gameObject = Instantiate(explosion2, initPos, Quaternion.identity);
                gameObject.AddComponent<ExplosionEffect>();
                break;
            case EnEffectType.explosion3:
                gameObject = Instantiate(explosion3, initPos, Quaternion.identity);
                gameObject.AddComponent<ExplosionEffect>();
                break;
            default:
                break;
        }

        effectPool.Add(new KeyValuePair<EnEffectType, GameObject>(effectType, gameObject));
        
        gameObject.SetActive(false);

        return gameObject;
    }

    public GameObject GetEffect(EnEffectType effectType, Transform origin, Transform owner, Transform target = null)
    {
        GameObject gameObject = null;
        List<KeyValuePair<EnEffectType, GameObject>> effectList = effectPool.FindAll((type) => type.Key == effectType);
       
        if (effectList.Count > max_pool_size
            && effectList != null)
        {
            gameObject = effectList[0].Value;
        }
        else
        {
            gameObject = Create(effectType);
        }

        gameObject.transform.position = origin.position;
        gameObject.SetActive(true);
        
        if (effectType != EnEffectType.explosion1 &&
            effectType != EnEffectType.explosion2 &&
            effectType != EnEffectType.explosion3)
        {
            gameObject.GetComponent<SkillProjectile>().Initialize(owner, target);
        }

        return gameObject;
    }

    public void ReleaseObject(GameObject gameObject)
    {
        GameObject releaseObject = effectPool.Find((obj) => obj.Value == gameObject).Value;

        gameObject.SetActive(false);
        gameObject.transform.position = initPos;
        gameObject.transform.SetParent(transform);
    }
}
