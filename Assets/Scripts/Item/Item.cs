using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    #region 변수
    [SerializeField] private ItemDataSO itemdataSO;
    #endregion // 변수

    #region 프로퍼티
    public ItemDataSO ItemData
    { 
        get => itemdataSO;
        set => itemdataSO = value;
    }
    #endregion // 프로퍼티

    #region 함수
    #endregion // 함수
}
