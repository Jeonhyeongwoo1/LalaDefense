using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TowerManager : MonoBehaviour
{
    Dictionary<Tower, Transform> towers = new Dictionary<Tower, Transform>();
    List<Tower> onlyTowers = new List<Tower>();//일단 혹시 몰라서 생성 필요없으면 삭제

    public Transform GetNode(Tower tower)
    {
        return towers.TryGetValue(tower, out Transform node) ? node : null;
    }

    public Tower GetTower(string towerName)
    {
        foreach (var t in towers)
        {
            if (t.Key.name == towerName) { return t.Key; }
        }

        return null;
    }

    public Tower GetTower(Tower tower)
    {
        foreach (var t in towers)
        {
            if (t.Key == tower) { return t.Key; }
        }

        return null;
    }

    public void AddTower(Tower tower, Transform node) => towers.Add(tower, node);

    public void CreatTower(Transform tower, Transform node)
    {
        Terrain terrain = FindObjectOfType<Terrain>();
        terrain.SelectNode(node);

        GameObject t = Instantiate(tower.gameObject, terrain.nodes.GetBuildPosition(), Quaternion.identity, transform);
        Tower tr = t.GetComponent<Tower>();
        tr.Create();
        towers.Add(tr, node);
        onlyTowers.Add(tr);
    }

    public void DeleteTower(Tower tower)
    {
        tower.Delete(() => DeletedTower(tower));
    }

    public void DeletedTower(Tower tower)
    {
        //노드를 원상복귀 한다.
        Transform node = GetNode(tower);
        node.gameObject.SetActive(true);

        towers.Remove(tower);
        onlyTowers.Remove(tower);

    }

}
