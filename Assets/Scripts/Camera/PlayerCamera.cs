using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    #region ����
    [Header("=====> ���� ���� <=====")]
    [SerializeField] private float mouseSensitivityX;
    [SerializeField] private float mouseSensitivityY;

    [Header("=====> ���� <=====")]
    public Transform orientation;
    public Transform viewDirection;

    private float xRotation;
    private float yRotation;
    #endregion // ����

    #region ������Ƽ
    public static bool isRotateCamera = true;
    #endregion // ������Ƽ

    #region �Լ�
    /** �ʱ�ȭ => ���¸� �����Ѵ� */
    private void Update()
    {
        if(isRotateCamera == false) { return; }

        // ���콺 ����/����
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivityY;

        // ���콺�� ���� �ö� �� X�� �ٲ�
        yRotation += mouseX;
        xRotation -= mouseY;

        // ����ȸ�� ����
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // ȸ�� ����
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        viewDirection.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
    #endregion // �Լ�
}
