using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    #region 변수
    [Header("=====> 감도 설정 <=====")]
    [SerializeField] private float mouseSensitivityX;
    [SerializeField] private float mouseSensitivityY;

    [Header("=====> 방향 <=====")]
    public Transform orientation;
    public Transform viewDirection;

    private float xRotation;
    private float yRotation;
    #endregion // 변수

    #region 프로퍼티
    public static bool isRotateCamera = true;
    #endregion // 프로퍼티

    #region 함수
    /** 초기화 => 상태를 갱신한다 */
    private void Update()
    {
        if(isRotateCamera == false) { return; }

        // 마우스 수평/수직
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivityY;

        // 마우스가 위로 올라갈 때 X가 바뀜
        yRotation += mouseX;
        xRotation -= mouseY;

        // 수직회전 고정
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // 회전 적용
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        viewDirection.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
    #endregion // 함수
}
