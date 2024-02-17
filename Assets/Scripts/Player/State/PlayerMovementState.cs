using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementState : BaseState
{
    #region ����
    private PlayerState playerState;
    private PlayerKeyCode playerKeyCode;
    #endregion // ����

    #region ������
    public PlayerMovementState(PlayerState playerState, PlayerKeyCode playerKeyCode)
    {
        this.playerState = playerState;
        this.playerKeyCode = playerKeyCode;
    }
    #endregion // ������

    /** ���� ���� */
    public override void StateEnter()
    {
        playerState.GetComponent<PlayerMovement>().enabled = true;
        Debug.Log(" ���� ");
    }

    /** ���� ���� */
    public override void StateExit()
    {
        playerState.GetComponent<PlayerMovement>().enabled = false;
    }

    /** ���� ������Ʈ */
    public override void StateUpdate()
    {
        if (Input.GetKeyDown(playerKeyCode.JumpKey))
        {
            playerState.ChangeState(PlayerState.EPlayerStateType.None);
        }
    }
}
