using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TowerInfoUI : BaseTheme, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Transform m_Frame;
    [SerializeField] float m_InitAxisX;
    [SerializeField] float m_OpenAxisX;
    [SerializeField, Range(0, 1)] float m_OpenDuration = 0.3f;
    [SerializeField, Range(0, 1)] float m_CloseDuration = 0.3f;
    [SerializeField] AnimationCurve m_Curve;

    [SerializeField] Button m_Close;
    [SerializeField] Image m_TowerImg;
    [SerializeField] TextMeshProUGUI m_TowerName;
    [SerializeField] TextMeshProUGUI m_TowerLevel;
    [SerializeField] TextMeshProUGUI m_Attack;
    [SerializeField] TextMeshProUGUI m_SpecialAttack;
    [SerializeField] TextMeshProUGUI m_AttackRange;
    [SerializeField] TextMeshProUGUI m_AttackSpeed;

    bool m_IsOnPointerEnter = false;
    Tower m_TargetTower;

    public void Setup(Sprite towerSprite, Tower tower)
    {
        m_TargetTower = tower;
        m_TowerImg.sprite = towerSprite;
        m_TowerName.text = tower.towerInfo.towerName;
        m_TowerLevel.text = "LEVEL : " + tower.towerInfo.towerLevels[tower.currentLevel].level.ToString();

        AttackInfo attackInfo = tower.GetCurLevelAttackInfo();
        m_Attack.text = attackInfo.damage.ToString();
        m_SpecialAttack.text = attackInfo.specialAttack.ToUpper();
        m_AttackRange.text = attackInfo.range.ToString();

        if(tower.towerInfo.towerName == "Acid" || tower.towerInfo.towerName == "Fire")
        {
            m_AttackSpeed.text = "FAST";
        }
        else
        {
            m_AttackSpeed.text = attackInfo.speed.ToString();
        }

     
    }

    public override void Open(UnityAction done)
    {
        
        gameObject.SetActive(true);
        Vector3 pos = m_Frame.localPosition;
        StartCoroutine(CoUtilize.Lerp((v) => m_Frame.localPosition = new Vector3(v, pos.y, pos.z), m_Frame.localPosition.x, m_OpenAxisX, m_OpenDuration, done, m_Curve));
    }

    public override void Close(UnityAction done)
    {
        if (!m_Frame.gameObject.activeSelf) { return; }
        Vector3 pos = m_Frame.localPosition;
        StartCoroutine(CoUtilize.Lerp((v) => m_Frame.localPosition = new Vector3(v, pos.y, pos.z), m_Frame.localPosition.x, m_InitAxisX, m_CloseDuration, () => Closed(done), m_Curve));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_IsOnPointerEnter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_IsOnPointerEnter = false;
    }

    void Closed(UnityAction done)
    {
        Core.plugs.GetPlugable<Theme>()?.RemoveOpenedTheme(this);
        gameObject.SetActive(false);
        done?.Invoke();
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

    void OnEnable()
    {
        StartCoroutine(CheckingMousePoint());
    }

    void OnDisable()
    {
        m_IsOnPointerEnter = false;
        StopAllCoroutines();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Close.onClick.AddListener(() => Close(null));
    }

}
