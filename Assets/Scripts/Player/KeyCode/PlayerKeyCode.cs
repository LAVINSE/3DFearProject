using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyCode : MonoBehaviour
{
    #region ������Ƽ
    [Header("=====> Ű �Է� <=====")]
    [Tooltip(" ���� Ű ")] public KeyCode JumpKey = KeyCode.Space;
    [Tooltip(" �޸��� Ű ")] public KeyCode SprintKey = KeyCode.LeftShift;
    [Tooltip(" ��ũ���� Ű ")] public KeyCode CrouchKey = KeyCode.LeftControl;
    [Tooltip(" �κ��丮 ������ ȸ�� Ű ")] public KeyCode RotateItemKeyCode = KeyCode.R;
    #endregion // ������Ƽ
}
