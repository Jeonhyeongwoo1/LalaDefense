using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class RoundUI : BaseTheme
{
    [SerializeField] CanvasGroup m_Background;
    [SerializeField] Transform m_RoundTitle;
    [SerializeField] TextMeshProUGUI m_Text;

    [SerializeField] Vector3 m_TitleInitPos;
    [SerializeField, Range(0, 1)] float m_AlphaFadeDuration = 0.5f;
    [SerializeField, Range(0, 1)] float m_TitleMoveDuration = 0.3f;
    [SerializeField] float m_WaveAmount = 0f;
    [SerializeField, Range(0, 3)] float m_WaveDuration = 3f;
    [SerializeField] float m_WaveSpeed;
    [SerializeField] AnimationCurve m_Curve;

    public void SetRoundInfo(int curRound, int totalRound)
    {
        m_Text.text = "ROUND " + "<size=80>" + curRound.ToString() + "/" + totalRound.ToString() + "</font>";
    }

    public override void Open(UnityAction done)
    {
        StartCoroutine(Opening(done));
    }

    public override void Close(UnityAction done)
    {
        StartCoroutine(CoUtilize.Lerp((v) => m_Background.alpha = v, 1, 0, m_AlphaFadeDuration, () => Closed(done), m_Curve));
    }

    void Closed(UnityAction done)
    {
        Core.plugs.GetPlugable<Theme>()?.RemoveOpenedTheme(this);
        m_RoundTitle.localPosition = m_TitleInitPos;
        done?.Invoke();
        gameObject.SetActive(false);
    }

    Vector3 Wave(float amount, float time)
    {
        return new Vector3(0, Mathf.Sin(time) * amount, 0);
    }

    IEnumerator Opening(UnityAction done)
    {
        yield return CoUtilize.Lerp((v) => m_Background.alpha = v, 0, 1, m_AlphaFadeDuration, null, m_Curve);
        yield return CoUtilize.VLerp((v) => m_RoundTitle.localPosition = v, m_TitleInitPos, Vector3.zero, m_TitleMoveDuration, null, m_Curve);
        yield return new WaitForSeconds(1.5f);
        done?.Invoke();
        /*
                TMP_TextInfo tMP_TextInfo = m_Text.GetTextInfo(m_Text.text);
                int count = tMP_TextInfo.characterCount;
                Vector3 wv = Vector3.zero;
                Vector3[][] vertex_Base = new Vector3[m_Text.textInfo.meshInfo.Length][];
                float elapsed = 0f;

                for (int i = 0; i < m_Text.textInfo.meshInfo.Length; ++i)
                {
                    vertex_Base[i] = new Vector3[m_Text.textInfo.meshInfo[i].vertices.Length];
                    System.Array.Copy(m_Text.textInfo.meshInfo[i].vertices, vertex_Base[i], m_Text.textInfo.meshInfo[i].vertices.Length);
                }

                while (elapsed < 100)
                {

                    for (int i = 0; i < m_Text.textInfo.meshInfo.Length; ++i)
                    {
                        wv = Wave(m_WaveAmount, elapsed * m_WaveSpeed);
                        for (int v = 0; v < m_Text.textInfo.meshInfo[i].vertices.Length; v += 4)
                        {
                            for (byte k = 0; k < 4; ++k)
                                m_Text.textInfo.meshInfo[i].vertices[v + k] = vertex_Base[i][v + k];

                            for (byte k = 0; k < 4; ++k)
                            {

                                m_Text.textInfo.meshInfo[i].vertices[v + k] += wv * m_WaveAmount;

                            }

                        }
                        m_Text.UpdateVertexData();
                    }

                    elapsed += Time.deltaTime;
                    yield return null;
                }
        */
    }

    [ContextMenu("TEST")]
    public void Test()
    {
        StartCoroutine(Opening(() => Close(null)));
    }

}
