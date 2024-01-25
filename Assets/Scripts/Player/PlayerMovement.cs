using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region ����
    [Header("=====> �̵� <=====")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float correctMoveSpeed;
    [SerializeField] private float baseDrag;
    [SerializeField] private Transform orientation; 

    [Header("=====> ���� ���� <=====")]
    [SerializeField] private float jumpPower;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float playerHeight;
    [SerializeField] private float correctPlayerHeight;
    [SerializeField] private LayerMask groundLayer;

    [Header("=====> Ű �Է� <=====")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    [Header("=====> �ν����� �� Ȯ�� <=====")]
    [SerializeField] private float airMultiplier;

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
        rigid.freezeRotation = true;

        // ���� �ʱ�ȭ
        PlayerResetJump();
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
