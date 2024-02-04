using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    #region 변수
    [HideInInspector] public ItemGrid selectedItemGrid;
    #endregion // 변수

    #region 함수
    private void Update()
    {
        if(selectedItemGrid == null) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(selectedItemGrid.GetTileGridPosition(Input.mousePosition));
        }
    }
    #endregion // 함수
}
