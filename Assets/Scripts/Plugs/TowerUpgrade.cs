using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TowerUpgrade : BaseTheme, IPointerEnterHandler, IPointerExitHandler
{
    public enum State { Open, Close }
    public State state;
    public Vector3 offset = new Vector3(0, 6, 1);

    [SerializeField, Range(0, 1)] float m_Ratio = 0.7f;
    [SerializeField] Button m_UpgradeBtn;
    [SerializeField] Button m_SellBtn;
    [SerializeField] Text m_Upgrade;
    [SerializeField] Text m_Sell;
    [SerializeField] AnimationCurve m_Curve;

    [SerializeField] Tower m_TargetTower;

    bool m_IsOnPointerEnter = false;
    GameObject m_TowerManager = null;

    public override void Open(UnityAction done)
    {
        state = State.Open;
        transform.LookAt(2 * transform.position - Camera.main.transform.position);
        gameObject.SetActive(true);
    }

    public override void Close(UnityAction done)
    {
        Core.plugs.GetPlugable<Theme>()?.RemoveOpenedTheme(this);
        state = State.Close;
        done?.Invoke();
        gameObject.SetActive(false);
    }

    public void Setup(Tower tower)
    {
        float level = tower.towerInfo.towerLevels[tower.currentLevel].level;
        float upgradePrice = level == 3 ? tower.towerInfo.towerLevels[tower.currentLevel].price
                                        : tower.towerInfo.towerLevels[tower.currentLevel + 1].price;
        float sellPrice = tower.towerInfo.towerLevels[tower.currentLevel].price;

        m_Upgrade.text = string.Format("{0:#,###}", level == 3 ? "MAX" : upgradePrice.ToString());
        m_Sell.text = string.Format("{0:#,###}", (sellPrice * m_Ratio));
        m_TargetTower = tower;
    }

    public void Upgrade()
    {
        float userMoney = Core.state.money;
        float level = m_TargetTower.towerInfo.towerLevels[m_TargetTower.currentLevel].level;

        if (level == 3)
        {
            //Open Popup
            Popup popup = Core.plugs.GetPlugable<Popup>();
            if (!popup.IsOpenedPopup<NotifyPopup>())
            {
                popup?.GetPopup<NotifyPopup>().SetContent("??? ?????? ??? ??? ????????????.");
                popup.Open<NotifyPopup>();
            }
            
            return;
        }

        float price = m_TargetTower.towerInfo.towerLevels[m_TargetTower.currentLevel + 1].price;
        if (userMoney < price)
        {
            //Open Popup
            Popup popup = Core.plugs.GetPlugable<Popup>();
            if (!popup.IsOpenedPopup<NotifyPopup>())
            {
                popup?.GetPopup<NotifyPopup>().SetContent("?????? ???????????????. !!");
                popup.Open<NotifyPopup>();
            }
            return;
        }

        Debug.Log("Upgrade Tower :" + m_TargetTower.name);

        if (m_TowerManager == null)
        {
            m_TowerManager = GameObject.FindGameObjectWithTag("Towers");
        }

        if (m_TargetTower.towerState == Tower.TowerState.Creating || m_TargetTower.towerState == Tower.TowerState.Upgrading)
        {
            Popup popup = Core.plugs.GetPlugable<Popup>();
            if (!popup.IsOpenedPopup<NotifyPopup>())
            {
                popup?.GetPopup<NotifyPopup>().SetContent("?????? ?????????..");
                popup.Open<NotifyPopup>();
            }
            
            return;
        }

        TowerManager t = m_TowerManager.GetComponent<TowerManager>();
        t.UpgradeTower(m_TargetTower);
        Core.state.money -= price;
        Close(null);
    }

    public void Sell()
    {
        if (m_TargetTower == null) { return; }

        Debug.Log("Sell Tower : " + m_TargetTower.name);

        float price = m_TargetTower.towerInfo.towerLevels[m_TargetTower.currentLevel].price;
        float sell = price * m_Ratio;
        Core.state.money += sell;

        if (m_TowerManager == null)
        {
            m_TowerManager = GameObject.FindGameObjectWithTag("Towers");
        }

        TowerManager t = m_TowerManager.GetComponent<TowerManager>();
        t.DeleteTower(m_TargetTower);

        Close(null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_IsOnPointerEnter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_IsOnPointerEnter = false;
    }

    void OnEnable()
    {
        StartCoroutine(CheckingMousePoint());
    }

    void OnDisable()
    {
        m_IsOnPointerEnter = false;
        StopAllCoroutines();
    }

    void Start()
    {
        m_UpgradeBtn.onClick.AddListener(Upgrade);
        m_SellBtn.onClick.AddListener(Sell);
    }

    IEnumerator CheckingMousePoint()
    {
        Transform body = null;
        if (m_TargetTower != null)
        {
            Transform[] childs = m_TargetTower.GetComponentsInChildren<Transform>();
            foreach (Transform t in childs)
            {
                if (t.name == "Body") { body = t; }
            }
        }

        RaycastHit hit;
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!m_IsOnPointerEnter)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        if (hit.transform != body)
                        {
                            if (hit.transform.name != "Body")
                            {
                                Close(null);
                            }
                        }
                    }
                }

            }

            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(2 * transform.position - Camera.main.transform.position);
    }
}
