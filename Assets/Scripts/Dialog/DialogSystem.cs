using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    #region ����
    [SerializeField] private Canvas canvas; // ĵ����
    [SerializeField] private GameObject chatWindowPrefab; // �ؽ�Ʈ ������
    [SerializeField] private DialogDataSO dialogs; // ���� �б��� ��� ��� �迭
    [SerializeField] private float typingSpeed = 0.5f; // �ؽ�Ʈ Ÿ���� ȿ���� ��� �ӵ�
    [SerializeField] private bool isAutoStart = true; // �ڵ� ���� ����

    private ChatWindow chatWindow;

    private bool isFirst = true; // ���� 1ȸ�� ȣ���ϱ� ���� ����
    private bool isTypingEffect = false; // �ؽ�Ʈ Ÿ���� ȿ���� ���������

    private int currentDialogIndex = -1; // ���� ��� ����, �ʱⰪ -1    
    #endregion // ����

    #region �Լ� 
    /** �ʱ�ȭ */
    private void Awake()
    {
        // ��ȭâ ���� �� ����
        CreateChatWindowSetUp();
    }

    /** ��ȭâ ���� �� ���� */
    private void CreateChatWindowSetUp()
    {
        // ��ȭâ�� ���� ��� ����
        if(canvas.GetComponentInChildren<ChatWindow>(true) == null)
        {
            var chatWindowObject = Instantiate(chatWindowPrefab);
            chatWindowObject.transform.SetParent(canvas.transform);
            chatWindowObject.transform.localPosition = Vector3.zero;
            chatWindow = chatWindowObject.GetComponent<ChatWindow>();
        }

        // ��ȭâ ������Ʈ�� ��Ȱ��ȭ�Ѵ�
        SetActiveObjects(chatWindow, false);
    }

    /** ��ȭâ ������Ʈ�� Ȱ��ȭ/��Ȱ��ȭ�Ѵ� */
    private void SetActiveObjects(ChatWindow chatWindow, bool isActive)
    {
        chatWindow.gameObject.SetActive(isActive);

        // ȭ��ǥ�� ��簡 ����Ǿ��� ���� Ȱ��ȭ�ϱ� ������ �׻� false
        chatWindow.objectArrow.SetActive(false);
    }

    /** ��縦 �����Ű�� ��� ��簡 ������ ��� true ��ȯ */
    public bool UpdateDialog()
    {
        // ��� �бⰡ ���۵� �� 1ȸ�� ȣ��
        if(isFirst == true)
        {
            // ��ȭâ ���� �� ����
            CreateChatWindowSetUp();

            // �ڵ� ������� �����Ǿ� ������ ù ��° ��� ���
            if (isAutoStart)
            {
                // ���� ��縦 ����/�����Ѵ�
                SetNextDialog();
            }

            isFirst = false;
        }

        // ���͸� ������ ���
        if(Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            // �ؽ�Ʈ Ÿ���� ȿ���� ������϶� ���͸� Ŭ���ϸ� Ÿ���� ȿ�� ����
            if (isTypingEffect == true)
            {
                isTypingEffect = false;

                // Ÿ���� ȿ���� �����ϰ�, ���� ��� ��ü�� ����Ѵ�
                StopCoroutine("OnTypingText");
                chatWindow.DialogueText.text = dialogs.dialogST[currentDialogIndex].dialogue;
                
                // ��簡 �Ϸ�Ǿ��� �� ��µǴ� Ŀ�� Ȱ��ȭ
                chatWindow.objectArrow.SetActive(true);
                
                return false;
            }

            // ��簡 �������� ��� ���� ��� ����
            if(dialogs.dialogST.Length > currentDialogIndex + 1)
            {
                // ���� ��縦 ����/�����Ѵ�
                SetNextDialog();
            }
            // ��簡 �� �̻� ���� ��� ��� ������Ʈ�� ��Ȱ��ȭ�ϰ� true ��ȯ
            else
            {
                // ��ȭâ ������Ʈ�� ��Ȱ��ȭ�Ѵ�
                SetActiveObjects(chatWindow, false);

                return true;
            }
        }

        return false;
    }

    /** ���� ��縦 ����/�����Ѵ� */
    private void SetNextDialog()
    {
        // ���� ��縦 ����
        currentDialogIndex++;

        // ��ȭâ ������Ʈ�� Ȱ��ȭ�Ѵ�
        SetActiveObjects(chatWindow, true);

        // ���� ȭ�� �̸� �ؽ�Ʈ ����
        chatWindow.characterNameText.text = dialogs.dialogST[currentDialogIndex].dialogcharacterName;

        // ���� ȭ���� ��� �ؽ�Ʈ ����
        StartCoroutine("OnTypingText");
    }
    #endregion // �Լ�

    #region �ڷ�ƾ
    /** ���� ȭ���� ��縦 �ѱ��ھ� Ÿ������ �Ѵ� */
    private IEnumerator OnTypingText()
    {
        int index = 0;

        isTypingEffect = true;

        // �ؽ�Ʈ�� �ѱ��ھ� Ÿ����ġ�� ���
        while (index < dialogs.dialogST[currentDialogIndex].dialogue.Length)
        {
            // ���� ȭ���� ��縦 �ѱ��ھ� Ÿ����
            chatWindow.DialogueText.text = dialogs.dialogST[currentDialogIndex].dialogue.Substring(0, index + 1);

            index++;

            // Ÿ���� �ӵ�
            yield return new WaitForSeconds(typingSpeed);
        }

        isTypingEffect = false;

        // ��簡 �Ϸ�Ǿ��� �� ��µǴ� Ŀ�� Ȱ��ȭ
        chatWindow.objectArrow.SetActive(true);
    }
    #endregion // �ڷ�ƾ
}
