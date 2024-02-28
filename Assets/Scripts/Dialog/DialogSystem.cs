using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    #region 변수
    [SerializeField] private Canvas canvas; // 캔버스
    [SerializeField] private GameObject chatWindowPrefab; // 텍스트 프리팹
    [SerializeField] private DialogDataSO dialogs; // 현재 분기의 대사 목록 배열
    [SerializeField] private float typingSpeed = 0.5f; // 텍스트 타이핑 효과의 재생 속도
    [SerializeField] private bool isAutoStart = true; // 자동 시작 여부

    private ChatWindow chatWindow;

    private bool isFirst = true; // 최초 1회만 호출하기 위한 변수
    private bool isTypingEffect = false; // 텍스트 타이핑 효과를 재생중인지

    private int currentDialogIndex = -1; // 현재 대사 순번, 초기값 -1    
    #endregion // 변수

    #region 함수 
    /** 초기화 */
    private void Awake()
    {
        // 대화창 생성 및 설정
        CreateChatWindowSetUp();
    }

    /** 대화창 생성 및 설정 */
    private void CreateChatWindowSetUp()
    {
        // 대화창이 없을 경우 생성
        if(canvas.GetComponentInChildren<ChatWindow>(true) == null)
        {
            var chatWindowObject = Instantiate(chatWindowPrefab);
            chatWindowObject.transform.SetParent(canvas.transform);
            chatWindowObject.transform.localPosition = Vector3.zero;
            chatWindow = chatWindowObject.GetComponent<ChatWindow>();
        }

        // 대화창 오브젝트를 비활성화한다
        SetActiveObjects(chatWindow, false);
    }

    /** 대화창 오브젝트를 활성화/비활성화한다 */
    private void SetActiveObjects(ChatWindow chatWindow, bool isActive)
    {
        chatWindow.gameObject.SetActive(isActive);

        // 화살표는 대사가 종료되었을 때만 활성화하기 때문에 항상 false
        chatWindow.objectArrow.SetActive(false);
    }

    /** 대사를 재생시키고 모든 대사가 끝났을 경우 true 반환 */
    public bool UpdateDialog()
    {
        // 대사 분기가 시작될 때 1회만 호출
        if(isFirst == true)
        {
            // 대화창 생성 및 설정
            CreateChatWindowSetUp();

            // 자동 재생으로 설정되어 있으면 첫 번째 대사 재생
            if (isAutoStart)
            {
                // 다음 대사를 진행/설정한다
                SetNextDialog();
            }

            isFirst = false;
        }

        // 엔터를 눌렀을 경우
        if(Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            // 텍스트 타이핑 효과를 재생중일때 엔터를 클릭하면 타이핑 효과 종료
            if (isTypingEffect == true)
            {
                isTypingEffect = false;

                // 타이핑 효과를 중지하고, 현재 대사 전체를 출력한다
                StopCoroutine("OnTypingText");
                chatWindow.DialogueText.text = dialogs.dialogST[currentDialogIndex].dialogue;
                
                // 대사가 완료되었을 때 출력되는 커서 활성화
                chatWindow.objectArrow.SetActive(true);
                
                return false;
            }

            // 대사가 남아있을 경우 다음 대사 진행
            if(dialogs.dialogST.Length > currentDialogIndex + 1)
            {
                // 다음 대사를 진행/설정한다
                SetNextDialog();
            }
            // 대사가 더 이상 없을 경우 모든 오브젝트를 비활성화하고 true 반환
            else
            {
                // 대화창 오브젝트를 비활성화한다
                SetActiveObjects(chatWindow, false);

                return true;
            }
        }

        return false;
    }

    /** 다음 대사를 진행/설정한다 */
    private void SetNextDialog()
    {
        // 다음 대사를 진행
        currentDialogIndex++;

        // 대화창 오브젝트를 활성화한다
        SetActiveObjects(chatWindow, true);

        // 현재 화자 이름 텍스트 설정
        chatWindow.characterNameText.text = dialogs.dialogST[currentDialogIndex].dialogcharacterName;

        // 현재 화자의 대사 텍스트 설정
        StartCoroutine("OnTypingText");
    }
    #endregion // 함수

    #region 코루틴
    /** 현재 화자의 대사를 한글자씩 타이핑을 한다 */
    private IEnumerator OnTypingText()
    {
        int index = 0;

        isTypingEffect = true;

        // 텍스트를 한글자씩 타이핑치듯 재생
        while (index < dialogs.dialogST[currentDialogIndex].dialogue.Length)
        {
            // 현재 화자의 대사를 한글자씩 타이핑
            chatWindow.DialogueText.text = dialogs.dialogST[currentDialogIndex].dialogue.Substring(0, index + 1);

            index++;

            // 타이핑 속도
            yield return new WaitForSeconds(typingSpeed);
        }

        isTypingEffect = false;

        // 대사가 완료되었을 때 출력되는 커서 활성화
        chatWindow.objectArrow.SetActive(true);
    }
    #endregion // 코루틴
}
