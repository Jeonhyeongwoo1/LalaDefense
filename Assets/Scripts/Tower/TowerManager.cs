using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TowerManager : MonoBehaviour
{
    public Transform shots;
    [SerializeField] Transform[] towerParents;

    Dictionary<Tower, Transform> activeTowers = new Dictionary<Tower, Transform>();
    List<Tower> onlyTowers = new List<Tower>();

    public Transform GetNode(Tower tower)
    {
        return activeTowers.TryGetValue(tower, out Transform node) ? node : null;
    }

    public Tower GetActiveTower(string towerName)
    {
        foreach (var t in activeTowers)
        {
            if (t.Key.name == towerName) { return t.Key; }
        }

        return null;
    }

    public Tower GetActiveTower(Tower tower)
    {
        foreach (var t in activeTowers)
        {
            if (t.Key == tower) { return t.Key; }
        }

        return null;
    }

    public Transform GetTowerParent(Transform tower)
    {
        foreach(Transform tr in towerParents)
        {
            if(tr.name == tower.name)
            {
                return tr;
            }
        }

        return null;
    }

    public void AddTower(Tower tower, Transform node) => activeTowers.Add(tower, node);

    public void CreateTower(Transform tower, Transform node)
    {
        Terrain terrain = Core.models.GetModel<Terrain>();
        terrain.SelectNode(node);

        Transform t = Instantiate(tower, terrain.nodes.GetBuildPosition(), Quaternion.identity, transform);
        Tower tr = t.GetComponent<Tower>();
        tr.transform.SetParent(GetTowerParent(tower.transform));
        tr.Create();
        tr.shots = shots;
        activeTowers.Add(tr, node);
        onlyTowers.Add(tr);
    }

    public void DestroyImmediateAllTower()
    {
        if (onlyTowers == null) { return; }
        if (onlyTowers.Count == 0) { return; }

        onlyTowers.ForEach((v) => Destroy(v.gameObject));
        activeTowers.Clear();
        onlyTowers.Clear();
    }

    public void DeleteTower(Tower tower)
    {
        tower.Delete(() => DeletedTower(tower));
    }

    public void UpgradeTower(Tower tower)
    {
        tower.UpgradeTower();
    }

    void DeletedTower(Tower tower)
    {
        //노드를 원상복귀 한다.
        Transform node = GetNode(tower);
        node.gameObject.SetActive(true);

        activeTowers.Remove(tower);
        onlyTowers.Remove(tower);
    }

}
