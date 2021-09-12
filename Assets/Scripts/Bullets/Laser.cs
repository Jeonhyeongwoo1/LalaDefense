using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Laser : Shot
{
    public LineRenderer m_Beam;
    public Transform m_BeamEffect;

    [SerializeField] Vector3 m_BeamEffectOffset = Vector3.zero;

    public override void Seek(Enemy target)
    {
        enemy = target;
    }

    public override void Init(AttackInfo info, Transform bombPoint)
    {
        this.info = info;
        this.bombPoint = bombPoint;
    }

    public override void Attack(UnityAction done = null)
    {
        if (enemy == null) { return; }
        if (!gameObject.activeSelf) { gameObject.SetActive(true); }

        Vector3 center = enemy.skinnedMeshRenderer.GetComponent<Renderer>().bounds.center;
        m_Beam.SetPosition(0, bombPoint.position);
        m_Beam.SetPosition(1, center);

        Vector3 dir = bombPoint.position - enemy.transform.position;
        m_BeamEffect.rotation = Quaternion.LookRotation(dir);
        m_BeamEffect.position = m_Beam.GetPosition(1) + m_BeamEffectOffset;

        enemy.TakeDamage(info.damage);

    }

    public override void Standby(UnityAction done = null)
    {
        if (gameObject.activeSelf) { gameObject.SetActive(false); }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
