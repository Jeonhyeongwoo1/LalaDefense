using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TowerUpgrade : BaseTheme
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

    public override void Open(UnityAction done)
    {
        state = State.Open;
        gameObject.SetActive(true);
    }

    public override void Close(UnityAction done)
    {
        Core.plugs.GetPlugable<Theme>()?.RemoveOpenedTheme(this);
        state = State.Close;
        done?.Invoke();
        gameObject.SetActive(false);
    }

    public void Setup(float level, float price, Tower tower)
    {
        m_Upgrade.text = string.Format("{0:#,###}", level == 3 ? "MAX" : price.ToString());
        m_Sell.text = string.Format("{0:#,###}", (price * m_Ratio));
        m_TargetTower = tower;
        m_UpgradeBtn.enabled = level == 3 ? false : true;
    }

    public void Upgrade()
    {
        Theme theme = Core.plugs.GetPlugable<Theme>();
        float userMoney = theme.GetTheme<UserInfoUI>().money;

        if (userMoney <= 0) { return; }

        float price = m_TargetTower.towerInfo.towerLevels[m_TargetTower.currentLevel].price;

        if (userMoney < price)
        {
            //Open Popup
            Popup popup = Core.plugs.GetPlugable<Popup>();
            popup.Open<NotifyPopup>();
            popup?.GetPopup<NotifyPopup>().SetContent("돈이 부족합니다. !!");
            return;
        }

        Debug.Log("Upgrade Tower :" + m_TargetTower.name);

        m_TargetTower.UpgradeTower();
        //Price 
        theme.GetTheme<UserInfoUI>().money -= price;
        Close(null);
    }

    public void Sell()
    {
        if (m_TargetTower == null) { return; }

        Debug.Log("Sell Tower : " + m_TargetTower.name);

        float price = m_TargetTower.towerInfo.towerLevels[m_TargetTower.currentLevel].price;
        float sell = price * m_Ratio;
        Theme theme = Core.plugs.GetPlugable<Theme>();
        theme.GetTheme<UserInfoUI>().money += sell;

        TowerManager t = m_TargetTower.transform.parent.GetComponent<TowerManager>();
        t.DeleteTower(m_TargetTower);

        Close(null);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_UpgradeBtn.onClick.AddListener(Upgrade);
        m_SellBtn.onClick.AddListener(Sell);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(2 * transform.position - Camera.main.transform.position);
    }
}
