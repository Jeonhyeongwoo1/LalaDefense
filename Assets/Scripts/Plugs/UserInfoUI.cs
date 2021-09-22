using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UserInfoUI : BaseTheme
{
    [SerializeField] Text m_Heart;
    [SerializeField] Text m_Money;
    [SerializeField] Text m_Score;

    public override void Open(UnityAction done)
    {
        m_Heart.text = Core.state.heart.ToString();
        m_Money.text = Core.state.money.ToString();
        m_Score.text = Core.state.score.ToString();
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
        Core.state.Listen(nameof(Core.state.heart), OnValueChanged);
        Core.state.Listen(nameof(Core.state.money), OnValueChanged);
        Core.state.Listen(nameof(Core.state.score), OnValueChanged);
    }

    void OnDisable()
    {
        if (Core.state == null) { return; }
        Core.state.Remove(nameof(Core.state.heart), OnValueChanged);
        Core.state.Remove(nameof(Core.state.money), OnValueChanged);
        Core.state.Remove(nameof(Core.state.score), OnValueChanged);
    }

    void OnValueChanged(string key, object o)
    {
        switch (key)
        {
            case nameof(Core.state.heart):
                int heart = int.Parse(o.ToString());
                m_Heart.text = heart == 0 ? "0" : string.Format("{0:#,###}", heart);
                break;
            case nameof(Core.state.money):
                int money = int.Parse(o.ToString());
                m_Money.text = money == 0 ? "0" : string.Format("{0:#,###}", money);
                break;
            case nameof(Core.state.score):
                int score = int.Parse(o.ToString());
                m_Score.text = score == 0 ? "0" : string.Format("{0:#,###}", score);
                break;
        }

    }

}
