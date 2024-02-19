using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryState : BaseState
{
    #region ����
    private PlayerState playerState;
    private PlayerAction playerAction;
    #endregion // ����

    #region ������
    public PlayerInventoryState(PlayerState playerState, PlayerAction playerAction) 
    { 
        this.playerState = playerState;
        this.playerAction = playerAction;
    }
    #endregion // ������

    /** ���� ���� */
    public override void StateEnter()
    {
        PlayerCamera.isRotateCamera = false;
    }

    /** ���� ���� */
    public override void StateExit()
    {
        PlayerCamera.isRotateCamera = true;
    }

    /** ���� ������Ʈ */
    public override void StateUpdate()
    {
        if(playerAction.InventoryObj.activeSelf == false)
        {
            playerState.ChangeState(PlayerState.EPlayerStateType.Movement);
        }
    }
}
