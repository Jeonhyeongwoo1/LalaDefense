using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class BaseTheme : MonoBehaviour
{
    public abstract void Open(UnityAction done);
    public abstract void Close(UnityAction done);
}

public class TowerStore : BaseTheme, IPlug
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

    Ray ray;
    RaycastHit hit;

    public void OnClickItem(GameObject prefab, float price)
    {
        print("tower : " + prefab + "Price : " + price);
        CanBuild = true;
        
        Terrain t = FindObjectOfType<Terrain>();
        t.nodes.gameObject.SetActive(true);
        SaveItem(prefab, price);
    }

    public override void Open(UnityAction done)
    {
        StartCoroutine(CoUtilize.VLerp((v) => m_TowerStoreUI.localScale = v, Vector2.up, Vector2.one, 0.3f, done, curve));
    }

    public override void Close(UnityAction done)
    {
        StartCoroutine(CoUtilize.VLerp((v) => m_TowerStoreUI.localScale = v, Vector2.one, Vector2.up, 0.3f, done, curve));
    }

    public void OpenCircleAsync(UnityAction done)
    {
        StartCoroutine(CoUtilize.VLerp((v) => m_CircleBtn.transform.localScale = v, Vector3.zero, Vector3.one, 0.3f, done, curve));
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
        StartCoroutine(CoUtilize.VLerp((v) => m_CircleBtn.transform.localScale = v, Vector3.one, Vector3.zero, 0.3f, () => Open(null), curve));
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Items.ForEach((v) => v.OnClickEvent.AddListener(OnClickItem));
        m_Close.onClick.AddListener(() => Close(OpenCircleBtn));
        m_CircleBtn.onClick.AddListener(CloseCircleBtn);
    }

    [ContextMenu("Open")]
    public void Test1()
    {
        Open(null);
    }

    [ContextMenu("Close")]
    public void Test2()
    {
        Close(null);
    }

    GameObject m_TowerItem;
    GameObject m_TowerCreation;
    float m_Price;

    public void SaveItem(GameObject tower, float price)
    {
        m_TowerItem = tower;
        m_Price = price;
    }

    void CreateTower(RaycastHit hit)
    {
        if (m_TowerCreation == null)
        {
            m_TowerCreation = GameObject.FindGameObjectWithTag("Towers");
        }

        //Node
        Terrain t = FindObjectOfType<Terrain>();
        t.SelectNode(hit.transform);

        GameObject tower = Instantiate(m_TowerItem, t.nodes.GetBuildPosition(), Quaternion.identity, m_TowerCreation.transform);
        CanBuild = false;
        m_TowerItem = null;
        m_Price = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (!CanBuild) { return; }
                if (hit.transform.tag == "Node")
                {
                    CreateTower(hit);
                }
            }
        }
    }

}
