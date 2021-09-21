using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FireBomb : Shot
{
    public ParticleSystem fireEffect;
    [SerializeField] GameObject m_HitEffect; //prefab

    EnemyHitEffect m_EnemyHitEffect;

    public override void Seek(Enemy target)
    {
        enemy = target;
    }

    public override void Init(AttackInfo info, Transform bombPoint = null, Transform shots = null)
    {
        this.info = info;
        this.bombPoint = bombPoint;
        this.shots = shots;

        if (shots != null)
        {
            GameObject g = Instantiate(m_HitEffect, transform.position, Quaternion.identity, shots.GetChild(0));
            m_EnemyHitEffect = g.GetComponent<EnemyHitEffect>();
        }
    }

    public override void Attack(UnityAction done = null)
    {
        if (!fireEffect.isPlaying)
        {
            fireEffect.Play();
        }
    }

    public override void Standby(UnityAction done = null)
    {
        if (fireEffect.isPlaying)
        {
            fireEffect.Stop();
        }
    }

    void HitEffectOn()
    {
        if (m_EnemyHitEffect != null && m_EnemyHitEffect.hitEffect.isPlaying) { return; }
        m_EnemyHitEffect.SetPosition(enemy.transform);
        m_EnemyHitEffect.EffectOn();
    }

    void OnParticleCollision(GameObject other)
    {
        Enemy e = other.transform.parent?.GetComponent<Enemy>();

        if (e != null)
        {
            HitEffectOn();
            enemy.TakeDamage(info.damage);
        }
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        if (m_EnemyHitEffect != null)
        {
            m_EnemyHitEffect.StopAllCoroutines();
            DestroyImmediate(m_EnemyHitEffect.gameObject);
        }
    }

}
