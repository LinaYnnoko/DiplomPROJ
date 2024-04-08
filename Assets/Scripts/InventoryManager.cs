using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PixelCrushers.QuestMachine.Demo.DemoInventory;

public class InventoryManager : MonoBehaviour
{
    public Sprite slotSprite;
    public Transform inventoryPanel;
    public List<InventorySlot> slots = new List<InventorySlot>();

    PlayerAnimation player;

    void Start()
    {
        player = GetComponent<PlayerAnimation>();

        for (int i = 0; i < inventoryPanel.childCount; i++)
        {
            if (inventoryPanel.GetChild(i).GetComponent<InventorySlot>() != null)
            {
                slots.Add(inventoryPanel.GetChild(i).GetComponent<InventorySlot>());
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (slots[0] != null)
            {
                slots[0].amount--;
                player.Heal(slots[0].item.changeHealth);
                if (slots[0].amount <= 1)
                {
                    NullifySlotData(slots[0]);
                }

            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (slots[1] != null)
            {
                slots[1].amount--;
                player.Stamina(slots[1].item.changeStamina);
                if (slots[1].amount <= 1)
                {
                    NullifySlotData(slots[1]);
                }

            }
        }
    }

    public void AddItem(ItemScriptableObject _item, int _amount)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == _item)
            {
                slot.amount += _amount;
                slot.itemAmount.text = slot.amount.ToString();
                return;

            }
        }

        if (_item.potionType == PotionType.Heal)
        {
            slots[0].item = _item;
            slots[0].amount = _amount;
            slots[0].isEmpty = false;
            slots[0].SetIcon(_item.itemIcon);
            slots[0].itemAmount.text = _amount.ToString();
        }

        if(_item.potionType == PotionType.Stamina)
        {
            slots[1].item = _item;
            slots[1].amount = _amount;
            slots[1].isEmpty = false;
            slots[1].SetIcon(_item.itemIcon);
            slots[1].itemAmount.text = _amount.ToString();
        }



        /*foreach (InventorySlot slot in slots)
        {
            if (slot.isEmpty == true)
            {
                slot.item = _item;
                slot.amount = _amount;
                slot.isEmpty = false;
                slot.SetIcon(_item.itemIcon);
                slot.itemAmount.text = _amount.ToString();
                break;
            }

        }*/
    }
    public void NullifySlotData(InventorySlot slot)
    {
        slot.item = null;
        slot.amount = 0;
        slot.isEmpty = true;
        slot.iconGO.GetComponent<Image>().color = new Color(56, 56, 56, 255);
        slot.iconGO.GetComponent<Image>().sprite = slotSprite;
        slot.itemAmount.text = "";
    }
}
