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

    public override void StateEnter()
    {
        
    }

    public override void StateExit()
    {
        
    }

    public override void StateUpdate()
    {
        if(playerAction.InventoryObj.activeSelf == false)
        {
            playerState.ChangeState(PlayerState.EPlayerStateType.Movement);
        }
    }
}
