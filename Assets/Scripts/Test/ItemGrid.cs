using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    #region 변수
    private const float tileSizeWidth = 32;
    private const float tileSizeHeight = 32;

    private RectTransform rectTransform;

    // 그리드 위치를 저장하는 변수
    private Vector2 positionOnTheGrid = new Vector2();
    private Vector2Int tileGridPosition = new Vector2Int();
    #endregion // 변수

    #region 함수
    /** 초기화 */
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    /** 마우스 위치에 따라 좌표를 반환한다 */
    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        // 마우스 위치를 UI 좌표로 변환하여 저장
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = rectTransform.position.y - mousePosition.y;

        // 타일의 그리드 위치 계산, 정수형으로 변환하여 저장
        tileGridPosition.x = (int)(positionOnTheGrid.x / tileSizeWidth);
        tileGridPosition.y = (int)(positionOnTheGrid.y / tileSizeHeight);

        return tileGridPosition;
    }
    #endregion // 함수
}
