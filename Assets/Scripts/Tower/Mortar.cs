using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using UnityEngine.EventSystems;

public class Mortar : Tower
{
    [SerializeField] float m_Speed = 1;
    [SerializeField] float m_axisXPlusMoving = 20;
    [SerializeField] float m_axisXMinusMoving = -100;

    bool arrow = true;

    public override void Standby()
    {

        float x = TransformUtils.GetInspectorRotation(turret).x;

        if (x > m_axisXPlusMoving) { arrow = false; }
        if (x < m_axisXMinusMoving) { arrow = true; }

        turret.Rotate((arrow ? Vector3.right : -Vector3.right) * Time.deltaTime * m_Speed);
    }

    bool isFire = false;
    public override void Attack()
    {
        if (!isFire)
        {
            isFire = true;
            GameObject go = Instantiate(shot.gameObject, bombPoint.position, Quaternion.identity);
            Shot b = go.GetComponent<Shot>();
            Bomb bullet = b.GetComponent<Bomb>();
            bullet.Seek(Target);
            bullet.Init(GetCurLevelAttackInfo(), bombPoint);
            bullet.Shoot(() => isFire = false);
        }
    }

    public override void DestroyImmediate(UnityAction done = null)
    {
        towerState = TowerState.Deleting;
        DestroyImmediate(gameObject);
    }

    public override void Init(Transform curTower)
    {
        turret = GetChild(curTower, nameof(turret));
        projectile = GetChild(curTower, nameof(projectile));
        bombPoint = GetChild(curTower, nameof(bombPoint));
    }

    public override void Create(UnityAction done = null)
    {
        base.Create(done);
    }

    public override void Delete(UnityAction done = null)
    {
        base.Delete(done);
    }

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
