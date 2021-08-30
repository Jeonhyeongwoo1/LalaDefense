using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using UnityEngine.EventSystems;

public class Mortar : Tower
{
    [SerializeField] AnimationCurve m_Curve;
    [SerializeField] float m_Speed = 1;
    [SerializeField] string m_StandbyObjName = "Turret";
    [SerializeField] float m_axisXPlusMoving = 20;
    [SerializeField] float m_axisXMinusMoving = -100;
    
    Transform m_Turret;
    bool arrow = true;

    public Transform target;

   
    //곡선형태로 그린다.
    [ContextMenu("Attack")]
    public override void Attack()
    {
       StartCoroutine(SSS());
    }

    IEnumerator SSS()
    {
        for(int i = 0; i< 10; i++)
        {
            GameObject go = Instantiate(bomb.gameObject, bombPoint.position, Quaternion.identity);
            Bomb b = go.GetComponent<Bomb>();
           // b.Seek(target);
            b.Attack();
            yield return new WaitForSeconds(0.5f);
        }
       
    }

    public override void Standby()
    {
        if (!m_Turret) { m_Turret = GetChild(transform, m_StandbyObjName); }

        float x = TransformUtils.GetInspectorRotation(m_Turret).x;

        if (x > m_axisXPlusMoving) { arrow = false; }
        if (x < m_axisXMinusMoving) { arrow = true; }

        m_Turret.Rotate((arrow ? Vector3.right : -Vector3.right) * Time.deltaTime * m_Speed);

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
    }

    // Update is called once per frame
    void Update()
    {
        if (towerState == TowerState.Standby)
        {
            Standby();
        }
    }
}
