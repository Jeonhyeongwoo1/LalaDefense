using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Machinegun : Tower
{
    ParticleSystem muzzleEffect;
    float m_Elapsed = 0;
    Shot m_AliveShot = null;

    public override void Attack()
    {
        if (Target == null) { return; }

        turret.LookAt(Target.transform);

        if (muzzleEffect.isPlaying) { muzzleEffect.Stop(); }

        m_Elapsed += Time.deltaTime;
        if (m_Elapsed >= GetCurLevelAttackInfo().speed)
        {
            m_Elapsed = 0;
            GameObject go = Instantiate(shot.gameObject, bombPoint.position, Quaternion.identity, shots);
            Shot b = go.GetComponent<Shot>();
            m_AliveShot = b;
            b.Seek(Target);
            b.Init(GetCurLevelAttackInfo());
            b.Attack(AttackComplete);
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
        base.Delete(done);
    }

    public override void Init(Transform curTower)
    {
        turret = GetChild(curTower, nameof(turret));
        projectile = GetChild(curTower, nameof(projectile));
        bombPoint = GetChild(curTower, nameof(bombPoint));
        muzzleEffect = GetChild(curTower, nameof(muzzleEffect))?.GetComponent<ParticleSystem>();
    }

    void AttackComplete()
    {
        if (m_AliveShot != null)
        {
            Destroy(m_AliveShot.gameObject);
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
