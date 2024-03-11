using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyCode : MonoBehaviour
{
    #region 변수
    [Header("=====> 키 입력 <=====")]
    [Tooltip(" 점프 키 ")] public KeyCode JumpKey = KeyCode.Space;
    [Tooltip(" 달리기 키 ")] public KeyCode SprintKey = KeyCode.LeftShift;
    [Tooltip(" 웅크리기 키 ")] public KeyCode CrouchKey = KeyCode.LeftControl;
    [Tooltip(" 인벤토리 키 ")] public KeyCode InventoryKey = KeyCode.I;
    [Tooltip(" 인벤토리 아이템 회전 키 ")] public KeyCode RotateItemKeyCode = KeyCode.R;
    [Tooltip(" 손전등 키 ")] public KeyCode FlashLightKeyCode = KeyCode.X;
    [Tooltip(" 아이템 획득 키 ")] public KeyCode PickupItemKeyCode = KeyCode.E;
    #endregion // 변수
}
