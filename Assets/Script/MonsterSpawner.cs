using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{

    public float SpawnInterval = 5f;
    public int MaxMonsterCount = 5;
    int CurMonsterCount;
    public int FarDist = 5;
    public int NearDist= 3;
    public int TerrainWidth = 40;
    public Player player;

    public GameObject MonsterPrefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnMonster());

    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateMonster()
    {
        Vector3 SpawnPosition = GetSpawnPosition();
        GameObject InstantMonster = Instantiate(MonsterPrefab, SpawnPosition, Quaternion.identity);
        player.Monsters.Add(InstantMonster);
    }

    Vector3 GetSpawnPosition()
    {
        Vector3 Pos = player.transform.position;
        float x = Random.Range(-FarDist, FarDist);
        x = RandPoint(Pos.x, x);
        float z = Random.Range(-FarDist, FarDist);
        z = RandPoint(Pos.z, z);
        return Pos + new Vector3(x,0,z);
    }

    float RandPoint(float origin, float x)
    {
        while (Mathf.Abs(origin + x) < NearDist || Mathf.Abs(origin + x) > TerrainWidth) x = Random.Range(-FarDist, FarDist);
        return x;
    }

    IEnumerator SpawnMonster()
    {
        yield return new WaitForSeconds(SpawnInterval);
        if (CurMonsterCount < MaxMonsterCount) CreateMonster();
        CurMonsterCount = player.Monsters.Count;
        StartCoroutine(SpawnMonster());
    }
}
