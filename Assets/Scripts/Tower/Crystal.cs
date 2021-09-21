using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Crystal : Tower
{
    [SerializeField] Animator m_Animator;
    [SerializeField] Transform m_Crystal;
    bool m_IsAttacking = false;
    Shot m_AliveShot = null;

    public override void UpgradeTower()
    {
        base.UpgradeTower();

        m_IsAttacking = false;
        if(m_AliveShot != null)
        {
            m_AliveShot.GetComponent<ElectricBomb>().ResetState();
            GetComponent<ObjectPool>()?.Return(m_AliveShot.transform);
        }
    }

    public override void Attack() 
    {
        if(!m_Animator)
        {
            m_Animator = GetChild(transform, nameof(projectile))?.GetComponent<Animator>();
        }

        m_Animator.enabled = false;

        while(!m_IsAttacking)
        {
            m_IsAttacking = true;
            GameObject go = GetComponent<ObjectPool>().Get(shots, bombPoint.position)?.gameObject;
            if (go == null) { return; }

            Shot b = go.GetComponent<Shot>();
            m_AliveShot = b;
            b.GetComponent<ElectricBomb>()?.SetBombLevel(currentLevel);
            b.Seek(Target);
            b.Init(GetCurLevelAttackInfo(), bombPoint, shots);
            b.Attack(()=> AttackComplete(b.transform));
        }
    }

    void AttackComplete(Transform shot)
    {
        m_IsAttacking = false;
        shot.GetComponent<ElectricBomb>().ResetState();
        GetComponent<ObjectPool>()?.Return(shot);
    }

    public override void Standby()
    {
        if (!m_Animator)
        {
            m_Animator = GetChild(transform, nameof(projectile))?.GetComponent<Animator>();
        }

        m_Animator.enabled = true;
    }

    public override void Create(UnityAction done = null)
    {
        base.Create(done);
    }

    public override void Delete(UnityAction done = null)
    {
        DestroyShot();
        base.Delete(done);
    }

    public override void DestroyImmediate(UnityAction done = null)
    {
        towerState = TowerState.Deleting;
        DestroyShot();
        DestroyImmediate(gameObject);
    }

    public override void Init(Transform curTower)
    {
        m_Crystal = GetChild(curTower, nameof(m_Crystal));
        projectile = GetChild(curTower, nameof(projectile));
        bombPoint = GetChild(curTower, nameof(bombPoint));
        m_Animator = GetChild(transform, nameof(projectile))?.GetComponent<Animator>();

        ObjectPool pool = GetComponent<ObjectPool>();
        if (pool.GetCount() == 0)
        {
            pool.Initialize(shots, 2);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (towerState == TowerState.Creating || towerState == TowerState.Upgrading) { return; }

        if (towerState == TowerState.Standby)
        {
            Standby();
        }
        else if (towerState == TowerState.Attack)
        {
            Attack();
        }
        
    }
}
