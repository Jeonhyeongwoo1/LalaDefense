using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Cannon : Tower
{
    [SerializeField] AnimationCurve m_Curve;
    
    float m_Elapsed = 0;

    public override void Attack()
    {
        if (Target == null) { return; }

        projectile.LookAt(Target.transform);

        m_Elapsed += Time.deltaTime;
        if (m_Elapsed >= GetCurLevelAttackInfo().speed)
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
        //조금 부자연스러운것 같음
        if (projectile.localEulerAngles != Vector3.zero)
        {
            projectile.localEulerAngles = new Vector3(0, 0, 0);
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

    public override void Start()
    {
        base.Start();
        InvokeRepeating("UpdateTarget", 0, 0.5f);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
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

    //[SerializeField] float m_Radius;
    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, m_Radius);
    // }

    [ContextMenu("Upgrade")]
    public void UpgradeTowers()
    {
       UpgradeTower();
    }

    [ContextMenu("Attack")]
    public void Test2()
    {
        GameObject go = Instantiate(bomb.gameObject, bombPoint.position, Quaternion.identity);
        Bomb b = go.GetComponent<Bomb>();
      //  b.Seek(target.transform);
    }

    [ContextMenu("Delete")]
    public void Test()
    {
        TowerManager t = transform.parent.GetComponent<TowerManager>();
        StartCoroutine(DeletingTower(() => t.DeletedTower(this)));
    }

}
