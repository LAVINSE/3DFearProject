using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightItem : MonoBehaviour
{
    #region 변수
    [SerializeField] private GameObject flashLightPrefab;
    [SerializeField] private AudioSource turnOnSound;
    [SerializeField] private AudioSource turnOffSound;
    [SerializeField] private bool isOn = false;
    #endregion // 변수

    #region 함수
    /** 초기화 */
    private void Start()
    {
        flashLightPrefab.SetActive(false);
    }

    private void Update()
    {
        if(!isOn && Input.GetKeyDown(KeyCode.E))
        {
            flashLightPrefab.SetActive(true);
            isOn = true;
            // 배터리 시스템 온
        }
        else if(isOn && Input.GetKeyDown(KeyCode.E))
        {
            flashLightPrefab.SetActive(false);
            isOn = false;
            // 배터리 시스템 오프
        }
    }
    #endregion // 함수
}
