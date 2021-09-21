using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHitEffect : MonoBehaviour
{
    public ParticleSystem hitEffect;

    public void SetPosition(Transform target)
    {
        transform.position = target.position;
    }

    public void EffectOn(UnityAction done = null)
    {
        StartCoroutine(ProceedingEffect(done));
    }

    public void EffectOff()
    {
        hitEffect.Stop();
        StopAllCoroutines();
    }

    IEnumerator ProceedingEffect(UnityAction done)
    {
        hitEffect.Play();

        while (hitEffect.isPlaying)
        {
            yield return null;
        }

        hitEffect.Stop();
        done?.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (hitEffect == null)
        {
            hitEffect = GetComponent<ParticleSystem>();
        }

    }
}
