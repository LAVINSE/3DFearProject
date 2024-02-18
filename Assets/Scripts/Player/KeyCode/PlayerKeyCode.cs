using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyCode : MonoBehaviour
{
    #region ����
    [Header("=====> Ű �Է� <=====")]
    [Tooltip(" ���� Ű ")] public KeyCode JumpKey = KeyCode.Space;
    [Tooltip(" �޸��� Ű ")] public KeyCode SprintKey = KeyCode.LeftShift;
    [Tooltip(" ��ũ���� Ű ")] public KeyCode CrouchKey = KeyCode.LeftControl;
    [Tooltip(" �κ��丮 Ű ")] public KeyCode InventoryKey = KeyCode.I;
    [Tooltip(" �κ��丮 ������ ȸ�� Ű ")] public KeyCode RotateItemKeyCode = KeyCode.R;

    [Header("=====> �׽�Ʈ Ű �Է� <=====")]
    [Tooltip(" �׽�Ʈ ������ ���� Ű ")] public KeyCode testCreateItemKey = KeyCode.Q;
    [Tooltip(" �׽�Ʈ ������ �߰� Ű ")] public KeyCode testAddItemKey = KeyCode.W;
    #endregion // ����
}
