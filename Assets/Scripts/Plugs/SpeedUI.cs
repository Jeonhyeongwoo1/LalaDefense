using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SpeedUI : BaseTheme
{
    [SerializeField] Button m_Slow;
    [SerializeField] Button m_Fast;
    [SerializeField] Text m_SpeedInfo;
    [SerializeField, Range(0, 1)] float m_SpeedRange = 0.5f;
    [SerializeField] float m_Max = 2f;
    [SerializeField] float m_Min = 0.5f;

    public override void Open(UnityAction done)
    {
        SetSpeedInfo(Core.gameManager.gameSpeed);
        gameObject.SetActive(true);
        done?.Invoke();
    }

    public override void Close(UnityAction done)
    {
        Core.plugs.GetPlugable<Theme>().RemoveOpenedTheme(this);
        done?.Invoke();
        gameObject.SetActive(false);
    }

    void ControlGameSpeed(bool isFast)
    {
        if ((isFast && Core.gameManager.gameSpeed == m_Max) || (!isFast && Core.gameManager.gameSpeed == m_Min))
        {
            Debug.Log("Can not Change Speed");
            return;
        }

        Core.gameManager.gameSpeed += isFast ? m_SpeedRange : (-m_SpeedRange);
        Time.timeScale = Core.gameManager.gameSpeed;
        SetSpeedInfo(Core.gameManager.gameSpeed);
    }

    void SetSpeedInfo(float speed)
    {
        m_SpeedInfo.text = "X" + speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Slow.onClick.AddListener(() => ControlGameSpeed(false));
        m_Fast.onClick.AddListener(() => ControlGameSpeed(true));

    }

}
