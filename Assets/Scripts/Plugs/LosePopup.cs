using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LosePopup : BasePopup
{
    public Transform popup;

    [SerializeField] Button m_Close;
    [SerializeField] Button m_Home;
    [SerializeField] Button m_Restart;
    [SerializeField] AnimationCurve m_Curve;

    public override void Open(UnityAction done)
    {
        StartCoroutine(CoUtilize.VLerp((v) => popup.localScale = v, Vector3.zero, Vector3.one, 0.2f, () => Opend(done), m_Curve));
    }

    void Opend(UnityAction done)
    {
        done?.Invoke();
    }

    public override void Close(UnityAction done)
    {
        gameObject.SetActive(false);
    }

    void GoHome()
    {

    }

    void OnRestart()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        m_Close.onClick.AddListener(() => Close(null));
        m_Home.onClick.AddListener(GoHome);
        m_Restart.onClick.AddListener(OnRestart);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
