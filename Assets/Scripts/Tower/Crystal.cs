using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Crystal : Tower
{
    [SerializeField] Animator m_Animator;
    [SerializeField] Transform m_Crystal;
    Shot m_AliveShot = null;
    bool m_IsAttacking = false;

    public override void UpgradeTower()
    {
        base.UpgradeTower();

        m_IsAttacking = false;
        if(m_AliveShot != null)
        {
            Destroy(m_AliveShot.gameObject);
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
            GameObject go = Instantiate(shot.gameObject, bombPoint.position, Quaternion.identity, shots);
            Shot b = go.GetComponent<Shot>();
            m_AliveShot = b;
            b.GetComponent<ElectricBomb>()?.SetBombLevel(currentLevel);
            b.Seek(Target);
            b.Init(GetCurLevelAttackInfo());
            b.Attack(AttackComplete);
        }
    }

    void AttackComplete()
    {
        m_IsAttacking = false;
        if (m_AliveShot != null)
        {
            Destroy(m_AliveShot.gameObject);
        }
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
        base.Delete(done);

        if(m_AliveShot != null)
        {
            Destroy(m_AliveShot.gameObject);
        }
    }

    public override void Init(Transform curTower)
    {
        m_Crystal = GetChild(curTower, nameof(m_Crystal));
        projectile = GetChild(curTower, nameof(projectile));
        bombPoint = GetChild(curTower, nameof(bombPoint));
        m_Animator = GetChild(transform, nameof(projectile))?.GetComponent<Animator>();
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
