using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogTest : MonoBehaviour
{
    #region ����
    [SerializeField] private List<DialogSystem> dislogSystemList = new List<DialogSystem>();
    #endregion // ����

    #region �Լ�
    private IEnumerator Start()
    {
        foreach (var dialogSystem in dislogSystemList)
        {
            dialogSystem.CreateChatWindowSetUp();
            // ��� �б� ����
            yield return new WaitUntil(() => dialogSystem.UpdateDialog());

            // ���⿡ ���бⰡ �Ѿ�� �� �ൿ �߰�����
            Debug.Log(" ���� ��� �б� ������ ���� ");
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();

        foreach (var dialogSystem in dislogSystemList)
        {
            dialogSystem.DisableWindow();
        }   
    }
    #endregion // �Լ�
}
