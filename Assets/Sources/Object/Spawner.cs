using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Spawner : NetworkBehaviour
{
    [SerializeField]
    float moneySpawnGab = 1.5f;
    [SerializeField]
    float monsterSpawnGab = 30f;
    [SerializeField]
    float monsterSpawnDelay = 60f;
    [SerializeField]
    GameObject money;
    [SerializeField]
    GameObject monster;
    [SerializeField]
    GameObject spawnPos;
    [SerializeField]
    Transform pos1;
    [SerializeField]
    Transform pos2;
    public bool onSpawn = false;

    float moneySpawnTimer;
    float monsterSpawnTimer;
    // Use this for initialization
    void Start () {
        moneySpawnTimer = 0;
        monsterSpawnTimer = monsterSpawnDelay;

    }
	
	// Update is called once per frame
	void Update () {
        if (!onSpawn) return;
        moneySpawnTimer -= Time.deltaTime;
        monsterSpawnTimer -= Time.deltaTime;
        if (moneySpawnTimer <= 0f)
        {
            GameObject newMoney = Instantiate(money, spawnPos.transform.position, spawnPos.transform.rotation);
            Destroy(newMoney, 20f);
            NetworkServer.Spawn(newMoney);

            moneySpawnTimer += moneySpawnGab + UnityEngine.Random.Range(0.0f, 1.0f) - 0.5f;

        }
        if (monsterSpawnTimer <= 0f)
        {
            GameObject newMonster = Instantiate(monster, spawnPos.transform.position, spawnPos.transform.rotation);
            Destroy(newMonster, 20f);
            NetworkServer.Spawn(newMonster);

            monsterSpawnTimer += monsterSpawnGab;

        }
        spawnPos.transform.position = Vector3.Lerp(pos1.position, pos2.position, UnityEngine.Random.Range(0.0f, 1.0f));
	}
}
