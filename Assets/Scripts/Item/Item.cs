using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    #region ����
    [SerializeField] private ItemDataSO itemdataSO;
    #endregion // ����

    #region ������Ƽ
    public ItemDataSO ItemData
    { 
        get => itemdataSO;
        set => itemdataSO = value;
    }
    #endregion // ������Ƽ

    #region �Լ�
    #endregion // �Լ�
}
