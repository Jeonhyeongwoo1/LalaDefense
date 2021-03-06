using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Cannon : Tower
{
    ParticleSystem muzzleEffect;
    float m_Elapsed = 0;

    public override void Attack()
    {
        if (Target == null) { return; }

        turret.LookAt(Target.transform);

        if (muzzleEffect.isPlaying) { muzzleEffect.Stop(); }

        m_Elapsed += Time.deltaTime;
        if (m_Elapsed >= GetCurLevelAttackInfo().speed)
        {
            m_Elapsed = 0;
            GameObject go = GetComponent<ObjectPool>().Get(shots, bombPoint.position)?.gameObject;
            if (go == null) { return; }

            Shot b = go.GetComponent<Shot>();
            b.Seek(Target);
            b.Init(GetCurLevelAttackInfo(), bombPoint, shots);
            b.Attack(() => AttackComplete(b.transform));
            muzzleEffect.Play();
        }
    }

    public override void Standby()
    {
        if (muzzleEffect.isPlaying) { muzzleEffect.Stop(); }
        if (turret.localEulerAngles != Vector3.zero)
        {
            turret.localEulerAngles = Vector3.Slerp(turret.localEulerAngles, Vector3.zero, Time.deltaTime * 3);
        }
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

    public override void DestroyImmediate(UnityAction done = null)
    {
        towerState = TowerState.Deleting;
        DestroyShot();
        DestroyImmediate(gameObject);
    }

    public override void Init(Transform curTower)
    {
        turret = GetChild(curTower, nameof(turret));
        projectile = GetChild(curTower, nameof(projectile));
        bombPoint = GetChild(curTower, nameof(bombPoint));
        muzzleEffect = GetChild(curTower, nameof(muzzleEffect))?.GetComponent<ParticleSystem>();

        ObjectPool pool = GetComponent<ObjectPool>();
        if (pool.GetCount() == 0)
        {
            pool.Initialize(shots, 2);
        }
    }

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0, 0.5f);
    }

    void AttackComplete(Transform shot)
    {
        GetComponent<ObjectPool>()?.Return(shot);
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

}
