using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class StageList : MonoBehaviour
{
    public List<StageItem> stageItems = new List<StageItem>();

    public void InitStage(StageItem.StageType stageType, float starCount = 0)
    {
        stageItems.ForEach((v) => v.InitStageItem(stageType, starCount));
    }

    void Start()
    {
    }

}
