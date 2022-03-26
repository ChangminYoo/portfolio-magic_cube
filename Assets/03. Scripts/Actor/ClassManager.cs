using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClassType
{
    Archer = 0,
    Barbarian,
    Knight,
    Wizard,
    Human,
    Monster
}

public class ClassManager
{
    static ClassManager instance;
    public static ClassManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ClassManager();
            }

            return instance;
        }
    }

    public ClassType CurrentClass { get; set; }
    public int CostumeIndex { get; set; }
    public int FaceIndex { get; set; }

    public void ChangeClass(ClassType nextClass)
    {
        Transform player = GameObject.Find("Player").transform;
        Transform nextPlayer = null;
        
        switch (nextClass)
        {            
            case ClassType.Archer:
                nextPlayer = player.Find("Archer").GetChild(CostumeIndex);
                if (nextPlayer.gameObject.GetComponent<ArcherClass>() == null)
                {
                    nextPlayer.gameObject.AddComponent<ArcherClass>().Init();
                }
                break;
            case ClassType.Barbarian:
                nextPlayer = player.Find("Barbarian").GetChild(CostumeIndex);
                if (nextPlayer.gameObject.GetComponent<BarbarianClass>() == null)
                {
                    nextPlayer.gameObject.AddComponent<BarbarianClass>().Init();
                }
                break;
            case ClassType.Knight:
                nextPlayer = player.Find("Knight").GetChild(CostumeIndex);
                if (nextPlayer.gameObject.GetComponent<KnightClass>() == null)
                {
                    nextPlayer.gameObject.AddComponent<KnightClass>().Init();
                }
                break;
            case ClassType.Wizard:
                nextPlayer = player.Find("Wizard").GetChild(CostumeIndex);
                if (nextPlayer.gameObject.GetComponent<WizardClass>() == null)
                {
                    nextPlayer.gameObject.AddComponent<WizardClass>().Init();
                }
                break;
            case ClassType.Human:
                break;
            default:
                break;
        }

        SettingNextPlayer(nextPlayer, nextClass);
    }

    void SettingNextPlayer(Transform nextPlayer, ClassType nextClass)
    {
        nextPlayer.gameObject.SetActive(true);
        
        if (FaceIndex > 0)
        {
            FindFaceBone(nextPlayer).GetChild(FaceIndex - 1).gameObject.SetActive(true);
        }

        Camera.main.GetComponent<MainCamera>().ChangeTarget(nextPlayer);
        Player nextActor = nextPlayer.GetComponent<Player>();
        if (nextActor == null)
        {
            nextActor = nextPlayer.gameObject.AddComponent<Player>();
        }

        nextActor.CurrentClass = nextClass;
    }

    public Transform FindFaceBone(Transform character)
    {
        Transform root = character.Find("RigPelvis");
        return root.Find("RigSpine1/RigSpine2/RigRibcage/RigNeck/RigHead/Dummy Prop Head/Face");
    }
}