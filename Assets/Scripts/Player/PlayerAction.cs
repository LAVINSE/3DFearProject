using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    #region 변수
    [SerializeField] private GameObject inventoryPrefab;

    private PlayerKeyCode playerKeyCode;

    public GameObject InventoryObj;
    private Transform canvasTransform;
    #endregion // 변수

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
    #endregion // 함수
}
