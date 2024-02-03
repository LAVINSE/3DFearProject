using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // �̵� ����
    private enum MovementState
    {
        walk,
        sprint,
        crouch,
        air,
    }

    #region ����
    [Header("=====> �̵� <=====")]
    [SerializeField, Tooltip(" �ȴ� �ӵ� ")] private float walkSpeed;
    [SerializeField, Tooltip(" �޸��� �ӵ� ")] private float sprintSpeed;
    [SerializeField, Tooltip(" ���� �� ")] private float correctMoveSpeed;
    [SerializeField, Tooltip(" ���� �� ")] private float baseDrag;
    [SerializeField, Tooltip(" ���ư� ���� ")] private Transform orientation;
    [SerializeField, Tooltip(" �̵� ���� ")] private MovementState movementState;

    [Header("=====> ��ũ���� ���� <=====")]
    [SerializeField, Tooltip(" ��ũ���� �ӵ� ")] private float crouchSpeed;
    [SerializeField, Tooltip(" ��ũ���� Y �� ")] private float crouchScaleY;
    [SerializeField, Tooltip(" ��ũ���� ������ Y �� ")] private float crouchStartScaleY;

    [Header("=====> ���� ���� <=====")]
    [SerializeField, Tooltip(" ���� �� ")] private float jumpPower;
    [SerializeField, Tooltip(" ���� ��Ÿ�� ")] private float jumpCooldown;
    [SerializeField, Tooltip(" �÷��̾� ��ü ���� ")] private float playerHeight;
    [SerializeField, Tooltip(" �÷��̾� ��ü ���� ���� �� ")] private float correctPlayerHeight;
    [SerializeField, Tooltip(" �ٴ� ���̾� ")] private LayerMask groundLayer;

    [Header("=====> Ű �Է� <=====")]
    [SerializeField, Tooltip(" ���� Ű ")] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField, Tooltip(" �޸��� Ű ")] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField, Tooltip(" ��ũ���� Ű ")] private KeyCode crouchKey = KeyCode.LeftControl;

    [Header("=====> �ν����� �� Ȯ�� <=====")]
    [SerializeField] private float airMultiplier;
    [SerializeField] private float moveSpeed;

    private bool isJump;
    private bool isGround;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;

    private Rigidbody rigid;
    #endregion // ����

    #region �Լ�
    /** �ʱ�ȭ */
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();   
    }

    /** �ʱ�ȭ */
    private void Start()
    {
        rigid.freezeRotation = true;

        // ���� �ʱ�ȭ
        PlayerResetJump();

        // �� ����
        crouchStartScaleY = this.transform.localScale.y;
    }

    /** �ʱ�ȭ => ���¸� �����Ѵ� */
    private void Update()
    {
        // ������Ʈ�� ���� ���� + ������
        isGround = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + correctPlayerHeight, groundLayer);

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
        // �÷��̾� �̵�
        PlayerMove();
    }

    /** �Է�ó�� */
    private void PlayerInput()
    {
        // ������
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        // ����
        if(Input.GetKey(jumpKey) && isJump && isGround)
        {
            isJump = false;

            // ����
            PlayerJump();

            // ���� ��Ÿ��
            Invoke(nameof(PlayerResetJump), jumpCooldown);
        }

        // ��ũ���� ����
        if (Input.GetKeyDown(crouchKey))
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x, crouchScaleY, this.transform.localScale.z);
            rigid.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        // ��ũ���� ����
        if (Input.GetKeyUp(crouchKey))
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x, crouchStartScaleY, this.transform.localScale.z);
        }
    }

    /** �̵� ���� ���� */
    private void MovementStateHandler()
    {
        // ��ũ����
        if (Input.GetKey(crouchKey))
        {
            movementState = MovementState.crouch;
            moveSpeed = crouchSpeed;
        }

        // �޸���
        if(isGround && Input.GetKey(sprintKey))
        {

            movementState = MovementState.sprint;
            moveSpeed = sprintSpeed;
        }
        // �ȱ�
        else if (isGround)
        {
            movementState = MovementState.walk;
            moveSpeed = walkSpeed;
        }
        // ����
        else
        {
            movementState = MovementState.air;

        }
    }

    /** �÷��̾� �̵� */
    private void PlayerMove()
    {
        // �̵����� ���
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // �̵�
        rigid.AddForce(moveDirection.normalized * moveSpeed * correctMoveSpeed * airMultiplier, ForceMode.Force);
    }

    /** �÷��̾� �ӵ����� */
    private void PlayerSpeedControl()
    {
        // �ӵ�
        Vector3 currentVelocity = new Vector3(rigid.velocity.x, 0f, rigid.velocity.z);

        // ���� �ӵ��� �̵��ӵ����� Ŭ ���
        if(currentVelocity.magnitude > moveSpeed)
        {
            // ���� ���� moveSpeed�� ũ�� ����
            Vector3 limitVelocity = currentVelocity.normalized * moveSpeed;
            rigid.velocity = new Vector3(limitVelocity.x, rigid.velocity.y, limitVelocity.z);
        }
    }

    /** �÷��̾� ���� */
    private void PlayerJump()
    {
        rigid.velocity = new Vector3(rigid.velocity.x, 0f, rigid.velocity.z);

        rigid.AddForce(transform.up * jumpPower, ForceMode.Impulse);
    }

    /** ���� �ʱ�ȭ */
    private void PlayerResetJump()
    {
        isJump = true;
    }

    /** ���� �� ���� */
    private void DragAirControl()
    {
        // ��������
        rigid.drag = isGround ? baseDrag : 1;

        // ���������� �������� �����̴� �ӵ� ����
        airMultiplier = isGround ? 1 : (1 / (baseDrag + 1));
    }
    #endregion  // �Լ�
}
