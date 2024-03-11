using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightItem : MonoBehaviour
{
    #region 변수
    [SerializeField] private GameObject lightPrefab;
    [SerializeField] private AudioSource turnOnSound;
    [SerializeField] private AudioSource turnOffSound;
    [SerializeField] private bool isOn = false;

    private PlayerKeyCode playerKeycode;

    private Coroutine flashLightBatteryCo;
    #endregion // 변수

    #region 프로퍼티
    public float batteryTime = 60f; // TODO : 배터리 시간 설정해야됨
    #endregion // 프로퍼티

    #region 함수
    /** 초기화 */
    private void Awake()
    {
        playerKeycode = GetComponentInParent<PlayerKeyCode>();
    }

    /** 초기화 */
    private void Start()
    {
        FlashLightState(false);
    }

    /** 초기화 => 상태를 갱신한다 */
    private void Update()
    {
        if (Input.GetKeyDown(playerKeycode.FlashLightKeyCode))
        {
            FlashLightController();
        }
    }

    /** 손전등을 컨트롤 한다 */
    private void FlashLightController()
    {
        // 손전등이 켜져있지 않을 경우
        if (!isOn && batteryTime > 0)
        {
            // 손전등 상태를 관리한다
            FlashLightState(true);

            // 배터리 시스템 온
            flashLightBatteryCo = StartCoroutine(FlashLightBatteryCO());
        }
        else if(isOn)
        {
            // 손전등 상태를 관리한다
            FlashLightState(false);

            // 배터리 코루틴이 존재할 경우
            if (flashLightBatteryCo != null)
            {
                StopCoroutine(flashLightBatteryCo);
            }
        }
    }

    /** 손전등 상태를 관리한다 */
    private void FlashLightState(bool isState)
    {
        isOn = isState;
        lightPrefab.SetActive(isState);

        // 활성화 상태일 경우
        if (isState)
        {
            //turnOnSound.Play();
        }
        else
        {
            //turnOffSound.Play();
        }
    }
    #endregion // 함수

    #region 코루틴
    /** 배터리 코루틴 */
    private IEnumerator FlashLightBatteryCO()
    {
        // 배터리가 0 보다 크고 손전등이 활성화 상태일 경우
        while (batteryTime > 0 && isOn)
        {
            // 배터리를 감소시킨다
            batteryTime -= Time.deltaTime;

            yield return null;
        }

        // 배터리가 소진되었을 때의 추가 처리
        if (batteryTime <= 0)
        {
            // 손전등을 끈다
            lightPrefab.SetActive(false);
            isOn = false;
        }
    }
    #endregion // 코루틴
}
