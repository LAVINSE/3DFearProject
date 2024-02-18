using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public enum EPlayerStateType
    {
        Movement,
        Inventory,
    }

    #region ����
    public BaseState[] stateArray;
    public BaseState currentState;

    private PlayerKeyCode playerKeyCode;
    private PlayerAction playerAction;
    #endregion // ����

    #region �Լ�
    /** �ʱ�ȭ */
    private void Awake()
    {
        playerKeyCode = this.GetComponent<PlayerKeyCode>();
        playerAction = this.GetComponent<PlayerAction>();

        stateArray = new BaseState[10];

        stateArray[(int)EPlayerStateType.Movement] = new PlayerMovementState(this, playerKeyCode);
        stateArray[(int)EPlayerStateType.Inventory] = new PlayerInventoryState(this, playerAction);
    }

    /** �ʱ�ȭ */
    private void Start()
    {
        currentState = stateArray[(int)EPlayerStateType.Movement];
        currentState.StateEnter();
    }

    /** �ʱ�ȭ => ���¸� �����Ѵ� */
    private void Update()
    {
        // ���°� ������ ���
        if (currentState != null)
        {
            currentState.StateUpdate();
        }
    }

    /** ���� ���� */
    public void ChangeState(EPlayerStateType changeType)
    {
        if (stateArray[(int)changeType] == null) { return; }

        if (currentState != null)
        {
            currentState.StateExit();
        }

        currentState = stateArray[(int)changeType];
        currentState.StateEnter();
    }
    #endregion // �Լ�
}
