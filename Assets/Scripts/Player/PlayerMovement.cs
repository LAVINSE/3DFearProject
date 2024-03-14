using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // �̵� ����
    private enum MovementState
    {
        None,
        walk,
        sprint,
        crouch,
        air,
    }

    #region ����
    [Header("=====> �ִϸ��̼� <=====")]
    [SerializeField, Tooltip(" �÷��̾� �� ")] private GameObject playerModel;
    [SerializeField, Tooltip(" �ִϸ����� ")] private Animator animator;

    [Header("=====> �̵� <=====")]
    [SerializeField] private float moveSpeed = 0;
    [SerializeField, Tooltip(" �ȴ� �ӵ� ")] private float walkSpeed;
    [SerializeField, Tooltip(" �޸��� �ӵ� ")] private float sprintSpeed;
    [SerializeField, Tooltip(" ���� �� ")] private float correctMoveSpeed;
    [SerializeField, Tooltip(" ���� �� ")] private float baseDrag;
    [SerializeField, Tooltip(" ���� �� ")] private float correctPlayerHeight;
    [SerializeField, Tooltip(" ���ư� ���� ")] private Transform orientation;
    [SerializeField, Tooltip(" �ٴ� ���̾� ")] private LayerMask groundLayer;
    [SerializeField, Tooltip(" �̵� ���� ")] private MovementState movementState;

    [Header("=====> ���� ���� <=====")]
    [SerializeField, Tooltip(" �ִ� �� ")] private float maxSlopeAngle;
    [SerializeField, Tooltip(" ���� �� ")] private float correctPlayerSlopeHeight;
    [SerializeField] private bool isExitingSlop;
    [SerializeField] private RaycastHit slopeHit;
    
    [Header("=====> ��ũ���� ���� <=====")]
    [SerializeField, Tooltip(" ��ũ���� �ӵ� ")] private float crouchSpeed;
    [SerializeField, Tooltip(" ��ũ���� Y �� ")] private float crouchScaleY;
    [SerializeField, Tooltip(" ��ũ���� ������ Y �� ")] private float crouchStartScaleY;

    [Header("=====> ���� ���� <=====")]
    [SerializeField, Tooltip(" ���� �� ")] private float jumpPower;
    [SerializeField, Tooltip(" ���� ��Ÿ�� ")] private float jumpCooldown;

    private bool isJump;
    private bool isGround;

    private float airMultiplier = 0;
    private float horizontalInput;
    private float verticalInput;

    // ����
    private Vector3 moveDirection;

    private Rigidbody rigid;

    private PlayerKeyCode playerKeyCode;
    private PlayerState playerState;
    #endregion // ����

    #region �Լ�
    /** �ʱ�ȭ */
    private void Awake()
    {
        rigid = this.GetComponent<Rigidbody>();
        playerKeyCode = this.GetComponent<PlayerKeyCode>();
        playerState = this.GetComponent<PlayerState>();
    }

    /** �ʱ�ȭ */
    private void Start()
    {
        rigid.freezeRotation = true;

        /*
        // ���� �ʱ�ȭ
        PlayerResetJump();
        */

        /*
        // �� ����
        crouchStartScaleY = this.transform.localScale.y;
        */
    }

    /** �ʱ�ȭ => ���¸� �����Ѵ� */
    private void Update()
    {
        if (playerState.currentState != playerState.stateArray[(int)PlayerState.EPlayerStateType.Movement]) { return; }

        // ������Ʈ�� ���� ���� + ������
        isGround = Physics.Raycast(transform.position, Vector3.down, correctPlayerHeight, groundLayer);

        if (isGround)
        {
            Debug.Log("test");
        }

        playerModel.transform.localRotation = orientation.transform.localRotation;

        // �÷��̾� �Է�ó��
        PlayerInput();
        // �÷��̾� �ӵ�����
        PlayerSpeedControl();
        // �̵� ���� ����
        MovementStateHandler();
        // ���� �� ����
        DragAirControl();
    }

    /** �ʱ�ȭ => ���¸� �����Ѵ� */
    private void FixedUpdate()
    {
        if (playerState.currentState != playerState.stateArray[(int)PlayerState.EPlayerStateType.Movement]) { return; }

        // �÷��̾� �̵�
        PlayerMove();
    }

    /** �Է�ó�� */
    private void PlayerInput()
    {
        // ������
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        /*
        // ����
        if(Input.GetKey(playerKeyCode.JumpKey) && isJump && isGround)
        {
            isJump = false;

            // ����
            PlayerJump();

            // ���� ��Ÿ��
            Invoke(nameof(PlayerResetJump), jumpCooldown);
        }
        */

        /*
        // ��ũ���� ����
        if (Input.GetKeyDown(playerKeyCode.CrouchKey))
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x, crouchScaleY, this.transform.localScale.z);
            rigid.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        // ��ũ���� ����
        if (Input.GetKeyUp(playerKeyCode.CrouchKey))
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x, crouchStartScaleY, this.transform.localScale.z);
            movementState = MovementState.None;
        }
        */
    }

    /** �̵� ���� ���� */
    private void MovementStateHandler()
    {
        // �޸���
        if (isGround && Input.GetKey(playerKeyCode.SprintKey))
        {
            movementState = MovementState.sprint;
            moveSpeed = sprintSpeed;

            // �÷��̾� �̵� �ִϸ��̼�
            PlayerMoveAnimation();
        }
        // �ȱ�
        else if (isGround && movementState != MovementState.crouch)
        {
            movementState = MovementState.walk;
            moveSpeed = walkSpeed;

            // �÷��̾� �̵� �ִϸ��̼�
            PlayerMoveAnimation();
        }

        /*
        // ����
        else
        {
            movementState = MovementState.air;
        }
        */

        /*
        // ��ũ����
        if (Input.GetKey(playerKeyCode.CrouchKey))
        {
            movementState = MovementState.crouch;
            moveSpeed = crouchSpeed;
        }
        */
    }

    /** �÷��̾� �̵� */
    private void PlayerMove()
    {
        // �̵����� ���
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // ���� �� ���
        if (OnSlope() && !isExitingSlop)
        {
            rigid.AddForce(GetSlopMoveDirection() * moveSpeed * correctMoveSpeed * airMultiplier, ForceMode.Force);
        }
        else
        {
            // �̵�
            rigid.AddForce(moveDirection.normalized * moveSpeed * correctMoveSpeed * airMultiplier, ForceMode.Force);
        }
    }

    /** �÷��̾� �̵� �ִϸ��̼� */
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

    /** �÷��̾� �ӵ����� */
    private void PlayerSpeedControl()
    {
        // ���� �� ���
        if (OnSlope() && !isExitingSlop)
        {
            if(rigid.velocity.magnitude > moveSpeed)
            {
                rigid.velocity = rigid.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            // �ӵ�
            Vector3 currentVelocity = new Vector3(rigid.velocity.x, 0f, rigid.velocity.z);

            // ���� �ӵ��� �̵��ӵ����� Ŭ ���
            if (currentVelocity.magnitude > moveSpeed)
            {
                // ���� ���� moveSpeed�� ũ�� ����
                Vector3 limitVelocity = currentVelocity.normalized * moveSpeed;
                rigid.velocity = new Vector3(limitVelocity.x, rigid.velocity.y, limitVelocity.z);
            }
        }
    }

    /** �÷��̾� ���� */
    private void PlayerJump()
    {
        isExitingSlop = true;

        rigid.velocity = new Vector3(rigid.velocity.x, 0f, rigid.velocity.z);

        rigid.AddForce(transform.up * jumpPower, ForceMode.Impulse);
    }

    /** ���� �ʱ�ȭ */
    private void PlayerResetJump()
    {
        isJump = true;
        isExitingSlop = false;
    }

    /** ���� �� ���� */
    private void DragAirControl()
    {
        // ��������
        rigid.drag = isGround ? baseDrag : 1;

        // ���������� �������� �����̴� �ӵ� ����
        airMultiplier = isGround ? 1 : (1 / (baseDrag + 1));
    }

    /** �������� Ȯ�� */
    private bool OnSlope()
    {
        if (Physics.Raycast(this.transform.position, Vector3.down, out slopeHit, correctPlayerSlopeHeight))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    /** ���鿡�� �̵����� �������� */
    private Vector3 GetSlopMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }
    #endregion  // �Լ�
}
