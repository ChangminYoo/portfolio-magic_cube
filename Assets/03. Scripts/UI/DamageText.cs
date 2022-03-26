using UnityEngine;
using TMPro;
public class DamageText : MonoBehaviour
{
    float lifeTime = 5f;
    float currentTime = 0;
    float damagePosOffset = 2.5f;

    TextMeshProUGUI damageText;
    public void Init(float damage)
    {
        damageText = transform.Find("DamageText").GetComponent<TextMeshProUGUI>();
        damageText.text = damage.ToString();
        damageText.color = new Color32(220, 80, 80, 255);

        RectTransform rt = transform.GetComponent<RectTransform>();
        rt.anchoredPosition3D = new Vector3(0, damagePosOffset, 0);

        RectTransform dRt = transform.Find("DamageText").GetComponent<RectTransform>();
        dRt.anchoredPosition3D = Vector3.zero;
    }

    private void LateUpdate()
    {
        if (currentTime > lifeTime)
        {
            Destroy(gameObject);
        }
        currentTime += Time.deltaTime;
        transform.LookAt(Camera.main.transform, Vector3.up);
        transform.Rotate(0, 180, 0);
    }
}
