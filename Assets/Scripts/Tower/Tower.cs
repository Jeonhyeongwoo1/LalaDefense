using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public enum TowerType
{
    GUN, //총알
    BOMB, //폭탄
    LASER, //레이저
    SPECIAL //특수공격
}

[Serializable]
public class AttackInfo
{
    public float speed;
    public float damage;
    public float attackRange; //폭탄 경우에만
    public string specialAttack;
}

[Serializable]
public class TowerLevel
{
    public GameObject towerPrefab;
    public float level;
    public float price;
    public AttackInfo attackInfo;
}

[Serializable]
public class TowerInfo
{
    public string towerName;
    public TowerType towerType;
    public TowerLevel[] towerLevels; // 1 ~ 3
}

public abstract class Tower : MonoBehaviour
{
    //타워 정보, 공격, 대기상태 생성될때 삭제될때 애니메이션
    public enum TowerState { Attack, Standby }
    public TowerState towerState;
    public TowerInfo towerInfo;
    public GameObject createEffect; //prefabs
    public GameObject deleteEffect;
    public Transform projectile; //발사체
    public Transform bombPoint; //발사 지점
    public Bomb bomb;
    public int currentLevel = 0;
    public abstract void Attack();
    public abstract void Standby();
    public abstract void Create(UnityAction done = null);
    public abstract void Delete(UnityAction done = null);

    [SerializeField, Range(0, 1)]
    protected float CreateDuration;
    [SerializeField, Range(0, 1)]
    protected float DeleteDuration;
    
    public virtual void Start()
    {
        projectile = GetChild(transform, nameof(projectile));
        bombPoint = GetChild(transform, nameof(bombPoint));
    }

    public virtual void UpgradeTower()
    {
        // 1 ~ 3
        if (currentLevel == towerInfo.towerLevels.Length)
        {
            Debug.Log("Max Upgrade!!");
            return;
        }

        GameObject currentTower = transform.GetChild(0).gameObject;
        GameObject upgradeTower = towerInfo.towerLevels[++currentLevel].towerPrefab;

        GameObject g = Instantiate(upgradeTower, transform.position, Quaternion.identity, transform);
        projectile = GetChild(g.transform, nameof(projectile));
        bombPoint = GetChild(g.transform, nameof(bombPoint));

        Destroy(currentTower);
    }

    protected void CreateEffectObj(GameObject effect, UnityAction done = null)
    {
        GameObject e = Instantiate(effect, transform.position, effect.transform.rotation, transform);
        StartCoroutine(ProceedingEffect(e.GetComponent<ParticleSystem>(), () =>
        {
            DestroyImmediate(e);
            done?.Invoke();
        }));
    }

    protected Transform GetChild(Transform tr, string name)
    {
        Transform[] childs = tr.GetComponentsInChildren<Transform>();
        foreach (var v in childs)
        {
            if (v.name.ToUpper() == name.ToUpper()) { return v; }
        }
        return null;
    }


    IEnumerator ProceedingEffect(ParticleSystem p, UnityAction done)
    {
        if (!p.isPlaying) { p.Play(); }
        while (!p.isStopped) { yield return null; }

        done?.Invoke();
    }

    protected IEnumerator DeletingTower(UnityAction done)
    {
        float elapsed = 0f;
        Vector3 v = Vector3.zero;

        while (elapsed < DeleteDuration)
        {
            elapsed += Time.deltaTime;
            v = new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), 0, UnityEngine.Random.Range(-0.1f, 0.1f));
            transform.localPosition += v;
            yield return null;
        }

        //타워 오브젝트를 감춘다.
        transform.GetChild(0).gameObject.SetActive(false);
        CreateEffectObj(deleteEffect, () =>
        {
            done?.Invoke();
            DestroyImmediate(gameObject);
        });
    }

}