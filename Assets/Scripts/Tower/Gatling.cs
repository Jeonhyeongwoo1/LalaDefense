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

    List<Shot> m_AliveShots = new List<Shot>();
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
                GameObject go = Instantiate(shot.gameObject, bombPoint.position, Quaternion.identity, shots);
                Shot b = go.GetComponent<Shot>();
                m_AliveShots.Add(b);
                b.Seek(Target);
                b.Init(GetCurLevelAttackInfo());
                b.Attack(AttackComplete);
                muzzleEffect.Play();
            }
        }

    }

    public override void Standby()
    {
        if (turret.localEulerAngles.x != 0)
        {
            turret.localEulerAngles = Vector3.zero;
            return;
        }

        if (muzzleEffect.isPlaying) { muzzleEffect.Stop(); }

    //    turret.localEulerAngles += new Vector3(0, m_StandbySpeed * m_StandbyMultiplier, 0);
    }

    public override void Create(UnityAction done = null)
    {
        base.Create(done);
    }

    public override void Delete(UnityAction done = null)
    {
        base.Delete(done);
    }

    public override void Init(Transform curTower)
    {
        m_barrel = GetChild(curTower, "Barrel");
        turret = GetChild(curTower, nameof(turret));
        projectile = GetChild(curTower, nameof(projectile));
        bombPoint = GetChild(curTower, nameof(bombPoint));
        muzzleEffect = GetChild(curTower, nameof(muzzleEffect))?.GetComponent<ParticleSystem>();
    }

    void AttackComplete()
    {
        if (m_AliveShots.Count != 0)
        {
            m_AliveShots.Clear();
        }
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
