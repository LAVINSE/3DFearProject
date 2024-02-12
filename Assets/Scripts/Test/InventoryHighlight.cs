using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHighlight : MonoBehaviour
{
    #region 변수
    [SerializeField] private RectTransform highlighter;
    #endregion // 변수

    #region 함수
    /** 하이라이트 이미지 사이즈를 설정한다 */
    public void SetSize(InventoryItem targetItem)
    {
        Vector2 size = new Vector2();
        size.x = targetItem.itemData.width * ItemGrid.tileSizeWidth;
        size.y = targetItem.itemData.heigth * ItemGrid.tileSizeHeight;

        highlighter.sizeDelta = size;
    }

    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem)
    {
        highlighter.SetParent(targetGrid.GetComponent<RectTransform>());

        // 아이템이 위치할 그리드의 좌표를 받아서 중심을 계산한다
        Vector2 pos = targetGrid.CalculatePositionOnGrid(targetItem, targetItem.onGridPositionX, targetItem.onGridPositionY);

        highlighter.localPosition = pos;
    }
    #endregion // 함수
}
