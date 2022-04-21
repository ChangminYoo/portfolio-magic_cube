using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnGameMode
{
    None,
    WaveDefence,
    CubeBreak,
    Maze
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
    int waveGameSpawnTerm = 10;
    [SerializeField]
    int weaponDropPoint = 30;

    public int WaveGameLevel => waveGameLevel;
    public float PlayerHP => playerHp;
    public float PlayerStamina => playerStamina;
    public float Monster1HP => monter1Hp;
    public int WaveGameClearPoint => waveGameClearPoint;
    public int WeaponDropPoint => weaponDropPoint;
    public int WaveGameSpawnTerm => waveGameSpawnTerm;

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

		switch (GameMode)
		{
			case EnGameMode.WaveDefence:
                classManager.ChangeClass(classManager.CurrentClass);
                StartCoroutine(WaveGameStart());
                break;
			case EnGameMode.CubeBreak:
				break;
			case EnGameMode.Maze:
				break;
			default:
				break;
		}

    }

    public void EndMode()
    {
        GameState = EnGameState.End;

        switch (GameMode)
        {
            case EnGameMode.WaveDefence:
                GameEventManager.instance.OnEventWaveStart(false);
                StopCoroutine(WaveGameStart());
                ClearAllObject();
                break;
            case EnGameMode.CubeBreak:
                break;
            case EnGameMode.Maze:
                break;
            default:
                break;
        }

        GameMode = EnGameMode.None;
    }

	#region WaveGame
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
	#endregion WaveGame

	#region CubeBreak;

	#endregion CubeBreak;

	#region MazeGame
	#endregion MazeGame
}
