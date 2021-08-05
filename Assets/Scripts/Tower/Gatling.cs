using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Gatling : Tower
{
    [SerializeField] AnimationCurve m_Curve;
    [SerializeField, Range(0, 3)] float m_StandbySpeed = 0;
    [SerializeField] float m_StandbyMultiplier = 0;
    [SerializeField] Transform m_barrel;
    [SerializeField] float m_barrelRotationSpeed = 0;

    public override void UpgradeTower()
    {
        base.UpgradeTower();
        m_barrel = GetChild(transform, "Barrel");
    }

    public override void Attack() 
    {
        m_barrel.localEulerAngles += new Vector3(0, 0, m_barrelRotationSpeed * Time.deltaTime);
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
