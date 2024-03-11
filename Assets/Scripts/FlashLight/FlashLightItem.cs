using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightItem : MonoBehaviour
{
    #region ����
    [SerializeField] private GameObject lightPrefab;
    [SerializeField] private AudioSource turnOnSound;
    [SerializeField] private AudioSource turnOffSound;
    [SerializeField] private bool isOn = false;

    private PlayerKeyCode playerKeycode;

    private Coroutine flashLightBatteryCo;
    #endregion // ����

    #region ������Ƽ
    public float batteryTime = 60f; // TODO : ���͸� �ð� �����ؾߵ�
    #endregion // ������Ƽ

    #region �Լ�
    /** �ʱ�ȭ */
    private void Awake()
    {
        playerKeycode = GetComponentInParent<PlayerKeyCode>();
    }

    /** �ʱ�ȭ */
    private void Start()
    {
        FlashLightState(false);
    }

    /** �ʱ�ȭ => ���¸� �����Ѵ� */
    private void Update()
    {
        if (Input.GetKeyDown(playerKeycode.FlashLightKeyCode))
        {
            FlashLightController();
        }
    }

    /** �������� ��Ʈ�� �Ѵ� */
    private void FlashLightController()
    {
        // �������� �������� ���� ���
        if (!isOn && batteryTime > 0)
        {
            // ������ ���¸� �����Ѵ�
            FlashLightState(true);

            // ���͸� �ý��� ��
            flashLightBatteryCo = StartCoroutine(FlashLightBatteryCO());
        }
        else if(isOn)
        {
            // ������ ���¸� �����Ѵ�
            FlashLightState(false);

            // ���͸� �ڷ�ƾ�� ������ ���
            if (flashLightBatteryCo != null)
            {
                StopCoroutine(flashLightBatteryCo);
            }
        }
    }

    /** ������ ���¸� �����Ѵ� */
    private void FlashLightState(bool isState)
    {
        isOn = isState;
        lightPrefab.SetActive(isState);

        // Ȱ��ȭ ������ ���
        if (isState)
        {
            //turnOnSound.Play();
        }
        else
        {
            //turnOffSound.Play();
        }
    }
    #endregion // �Լ�

    #region �ڷ�ƾ
    /** ���͸� �ڷ�ƾ */
    private IEnumerator FlashLightBatteryCO()
    {
        // ���͸��� 0 ���� ũ�� �������� Ȱ��ȭ ������ ���
        while (batteryTime > 0 && isOn)
        {
            // ���͸��� ���ҽ�Ų��
            batteryTime -= Time.deltaTime;

            yield return null;
        }

        // ���͸��� �����Ǿ��� ���� �߰� ó��
        if (batteryTime <= 0)
        {
            // �������� ����
            lightPrefab.SetActive(false);
            isOn = false;
        }
    }
    #endregion // �ڷ�ƾ
}
