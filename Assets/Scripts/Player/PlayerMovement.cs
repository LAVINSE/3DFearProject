using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region 변수
    [Header("=====> 이동 <=====")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float correctMoveSpeed;
    [SerializeField] private float baseDrag;
    [SerializeField] private Transform orientation; 

    [Header("=====> 점프 설정 <=====")]
    [SerializeField] private float jumpPower;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float playerHeight;
    [SerializeField] private float correctPlayerHeight;
    [SerializeField] private LayerMask groundLayer;

    [Header("=====> 키 입력 <=====")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    [Header("=====> 인스펙터 값 확인 <=====")]
    [SerializeField] private float airMultiplier;

    private bool isJump;
    private bool isGround;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;

    private Rigidbody rigid;
    #endregion // 변수

    #region 함수
    /** 초기화 */
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.freezeRotation = true;

        // 점프 초기화
        PlayerResetJump();
    }

    /** 초기화 => 상태를 갱신한다 */
    private void Update()
    {
        // 오브젝트의 높이 절반 + 보정값
        isGround = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + correctPlayerHeight, groundLayer);

        // 플레이어 입력처리
        PlayerInput();
        // 플레이어 속도제어
        PlayerSpeedControl();
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
    }

    /** 플레이어 이동 */
    private void PlayerMove()
    {
        // 이동방향 계산
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // 이동
        rigid.AddForce(moveDirection.normalized * moveSpeed * correctMoveSpeed * airMultiplier, ForceMode.Force);
    }

    /** 플레이어 속도제어 */
    private void PlayerSpeedControl()
    {
        // 속도
        Vector3 currentVelocity = new Vector3(rigid.velocity.x, 0f, rigid.velocity.z);

        // 현재 속도가 이동속도보다 클 경우
        if(currentVelocity.magnitude > moveSpeed)
        {
            // 같은 방향 moveSpeed로 크기 제한
            Vector3 limitVelocity = currentVelocity.normalized * moveSpeed;
            rigid.velocity = new Vector3(limitVelocity.x, rigid.velocity.y, limitVelocity.z);
        }
    }

    /** 플레이어 점프 */
    private void PlayerJump()
    {
        rigid.velocity = new Vector3(rigid.velocity.x, 0f, rigid.velocity.z);

        rigid.AddForce(transform.up * jumpPower, ForceMode.Impulse);
    }

    /** 점프 초기화 */
    private void PlayerResetJump()
    {
        isJump = true;
    }

    /** 저항 값 제어 */
    private void DragAirControl()
    {
        // 공기저항
        rigid.drag = isGround ? baseDrag : 1;

        // 점프했을때 안했을때 움직이는 속도 조정
        airMultiplier = isGround ? 1 : (1 / (baseDrag + 1));
    }
    #endregion  // 함수
}
