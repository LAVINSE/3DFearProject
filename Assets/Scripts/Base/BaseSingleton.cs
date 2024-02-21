using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Basesingleton<T> : MonoBehaviour where T : Basesingleton<T>
{
    #region ����
    private static T oInst = null;
    #endregion // ����

    #region Ŭ���� ������Ƽ
    public static T Inst
    {
        get
        {
            // �ν��Ͻ��� ���� ���
            if (Basesingleton<T>.oInst == null)
            {
                var Gameobj = new GameObject(typeof(T).Name);
                Basesingleton<T>.oInst = Gameobj.AddComponent<T>();
            }

            return Basesingleton<T>.oInst;
        }
    }
    #endregion // Ŭ���� ������Ƽ

    #region �Լ�
    /** �ʱ�ȭ */
    public virtual void Awake()
    {
        if (Basesingleton<T>.oInst != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Debug.Assert(Basesingleton<T>.oInst == null);

        if (oInst != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Basesingleton<T>.oInst = this as T;
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion // �Լ�

    #region Ŭ���� �Լ�
    /** �ν��Ͻ��� �����Ѵ� */
    public static T Create()
    {
        return Basesingleton<T>.Inst;
    }
    #endregion // Ŭ���� �Լ�
}