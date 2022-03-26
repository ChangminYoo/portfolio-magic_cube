using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ItemDropper : MonoBehaviour
{
    List<GameObject> archerWeapons = new List<GameObject>();
    List<GameObject> barbarianWeapons = new List<GameObject>();
    List<GameObject> knightWeapons = new List<GameObject>();
    List<GameObject> wizardWeapons = new List<GameObject>();

    ClassManager classManager;
    IngameManager ingameManager;

    SphereCollider collider;
    GameObject weaponEffect;
    GameObject activedWeapon;
    GameObject minimapIcon;

    int weaponIndex = 0;

    void Start()
    {
        classManager = ClassManager.Instance;
        ingameManager = IngameManager.Instance;

        Transform archerWeapon = transform.Find("ArcherWeapon");
        Transform barbarianWeapon = transform.Find("BarbarianWeapon");
        Transform knightWeapon = transform.Find("KnightWeapon");
        Transform wizardWeapon = transform.Find("WizardWeapon");

        weaponEffect = transform.Find("WeaponEffect").gameObject;
        minimapIcon = transform.Find("MinmapIcon").gameObject;

        collider = transform.GetComponent<SphereCollider>();
        collider.enabled = false;

        for (int i = 0; i < archerWeapon.childCount; i++)
        {
            archerWeapons.Add(archerWeapon.GetChild(i).gameObject);
        }
        for (int i = 0; i < barbarianWeapon.childCount; i++)
        {
            barbarianWeapons.Add(barbarianWeapon.GetChild(i).gameObject);
        }
        for (int i = 0; i < knightWeapon.childCount; i++)
        {
            knightWeapons.Add(knightWeapon.GetChild(i).gameObject);
        }
        for (int i = 0; i < wizardWeapon.childCount; i++)
        {
            wizardWeapons.Add(wizardWeapon.GetChild(i).gameObject);
        }

        GameEventManager.instance.EventPointUp += PointUp;
    }

    void OnDestroy()
    {
        classManager = null;
        ingameManager = null;

        GameEventManager.instance.EventPointUp -= PointUp;

        archerWeapons.Clear();
        barbarianWeapons.Clear();
        knightWeapons.Clear();
        wizardWeapons.Clear();
    }

    void PointUp(int point)
    {
        if (point % ingameManager.WeaponDropPoint == 0)
        {
            switch (classManager.CurrentClass)
            {
                case ClassType.Archer:
                    if (weaponIndex >= archerWeapons.Count) return;

                    activedWeapon = archerWeapons[weaponIndex];
                    break;
                case ClassType.Barbarian:
                    if (weaponIndex >= barbarianWeapons.Count) return;

                    activedWeapon = barbarianWeapons[weaponIndex];
                    break;
                case ClassType.Knight:
                    if (weaponIndex >= knightWeapons.Count) return;

                    activedWeapon = knightWeapons[weaponIndex];
                    break;
                case ClassType.Wizard:
                    if (weaponIndex >= wizardWeapons.Count) return;

                    activedWeapon = wizardWeapons[weaponIndex];
                    break;
                default:
                    return;
            }
            weaponIndex++;

            DropWeapon(true);
        }
    }

    void DropWeapon(bool bActive)
    {
        if (activedWeapon != null)
        {
            activedWeapon.SetActive(bActive);
        }
        weaponEffect.SetActive(bActive);
        minimapIcon.SetActive(bActive);
        collider.enabled = bActive;
    }

    void EquipWeapon(Transform player)
    {
        Transform equipTarget = null;
        BaseClass playerClass = player.GetComponent<BaseClass>();
        if (classManager.CurrentClass == ClassType.Archer)
        {
            equipTarget = playerClass.FindLeftEquipWeaponBone();
        }
        else
        {
            equipTarget = playerClass.FindRightEquipWeaponBone();
        }

        playerClass.ChangeWeapon(equipTarget, activedWeapon.transform);
        activedWeapon = null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EquipWeapon(other.transform);
            DropWeapon(false);
        }
    }
}
