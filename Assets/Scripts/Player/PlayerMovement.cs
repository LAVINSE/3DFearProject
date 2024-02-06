using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // 이동 상태
    private enum MovementState
    {
        walk,
        sprint,
        crouch,
        air,
    }

    #region 변수
    [Header("=====> 이동 <=====")]
    [SerializeField, Tooltip(" 걷는 속도 ")] private float walkSpeed;
    [SerializeField, Tooltip(" 달리기 속도 ")] private float sprintSpeed;
    [SerializeField, Tooltip(" 보정 값 ")] private float correctMoveSpeed;
    [SerializeField, Tooltip(" 저항 값 ")] private float baseDrag;
    [SerializeField, Tooltip(" 나아갈 방향 ")] private Transform orientation;
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
    [SerializeField, Tooltip(" 플레이어 객체 높이 ")] private float playerHeight;
    [SerializeField, Tooltip(" 보정 값 ")] private float correctPlayerJumpHeight;
    [SerializeField, Tooltip(" 바닥 레이어 ")] private LayerMask groundLayer;

    [Header("=====> 키 입력 <=====")]
    [SerializeField, Tooltip(" 점프 키 ")] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField, Tooltip(" 달리기 키 ")] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField, Tooltip(" 웅크리기 키 ")] private KeyCode crouchKey = KeyCode.LeftControl;

    [Header("=====> 인스펙터 값 확인 <=====")]
    [SerializeField] private float airMultiplier;
    [SerializeField] private float moveSpeed;

    private bool isJump;
    private bool isGround;

    private float horizontalInput;
    private float verticalInput;

    // 방향
    private Vector3 moveDirection;

    private Rigidbody rigid;
    #endregion // 변수

    #region 함수
    /** 초기화 */
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();   
    }

    /** 초기화 */
    private void Start()
    {
        rigid.freezeRotation = true;

        // 점프 초기화
        PlayerResetJump();

        // 값 설정
        crouchStartScaleY = this.transform.localScale.y;
    }

    /** 초기화 => 상태를 갱신한다 */
    private void Update()
    {
        // 오브젝트의 높이 절반 + 보정값
        isGround = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + correctPlayerJumpHeight, groundLayer);

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
        // 플레이어 이동
        PlayerMove();
    }

    /** 입력처리 */
    private void PlayerInput()
    {
        // 움직임
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        // 점프
        if(Input.GetKey(jumpKey) && isJump && isGround)
        {
            isJump = false;

            // 점프
            PlayerJump();

            // 점프 쿨타임
            Invoke(nameof(PlayerResetJump), jumpCooldown);
        }

        // 웅크리기 시작
        if (Input.GetKeyDown(crouchKey))
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x, crouchScaleY, this.transform.localScale.z);
            rigid.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        // 웅크리기 해제
        if (Input.GetKeyUp(crouchKey))
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x, crouchStartScaleY, this.transform.localScale.z);
        }
    }

    /** 이동 상태 제어 */
    private void MovementStateHandler()
    {
        // 웅크리기
        if (Input.GetKey(crouchKey))
        {
            movementState = MovementState.crouch;
            moveSpeed = crouchSpeed;
        }

        // 달리기
        if(isGround && Input.GetKey(sprintKey))
        {

            movementState = MovementState.sprint;
            moveSpeed = sprintSpeed;
        }
        // 걷기
        else if (isGround)
        {
            movementState = MovementState.walk;
            moveSpeed = walkSpeed;
        }
        // 공중
        else
        {
            movementState = MovementState.air;
        }
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
        // 
        if (Physics.Raycast(this.transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + correctPlayerSlopeHeight))
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
