using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bomb : Shot
{

    [SerializeField] float m_FlightDuration;
    public float firingAngle = 45.0f;
    public float gravity = 9.8f;

    float m_Damage;
    public float speed = 3f;

    public override void Seek(Enemy target)
    {
        enemy = target;
    }

    public override void Init(AttackInfo info, Transform bombPoint = null, Transform shots = null)
    {
        m_Damage = info.damage;
        this.bombPoint = bombPoint;
    }

    public override void Attack(UnityAction done = null)
    {

    }

    public void Shoot(UnityAction done)
    {
        StartCoroutine(Shooting(done));
    }

    IEnumerator Shooting(UnityAction done)
    {
        float elapse_time = 0;

        // Calculate distance to target
        float target_Distance = Vector3.Distance(bombPoint.position, enemy.transform.position);

        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity) * speed;

        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        // Calculate flight time.
        float flightDuration = target_Distance / Vx;
        transform.rotation = Quaternion.LookRotation(enemy.transform.position - transform.position);
        
        while (elapse_time < flightDuration)
        {
            transform.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
            elapse_time += Time.deltaTime;
            yield return null;
        }

        done?.Invoke();
    }

}
