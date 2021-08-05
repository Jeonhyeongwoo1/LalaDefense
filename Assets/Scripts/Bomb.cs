using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    enum BombType { BULLET, BOMB }

    [SerializeField, Range(0, 2)] float m_BulletSpeed;
    [SerializeField, Range(0, 2)] float m_BombSpeed;
    [SerializeField] BombType m_BombType;
    [SerializeField] float m_FlightDuration;

    public float firingAngle = 45.0f;
    public float gravity = 9.8f;

    public float hitDist = 1f;
    Transform m_Target;

    public void Seek(Transform target)
    {
        m_Target = target;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Attack()
    {
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {

        switch (m_BombType)
        {
            case BombType.BULLET:
                RaycastHit raycastHit;
                while (true)
                {
                    Vector3 dir = m_Target.position - transform.position;
                    if (Physics.Raycast(transform.position, dir.normalized, out raycastHit, hitDist))
                    {
                        //hit
                        if (raycastHit.transform == m_Target)
                        {
                            // 데미징...
                            Destroy(gameObject);
                        }
                    }

                    transform.Translate(dir.normalized * m_BulletSpeed, Space.World);
                    transform.LookAt(m_Target);

                    yield return null;
                }
            case BombType.BOMB:
                
                float elapse_time = 0;

                // Calculate distance to target
                float target_Distance = Vector3.Distance(transform.position, m_Target.position);

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
                    target_Distance = Vector3.Distance(transform.position, m_Target.position);

                    // Calculate the velocity needed to throw the object to the target at specified angle.
                    projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);
                    // Extract the X  Y componenent of the velocity
                    Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
                    Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

                    // Calculate flight time.
                    flightDuration = target_Distance / Vx;

                    print(flightDuration);
                    // Rotate projectile to face the target.
                    transform.rotation = Quaternion.LookRotation(m_Target.position - transform.position);
                    transform.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
                    elapse_time += Time.deltaTime; 
                    //elapse_time += Time.deltaTime;
                    transform.LookAt(m_Target);
                    yield return null;
                }
                break;
            default:
                break;
        }
    }


    // Update is called once per frame
    void Update()
    {
       
    }
}
