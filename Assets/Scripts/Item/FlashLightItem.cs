using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightItem : MonoBehaviour
{
    #region ����
    [SerializeField] private GameObject flashLightPrefab;
    [SerializeField] private AudioSource turnOnSound;
    [SerializeField] private AudioSource turnOffSound;
    [SerializeField] private bool isOn = false;
    #endregion // ����

    #region �Լ�
    /** �ʱ�ȭ */
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
            // ���͸� �ý��� ��
        }
        else if(isOn && Input.GetKeyDown(KeyCode.E))
        {
            flashLightPrefab.SetActive(false);
            isOn = false;
            // ���͸� �ý��� ����
        }
    }
    #endregion // �Լ�
}
