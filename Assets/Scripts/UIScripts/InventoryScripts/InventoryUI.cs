using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotContainer;
    private InventorySystem inventorySystem;

    private void Start()
    {
        inventorySystem = InventorySystem.instance;
        UpdateInventoryUI();
    }

    private void OnEnable()
    {
        InventorySystem.instance.onInventoryUpdated += UpdateInventoryUI;
    }
    
    private void OnDisable()
    {
        InventorySystem.instance.onInventoryUpdated -= UpdateInventoryUI;
    }

    private void UpdateInventoryUI()
    {
        foreach (Transform child in slotContainer)
        {
            Destroy(child.gameObject);
        }
        
        foreach (var slot in inventorySystem.inventorySlots)
        {
            var slotUI = Instantiate(slotPrefab, slotContainer);

            if (slot.itemData != null)
            {
                Sprite iconImage = slot.itemData.icon;
                Image[] images = slotUI.GetComponentsInChildren<Image>();
                if (images.Length > 0)
                {
                    images[1].sprite = iconImage;
                }
            }
            else
            {
                Debug.LogError("Inventory slot has a null itemData. Check your inventory system!");
            }

            TextMeshProUGUI quantityText = slotUI.GetComponentInChildren<TextMeshProUGUI>();
            quantityText.text = slot.itemData.quantity.ToString();
        }
    }
}
