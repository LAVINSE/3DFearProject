using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    #region ����
    [SerializeField] private GameObject inventoryUIPrefab;

    private PlayerKeyCode playerKeyCode;

    private Transform canvasTransform;
    #endregion // ����

    #region ������Ƽ
    public GameObject InventoryObj { get; set; }
    #endregion // ������Ƽ

    #region �Լ�
    /** �ʱ�ȭ */
    private void Awake()
    {
        playerKeyCode = this.GetComponent<PlayerKeyCode>();

        canvasTransform = GameObject.FindWithTag("Canvas").transform;
    }

    /** �ʱ�ȭ => ���¸� �����Ѵ� */
    private void Update()
    {
        // �Է�ó��
        PlayerActionInput();
    }

    /** �Է�ó�� */
    private void PlayerActionInput()
    {
        if (Input.GetKeyDown(playerKeyCode.InventoryKey))
        {
            if (InventoryObj == null)
            {
                InventoryObj = Instantiate(inventoryUIPrefab);
                InventoryObj.GetComponent<RectTransform>().SetParent(canvasTransform);
                InventoryObj.transform.localPosition = Vector3.zero;
            }
            else
            {
                InventoryActive(InventoryObj.activeSelf);
            }
        }
    }

    private void InventoryActive(bool isActive)
    {
        InventoryObj.SetActive(!isActive);
    }
    #endregion // �Լ�
}
