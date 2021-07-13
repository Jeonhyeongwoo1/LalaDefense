using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class IntroAni : MonoBehaviour
{
    public Transform props; // Tree, barrier
    public Transform tower;
    [SerializeField] CinemachineVirtualCamera introCam;

    [SerializeField] float scaleUp;
    [SerializeField] AnimationCurve curve;

    private float totalIntroTime = 7f;

    public void StartIntro()
    {
        StartCoroutine(StartingIntroAni());        
    }

    // 1. Dolly Camera를 사용하여 맵을 돈다. (카메라 회전에 대해서 고려해봐야함 -> 애니메이션으로 만들지 생각)
    // 2. 먼저 나무 애니메이션
    // 3. 그외의 부속품, 파티클 생성
    // 4. 타워들 애니메이션
    // 5. 끝날때 쯤 타이틀 등장하게끔 하기
    
    IEnumerator StartingIntroAni()
    {


        yield return null;
    }




    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
    
    }
}
