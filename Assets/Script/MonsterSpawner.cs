using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{

    public float SpawnInterval = 5f;
    public float SpawnRange = 5f;
    public int MaxMonsterCount = 5;
    int CurMonsterCount;
    public int FarDist = 5;
    public int NearDist= 3;
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
        Vector3 SpawnPosition = player.transform.position + new Vector3(RandomDist(), RandomDist(), 0);
        GameObject InstantMonster = Instantiate(MonsterPrefab, SpawnPosition, Quaternion.identity);
        player.Monsters.Add(InstantMonster);
    }

    int RandomDist()
    {
        int Dist = Random.Range(-FarDist, FarDist);
        while (Mathf.Abs(Dist) < NearDist) Dist = Random.Range(-FarDist, FarDist);
        return Dist;
    }

    IEnumerator SpawnMonster()
    {
        yield return new WaitForSeconds(SpawnInterval);
        if (CurMonsterCount < MaxMonsterCount) CreateMonster();
        CurMonsterCount = player.Monsters.Count;
        StartCoroutine(SpawnMonster());
    }
}
