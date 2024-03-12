using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    #region ����
    [SerializeField] private GameObject inventoryUIPrefab;
    [SerializeField] private GameObject viewDirection; // �÷��̾ ���� ����
    [SerializeField] private float itemPickRange = 2f; // ������ ���� ������ �ִ� �Ÿ�
    [SerializeField] private LayerMask layerMask; // ������ ���̾�
    [SerializeField] private TMP_Text actionText; // ������ �ݴ� �׼� �ؽ�Ʈ

    [SerializeField] private GameObject flashLightPrefab;

    private bool pickupActivated = false; // ���� ������ �� true;

    private PlayerKeyCode playerKeyCode;
    private InventoryController inventoryController;

    private RaycastHit itemhitInfo; // �浹ü ���� ����
    private Transform canvasTransform;
    #endregion // ����

    #region ������Ƽ
    public static PlayerAction instance { get; private set; }
    public GameObject InventoryObj { get; set; }
    #endregion // ������Ƽ

    #region �Լ�
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(viewDirection.transform.position, viewDirection.transform.TransformDirection(Vector3.forward) * itemPickRange);
    }
    /** �ʱ�ȭ */
    private void Awake()
    {
        instance = this;

        playerKeyCode = this.GetComponent<PlayerKeyCode>();
        inventoryController = this.GetComponentInChildren<InventoryController>();

        canvasTransform = GameObject.FindWithTag("Canvas").transform;

        // �κ��丮�� �����Ѵ�
        CreateInventory();
    }

    /** �ʱ�ȭ => ���¸� �����Ѵ� */
    private void Update()
    {
        // �Է�ó��
        PlayerActionInput();

        // �������� ȹ���� �� �ִ��� ���̷� Ȯ���Ѵ�
        CheckItemPickRayCast();
    }

    /** �Է�ó�� */
    private void PlayerActionInput()
    {
        // �κ��丮 Ű
        if (Input.GetKeyDown(playerKeyCode.InventoryKey))
        {
            if(InventoryObj != null)
            {
                // �κ��丮 Ȱ��ȭ/��Ȱ��ȭ
                InventoryActive(InventoryObj.activeSelf);
            }
            else
            {
                Debug.Log(" �κ��丮�� �������� �ʾҽ��ϴ� ");
            }
        }

        // �������� ȹ�� Ű
        if (Input.GetKeyDown(playerKeyCode.PickupItemKeyCode))
        {
            // �������� ȹ���� �� �������
            if (pickupActivated)
            {
                if(itemhitInfo.transform != null)
                {
                    Debug.Log($"{itemhitInfo.transform.GetComponent<Item>().ItemData.itemName} �� ȹ���߽��ϴ�");

                    bool isCheck;

                    // TODO : ����Ȯ���ؾߵ�
                    isCheck = inventoryController.AddItem(itemhitInfo.transform.GetComponent<Item>().ItemData);
                    HideItemPickText();

                    if (isCheck)
                    {
                        // ������ ����
                        Destroy(itemhitInfo.transform.gameObject);
                    }
                    else
                    {
                        Debug.Log(" �κ��丮�� ���� á���ϴ� ");
                    }
                    
                }
            }
        }
    }

    /** �κ��丮�� �����Ѵ� */
    private void CreateInventory()
    {
        if (InventoryObj == null)
        {
            // �κ��丮 ����
            InventoryObj = Instantiate(inventoryUIPrefab);
            InventoryObj.GetComponent<RectTransform>().SetParent(canvasTransform);
            InventoryObj.transform.localPosition = Vector3.zero;
            InventoryObj.SetActive(false);
        }
    }

    /** �������� ȹ���� �� �ִ��� ���̷� Ȯ���Ѵ� */
    private void CheckItemPickRayCast()
    {
        // �÷��̾� ���� �ٶ󺸰� �ִ� ����, �ݱ� �Ÿ�, ������ ���̾ �´��� �˻��ϰ�, hitInfo�� �����Ѵ�
        if (Physics.Raycast(viewDirection.transform.position, this.viewDirection.transform.TransformDirection(Vector3.forward),
            out itemhitInfo, itemPickRange, layerMask))
        {
            // �������� ���
            if (itemhitInfo.transform.CompareTag("Item"))
            {
                // ������ ȹ�� �ؽ�Ʈ�� �����ش�
                ShowItemPickText();
            }
        }
        else
        {
            // ������ ȹ�� �ؽ�Ʈ�� �����
            HideItemPickText();
        }
    }

    /** ������ ȹ�� �ؽ�Ʈ�� �����ش� */
    private void ShowItemPickText()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);

        actionText.text = itemhitInfo.transform.GetComponent<Item>().ItemData.itemName + "ȹ���ϱ�";
    }

    /** ������ ȹ�� �ؽ�Ʈ�� ����� */
    private void HideItemPickText()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }

    /** �κ��丮 Ȱ��ȭ/��Ȱ��ȭ �Ѵ� */
    private void InventoryActive(bool isActive)
    {
        InventoryObj.SetActive(!isActive);
    }

    /** �������� Ȱ��ȭ �Ѵ� */
    public void FlashLightActive(bool isActive)
    {
        flashLightPrefab.SetActive(isActive);
    }
    #endregion // �Լ�
}
