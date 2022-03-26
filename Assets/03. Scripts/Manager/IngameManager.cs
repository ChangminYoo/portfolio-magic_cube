using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnGameMode
{
    None,
    WaveDefence,
    CubeBreak,
}

public enum EnGameState
{
    Start,
    Pause,
    End
}

public class IngameManager : MonoBehaviour
{
    static IngameManager instance;
    public static IngameManager Instance
    {
        get
        {
            var obj = FindObjectOfType<IngameManager>();
            if (obj != null)
            {
                instance = obj;
            }
            else
            {
                instance = Create();
            }
            return instance;
        }

        private set { instance = value; }
    }
    static IngameManager Create()
    {
        var sceneLoader = Resources.Load<IngameManager>("IngameManager");
        return Instantiate(sceneLoader);
    }

    int wavGamePoint = 0;

    public EnGameMode GameMode { get; private set; }
    public EnGameState GameState { get; private set; }
    [HideInInspector]
    public List<Monster> MonsterList = new List<Monster>();

    [InspectorName ("Status")]
    [SerializeField]
    float playerHp = 100;
    [SerializeField]
    float playerStamina = 100;
    [SerializeField]
    float monter1Hp = 100;

    [InspectorName("Game Condition")]
    [SerializeField]
    int waveGameClearPoint = 100;
    int waveGameLevel = 1;
    [SerializeField]
    int weaponDropPoint = 30;

    public int WaveGameLevel { get { return waveGameLevel; } }
    public float PlayerHP { get { return playerHp; } }
    public float PlayerStamina { get { return playerStamina; } }
    public float Monster1HP { get { return monter1Hp; } }
    public int WaveGameClearPoint { get { return waveGameClearPoint; } }
    public int WeaponDropPoint { get {return weaponDropPoint; } }

    void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void OnDestroy()
    {
        MonsterList = null;
    }

    public void StartMode(EnGameMode startGameMode)
    {
        ClassManager classManager = ClassManager.Instance;
        
        GameMode = startGameMode;
        GameState = EnGameState.Start;
        if (GameMode == EnGameMode.WaveDefence)
        {
            classManager.ChangeClass(classManager.CurrentClass);

            StartCoroutine(WaveGameStart());
        }
    }

    public void EndMode()
    {
        GameState = EnGameState.End;

        if (GameMode == EnGameMode.WaveDefence)
        {
            GameEventManager.instance.OnEventWaveStart(false);
            StopCoroutine(WaveGameStart());
            ClearAllObject();
        }
    }

    IEnumerator WaveGameStart()
    {
        yield return new WaitForSeconds(3f);
        GameEventManager.instance.OnEventWaveStart(true);
        
        MonsterSpawner[] spawners = FindObjectsOfType<MonsterSpawner>();

        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i].EnableSpawn = true;
            spawners[i].StartSpawn();
        }

        while (true)
        {
            if (wavGamePoint >= waveGameClearPoint)
            {
                EndMode();
                yield break;
            }
            else
            {
                for (int i = 0; i < MonsterList.Count; i++)
                {
                    if (MonsterList[i].IsDead)
                    {
                        wavGamePoint += 10;
                        if (wavGamePoint % 100 == 0)
                        {
                            waveGameLevel += 1;
                        }

                        GameEventManager.Instance.OnEventPointUp(wavGamePoint);
                        MonsterList.RemoveAt(i--);
                    }
                }
            }
            yield return null;
        }
    }

    void ClearAllObject()
    {
        for (int i = 0; i < MonsterList.Count; i++)
        {
            Destroy(MonsterList[i].gameObject);
        }

        MonsterSpawner[] spawners = FindObjectsOfType<MonsterSpawner>();
        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i].EnableSpawn = false;
        }
    }
}
