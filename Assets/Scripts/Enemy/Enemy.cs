using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

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
    TurtleShell,
    BOrc,
    Gold
}

[Serializable]
public class EnemyInfo
{
    public enum Level { Normal, Boss, Bonus }
    public Level level;
    public EnemyType enemyType;
    public float speed;
    public float health;
    public float rewardMoney;
}

public class Enemy : MonoBehaviour
{
    public enum Status { None, Normal, Attacked, beingAttacked, Die }
    public Status enemyStatus;
    public EnemyInfo enemyInfo;
    public Transform skinnedMeshRenderer;
    public Vector3 spawnOffset = Vector3.zero;
    public bool isDie = false;

    [SerializeField] Image m_HealthBar;
    [SerializeField] float m_Distance = 0;
    [SerializeField] float m_CurHealth = 0;
    [SerializeField] List<Transform> m_WayPoints = new List<Transform>();

    //데미지를 입었을 때
    [SerializeField] float m_Checkinghealth = 0;
    [SerializeField] float m_RealTimeSpeed = 0;

    Animator m_Animator;
    Transform m_Target;
    int m_WavePointIndex = 0;

    bool isbeingStunAttacked = false;
    bool isbeingElectricAttacked = false;

    public void SetAnimator(string name, bool value)
    {
        if (m_Animator == null) { m_Animator = GetComponent<Animator>(); }
        m_Animator.SetBool(name, value);
    }

    public void SetStatus(Status status) => enemyStatus = status;

    public Status GetStatus() => enemyStatus;

    public void ShowHideHealthBar(bool show) => m_HealthBar.gameObject.SetActive(show);

    public Animator GetAnimator() => m_Animator;

    public Vector3 GetBodyTransform() => skinnedMeshRenderer.position;

    public void TakeDamage(float amount, string specialAttack = null, string specialAttackInfo = null)
    {
        if (enemyStatus == Status.Normal)
        {
            enemyStatus = Status.Attacked;
        }

        m_CurHealth -= amount;
        m_HealthBar.fillAmount -= amount / enemyInfo.health;

        if (!string.IsNullOrEmpty(specialAttackInfo) && !string.IsNullOrEmpty(specialAttackInfo))
        {
            GotSpecialAttack(specialAttack, specialAttackInfo);
        }

        if (m_CurHealth <= 0)
        {
            ActionDie();
        }
    }

    void GotSpecialAttack(string specialAttack, string specialAttackInfo)
    {

        switch (specialAttack)
        {
            case "Slow":
                if (specialAttackInfo == null) { Debug.Log("Input Sepcial Attack Info"); break; }
                try
                {
                    if (isbeingStunAttacked) { break; }
                    m_RealTimeSpeed = enemyInfo.speed * (1 - (float.Parse(specialAttackInfo) / 100)); // 0.1 0.15 speed = specialAttackInfo / 100
                }
                catch (Exception e)
                {
                    Debug.Log("Error : " + e);
                    //orignally
                }
                break;
            case "Electric":
                if (specialAttackInfo == null) { Debug.Log("Input Sepcial Attack Info"); break; }

                if (!isbeingElectricAttacked) //중첩 불가
                {
                    isbeingElectricAttacked = true;
                    StartCoroutine(IsBeingElectricAttacked(specialAttackInfo, () => isbeingElectricAttacked = false));
                }
                break;
            case "Stun":
                if (specialAttackInfo == null) { Debug.Log("Input Sepcial Attack Info"); break; }

                if (!isbeingStunAttacked) //중첩 불가
                {
                    isbeingStunAttacked = true;
                    StartCoroutine(IsBeingStunAttakced(specialAttackInfo, () => isbeingStunAttacked = false));
                }
                break;
                
        }
    }

    IEnumerator IsBeingStunAttakced(string specialAttackInfo, UnityAction done = null)
    {
        //D : 3, W : 30
        //D : Duration, W : weight
        float elapsed = 0;
        char separator = ',';
        string[] sp = specialAttackInfo.Split(separator);
        string d = sp[0].Substring(sp[0].IndexOf(":") + 1);
        string w = sp[1].Substring(sp[1].IndexOf(":") + 1);
        float duration = 0;
        int wegiht = 0;
        Dictionary<string, int> random = new Dictionary<string, int>();

        try
        {
            duration = float.Parse(d);
            wegiht = int.Parse(w);
        }
        catch (Exception e)
        {
            Debug.Log("Error : " + e);
            yield break;
        }
        random.Add("Stun", wegiht);
        random.Add("None", 100 - wegiht);
        
        string value = WeightedRandomizer.TakeOne(random);
        if(value == "None")
        {
            done?.Invoke();
            yield break;
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            m_RealTimeSpeed = 0;
            yield return null;
        }

        done?.Invoke();
        m_RealTimeSpeed = enemyInfo.speed;
    }

