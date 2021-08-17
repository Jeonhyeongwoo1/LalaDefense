using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    float m_AliveEnemyCount;
    public float aliveEnemyCount
    {
        get => m_AliveEnemyCount;
        set
        {
            m_AliveEnemyCount = value;
            if (aliveEnemyCount == 0) { RoundPlayer.Instance.SetState(RoundPlayer.RoundState.Done); }
        }
    }

    public List<Enemy> enemies = new List<Enemy>(); //적 리스트
    public List<Enemy> aliveEnemies = new List<Enemy>();
    [SerializeField, Range(0, 5)] float m_SpawnTime;

    public GameObject testEnemy;

    public void SetSpawnTime(float t) { m_SpawnTime = t; }

    public void SaveEnemyInfo() { }

    public void EnemySpawn(EnemyInfo enemy, float count)
    {
        Enemy e = enemies.Find((v) => enemy.enemyType == v.enemyInfo.enemyType);
        aliveEnemyCount = count;

        if (e == null)
        {
            Debug.Log("Enemy가 존재하지 않습니다.");
            return;
        }

        StartCoroutine(Spawning(e.transform, count));
    }

    IEnumerator Spawning(Transform enemy, float count)
    {
        Terrain terrain = FindObjectOfType<Terrain>();
        WayPoint wayPoint = terrain.wayPoint;
        Transform spawnPoint = wayPoint.wayPoints[0];

        while (count != 0)
        {
            count--;
            Transform e = Instantiate(enemy, spawnPoint.transform.position, Quaternion.identity, transform);
            aliveEnemies.Add(e.GetComponent<Enemy>());
            yield return new WaitForSeconds(m_SpawnTime);
        }

    }

    [ContextMenu("TEst2")]
    public void Test2()
    {
        StartCoroutine((Spawning(testEnemy.transform, aliveEnemyCount)));
    }

    [ContextMenu("Spawn")]
    public void Test()
    {

        StartCoroutine(sss());
    }

    IEnumerator sss()
    {
        foreach (var v in enemies)
        {
            yield return new WaitForSeconds(2);
            yield return (Spawning(v.transform, 1));
        }
    }

}
