using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    #region 변수
    [SerializeField] private GameObject inventoryUIPrefab;

    private PlayerKeyCode playerKeyCode;

    private Transform canvasTransform;
    #endregion // 변수

    #region 프로퍼티
    public GameObject InventoryObj { get; set; }
    #endregion // 프로퍼티

    #region 함수
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

    private void InventoryActive(bool isActive)
    {
        InventoryObj.SetActive(!isActive);
    }
    #endregion // 함수
}
