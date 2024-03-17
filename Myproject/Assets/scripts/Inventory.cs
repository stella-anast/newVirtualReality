﻿using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;

public class InventorySystem : MonoBehaviour
{

    public static InventorySystem Instance { get; set; }
    public static GameObject itemBeingClicked;


    public GameObject inventoryScreenUI;
    public GameObject slotUI;


    public List<GameObject> slotList = new List<GameObject>();
    public List<String> itemList = new List<string>();

    private GameObject whatSlotToEquip;
    private GameObject itemToAdd;

    //public GameObject alertDialog; // Reference to the AlertDialog object


    public bool isOpen;
    //public bool isFull;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    void Start()
    {
        isOpen = false;
        //isFull = false;
        inventoryScreenUI.SetActive(false);

        PopulateSlotList();

        // Add event triggers to each slot to detect clicks
        foreach (GameObject slot in slotList)
        {
            EventTrigger trigger = slot.GetComponent<EventTrigger>();
            if (trigger == null)
                trigger = slot.AddComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
            entry.callback.AddListener((data) => { OnSlotClicked(slot);
            });
            trigger.triggers.Add(entry);
        }
    }

    private void PopulateSlotList()
    {
        foreach (Transform child in slotUI.transform)
        {
            if (child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I) && !isOpen)
        {

            inventoryScreenUI.SetActive(true);
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            isOpen = false;
        }
    }

    public void AddToInventory(string itemName)
    {

        whatSlotToEquip = FindNextEmptySlot();

        itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position,
            whatSlotToEquip.transform.rotation);

        itemToAdd.transform.SetParent(whatSlotToEquip.transform);

        itemList.Add(itemName);

        QuestManager.Instance.RefreshTrackerList();

    }
    private GameObject FindNextEmptySlot()

    {
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }

        return new GameObject();
    }

    public bool CheckIfFull()
    {
        int counter = 0;
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount == 0)
            {
                counter += 1;
            }
            
        }
        if (counter == 24)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int CheckItemAmount(string name)
    {
        int itemCounter = 0;

        foreach (string item in itemList)
        {
            if (item == name)
            {
                itemCounter++;
            }
        }
        return itemCounter;
    }

    public void RemoveItem(string nameToRemove, int amountToRemove)
    {
        int counter = amountToRemove;

        for (var i = slotList.Count - 1; i >= 0; i--)
        {
            if (slotList[i].transform.childCount > 0)
            {
                if (slotList[i].transform.GetChild(0).name == nameToRemove + "(Clone)" && counter != 0)
                {
                    DestroyImmediate(slotList[i].transform.GetChild(0).gameObject);
                    counter -= 1;
                    itemList.Remove(nameToRemove);
                }
            }
        }

        QuestManager.Instance.RefreshTrackerList();

    }

    private void OnSlotClicked(GameObject slot)
    {
        // Check if the clicked slot contains an item
        if (slot.transform.childCount > 0)
        {
            // Get the name of the item in the slot
            string itemName = slot.transform.GetChild(0).name.Replace("(Clone)", string.Empty);

            AlertDialog alertDialog = FindAnyObjectByType<AlertDialog>();

            // Show alert dialog
            alertDialog.ShowDialog("Are you sure you want to remove this item?", (response) =>
            {
                if (response)
                {
                    // Remove the item from the inventory
                    RemoveItem(itemName, 1);
                }
            });
        }
    }
}