using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSignalReceiver : MonoBehaviour
{
    [SerializeField, Range(0, 2)] float m_TowerScaleUp;

    public void ActiveTower(GameObject tower)
    {
        tower.SetActive(!tower.activeSelf);
    }

    public void ScaleUp(GameObject tower)
    {
        StartCoroutine(ScaleUp(tower.transform));
    }

    IEnumerator ScaleUp(Transform tower)
    {
        float elapsed = 0;

        while(elapsed < m_TowerScaleUp)
        {
            elapsed += Time.deltaTime;
            tower.localScale = Vector3.Lerp(tower.localScale, Vector3.one, elapsed / m_TowerScaleUp);
            yield return null;
        }
    }

}
