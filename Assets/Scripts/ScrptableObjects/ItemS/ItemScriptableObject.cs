using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {Default, Food, Keys, }
public enum PotionType {Default, Heal, Stamina, }

[CreateAssetMenu(fileName = "ItemData", menuName = "Data/Item")]
public class ItemScriptableObject : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;
    public ItemType type;
    public PotionType potionType;  
    public GameObject itemPrefab;

    [Header("Consumable Characteristics")]
    public int changeHealth;
    public int changeHunger;
    public float changeStamina;
}
