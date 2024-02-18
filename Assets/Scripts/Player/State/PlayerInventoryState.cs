using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryState : BaseState
{
    #region 변수
    private PlayerState playerState;
    private PlayerAction playerAction;
    #endregion // 변수

    #region 생성자
    public PlayerInventoryState(PlayerState playerState, PlayerAction playerAction) 
    { 
        this.playerState = playerState;
        this.playerAction = playerAction;
    }
    #endregion // 생성자

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
