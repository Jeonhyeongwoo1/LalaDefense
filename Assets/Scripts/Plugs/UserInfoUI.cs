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

    float h;
    public float heart
    {
        get => h;
        set
        {
            h = value;
            m_Heart.text = value == 0 ? "0" : string.Format("{0:#,###}", value);
            if (h == 0)
            {
                //Die
                Die();
            }
        }
    }

    float m;
    public float money
    {
        get => m;
        set
        {
            m = value;
            m_Money.text = value == 0 ? "0" : string.Format("{0:#,###}", value);
        }
    }

    float s;
    public float score
    {
        get => s;
        set
        {
            s = value;
            m_Score.text = value == 0 ? "0" : string.Format("{0:#,###}", value);
        }
    }

    public void SetUserInfo(float heart, float money, float score)
    {
        this.heart = heart;
        this.money = money;
        this.score = score;
    }

    void Die()
    {
        Core.gameManager.GameOver();
    }


    public override void Open(UnityAction done)
    {
        done?.Invoke();
    }

    public override void Close(UnityAction done)
    {
        Core.plugs.GetPlugable<Theme>()?.RemoveOpenedTheme(this);
        done?.Invoke();
        gameObject.SetActive(false);
    }
}
