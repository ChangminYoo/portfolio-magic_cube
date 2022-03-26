using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] monsters;
    [SerializeField]
    int maxCount = 20;

    float tearm = 10f;

    IEnumerator IeStartSpawn = null;

    public bool EnableSpawn { get; set; }

    void Start()
    {
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
        WaitForSeconds nw = new WaitForSeconds(tearm);
        int monsterNo = 0;
        int monsterCount = 0;

        while (EnableSpawn)
        {
            if (monsterCount <= maxCount)
            {
                Vector3 pos = transform.position + (Random.insideUnitSphere * 20);
                pos.y = 0;

                GameObject go = Instantiate(monsters[monsterNo], transform.position, Quaternion.identity, transform);
                Monster monster = go.GetComponent<Monster>();
                monster.InitalizeMonster(50, 5, 500);
                go.name = "Monster" + (monsterNo + 1) + "_" + monsterCount;
                monsterCount++;

                IngameManager.Instance.MonsterList.Add(monster);
            }

            yield return nw;
        }
    }
}
