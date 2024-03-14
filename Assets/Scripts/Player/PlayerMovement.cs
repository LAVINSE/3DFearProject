using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // 이동 상태
    private enum MovementState
    {
        None,
        walk,
        sprint,
        crouch,
        air,
    }

    #region 변수
    [Header("=====> 애니메이션 <=====")]
    [SerializeField, Tooltip(" 플레이어 모델 ")] private GameObject playerModel;
    [SerializeField, Tooltip(" 애니메이터 ")] private Animator animator;

    [Header("=====> 이동 <=====")]
    [SerializeField] private float moveSpeed = 0;
    [SerializeField, Tooltip(" 걷는 속도 ")] private float walkSpeed;
    [SerializeField, Tooltip(" 달리기 속도 ")] private float sprintSpeed;
    [SerializeField, Tooltip(" 보정 값 ")] private float correctMoveSpeed;
    [SerializeField, Tooltip(" 저항 값 ")] private float baseDrag;
    [SerializeField, Tooltip(" 보정 값 ")] private float correctPlayerHeight;
    [SerializeField, Tooltip(" 나아갈 방향 ")] private Transform orientation;
    [SerializeField, Tooltip(" 바닥 레이어 ")] private LayerMask groundLayer;
    [SerializeField, Tooltip(" 이동 상태 ")] private MovementState movementState;

    [Header("=====> 경사면 설정 <=====")]
    [SerializeField, Tooltip(" 최대 각 ")] private float maxSlopeAngle;
    [SerializeField, Tooltip(" 보정 값 ")] private float correctPlayerSlopeHeight;
    [SerializeField] private bool isExitingSlop;
    [SerializeField] private RaycastHit slopeHit;
    
    [Header("=====> 웅크리기 설정 <=====")]
    [SerializeField, Tooltip(" 웅크리기 속도 ")] private float crouchSpeed;
    [SerializeField, Tooltip(" 웅크리기 Y 값 ")] private float crouchScaleY;
    [SerializeField, Tooltip(" 웅크리기 시작전 Y 값 ")] private float crouchStartScaleY;

    [Header("=====> 점프 설정 <=====")]
    [SerializeField, Tooltip(" 점프 힘 ")] private float jumpPower;
    [SerializeField, Tooltip(" 점프 쿨타임 ")] private float jumpCooldown;

    private bool isJump;
    private bool isGround;

    private float airMultiplier = 0;
    private float horizontalInput;
    private float verticalInput;

    // 방향
    private Vector3 moveDirection;

    private Rigidbody rigid;

    private PlayerKeyCode playerKeyCode;
    private PlayerState playerState;
    #endregion // 변수

    #region 함수
    /** 초기화 */
    private void Awake()
    {
        rigid = this.GetComponent<Rigidbody>();
        playerKeyCode = this.GetComponent<PlayerKeyCode>();
        playerState = this.GetComponent<PlayerState>();
    }

    /** 초기화 */
    private void Start()
    {
        rigid.freezeRotation = true;

        /*
        // 점프 초기화
        PlayerResetJump();
        */

        /*
        // 값 설정
        crouchStartScaleY = this.transform.localScale.y;
        */
    }

    /** 초기화 => 상태를 갱신한다 */
    private void Update()
    {
        if (playerState.currentState != playerState.stateArray[(int)PlayerState.EPlayerStateType.Movement]) { return; }

        // 오브젝트의 높이 절반 + 보정값
        isGround = Physics.Raycast(transform.position, Vector3.down, correctPlayerHeight, groundLayer);

        if (isGround)
        {
            Debug.Log("test");
        }

        playerModel.transform.localRotation = orientation.transform.localRotation;

        // 플레이어 입력처리
        PlayerInput();
        // 플레이어 속도제어
        PlayerSpeedControl();
        // 이동 상태 제어
        MovementStateHandler();
        // 저항 값 제어
        DragAirControl();
    }

    /** 초기화 => 상태를 갱신한다 */
    private void FixedUpdate()
    {
        if (playerState.currentState != playerState.stateArray[(int)PlayerState.EPlayerStateType.Movement]) { return; }

        // 플레이어 이동
        PlayerMove();
    }

    /** 입력처리 */
    private void PlayerInput()
    {
        // 움직임
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        /*
        // 점프
        if(Input.GetKey(playerKeyCode.JumpKey) && isJump && isGround)
        {
            isJump = false;

            // 점프
            PlayerJump();

            // 점프 쿨타임
            Invoke(nameof(PlayerResetJump), jumpCooldown);
        }
        */

        /*
        // 웅크리기 시작
        if (Input.GetKeyDown(playerKeyCode.CrouchKey))
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x, crouchScaleY, this.transform.localScale.z);
            rigid.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        // 웅크리기 해제
        if (Input.GetKeyUp(playerKeyCode.CrouchKey))
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x, crouchStartScaleY, this.transform.localScale.z);
            movementState = MovementState.None;
        }
        */
    }

    /** 이동 상태 제어 */
    private void MovementStateHandler()
    {
        // 달리기
        if (isGround && Input.GetKey(playerKeyCode.SprintKey))
        {
            movementState = MovementState.sprint;
            moveSpeed = sprintSpeed;

            // 플레이어 이동 애니메이션
            PlayerMoveAnimation();
        }
        // 걷기
        else if (isGround && movementState != MovementState.crouch)
        {
            movementState = MovementState.walk;
            moveSpeed = walkSpeed;

            // 플레이어 이동 애니메이션
            PlayerMoveAnimation();
        }

        /*
        // 공중
        else
        {
            movementState = MovementState.air;
        }
        */

        /*
        // 웅크리기
        if (Input.GetKey(playerKeyCode.CrouchKey))
        {
            movementState = MovementState.crouch;
            moveSpeed = crouchSpeed;
        }
        */
    }

    /** 플레이어 이동 */
    private void PlayerMove()
    {
        // 이동방향 계산
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // 경사면 일 경우
        if (OnSlope() && !isExitingSlop)
        {
            rigid.AddForce(GetSlopMoveDirection() * moveSpeed * correctMoveSpeed * airMultiplier, ForceMode.Force);
        }
        else
        {
            // 이동
            rigid.AddForce(moveDirection.normalized * moveSpeed * correctMoveSpeed * airMultiplier, ForceMode.Force);
        }
    }

    /** 플레이어 이동 애니메이션 */
    private void PlayerMoveAnimation()
    {
        if (rigid.velocity.magnitude * verticalInput != 0 || rigid.velocity.magnitude * horizontalInput != 0)
        {
            animator.SetBool("isWalk", true);
        }
        else
        {
            animator.SetBool("isWalk", false);
        }
    }

    /** 플레이어 속도제어 */
    private void PlayerSpeedControl()
    {
        // 경사면 일 경우
        if (OnSlope() && !isExitingSlop)
        {
            if(rigid.velocity.magnitude > moveSpeed)
            {
                rigid.velocity = rigid.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            // 속도
            Vector3 currentVelocity = new Vector3(rigid.velocity.x, 0f, rigid.velocity.z);

            // 현재 속도가 이동속도보다 클 경우
            if (currentVelocity.magnitude > moveSpeed)
            {
                // 같은 방향 moveSpeed로 크기 제한
                Vector3 limitVelocity = currentVelocity.normalized * moveSpeed;
                rigid.velocity = new Vector3(limitVelocity.x, rigid.velocity.y, limitVelocity.z);
            }
        }
    }

    /** 플레이어 점프 */
    private void PlayerJump()
    {
        isExitingSlop = true;

        rigid.velocity = new Vector3(rigid.velocity.x, 0f, rigid.velocity.z);

        rigid.AddForce(transform.up * jumpPower, ForceMode.Impulse);
    }

    /** 점프 초기화 */
    private void PlayerResetJump()
    {
        isJump = true;
        isExitingSlop = false;
    }

    /** 저항 값 제어 */
    private void DragAirControl()
    {
        // 공기저항
        rigid.drag = isGround ? baseDrag : 1;

        // 점프했을때 안했을때 움직이는 속도 조정
        airMultiplier = isGround ? 1 : (1 / (baseDrag + 1));
    }

    /** 경사면인지 확인 */
    private bool OnSlope()
    {
        if (Physics.Raycast(this.transform.position, Vector3.down, out slopeHit, correctPlayerSlopeHeight))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    /** 경사면에서 이동방향 가져오기 */
    private Vector3 GetSlopMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }
    #endregion  // 함수
}
