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
    [SerializeField] float hitDist = 1f;

    float m_Elapsed = 0;
    RaycastHit m_RaycastHit;

    public override void Seek(Enemy target)
    {
        enemy = target;
    }

    public override void Init(AttackInfo info, Transform bombPoint = null)
    {
        this.info = info;
        this.bombPoint = bombPoint;
    }

    public override void Attack(UnityAction done = null)
    {
        StartCoroutine(TargetAttack(done));
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
                Destroy(gameObject);
                yield break;
            }

            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.zero, m_Scale, elapsed / info.speed);
            GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2(elapsed, 0);
            yield return null;
        }

        while (true)
        {
            m_Elapsed += Time.deltaTime;
            GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2(elapsed, 0);

            if (enemy == null)
            {
                done?.Invoke();
                Destroy(gameObject);
                yield break;
            }

            if (m_Elapsed > m_AliveDuration)
            {
                m_Elapsed = 0;
                done?.Invoke();
                Destroy(gameObject);
                yield break;
            }

            Vector3 dir = enemy.transform.position - transform.position;
            if (Physics.Raycast(transform.position, dir.normalized, out m_RaycastHit, hitDist))
            {
                //hit
                if (m_RaycastHit.transform == enemy.skinnedMeshRenderer)
                {
                    enemy.TakeDamage(info.damage, info.specialAttack, info.specialAttackInfo);
                    HitEffectOn();
                    done?.Invoke();
                    Destroy(gameObject);
                }
            }

            transform.Translate(dir.normalized * 2, Space.World);
            transform.LookAt(enemy.transform);
            yield return null;
        }

    }


    public void HitEffectOn()
    {
        GameObject effect = Instantiate(m_HitEffect, enemy.transform.position, Quaternion.identity);
        EnemyHitEffect ef = effect.GetComponent<EnemyHitEffect>();
        ef.EffectOn();
    }

}
