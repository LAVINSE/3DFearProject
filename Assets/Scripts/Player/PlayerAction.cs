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
    [SerializeField] private GameObject orientation; // �÷��̾� ����
    [SerializeField] private float itemPickRange = 2f; // ������ ���� ������ �ִ� �Ÿ�
    [SerializeField] private LayerMask layerMask; // ������ ���̾�
    [SerializeField] private TMP_Text actionText; // ������ �ݴ� �׼� �ؽ�Ʈ

    private bool pickupActivated = false; // ���� ������ �� true;

    private PlayerKeyCode playerKeyCode;
    private InventoryController inventoryController;

    private RaycastHit itemhitInfo; // �浹ü ���� ����
    private Transform canvasTransform;
    #endregion // ����

    #region ������Ƽ
    public GameObject InventoryObj { get; set; }
    #endregion // ������Ƽ

    #region �Լ�
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(orientation.transform.position, orientation.transform.TransformDirection(Vector3.forward) * itemPickRange);
    }
    /** �ʱ�ȭ */
    private void Awake()
    {
        playerKeyCode = this.GetComponent<PlayerKeyCode>();
        inventoryController = this.GetComponentInChildren<InventoryController>();

        canvasTransform = GameObject.FindWithTag("Canvas").transform;

        // �κ��丮 ���� 
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
        if (Input.GetKeyDown(KeyCode.E))
        {
            // �������� ȹ���� �� �������
            if (pickupActivated)
            {
                if(itemhitInfo.transform != null)
                {
                    Debug.Log($"{itemhitInfo.transform.GetComponent<Item>().ItemData.itemName} �� ȹ���߽��ϴ�");

                    // TODO : ����Ȯ���ؾߵ�
                    inventoryController.AddItem(itemhitInfo.transform.GetComponent<Item>().ItemData);
                    HideItemPickText();

                    // ������ ����
                    Destroy(itemhitInfo.transform.gameObject);
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
        if (Physics.Raycast(orientation.transform.position, this.orientation.transform.TransformDirection(Vector3.forward),
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
    #endregion // �Լ�
}
