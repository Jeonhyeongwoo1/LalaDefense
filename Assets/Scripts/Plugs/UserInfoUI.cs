using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UserInfoUI : BaseTheme
{

    [SerializeField] Text m_Heart;
    [SerializeField] Text m_Money;

    public void SetUserInfo(float heart, float money)
    {
        m_Heart.text = heart.ToString();
        m_Money.text = money.ToString();
    }

    public override void Open(UnityAction done)
    {
    
    }

    public override void Close(UnityAction done)
    {
    
    }
}
