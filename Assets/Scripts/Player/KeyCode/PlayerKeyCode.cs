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

    [Header("=====> 테스트 키 입력 <=====")]
    [Tooltip(" 테스트 아이템 생성 키 ")] public KeyCode testCreateItemKey = KeyCode.Q;
    [Tooltip(" 테스트 아이템 추가 키 ")] public KeyCode testAddItemKey = KeyCode.W;
    #endregion // 변수
}