    IEnumerator IsBeingElectricAttacked(string specialAttackInfo, UnityAction done = null)
    {
        //D:3,A:2
        //D: Duration A: AttackDmanage
        float elapsed = 0;
        char separator = ',';
        string[] sp = specialAttackInfo.Split(separator);
        string d = sp[0].Substring(sp[0].IndexOf(":") + 1);
        string a = sp[1].Substring(sp[1].IndexOf(":") + 1);
        float duration = 0;
        float attackDamage = 0;

        try
        {
            duration = float.Parse(d);
            attackDamage = float.Parse(a);
        }
        catch (Exception e)
        {
            Debug.Log("Error : " + e);
            duration = 0;
            attackDamage = 0;
            yield break;
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            TakeDamage(attackDamage);
            yield return null;
        }

        done?.Invoke();
    }

    IEnumerator CheckingStatus()
    {
        float elapsed = 0;
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        List<Color> defaultColor = new List<Color>();
        for (int i = 0; i < renderers.Length; i++)
        {
            defaultColor.Add(renderers[i].material.color);
        }

        while (enemyStatus != Status.Die)
        {
            if (enemyStatus == Status.Attacked)
            {
                ChangeRenderersColor(renderers, null);
                enemyStatus = Status.beingAttacked;
            }
            else if (enemyStatus == Status.beingAttacked)
            {
                elapsed += Time.deltaTime;
                //2초가 지난 후에 공격을 받고 있는지 확인
                if (elapsed >= 2)
                {
                    elapsed = 0;
                    if (m_Checkinghealth == m_CurHealth) //공격을 받지않는 상태..
                    {
                        enemyStatus = Status.Normal;
                        ChangeRenderersColor(renderers, defaultColor);

                        if (m_RealTimeSpeed != enemyInfo.speed)
                        {
                            m_RealTimeSpeed = enemyInfo.speed;
                        }
                    }
                }
            }

            yield return null;

        }
    }

    void ChangeRenderersColor(Renderer[] renderers, List<Color> color)
    {
        if (renderers == null) { return; }

        for (int i = 0; i < renderers.Length; i++)
        {
            if (color == null)
            {
                renderers[i].material.color = Color.red;
                continue;
            }

            renderers[i].material.color = color[i];
        }

    }

    void Die()
    {
        AnimatorStateInfo info = m_Animator.GetCurrentAnimatorStateInfo(0);

        if (info.normalizedTime >= 0.9f && info.IsName("Die"))
        {
            RemoveAliveEnemy();

            if (m_CurHealth <= 0) //When it was attacked by Tower..
            {
                UpdateUserInfoUI();
            }

            Destroy(gameObject);
        }
    }

    void RemoveAliveEnemy()
    {
        EnemyManager e = transform.parent.GetComponent<EnemyManager>();
        e.aliveEnemyCount--;
        e.aliveEnemies.Remove(this);
    }

    void UpdateUserInfoUI()
    {
        Theme theme = Core.plugs.GetPlugable<Theme>();
        UserInfoUI userInfoUI = theme.GetTheme<UserInfoUI>();
        float score = Core.gameManager.roundPlayer.roundCurScore;
        userInfoUI.score += score;
        userInfoUI.money += enemyInfo.rewardMoney;
    }

    void BounsEnemyDie()
    {
        if (m_CurHealth <= 0)
        {
            UpdateUserInfoUI();
        }

        RemoveAliveEnemy();
        Destroy(gameObject);
    }

    void ActionDie()
    {
        enemyStatus = Status.Die;
        if (enemyInfo.level == EnemyInfo.Level.Bonus)
        {
            BounsEnemyDie();
            return;
        }

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

    void EndPath()
    {
        Theme theme = Core.plugs.GetPlugable<Theme>();
        theme.GetTheme<UserInfoUI>().heart--;
        ActionDie();
    }

    void CheckHealth() => m_Checkinghealth = m_CurHealth;

    // Start is called before the first frame update
    void Start()
    {
        Terrain terrain = Core.models.GetModel<Terrain>();
        m_WayPoints.AddRange(terrain.wayPoint.wayPoints);
        m_Target = m_WayPoints[0];
        m_Animator = GetComponent<Animator>();
        m_CurHealth = enemyInfo.health;
        m_RealTimeSpeed = enemyInfo.speed;
        enemyStatus = Status.Normal;

        if (enemyInfo.level == EnemyInfo.Level.Normal)
        {
            string fwd = enemyInfo.enemyType == EnemyType.Bat || enemyInfo.enemyType == EnemyType.Dragon ? "FlyFWD" : "WalkFWD";
            m_Animator.SetBool(fwd, true);
        }

        InvokeRepeating("CheckHealth", 0, 1f);
        StartCoroutine(CheckingStatus());
    }

    // Update is called once per frame
    void Update()
    {
        if (Core.gameManager.bossAppearAniPlaying) { return; }

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

        transform.Translate(dir.normalized * m_RealTimeSpeed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, m_Target.position) <= m_Distance)
        {
            GetNextWayPoint();
        }

    }
}
