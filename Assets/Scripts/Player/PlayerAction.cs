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
    [SerializeField] private float itemPickRange = 2f; // 아이템 습득 가능한 최대 거리
    [SerializeField] private LayerMask layerMask; // 아이템 레이어
    [SerializeField] private TMP_Text actionText; // 아이템 줍는 액션 텍스트

    private bool pickupActivated = false; // 습득 가능할 시 true;

    private PlayerKeyCode playerKeyCode;
    private InventoryController inventoryController;

    private RaycastHit itemhitInfo; // 충돌체 정보 저장
    private Transform canvasTransform;
    #endregion // 변수

    #region 프로퍼티
    public GameObject InventoryObj { get; set; }
    #endregion // 프로퍼티

    #region 함수
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(orientation.transform.position, orientation.transform.TransformDirection(Vector3.forward) * itemPickRange);
    }
    /** 초기화 */
    private void Awake()
    {
        playerKeyCode = this.GetComponent<PlayerKeyCode>();
        inventoryController = this.GetComponentInChildren<InventoryController>();

        canvasTransform = GameObject.FindWithTag("Canvas").transform;

        // 인벤토리 생성 
        CreateInventory();
    }

    /** 초기화 => 상태를 갱신한다 */
    private void Update()
    {
        // 입력처리
        PlayerActionInput();

        // 아이템을 획득할 수 있는지 레이로 확인한다
        CheckItemPickRayCast();
    }

    /** 입력처리 */
    private void PlayerActionInput()
    {
        // 인벤토리 키
        if (Input.GetKeyDown(playerKeyCode.InventoryKey))
        {
            if(InventoryObj != null)
            {
                // 인벤토리 활성화/비활성화
                InventoryActive(InventoryObj.activeSelf);
            }
            else
            {
                Debug.Log(" 인벤토리가 생성되지 않았습니다 ");
            }
        }

        // 아이템을 획득 키
        if (Input.GetKeyDown(KeyCode.E))
        {
            // 아이템을 획득할 수 있을경우
            if (pickupActivated)
            {
                if(itemhitInfo.transform != null)
                {
                    Debug.Log($"{itemhitInfo.transform.GetComponent<Item>().ItemData.itemName} 을 획득했습니다");

                    // TODO : 오류확인해야됨
                    inventoryController.AddItem(itemhitInfo.transform.GetComponent<Item>().ItemData);
                    HideItemPickText();

                    // 아이템 삭제
                    Destroy(itemhitInfo.transform.gameObject);
                }
            }
        }
    }

    /** 인벤토리를 생성한다 */
    private void CreateInventory()
    {
        if (InventoryObj == null)
        {
            // 인벤토리 생성
            InventoryObj = Instantiate(inventoryUIPrefab);
            InventoryObj.GetComponent<RectTransform>().SetParent(canvasTransform);
            InventoryObj.transform.localPosition = Vector3.zero;
            InventoryObj.SetActive(false);
        }
    }

    /** 아이템을 획득할 수 있는지 레이로 확인한다 */
    private void CheckItemPickRayCast()
    {
        // 플레이어 현재 바라보고 있는 방향, 줍기 거리, 아이템 레이어가 맞는지 검사하고, hitInfo에 저장한다
        if (Physics.Raycast(orientation.transform.position, this.orientation.transform.TransformDirection(Vector3.forward),
            out itemhitInfo, itemPickRange, layerMask))
        {
            // 아이템일 경우
            if (itemhitInfo.transform.CompareTag("Item"))
            {
                // 아이템 획득 텍스트를 보여준다
                ShowItemPickText();
            }
        }
        else
        {
            // 아이템 획득 텍스트를 숨긴다
            HideItemPickText();
        }
    }

    /** 아이템 획득 텍스트를 보여준다 */
    private void ShowItemPickText()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);

        actionText.text = itemhitInfo.transform.GetComponent<Item>().ItemData.itemName + "획득하기";
    }

    /** 아이템 획득 텍스트를 숨긴다 */
    private void HideItemPickText()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }

    /** 인벤토리 활성화/비활성화 한다 */
    private void InventoryActive(bool isActive)
    {
        InventoryObj.SetActive(!isActive);
    }
    #endregion // 함수
}
