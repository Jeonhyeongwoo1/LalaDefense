using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Acid : Tower
{
    [SerializeField] AnimationCurve m_Curve;
    [SerializeField] Transform m_Turret;
    [SerializeField] float m_StandbySpeed = 1f;
    [SerializeField] float m_StandbyMultiplier = 1f;

    float m_AttackAxisX = 22f;

    public override void UpgradeTower()
    {
        base.UpgradeTower();

        m_Turret = GetChild(transform, "Turret");
    }

    public override void Attack()
    {
        m_Turret.localEulerAngles = new Vector3(m_AttackAxisX, m_Turret.localEulerAngles.y, m_Turret.localEulerAngles.z);
    }

    public override void Standby()
    {
        if (m_Turret.transform.localEulerAngles.x != 0) m_Turret.localEulerAngles = new Vector3(0, m_Turret.localEulerAngles.y, m_Turret.localEulerAngles.z);
        m_Turret.localEulerAngles += new Vector3(0, m_StandbySpeed * m_StandbyMultiplier, 0);
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
        m_Turret = GetChild(transform, "Turret");
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
