using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementState : BaseState
{
    #region 변수
    private PlayerState playerState;
    private PlayerKeyCode playerKeyCode;
    #endregion // 변수

    #region 생성자
    public PlayerMovementState(PlayerState playerState, PlayerKeyCode playerKeyCode)
    {
        this.playerState = playerState;
        this.playerKeyCode = playerKeyCode;
    }
    #endregion // 생성자

    /** 상태 시작 */
    public override void StateEnter()
    {
        
        Debug.Log(" 진입 ");
    }

    /** 상태 종료 */
    public override void StateExit()
    {
        
    }

    /** 상태 업데이트 */
    public override void StateUpdate()
    {
        
    }
}
