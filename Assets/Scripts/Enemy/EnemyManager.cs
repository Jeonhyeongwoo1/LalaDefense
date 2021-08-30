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
            
            if (aliveEnemyCount == 0)
            {
                Core.gameManager?.roundPlayer?.SetState(RoundPlayer.RoundState.Done);
            }
        }
    }

    public List<Enemy> enemies = new List<Enemy>(); //적 리스트
    public List<Enemy> aliveEnemies = new List<Enemy>();
    [SerializeField, Range(0, 5)] float m_SpawnTime;

    public GameObject testEnemy;

    public void SetSpawnTime(float t) { m_SpawnTime = t; }

    public void SaveEnemyInfo() { }

    public void DestroyEnemy()
    {
        if (aliveEnemies == null) { return; }
        if (aliveEnemies.Count == 0) { return; }

        aliveEnemies.ForEach((v) => Destroy(v.gameObject));
        aliveEnemies.Clear();
        aliveEnemyCount = 0;
    }

    public void EnemySpawn(EnemyInfo enemy, float count)
    {
        Enemy e = enemies.Find((v) => enemy.enemyType == v.enemyInfo.enemyType
                                            && enemy.level == v.enemyInfo.level);
        aliveEnemyCount = count;

        if (e == null)
        {
            Debug.Log("Enemy가 존재하지 않습니다.");
            return;
        }

        if(aliveEnemyCount == 0) 
        {
            Debug.Log("Enemy Count가 0 입니다.");
            return;
        }

        StartCoroutine(Spawning(e, enemy, count));
    }

    IEnumerator Spawning(Enemy enemy, EnemyInfo enemyInfo, float count)
    {
        Terrain terrain = Core.models.GetModel<Terrain>();
        WayPoint wayPoint = terrain.wayPoint;
        Transform spawnPoint = wayPoint.wayPoints[0];

        while (count != 0)
        {
            count--;
            Transform e = Instantiate(enemy.transform, spawnPoint.position, Quaternion.identity, transform);
            e.GetComponent<Enemy>().enemyInfo = enemyInfo;
            aliveEnemies.Add(e.GetComponent<Enemy>());
            yield return new WaitForSeconds(m_SpawnTime);
        }

    }
    
}
