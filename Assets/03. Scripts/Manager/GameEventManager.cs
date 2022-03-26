using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager
{
    public static GameEventManager instance;

    public static GameEventManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameEventManager();
            }
            return instance;
        }
        
    }

    public event Delegate<Transform, float> EventChangeHP;
    public event Delegate<float> EventChangeStamina;
    public event Delegate<int> EventPointUp;
    public event Delegate<bool> EventWaveStart;
    public event Delegate<bool> GameOver;

    public void OnEventChangeHP(Transform tr, float lostHp)
    {
        EventChangeHP?.Invoke(tr, lostHp);
    }

    public void OnEventChangeStamina(float stamina)
    {
        EventChangeStamina?.Invoke(stamina);
    }

    public void OnEventPointUp(int point)
    {
        EventPointUp?.Invoke(point);
    }

    public void OnEventWaveStart(bool isStart)
    {
        EventWaveStart?.Invoke(isStart);
    }
}
