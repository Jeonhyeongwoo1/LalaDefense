using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHitEffect : MonoBehaviour
{
    public ParticleSystem hitEffect;

    public void EffectOn(UnityAction done = null)
    {
        hitEffect.Play();
        StartCoroutine(ProceedingEffect(done));
    }

    IEnumerator ProceedingEffect(UnityAction done)
    {
        
        while(hitEffect.isPlaying)
        {
            yield return null;
        }
        
        hitEffect.Stop();
        done?.Invoke();
        Destroy(gameObject);
    }

    public void EffectOff()
    {
        hitEffect.Stop();
        StopAllCoroutines();
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
