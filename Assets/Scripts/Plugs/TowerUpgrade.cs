using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TowerUpgrade : BaseTheme
{
    public enum State { Open, Close }
    public State state;

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
        state = State.Close;
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
        m_TargetTower.UpgradeTower();
        Close(null);
    }

    public void Sell()
    {
        m_TargetTower.Delete();
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
