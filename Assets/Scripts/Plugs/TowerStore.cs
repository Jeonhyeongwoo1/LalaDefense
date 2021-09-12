using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TowerStore : BaseTheme
{
    bool m_CanBuild = false;
    public bool CanBuild
    {
        set => m_CanBuild = value;
        get => m_CanBuild;
    }


    [SerializeField] Button m_CircleBtn;
    [SerializeField] Button m_Close;
    [SerializeField] Transform m_TowerStoreUI;
    [SerializeField] AnimationCurve curve;
    [SerializeField] AnimationCurve m_CloseCircle;
    [SerializeField] List<TowerStoreItem> m_Items = new List<TowerStoreItem>();

    GameObject m_TowerManager;

    public void OnClickItem(GameObject prefab, float price)
    {
        print("tower : " + prefab + "Price : " + price);
        CanBuild = true;

        Terrain t = Core.models.GetModel<Terrain>();
        t.nodes.gameObject.SetActive(true);
        StartCoroutine(CheckingMousePoint(prefab, price));
    }

    public override void Open(UnityAction done)
    {
        OpenCircleAsync(done);
    }

    public override void Close(UnityAction done)
    {
        Core.plugs.GetPlugable<Theme>()?.RemoveOpenedTheme(this);
        done?.Invoke();
        OpenCircleBtn();
        CloseTowerStore(() => gameObject.SetActive(false));
    }

    public void OpenCircleAsync(UnityAction done)
    {
        StartCoroutine(CoUtilize.VLerp((v) => m_CircleBtn.transform.localScale = v, Vector3.zero, Vector3.one, 0.3f, done, curve));
    }

    void CreateTower(RaycastHit hit, GameObject tower, float price)
    {
        if (m_TowerManager == null)
        {
            m_TowerManager = GameObject.FindGameObjectWithTag("Towers");
        }

        Theme theme = Core.plugs.GetPlugable<Theme>();
        UserInfoUI userInfoUI = theme.GetTheme<UserInfoUI>();
        float money = userInfoUI.money;

        if (price > money)
        {
            Popup popup = Core.plugs.GetPlugable<Popup>();
            popup.Open<NotifyPopup>();
            popup?.GetPopup<NotifyPopup>().SetContent("돈이 부족합니다. !!");
            return;
        }

        Debug.Log("Create Tower :" + tower.name);

        userInfoUI.money -= price;
        TowerManager manager = m_TowerManager.GetComponent<TowerManager>();
        manager.CreateTower(tower.transform, hit.transform);
        CanBuild = false;
    }

    void CloseTowerStore(UnityAction done)
    {
        StopCoroutine("CheckingMousePoint");
        
        Terrain t = Core.models.GetModel<Terrain>();
        if (t.nodes.gameObject.activeSelf)
        {
            t.nodes.gameObject.SetActive(false);
        }
       
        
        StartCoroutine(CoUtilize.VLerp((v) => m_TowerStoreUI.localScale = v, Vector2.one, Vector2.up, 0.3f, done, curve));
    }

    void OpenCircleBtn()
    {
        m_TowerStoreUI.gameObject.SetActive(false);
        Image icon = m_CircleBtn.transform.GetChild(0).GetComponent<Image>();
        icon.fillAmount = 0f;
        m_CircleBtn.transform.localScale = Vector3.one;
        StartCoroutine(CoUtilize.Lerp((v) => icon.fillAmount = v, 0, 1, 0.7f, null, curve));
    }

    void CloseCircleBtn()
    {
        m_TowerStoreUI.gameObject.SetActive(true);
        StartCoroutine(CoUtilize.VLerp((v) => m_CircleBtn.transform.localScale = v, Vector3.one, Vector3.zero, 0.3f, () => OpenTowerStore(), curve));
    }

    void OpenTowerStore()
    {
        StartCoroutine(CoUtilize.VLerp((v) => m_TowerStoreUI.localScale = v, Vector2.up, Vector2.one, 0.3f, null, curve));
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Items.ForEach((v) => v.OnClickEvent.AddListener(OnClickItem));
        m_Close.onClick.AddListener(() => CloseTowerStore(OpenCircleBtn));
        m_CircleBtn.onClick.AddListener(CloseCircleBtn);
    }

    IEnumerator CheckingMousePoint(GameObject tower, float price)
    {
        RaycastHit hit;
        bool isHit = false;
        while (!isHit)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (!CanBuild) { yield break; }
                    if (hit.transform.tag == "Node")
                    {
                        CreateTower(hit, tower, price);
                        isHit = true;
                    }
                }
            }

            yield return null;
        }
    }

}
