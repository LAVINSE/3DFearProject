using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogTest : MonoBehaviour
{
    #region 변수
    [SerializeField] private List<DialogSystem> dislogSystemList = new List<DialogSystem>();
    #endregion // 변수

    #region 함수
    private IEnumerator Start()
    {
        foreach (var dialogSystem in dislogSystemList)
        {
            dialogSystem.CreateChatWindowSetUp();
            // 대사 분기 시작
            yield return new WaitUntil(() => dialogSystem.UpdateDialog());

            // 여기에 대사분기가 넘어가기 전 행동 추가가능
            Debug.Log(" 다음 대사 분기 있으면 시작 ");
        }
    }
    #endregion // 함수
}
