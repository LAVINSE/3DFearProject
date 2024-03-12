using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Inventory))]
public class InventoryInteract : MonoBehaviour, IPointerEnterHandler
{
    #region ����
    [SerializeField] private bool isLock = true;
    [SerializeField] private GameObject lockInvenotoryImg;

    private InventoryController inventoryController;
    private Inventory selectedItemInventory;
    #endregion // ����

    #region �Լ� 
    /** �ʱ�ȭ */
    private void Awake()
    {
        inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
        selectedItemInventory = GetComponent<Inventory>();

        // �κ��丮�� ��ݻ��°� �ƴϰ�, ���õ� �κ��丮�� ���� ���
        if (!isLock && inventoryController.SelectedInventory == null)
        {
            inventoryController.SelectedInventory = selectedItemInventory;
        }

        // �κ��丮�� ��ݻ����̰�, ��� �̹����� ���� ���
        if(isLock && lockInvenotoryImg != null)
        {
            UnLockInventory(true);
        }
    }

    /** ���콺 Ŀ���� �浹 ���� ������ ��� �ö� */
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isLock)
        {
            inventoryController.SelectedInventory = selectedItemInventory;
        }  
    }

    /** �κ��丮�� ��������Ѵ� */
    public void UnLockInventory(bool isLock)
    {
        // ��� �̹����� ���� ���
        if(lockInvenotoryImg == null) { return; }

        this.isLock = isLock;
        lockInvenotoryImg.SetActive(isLock);
    }
    #endregion // �Լ�
}
