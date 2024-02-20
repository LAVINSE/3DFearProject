using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    #region 변수
    [SerializeField] private ItemData itemdata;
    #endregion // 변수

    #region 프로퍼티
    public ItemData ItemData { get => itemdata; }
    #endregion // 프로퍼티

    #region 함수
    #endregion // 함수
}
