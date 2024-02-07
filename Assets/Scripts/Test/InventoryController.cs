using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    #region 변수
    [HideInInspector] public ItemGrid selectedItemGrid;

    private InventoryItem selectedItem;
    #endregion // 변수

    #region 함수
    private void Update()
    {
        if(selectedItemGrid == null) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2Int tileGridPosition = selectedItemGrid.GetTileGridPosition(Input.mousePosition);

            if(selectedItem == null)
            {
                selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
            }
            else
            {
                selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y);
            }
        }
    }
    #endregion // 함수
}
