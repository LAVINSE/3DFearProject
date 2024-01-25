using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region ����
    private bool IsEscDown;
    private bool IsCursor = true;
    #endregion // ����

    #region �Լ�
    /** �ʱ�ȭ => ���¸� �����Ѵ� */
    private void Update()
    {
        IsEscDown = Input.GetKeyDown(KeyCode.Escape);

        // Ŀ������� Ȱ��ȭ ���°� �ƴҰ��
        if (IsEscDown && !IsCursor)
        {
            CursorLock();
            IsCursor = true;

        }
        // Ŀ������� Ȱ��ȭ ������ ���
        else if (IsEscDown && IsCursor)
        {
            CursorUnLock();
            IsCursor = false;
        }
    }

    // Ŀ�� ���
    public void CursorLock()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Ŀ�� �������
    public void CursorUnLock()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    #endregion // �Լ�
}
