using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    #region 변수
    [SerializeField] private GameObject inventoryUIPrefab;
    [SerializeField] private GameObject orientation; // 플레이어 방향
    [SerializeField] private float pickItemRange; // 아이템 습득 가능한 최대 거리
    [SerializeField] private LayerMask layerMask; // 아이템 레이어
    [SerializeField] private TMP_Text actionText; // 아이템 줍는 액션 텍스트

    private bool pickupActivated = false; // 습득 가능할 시 true;

    private PlayerKeyCode playerKeyCode;

    private RaycastHit hitInfo; // 충돌체 정보 저장
    private Transform canvasTransform;
    #endregion // 변수

    #region 프로퍼티
    public GameObject InventoryObj { get; set; }
    #endregion // 프로퍼티

    #region 함수
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(orientation.transform.position, orientation.transform.TransformDirection(Vector3.forward) * pickItemRange);
    }
    /** 초기화 */
    private void Awake()
    {
        playerKeyCode = this.GetComponent<PlayerKeyCode>();

        canvasTransform = GameObject.FindWithTag("Canvas").transform;
    }

    /** 초기화 => 상태를 갱신한다 */
    private void Update()
    {
        // 입력처리
        PlayerActionInput();

        CheckItem();
        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();

        }
    }

    private void CheckItem()
    {
        if (Physics.Raycast(orientation.transform.position, this.orientation.transform.TransformDirection(Vector3.forward),
            out hitInfo, pickItemRange, layerMask))
        {
            if (hitInfo.transform.CompareTag("Item"))
            {
                ItemInfoAppear();
            }
        }
        else
        {
            InfoDisappear();
        }
    }

    private void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }

    private void ItemInfoAppear()
    {
        pickupActivated = true;

        actionText.gameObject.SetActive(true);

        actionText.text = hitInfo.transform.GetComponent<Item>().ItemData.itemName + "획득하기";
    }

    /** 입력처리 */
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

    /** 인벤토리 활성화/비활성화 한다 */
    private void InventoryActive(bool isActive)
    {
        InventoryObj.SetActive(!isActive);
    }
    #endregion // 함수
}
