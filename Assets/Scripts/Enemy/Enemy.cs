using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum EnemyType
{
    Bat,
    Dragon,
    EvilMage,
    Golem,
    MonsterPlant,
    Orc,
    Skeleton,
    Spider,
    Slime,
    TurtleShell
}

[Serializable]
public class EnemyInfo
{
    public EnemyType enemyType;
    public float speed;
    public float health;
    public float rewardMoney;
}

public class Enemy : MonoBehaviour
{
    public EnemyInfo enemyInfo;
    public Transform skinnedMeshRenderer;
    public bool isDie = false;

    [SerializeField] Image m_HealthBar;
    [SerializeField] float m_Distance = 0;
    [SerializeField] float m_CurHealth = 0;
    [SerializeField] List<Transform> m_WayPoints = new List<Transform>();

    Animator m_Animator;
    Transform m_Target;
    int m_WavePointIndex = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        Terrain terrain = FindObjectOfType<Terrain>();
        m_WayPoints.AddRange(terrain.wayPoint.wayPoints);
        m_Target = m_WayPoints[0];
        m_Animator = GetComponent<Animator>();
        string fwd = enemyInfo.enemyType == EnemyType.Bat || enemyInfo.enemyType == EnemyType.Dragon ? "FlyFWD" : "WalkFWD";
        m_Animator.SetBool(fwd, true);
        m_CurHealth = enemyInfo.health;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDie)
        {
            Die();
            return;
        }

        Vector3 dir = m_Target.position - transform.position;
        Vector3 s = dir.normalized;
        s = new Vector3(Mathf.Round(s.x), Mathf.Round(s.y), Mathf.Round(s.z));

        switch (s)
        {
            case Vector3 v when v.Equals(Vector3.forward):
                transform.localEulerAngles = Vector3.zero;
                break;
            case Vector3 v when v.Equals(Vector3.right):
                transform.localEulerAngles = new Vector3(0, 90, 0);
                break;
            case Vector3 v when v.Equals(Vector3.left):
                transform.localEulerAngles = new Vector3(0, -90, 0);
                break;
            case Vector3 v when v.Equals(Vector3.back):
                transform.localEulerAngles = new Vector3(0, 180, 0);
                break;
        }

        transform.Translate(dir.normalized * enemyInfo.speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, m_Target.position) <= m_Distance)
        {
            GetNextWayPoint();
        }

    }

    public void TakeDamage(float amount)
    {
        m_CurHealth -= amount;
        m_HealthBar.fillAmount -= amount / enemyInfo.health;

        if (m_CurHealth <= 0)
        {
            ActionDie();
        }
    }

    void Die()
    {
        AnimatorStateInfo info = m_Animator.GetCurrentAnimatorStateInfo(0);

        if (info.normalizedTime >= 0.9f && info.IsName("Die"))
        {
            EnemyManager e = transform.parent.GetComponent<EnemyManager>();
            e.aliveEnemyCount--;
            e.aliveEnemies.Remove(this);
            Destroy(gameObject);
        }
    }

    void ActionDie()
    {
        m_Animator.SetBool("Die", true);
        isDie = true;
    }

    void GetNextWayPoint()
    {
        if (m_WavePointIndex >= m_WayPoints.Count - 1)
        {
            EndPath();
            return;
        }

        m_WavePointIndex++;
        m_Target = m_WayPoints[m_WavePointIndex];
    }

    void EndPath() { ActionDie(); }

}
