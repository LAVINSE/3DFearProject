using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    #region ����
    [SerializeField] private Transform cameraPosition;
    #endregion // ����

    #region �Լ�
    /** �ʱ�ȭ => ���¸� �����Ѵ� */
    private void Update()
    {
        transform.position = cameraPosition.position;
    }
    #endregion // �Լ�
}
