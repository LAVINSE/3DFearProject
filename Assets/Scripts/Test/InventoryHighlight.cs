using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHighlight : MonoBehaviour
{
    #region 변수
    [SerializeField] private RectTransform highlighter;
    #endregion // 변수

    #region 함수
    /** 하이라이트 이미지를 활성화/비활성화 한다 */
    public void Show(bool isShow)
    {
        highlighter.gameObject.SetActive(isShow);
    }

    /** 하이라이트 이미지 사이즈를 설정한다 */
    public void SetSize(InventoryItem targetItem)
    {
        // 아이템의 크기와 타일의 크기를 곱하여 계산
        Vector2 size = new Vector2();
        size.x = targetItem.itemData.width * ItemGrid.tileSizeWidth;
        size.y = targetItem.itemData.heigth * ItemGrid.tileSizeHeight;

        highlighter.sizeDelta = size;
    }
    
    /** 하이라이트 이미지 위치 설정한다 (인벤토리에 이미 존재하는 아이템) */
    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem)
    {
        // 아이템이 위치할 그리드의 좌표를 받아서 중심을 계산한다
        Vector2 pos = targetGrid.CalculatePositionOnGrid(targetItem, targetItem.onGridPositionX, targetItem.onGridPositionY);

        highlighter.localPosition = pos;
    }

    /** 하이라이트 이미지 위치 설정한다 (인벤토리에 넣을 아이템) */
    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem, int posX, int posY)
    {
        // 아이템이 위치할 그리드의 좌표를 받아서 중심을 계산한다
        Vector2 pos = targetGrid.CalculatePositionOnGrid(targetItem, posX, posY);

        highlighter.localPosition = pos;
    }

    /** 하이라이트 이미지의 부모 객체를 설정한다 */
    public void SetParent(ItemGrid targetGrid)
    {
        // 인벤토리가 없을 경우 종료
        if(targetGrid == null) { return; }

        highlighter.SetParent(targetGrid.GetComponent<RectTransform>());
    }
    #endregion // 함수
}
