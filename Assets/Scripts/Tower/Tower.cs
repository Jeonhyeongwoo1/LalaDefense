using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

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
    public float range; //공격 범위
    public float speed;
    public float damage;
    public float boomRange; //폭탄 경우에만
    public string specialAttack;
    public string specialAttackInfo;
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

public abstract class Tower : MonoBehaviour, IPointerClickHandler
{
    public enum TowerState { None, Creating, Upgrading, Standby, Attack, Deleting }
    public TowerState towerState = TowerState.None;
    public TowerInfo towerInfo;
    public GameObject createEffect;
    public GameObject deleteEffect;
    public Transform projectile; //발사체
    public Transform bombPoint; //발사 지점
    public Transform turret;
    public Shot shot;
    public int currentLevel = 0; // 0~2 index

    public Enemy Target;
    public Transform shots;
    protected GameObject Enemies;
    [SerializeField, Range(0, 1)] protected float CreateDuration;
    [SerializeField, Range(0, 1)] protected float DeleteDuration;
    [SerializeField] protected AnimationCurve Curve;

    public abstract void Attack();
    public abstract void Standby();
    public abstract void Init(Transform tr);

    public virtual void UpgradeTower()
    {
        if(towerState == TowerState.Creating || towerState == TowerState.Deleting)
        {
            Popup popup = Core.plugs.GetPlugable<Popup>();
            popup?.GetPopup<NotifyPopup>().SetContent("타워 생성중..");
            popup.Open<NotifyPopup>();
            return;
        }

        towerState = TowerState.Upgrading;
        // 0 ~ 2
        if (currentLevel == towerInfo.towerLevels.Length)
        {
            Debug.Log("Max Upgrade!!");
            return;
        }

        GameObject currentTower = transform.GetChild(0).gameObject;
        GameObject upgradeTower = towerInfo.towerLevels[++currentLevel].towerPrefab;

        GameObject g = Instantiate(upgradeTower, transform.position, Quaternion.identity, transform);
        Init(g.transform);

        Destroy(currentTower);
        towerState = TowerState.Standby;
    }

    public virtual void Create(UnityAction done = null)
    {
        towerState = TowerState.Creating;
        GameObject defaultTower = towerInfo.towerLevels[0].towerPrefab;
        Transform g = Instantiate(defaultTower.transform, transform.position, Quaternion.identity);
        g.SetParent(transform);
        Init(g);
        StartCoroutine(CoUtilize.VLerp((v) => g.localScale = v, Vector3.zero, Vector3.one, CreateDuration, done, Curve));

        CreateEffectObj(createEffect, TowerCreated);
    }

    public virtual void Delete(UnityAction done = null)
    {
        towerState = TowerState.Deleting;
        StartCoroutine(DeletingTower(done));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Theme theme = Core.plugs.GetPlugable<Theme>();
        TowerUpgrade towerUpgrade = theme.GetTheme<TowerUpgrade>();

        if (towerUpgrade.state == TowerUpgrade.State.Open)
        {
            theme.Close<TowerUpgrade>();
        }

        towerUpgrade.transform.position = transform.position + towerUpgrade.offset;
        towerUpgrade.Setup(towerInfo.towerLevels[currentLevel].level, towerInfo.towerLevels[currentLevel].price, this);
        theme.Open<TowerUpgrade>();
    }

    protected void UpdateTarget()
    {
        if (Core.gameManager.bossAppearAniPlaying) { return; }
        if (towerState == TowerState.Creating || towerState == TowerState.Upgrading) { return; }

        if (!Enemies)
        {
            Enemies = GameObject.FindGameObjectWithTag("Enemies");
        }

        Enemy[] enemies = null;
        if (Enemies.GetComponent<EnemyManager>().aliveEnemies.Count != 0)
        {
            enemies = Enemies.GetComponent<EnemyManager>().aliveEnemies.ToArray();
        }

        if (enemies == null) { return; }

        float shortnestDist = Mathf.Infinity;
        Transform nearnestEnemy = null;

        foreach (var e in enemies)
        {
            if (e.isDie) { continue; }

            float distToEnemy = Vector3.Distance(transform.position, e.transform.position);

            if (shortnestDist > distToEnemy)
            {
                shortnestDist = distToEnemy;
                nearnestEnemy = e.transform;
            }
        }

        float range = GetCurLevelAttackInfo().range;
        if (nearnestEnemy != null && shortnestDist < range)
        {
            Target = nearnestEnemy.GetComponent<Enemy>();
            towerState = TowerState.Attack;
        }
        else
        {
            Target = null;
            towerState = TowerState.Standby;
        }
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

    protected AttackInfo GetCurLevelAttackInfo()
    {
        return towerInfo.towerLevels[currentLevel].attackInfo;
    }

    IEnumerator ProceedingEffect(ParticleSystem p, UnityAction done)
    {
        if(p == null)
        {
            done?.Invoke();
            yield break;
        }

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

    void TowerCreated() { towerState = TowerState.Standby; }
}