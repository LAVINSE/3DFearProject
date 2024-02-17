using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNoneState : BaseState
{
    #region ����
    private PlayerState playerState;
    private PlayerKeyCode playerKeyCode;
    #endregion // ����

    #region ������
    public PlayerNoneState(PlayerState playerState, PlayerKeyCode playerKeyCode)
    { 
        this.playerState = playerState;
        this.playerKeyCode = playerKeyCode;
    }
    #endregion // ������

    /** ���� ���� */
    public override void StateEnter()
    {
        Debug.Log(" none ");
    }

    /** ���� ���� */
    public override void StateExit()
    {
        
    }

    /** ���� ������Ʈ */
    public override void StateUpdate()
    {
        
    }
}
