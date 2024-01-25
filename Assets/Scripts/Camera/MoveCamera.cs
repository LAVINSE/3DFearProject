using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    #region 변수
    [SerializeField] private Transform cameraPosition;
    #endregion // 변수

    #region 함수
    /** 초기화 => 상태를 갱신한다 */
    private void Update()
    {
        transform.position = cameraPosition.position;
    }
    #endregion // 함수
}
