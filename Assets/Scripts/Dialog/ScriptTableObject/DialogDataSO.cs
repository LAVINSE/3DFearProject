using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "DialogDataSO", menuName = "ScriptableObjects/DialogDataSO/DialogData")]
public class DialogDataSO : ScriptableObject
{
    #region 변수
    [System.Serializable]
    public struct DialogST
    {
        public int dialogCount; // 배열 순서대로 입력
        public string dialogcharacterName; // 캐릭터 이름

        [TextArea] public string dialogue; // 대사
    }
    
    public DialogST[] dialogST;
    #endregion // 변수
}
