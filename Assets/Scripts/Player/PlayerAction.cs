using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    #region ����
    [SerializeField] private GameObject inventoryPrefab;

    private PlayerKeyCode playerKeyCode;

    public GameObject InventoryObj;
    private Transform canvasTransform;
    #endregion // ����

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
                InventoryObj = Instantiate(inventoryPrefab);
                InventoryObj.GetComponent<RectTransform>().SetParent(canvasTransform);
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
