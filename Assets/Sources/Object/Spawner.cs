using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Spawner : NetworkBehaviour
{
    [SerializeField]
    float spawnGab = 1.5f;
    [SerializeField]
    GameObject money;
    [SerializeField]
    GameObject spawnPos;
    [SerializeField]
    Transform pos1;
    [SerializeField]
    Transform pos2;
    public bool onSpawn = false;

    float spawnTimer;
    // Use this for initialization
    void Start () {
        spawnTimer = spawnGab;
	}
	
	// Update is called once per frame
	void Update () {
        if (!onSpawn) return;
        spawnTimer -= Time.deltaTime;
		if(spawnTimer <= 0f)
        {
            GameObject newMoney = Instantiate(money, spawnPos.transform.position, spawnPos.transform.rotation);
            Destroy(newMoney, 20f);
            NetworkServer.Spawn(newMoney);
            
            spawnTimer += spawnGab + UnityEngine.Random.Range(0.0f, 1.0f) - 0.5f;
            
        }
        spawnPos.transform.position = Vector3.Lerp(pos1.position, pos2.position, UnityEngine.Random.Range(0.0f, 1.0f));
	}
}
