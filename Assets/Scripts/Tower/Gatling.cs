using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Gatling : Tower
{
    [SerializeField] AnimationCurve m_Curve;
    [SerializeField, Range(0, 3)] float m_StandbySpeed = 0;
    [SerializeField, Range(0, 5)] float m_BulletCount = 3;
    [SerializeField] float m_StandbyMultiplier = 0;
    [SerializeField] Transform m_barrel;
    [SerializeField] float m_barrelRotationSpeed = 0;

    float m_Elapsed = 0;

    public override void UpgradeTower()
    {
        base.UpgradeTower();
    }

    public override void Attack()
    {
        if (m_barrel == null) { m_barrel = GetChild(transform, "Barrel"); }

        m_barrel.localEulerAngles += new Vector3(0, 0, m_barrelRotationSpeed * Time.deltaTime);

        if (Target == null) { return; }

        projectile.LookAt(Target.transform);

        m_Elapsed += Time.deltaTime;
        if (m_Elapsed >= GetCurLevelAttackInfo().speed)
        {
            m_Elapsed = 0;
            for (var i = 0; i < m_BulletCount; i++)
            {
                GameObject go = Instantiate(bomb.gameObject, bombPoint.position, Quaternion.identity);
                Bomb b = go.GetComponent<Bomb>();
                b.Seek(Target);
                b.Init(GetCurLevelAttackInfo().damage);
            }
        }

    }

    public override void Standby()
    {
        projectile.localEulerAngles += new Vector3(0, m_StandbySpeed * m_StandbyMultiplier, 0);
    }

    public override void Create(UnityAction done = null)
    {
        StartCoroutine(CoUtilize.VLerp((v) => transform.localScale = v, Vector3.zero, Vector3.one, CreateDuration, done, m_Curve));
        CreateEffectObj(createEffect);
    }

    public override void Delete(UnityAction done = null)
    {
        StartCoroutine(DeletingTower(done));
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        m_barrel = GetChild(transform, "Barrel");
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
