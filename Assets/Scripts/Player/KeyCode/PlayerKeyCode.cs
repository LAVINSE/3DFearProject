using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyCode : MonoBehaviour
{
    #region 프로퍼티
    [Header("=====> 키 입력 <=====")]
    [Tooltip(" 점프 키 ")] public KeyCode JumpKey = KeyCode.Space;
    [Tooltip(" 달리기 키 ")] public KeyCode SprintKey = KeyCode.LeftShift;
    [Tooltip(" 웅크리기 키 ")] public KeyCode CrouchKey = KeyCode.LeftControl;
    [Tooltip(" 인벤토리 아이템 회전 키 ")] public KeyCode RotateItemKeyCode = KeyCode.R;
    #endregion // 프로퍼티
}
