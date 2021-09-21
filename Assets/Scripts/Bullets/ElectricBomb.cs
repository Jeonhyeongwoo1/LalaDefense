using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElectricBomb : Shot
{
    [SerializeField] Material m_BombLevel1;
    [SerializeField] Material m_BombLevel2;
    [SerializeField] Material m_BombLevel3;
    [SerializeField] GameObject m_HitEffect;

    [SerializeField] Vector3 m_Scale = new Vector3(0.5f, 0.5f, 0.5f);
    [SerializeField, Range(0, 5)] float m_AliveDuration = 1.5f;

    EnemyHitEffect m_EnemyHitEffect;
    UnityAction m_AttackedEvent = null;

    public override void Seek(Enemy target)
    {
        enemy = target;
    }

    public override void Init(AttackInfo info, Transform bombPoint = null, Transform shots = null)
    {
        this.info = info;
        this.bombPoint = bombPoint;

        if (m_EnemyHitEffect == null)
        {
            GameObject effect = Instantiate(m_HitEffect, enemy.transform.position, Quaternion.identity, shots.GetChild(0));
            m_EnemyHitEffect = effect.GetComponent<EnemyHitEffect>();
        }
    }

    public override void Attack(UnityAction done = null)
    {
        m_AttackedEvent = done;
        StartCoroutine(TargetAttack(done));
    }

    public void ResetState()
    {
        StopAllCoroutines();
        transform.localScale = Vector3.zero;
        transform.position = Vector3.zero;
    }

    public void SetBombLevel(int level)
    {
        Material[] materials = GetComponent<MeshRenderer>().materials;
        switch (level)
        {
            case 0:
                materials[0] = m_BombLevel1;
                break;
            case 1:
                materials[0] = m_BombLevel2;
                break;
            case 2:
                materials[0] = m_BombLevel3;
                break;
        }

        GetComponent<MeshRenderer>().materials = materials;
    }

    IEnumerator TargetAttack(UnityAction done)
    {
        //ScaleUp 시간은 타워의 speed
        float elapsed = 0;

        while (elapsed < info.speed)
        {
            if (enemy == null)
            {
                done?.Invoke();
                yield break;
            }

            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.zero, m_Scale, elapsed / info.speed);
            GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2(elapsed, 0);
            yield return null;
        }

        elapsed = 0;

        while (true)
        {
            elapsed += Time.deltaTime;
            GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2(elapsed, 0);

            if (enemy == null)
            {
                done?.Invoke();
                yield break;
            }

            if (elapsed > m_AliveDuration)
            {
                elapsed = 0;
                done?.Invoke();
                yield break;
            }

            Vector3 dir = (enemy.transform.position - transform.position) + new Vector3(0, 1, 0);
            transform.Translate(dir.normalized * 1.7f, Space.World);
            transform.LookAt(enemy.transform);
            yield return null;
        }

    }

    public void HitEffectOn()
    {
        if (m_EnemyHitEffect != null)
        {
            m_EnemyHitEffect.SetPosition(enemy.transform);
            m_EnemyHitEffect.EffectOn();
        }
    }

    void OnDestroy()
    {
        if (m_EnemyHitEffect != null)
        {
            m_EnemyHitEffect.StopAllCoroutines();
            DestroyImmediate(m_EnemyHitEffect.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == enemy.skinnedMeshRenderer)
        {
            enemy.TakeDamage(info.damage, info.specialAttack, info.specialAttackInfo);
            HitEffectOn();
            m_AttackedEvent?.Invoke();
        }
    }

}
