using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RoundInfoUI : BaseTheme
{
    [SerializeField] Text m_EnemyInfo;
    [SerializeField] Text m_RoundInfo;

    float m_AliveEnemyTotalCount;
    float m_AliveEnemyCount;

    public float aliveEnemyCount
    {
        get => m_AliveEnemyCount;
        set
        {
            if (value < 0) { return; }
            m_AliveEnemyCount = value;
            m_EnemyInfo.text = "Alive Enemy : " + m_AliveEnemyCount + " / " + m_AliveEnemyTotalCount;
        }
    }

    public void SetRoundInfo(float roundCount, float totalRound, float aliveEnemyCount, float totalAliveEnemy)
    {
        m_EnemyInfo.text = "Alive Enemy : " + aliveEnemyCount + " / " + totalAliveEnemy;
        m_RoundInfo.text = "Round : " + roundCount + " / " + totalRound;
        m_AliveEnemyCount = aliveEnemyCount;
        m_AliveEnemyTotalCount = totalAliveEnemy;
    }

    public override void Open(UnityAction done) 
    {
        gameObject.SetActive(true);
        done?.Invoke();
    }
  
    public override void Close(UnityAction done) 
    {
        Core.plugs.GetPlugable<Theme>()?.RemoveOpenedTheme(this);
        done?.Invoke();
        gameObject.SetActive(false);
    }

}
