using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cannon : Tower
{
    [SerializeField] AnimationCurve m_Curve;
    [SerializeField, Range(0, 3)] float m_StandbySpeed = 0;
    [SerializeField] float m_StandbyMultiplier = 0;
    [SerializeField] float m_Radius;

    public Transform target;

    public override void Attack()
    {
        
    }

    public override void Standby()
    {
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

        }
    }

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_Radius);
    }

    [ContextMenu("Upgrade")]
    public void UpgradeTower()
    {
        UpgradeTower();
    }

    [ContextMenu("Attack")]
    public void Test2()
    {
        GameObject go = Instantiate(bomb.gameObject, bombPoint.position, Quaternion.identity);
        Bomb b = go.GetComponent<Bomb>();
        b.Seek(target);
    }

    [ContextMenu("Delete")]
    public void Test()
    {
        TowerManager t = transform.parent.GetComponent<TowerManager>();
        StartCoroutine(DeletingTower(() => t.DeletedTower(this)));
    }

}
