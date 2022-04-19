using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] monsters;
    [SerializeField]
    int maxCount = 20;

    float term = 10f;

    IEnumerator IeStartSpawn = null;

    public bool EnableSpawn { get; set; }

    void Start()
    {
        term = IngameManager.Instance.WaveGameSpawnTerm;
        if (monsters.Length == 0)
        {
            Debug.LogError("You Should Add Monster Prefab In Inspector");
        }
    }

    private void OnDestroy()
    {
        if (IeStartSpawn != null)
        {
            StopCoroutine(IeStartSpawn);
            IeStartSpawn = null;
        }
    }

    public void StartSpawn()
    {
        if (EnableSpawn)
        {
            if (IeStartSpawn == null)
            {
                IeStartSpawn = Spawn();
                StartCoroutine(IeStartSpawn);
            }
        }
    }

    IEnumerator Spawn()
    {
        WaitForSeconds nw = new WaitForSeconds(term);
        int monsterNo = 0;
        int monsterCount = 0;

        while (EnableSpawn)
        {
            if (monsterCount <= maxCount)
            {
                Vector3 spawnPos = transform.position + (Random.insideUnitSphere * 100 - Random.insideUnitSphere * 70);
                spawnPos.y = 1;
                GetSpawnPos(ref spawnPos);

                GameObject go = Instantiate(monsters[monsterNo], spawnPos, Quaternion.identity, transform);
                Monster monster = go.GetComponent<Monster>();
                monster.InitalizeMonster(50, 5, 500);
                go.name = "Monster" + (monsterNo + 1) + "_" + monsterCount;
                monsterCount++;

                IngameManager.Instance.MonsterList.Add(monster);
            }

            yield return nw;
        }
    }

    void GetSpawnPos(ref Vector3 spawnPos)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(spawnPos, out hit, 5f, NavMesh.AllAreas))
        {
            spawnPos = hit.position;
            Debug.Log("Monster Spawn!! position : " + spawnPos);
        }
    }
}
