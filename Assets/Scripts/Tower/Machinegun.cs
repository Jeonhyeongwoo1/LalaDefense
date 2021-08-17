using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Machinegun : Tower
{
    [SerializeField] AnimationCurve m_Curve;

    float m_Elapsed = 0;
    
    public override void Attack()
    {
        if (Target == null) { return; }

        projectile.LookAt(Target.transform);

        m_Elapsed += Time.deltaTime;
        if(m_Elapsed >= GetCurLevelAttackInfo().speed)
        {
            m_Elapsed = 0;
            GameObject go = Instantiate(bomb.gameObject, bombPoint.position, Quaternion.identity);
            Bomb b = go.GetComponent<Bomb>();
            b.Seek(Target);
            b.Init(GetCurLevelAttackInfo().damage);
        }
    }

    public override void Standby() 
    {
        if (projectile.localEulerAngles != Vector3.zero)
        {
            projectile.localEulerAngles = Vector3.zero;
        }
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
