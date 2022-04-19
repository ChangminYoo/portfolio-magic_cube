using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class CanvasMonsterHP : MonoBehaviour
{
    Transform hpBar;
    Slider hpSlider;
    
    [SerializeField]
    GameObject damageObject;
    Transform damage;

    float hpPosOffset = 2f;
    float updateHpSeconds = 0.5f;

    IEnumerator IeChangeHp;
    void Start()
    {
        hpBar = transform.Find("HPSlider");
        hpSlider = hpBar.GetComponent<Slider>();
    }

    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform, Vector3.up);
        if (hpSlider.value <= 0)
		{
            gameObject.SetActive(false);
		}
    }

    private void OnDestroy()
    {
        if (IeChangeHp != null)
        {
            StopCoroutine(IeChangeHp);
            IeChangeHp = null;
        }
    }

    public void TakeDamage(float hp, float lostHp)
    {
        CreateDamageText(lostHp);

        if (IeChangeHp != null)
        {
            StopCoroutine(IeChangeHp);
            IeChangeHp = null;
        }        
        IeChangeHp = ChangeHpBar(hp);        
        StartCoroutine(IeChangeHp);
    }

    IEnumerator ChangeHpBar(float hp)
    {
        float target = hp * 0.01f;
        float elpasedTime = 0f;

        while (elpasedTime < updateHpSeconds)
        {
            elpasedTime += Time.deltaTime;
            hpSlider.value = Mathf.Lerp(hpSlider.value, target, elpasedTime / updateHpSeconds);
            yield return null;
        }

        IeChangeHp = null;
    }

    void CreateDamageText(float damage)
    {
        GameObject go = Instantiate(damageObject, Vector3.zero, Quaternion.identity, transform);
        DamageText dt = go.transform.GetComponent<DamageText>();
        dt.Init(damage);
    }
}
