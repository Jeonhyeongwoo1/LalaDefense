using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Gatling : Tower
{
    [SerializeField, Range(0, 3)] float m_StandbySpeed = 0;
    [SerializeField, Range(0, 5)] float m_BulletCount = 3;
    [SerializeField] float m_StandbyMultiplier = 0;
    [SerializeField] Transform m_barrel;
    [SerializeField] float m_barrelRotationSpeed = 0;

    float m_Elapsed = 0;
    ParticleSystem muzzleEffect;

    public override void UpgradeTower()
    {
        base.UpgradeTower();
    }

    public override void Attack()
    {
        if (Target == null) { return; }

        turret.LookAt(Target.transform);

        m_barrel.localEulerAngles += new Vector3(0, 0, m_barrelRotationSpeed * Time.deltaTime);

        if (muzzleEffect.isPlaying) { muzzleEffect.Stop(); }

        m_Elapsed += Time.deltaTime;
        if (m_Elapsed >= GetCurLevelAttackInfo().speed)
        {
            m_Elapsed = 0;
            for (var i = 0; i < m_BulletCount; i++)
            {
                GameObject go = GetComponent<ObjectPool>().Get(shots, bombPoint.position)?.gameObject;
                if (go == null) { return; }
                
                Shot b = go.GetComponent<Shot>();
                b.Seek(Target);
                b.Init(GetCurLevelAttackInfo(), bombPoint, shots);
                b.Attack(() => AttackComplete(b.transform));
                muzzleEffect.Play();
            }
        }

    }

    public override void DestroyImmediate(UnityAction done = null)
    {
        towerState = TowerState.Deleting;
        DestroyShot();
        DestroyImmediate(gameObject);
    }

    public override void Standby()
    {
        if (muzzleEffect.isPlaying) { muzzleEffect.Stop(); }
        if (turret.localEulerAngles != Vector3.zero)
        {
            turret.localEulerAngles = Vector3.Slerp(turret.localEulerAngles, Vector3.zero, Time.deltaTime * 3);
        }

        //    turret.localEulerAngles += new Vector3(0, m_StandbySpeed * m_StandbyMultiplier, 0);
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

    public override void Init(Transform curTower)
    {
        m_barrel = GetChild(curTower, "Barrel");
        turret = GetChild(curTower, nameof(turret));
        projectile = GetChild(curTower, nameof(projectile));
        bombPoint = GetChild(curTower, nameof(bombPoint));
        muzzleEffect = GetChild(curTower, nameof(muzzleEffect))?.GetComponent<ParticleSystem>();
        
        ObjectPool pool = GetComponent<ObjectPool>();
        if (pool.GetCount() == 0)
        {
            pool.Initialize(shots, 12);
        }
    }

    void AttackComplete(Transform shot)
    {
        GetComponent<ObjectPool>()?.Return(shot);
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
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
