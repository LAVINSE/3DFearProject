using System;
using System.Collections;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    #region ����
    [SerializeField] private Canvas canvas; // ĵ����
    [SerializeField] private GameObject chatWindowPrefab; // �ؽ�Ʈ ������
    [SerializeField] private DialogDataSO dialogDataSo; // ���� �б��� ��� ��� �迭 ��ũ��Ʈ������Ʈ
    [SerializeField] private float typingSpeed = 0.5f; // �ؽ�Ʈ Ÿ���� ȿ���� ��� �ӵ�
    [SerializeField] private bool isAutoStart = true; // �ڵ� ���� ����

    private ChatWindow chatWindow;

    private bool isFirst = true; // ���� 1ȸ�� ȣ���ϱ� ���� ����
    private bool isTypingEffect = false; // �ؽ�Ʈ Ÿ���� ȿ���� ���������

    private int currentDialogIndex = -1; // ���� ��� ����, �ʱⰪ -1    
    #endregion // ����

    #region �Լ� 
    /** ��ȭâ ���� �� ���� */
    public void CreateChatWindowSetUp()
    {
        // ��ȭâ�� ���� ��� ����
        if(canvas.GetComponentInChildren<ChatWindow>(true) == null)
        {
            var chatWindowObject = Instantiate(chatWindowPrefab);
            chatWindowObject.transform.SetParent(canvas.transform);
            chatWindowObject.transform.localPosition = Vector3.zero;
            chatWindow = chatWindowObject.GetComponent<ChatWindow>();
        }
        else
        {
            chatWindow = canvas.GetComponentInChildren<ChatWindow>(true);
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
        if(Input.GetMouseButtonDown(1))
        {
            Debug.Log("�Է�");
            // �ؽ�Ʈ Ÿ���� ȿ���� ������϶� ���͸� Ŭ���ϸ� Ÿ���� ȿ�� ����
            if (isTypingEffect == true)
            {
                isTypingEffect = false;

                // Ÿ���� ȿ���� �����ϰ�, ���� ��� ��ü�� ����Ѵ�
                StopCoroutine("OnTypingText");
                chatWindow.DialogueText.text = dialogDataSo.dialogST[currentDialogIndex].dialogue;
                
                // ��簡 �Ϸ�Ǿ��� �� ��µǴ� Ŀ�� Ȱ��ȭ
                chatWindow.objectArrow.SetActive(true);
                
                return false;
            }

            // ��簡 �������� ��� ���� ��� ����
            if(dialogDataSo.dialogST.Length > currentDialogIndex + 1)
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
        chatWindow.characterNameText.text = dialogDataSo.dialogST[currentDialogIndex].dialogcharacterName;

        // ���� ȭ���� ��� �ؽ�Ʈ ����
        StartCoroutine("OnTypingText");
    }
    #endregion // �Լ�

    #region �ڷ�ƾ
    /** ���� ȭ���� ��縦 �ѱ��ھ� Ÿ������ �Ѵ� */
    private IEnumerator OnTypingText()
    {
        isTypingEffect = true;

        int index = 0;

        // �ؽ�Ʈ�� �ѱ��ھ� Ÿ����ġ�� ���
        while (index < dialogDataSo.dialogST[currentDialogIndex].dialogue.Length)
        {
            // ���� ȭ���� ��縦 �ѱ��ھ� Ÿ����
            chatWindow.DialogueText.text = dialogDataSo.dialogST[currentDialogIndex].dialogue.Substring(0, index + 1);

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
