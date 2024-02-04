using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    #region ����
    private const float tileSizeWidth = 32;
    private const float tileSizeHeight = 32;

    private RectTransform rectTransform;

    // �׸��� ��ġ�� �����ϴ� ����
    private Vector2 positionOnTheGrid = new Vector2();
    private Vector2Int tileGridPosition = new Vector2Int();
    #endregion // ����

    #region �Լ�
    /** �ʱ�ȭ */
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    /** ���콺 ��ġ�� ���� ��ǥ�� ��ȯ�Ѵ� */
    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        // ���콺 ��ġ�� UI ��ǥ�� ��ȯ�Ͽ� ����
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = rectTransform.position.y - mousePosition.y;

        // Ÿ���� �׸��� ��ġ ���, ���������� ��ȯ�Ͽ� ����
        tileGridPosition.x = (int)(positionOnTheGrid.x / tileSizeWidth);
        tileGridPosition.y = (int)(positionOnTheGrid.y / tileSizeHeight);

        return tileGridPosition;
    }
    #endregion // �Լ�
}
