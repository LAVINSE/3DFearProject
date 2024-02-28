using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatWindow : MonoBehaviour
{
    #region 변수
    public Image dialogImg; // 대화창 이미지
    public TextMeshProUGUI characterNameText; // 현재 대상중인 캐릭터 이름
    public TextMeshProUGUI DialogueText; // 현재 대사 출력 Text UI
    public GameObject objectArrow; // 대사가 완료되었을 때 출력되는 커서 오브젝트
    #endregion // 변수
}
