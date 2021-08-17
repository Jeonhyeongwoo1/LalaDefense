using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageNavigation : MonoBehaviour
{
    public List<Transform> pages = new List<Transform>();

    public void ChangePage(float curStage)
    {
        for (var i = 0; i < pages.Count; i++)
        {
            if (i == curStage) { pages[i].GetChild(0).gameObject.SetActive(true); }
            else { pages[i].GetChild(0).gameObject.SetActive(false); }
        }
    }
    
    //추후에 시간되면...
    void OnClick()
    {
        //gameObject.SendMessageUpwards("MoveStage")
    }

    void Start()
    {
        pages.ForEach((v) => v.GetComponent<Button>().onClick.AddListener(OnClick));
    }

}
