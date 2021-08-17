using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Acid : Tower
{
    [SerializeField] AnimationCurve m_Curve;
    [SerializeField] Transform m_Turret;
    [SerializeField] float m_StandbySpeed = 1f;
    [SerializeField] float m_StandbyMultiplier = 1f;
    [SerializeField] LineRenderer m_Beam;
    [SerializeField] Transform m_BeamEffect;
    [SerializeField] Vector3 m_BeamEffectOffset;
    float m_AttackAxisX = 22f;

    public override void UpgradeTower()
    {
        base.UpgradeTower();
    }

    public override void Attack()
    {
        if(m_Turret == null) { m_Turret = GetChild(transform, "Turret"); }

        m_Turret.LookAt(Target.transform);

        m_Beam.enabled = true;
        m_BeamEffect.gameObject.SetActive(true);

        //Vector3 point = Target.skinnedMeshRenderer.GetComponent<Renderer>().bounds.ClosestPointOnBounds(bombPoint.position);

        Vector3 center = Target.skinnedMeshRenderer.GetComponent<Renderer>().bounds.center;
        m_Beam.SetPosition(0, bombPoint.position);
        m_Beam.SetPosition(1, center);

        Vector3 dir = bombPoint.position - Target.transform.position;
        m_BeamEffect.rotation = Quaternion.LookRotation(dir);
        m_BeamEffect.position = Target.transform.position;
    }

    public override void Standby()
    {
        if (m_Turret == null) { m_Turret = GetChild(transform, "Turret"); }

        m_BeamEffect.gameObject.SetActive(false);
        m_Beam.enabled = false;

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
        InvokeRepeating("UpdateTarget", 0, 0.5f);
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
