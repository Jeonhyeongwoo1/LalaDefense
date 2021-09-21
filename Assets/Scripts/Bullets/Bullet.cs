using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Shot : MonoBehaviour
{
    protected Enemy enemy;
    protected AttackInfo info;
    protected Transform bombPoint;
    protected Transform shots;

    public abstract void Seek(Enemy target);
    public abstract void Init(AttackInfo info, Transform bombPoint = null, Transform shots = null);
    public abstract void Attack(UnityAction done = null);
    public virtual void Standby(UnityAction done = null) { done?.Invoke(); }
}

public class Bullet : Shot
{
    [SerializeField, Range(0, 3)] float m_AliveDuration = 1.5f;
    [SerializeField, Range(0, 2)] float m_BombSpeed;
    [SerializeField] GameObject m_HitEffect;
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
        StartCoroutine(Attacking(done));
    }

    IEnumerator Attacking(UnityAction done)
    {
        float elapsed = 0;

        while (true)
        {
            elapsed += Time.deltaTime;

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

            Vector3 dir = enemy.transform.position - transform.position;
            transform.Translate(dir.normalized * m_BombSpeed, Space.World);
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

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        if (m_EnemyHitEffect != null)
        {
            m_EnemyHitEffect.StopAllCoroutines();
            Destroy(m_EnemyHitEffect.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == enemy.skinnedMeshRenderer)
        {
            if (info.boomRange != 0)
            {
                Collider[] hits = Physics.OverlapSphere(transform.position, info.boomRange, 64);
                foreach (var v in hits)
                {
                    Enemy e = v.transform.parent.GetComponent<Enemy>();
                    if (e != null)
                    {
                        e.TakeDamage(info.damage, info.specialAttack, info.specialAttackInfo);
                    }
                }
            }
            else
            {
                enemy.TakeDamage(info.damage, info.specialAttack, info.specialAttackInfo);
            }

            HitEffectOn();
            m_AttackedEvent?.Invoke();
        }
    }


    /*
        IEnumerator CannonballMovement()
        {
            // Short delay added before Projectile is thrown
            yield return new WaitForSeconds(0f);

            // Calculate distance to target
            float target_Distance = Vector3.Distance(transform.position, testVector);

            // Calculate the velocity needed to throw the object to the target at specified angle.
            float projectile_Velocity = target_Distance / (Mathf.Sin(2 * privateRotation * Mathf.Deg2Rad) / dragDown);

            // Extract the X  Y componenent of the velocity
            float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(privateRotation * Mathf.Deg2Rad);
            float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(privateRotation * Mathf.Deg2Rad);

            // Calculate flight time.
            float flightDuration = target_Distance / Vx;

            // Rotate projectile to face the target.
            transform.rotation = Quaternion.LookRotation(testVector - transform.position);

            float elapse_time = 0;

            while (elapse_time < flightDuration)
            {
                transform.Translate(0, (Vy - (dragDown * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

                elapse_time += Time.deltaTime;

                yield return null;
            }
        }


            IEnumerator Shoot()
            {

                switch (m_BombType)
                {
                    case BombType.BULLET:
                        RaycastHit raycastHit;
                        while (true)
                        {
                            Vector3 dir = m_Enemy.transform.position - transform.position;
                            if (Physics.Raycast(transform.position, dir.normalized, out raycastHit, hitDist))
                            {
                                //hit
                                if (raycastHit.transform == m_Enemy)
                                {
                                    // 데미징...
                                    Destroy(gameObject);
                                }
                            }

                            transform.Translate(dir.normalized * m_BombSpeed, Space.World);
                            transform.LookAt(m_Enemy.transform);

                            yield return null;
                        }
                    case BombType.BOMB:

                        float elapse_time = 0;

                        // Calculate distance to target
                        float target_Distance = Vector3.Distance(transform.position, m_Enemy.transform.position);

                        // Calculate the velocity needed to throw the object to the target at specified angle.
                        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);
                        // Extract the X  Y componenent of the velocity
                        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
                        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

                        // Calculate flight time.
                        float flightDuration = target_Distance / Vx;

                        while (elapse_time < flightDuration)
                        {
                            // Calculate distance to target
                            target_Distance = Vector3.Distance(transform.position, m_Enemy.transform.position);

                            // Calculate the velocity needed to throw the object to the target at specified angle.
                            projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);
                            // Extract the X  Y componenent of the velocity
                            Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
                            Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

                            // Calculate flight time.
                            flightDuration = target_Distance / Vx;

                            print(flightDuration);
                            // Rotate projectile to face the target.
                            transform.rotation = Quaternion.LookRotation(m_Enemy.transform.position - transform.position);
                            transform.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
                            elapse_time += Time.deltaTime;
                            //elapse_time += Time.deltaTime;
                            transform.LookAt(m_Enemy.transform);
                            yield return null;
                        }
                        break;
                    default:
                        break;
                }
            }
        */
    // Update is called once per frame
    void Update()
    {


    }
}
