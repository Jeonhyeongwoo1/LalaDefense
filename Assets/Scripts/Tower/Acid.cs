using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Acid : Tower
{
    [SerializeField] float m_StandbySpeed = 1f;
    [SerializeField] float m_StandbyMultiplier = 1f;

    public override void UpgradeTower()
    {
        base.UpgradeTower();
    }

    public override void Attack()
    {
        if (Target == null) { return; }

        turret.LookAt(Target.transform);
        shot.Seek(Target);
        shot.Attack();

    }

    public override void Standby()
    {
        shot.Standby();

        if (turret.transform.localEulerAngles.x != 0)
        {
            turret.localEulerAngles = new Vector3(0, turret.localEulerAngles.y, turret.localEulerAngles.z);
        }

        turret.localEulerAngles += new Vector3(0, m_StandbySpeed * m_StandbyMultiplier, 0);
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
        shot = GetChild(curTower, "Laser").GetComponent<Laser>();
        shot?.Init(GetCurLevelAttackInfo(), bombPoint);
        shot?.gameObject.SetActive(false);
    }
    
    public override void DestroyImmediate(UnityAction done = null)
    {
        towerState = TowerState.Deleting;
        DestroyImmediate(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
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
