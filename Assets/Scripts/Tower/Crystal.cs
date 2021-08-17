using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Crystal : Tower
{
    public Animator animator;
    [SerializeField] AnimationCurve m_Curve;


    public override void UpgradeTower()
    {
        base.UpgradeTower();
        animator = GetChild(transform, nameof(projectile))?.GetComponent<Animator>();
    }

    public override void Attack() 
    {
        if(!animator)
        {
            animator = GetChild(transform, nameof(projectile))?.GetComponent<Animator>();
        }

        animator.enabled = false;
    }

    public override void Standby() 
    {
        if (!animator)
        {
            animator = GetChild(transform, nameof(projectile))?.GetComponent<Animator>();
        }

        animator.enabled = true;
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
        animator = GetChild(transform, nameof(projectile))?.GetComponent<Animator>();
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
