using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataSO", menuName = "ScriptableObjects/ItemDataSO/ItemData")]
public class ItemDataSO : ScriptableObject
{
    public string itemName = string.Empty;
    public int width = 1;
    public int heigth = 1;

    public Sprite itemIcon;
}
