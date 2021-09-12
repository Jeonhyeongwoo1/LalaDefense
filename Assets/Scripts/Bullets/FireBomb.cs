using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FireBomb : Shot
{
    public ParticleSystem fireEffect;
    [SerializeField] GameObject m_HitEffect;
  
    EnemyHitEffect m_EnemyHitEffect;

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
        if (!fireEffect.isPlaying)
        {
            fireEffect.Play();
        }
    }

    public override void Standby(UnityAction done = null)
    {
        if(fireEffect.isPlaying)
        {
            fireEffect.Stop();
        }
    }

    void HitEffectOn()
    {
        if (m_EnemyHitEffect != null && m_EnemyHitEffect.hitEffect.isPlaying) { return; }

        if (m_HitEffect != null)
        {
            GameObject effect = Instantiate(m_HitEffect.gameObject, enemy.transform.position, Quaternion.identity);
            m_EnemyHitEffect = effect.GetComponent<EnemyHitEffect>();
            m_EnemyHitEffect.EffectOn();
        }
    }

    void OnParticleCollision(GameObject other)
    {
        Enemy e = other.transform.parent?.GetComponent<Enemy>();
        
        if(e != null)
        {
            HitEffectOn();
            enemy.TakeDamage(info.damage);
        }
    }

}
