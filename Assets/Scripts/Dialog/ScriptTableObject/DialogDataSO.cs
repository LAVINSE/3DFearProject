using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "DialogDataSO", menuName = "ScriptableObjects/DialogDataSO/DialogData")]
public class DialogDataSO : ScriptableObject
{
    #region ����
    [System.Serializable]
    public struct DialogST
    {
        public int dialogCount; // �迭 ������� �Է�
        public string dialogcharacterName; // ĳ���� �̸�

        [TextArea] public string dialogue; // ���
    }
    
    public DialogST[] dialogST;
    #endregion // ����
}
