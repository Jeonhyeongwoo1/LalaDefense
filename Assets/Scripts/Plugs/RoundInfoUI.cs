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

    public void SetRoundInfo(float roundCount, float totalRound, float aliveEnemyCount, float totalAliveEnemy)
    {
        m_EnemyInfo.text = "Alive Enemy : " + aliveEnemyCount + " / " + totalAliveEnemy;
        m_RoundInfo.text = "Round : " + roundCount + " / " + totalRound;
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

    void OnEnable()
    {
        Core.state.Listen(nameof(Core.state.aliveEnemyCount), OnValueChanged);
    }

    void OnDisable()
    {
        if (Core.state == null) { return; }
        Core.state.Remove(nameof(Core.state.aliveEnemyCount), OnValueChanged);
    }

    void OnValueChanged(string key, object o)
    {
        if (key == nameof(Core.state.aliveEnemyCount))
        {
            m_EnemyInfo.text = "Alive Enemy : " + o.ToString() + " / " + m_AliveEnemyTotalCount;
        }
    }

}
