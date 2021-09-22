using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class EnemyManager : MonoBehaviour
{

    public List<Enemy> enemies = new List<Enemy>(); //적 리스트
    public List<Enemy> aliveEnemies = new List<Enemy>();
    [SerializeField, Range(0, 5)] float m_SpawnTime;

    [Space]
    [Header("[Boss]")]
    [SerializeField] CinemachineVirtualCamera m_BossCamera;
    [SerializeField] CinemachineSmoothPath m_SmoothPath;
    [SerializeField] CinemachineDollyCart m_DollyCart;
    [SerializeField] GameObject m_PointLight;

    CinemachineVirtualCamera m_MainCam;

    public GameObject testEnemy;

    public void SetMainCam(CinemachineVirtualCamera cam) { m_MainCam = cam; }

    public void SetSpawnTime(float t) { m_SpawnTime = t; }

    public void DestroyEnemy()
    {
        if (aliveEnemies == null) { return; }
        if (aliveEnemies.Count == 0) { return; }

        aliveEnemies.ForEach((v) => Destroy(v.gameObject));
        aliveEnemies.Clear();
        Core.state.aliveEnemyCount = 0;
    }

    public void EnemySpawn(EnemyInfo enemy, float count)
    {
        Enemy e = enemies.Find((v) => enemy.enemyType == v.enemyInfo.enemyType
                                            && enemy.level == v.enemyInfo.level);
        Core.state.aliveEnemyCount = count;

        if (e == null)
        {
            Debug.Log("Enemy가 존재하지 않습니다.");
            Core.gameManager.OnNextGame();
            return;
        }

        if (Core.state.aliveEnemyCount == 0)
        {
            Debug.Log("Enemy Count가 0 입니다.");
            return;
        }

        StartCoroutine(Spawning(e, enemy, count));
    }

    IEnumerator Spawning(Enemy enemy, EnemyInfo enemyInfo, float count)
    {
        Terrain terrain = Core.models.GetModel<Terrain>();
        WayPoint wayPoint = terrain.wayPoint;
        Transform spawnPoint = wayPoint.wayPoints[0];

        while (count != 0)
        {
            count--;
            Transform e = Instantiate(enemy.transform, spawnPoint.position + enemy.spawnOffset, Quaternion.identity, transform);
            Enemy ey = e.GetComponent<Enemy>();
            ey.enemyInfo = enemyInfo;
            aliveEnemies.Add(ey);

            if (enemyInfo.level == EnemyInfo.Level.Boss)
            {
                yield return BossAppearAnimation(ey);
            }

            yield return new WaitForSeconds(m_SpawnTime);
        }

    }

    public void BossAppear(Enemy e)
    {
        StartCoroutine(BossAppearAnimation(e));
    }

    bool IsLive(CinemachineVirtualCamera cam) => CinemachineCore.Instance.IsLive(cam) && !CinemachineCore.Instance.GetActiveBrain(0).IsBlending;

    IEnumerator BossAppearAnimation(Enemy e)
    {
        Core.gameManager.bossAppearAniPlaying = true;

        Theme theme = Core.plugs.GetPlugable<Theme>();
        theme.CloseOpenedThemes();
        e.ShowHideHealthBar(false);
        m_BossCamera.LookAt = e.transform;
        m_PointLight.SetActive(true);
        yield return CameraTransistion(m_MainCam, m_BossCamera);
        m_SmoothPath.gameObject.SetActive(true);
        m_DollyCart.gameObject.SetActive(true);

        while (m_DollyCart.m_Position != m_SmoothPath.PathLength)
        {
            e.SetAnimator("Taunting", true);
            yield return null;
        }

        e.SetAnimator("Taunting", false);
        yield return CameraTransistion(m_BossCamera, m_MainCam);

        theme.Open<TowerStore>();
        theme.Open<UserInfoUI>();
        theme.Open<Menu>();
        theme.Open<RoundInfoUI>();
        e.ShowHideHealthBar(true);
        e.SetAnimator("WalkFWD", true);
        m_PointLight.SetActive(false);
        m_SmoothPath.gameObject.SetActive(false);
        m_DollyCart.gameObject.SetActive(false);
        m_DollyCart.m_Position = 0;

        Core.gameManager.bossAppearAniPlaying = false;
    }

    IEnumerator CameraTransistion(CinemachineVirtualCamera from, CinemachineVirtualCamera to)
    {
        from.enabled = false;
        to.enabled = true;
        while (!IsLive(to)) { yield return null; }
    }

}
