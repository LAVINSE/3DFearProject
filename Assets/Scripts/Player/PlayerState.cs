using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public enum EPlayerStateType
    {
        None,
        Movement,
        Inventory,
    }

    #region 변수
    public BaseState[] stateArray;
    public BaseState currentState;

    private PlayerKeyCode playerKeyCode;
    #endregion // 변수

    #region 함수
    /** 초기화 */
    private void Awake()
    {
        playerKeyCode = this.GetComponent<PlayerKeyCode>();

        stateArray = new BaseState[10];

        stateArray[(int)EPlayerStateType.None] = new PlayerNoneState(this, playerKeyCode);
        stateArray[(int)EPlayerStateType.Movement] = new PlayerMovementState(this, playerKeyCode);
    }

    /** 초기화 */
    private void Start()
    {
        currentState = stateArray[(int)EPlayerStateType.Movement];
        currentState.StateEnter();
    }

    /** 초기화 => 상태를 갱신한다 */
    private void Update()
    {
        // 상태가 존재할 경우
        if (currentState != null)
        {
            currentState.StateUpdate();
        }
    }

    /** 상태 변경 */
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
    #endregion // 함수
}
