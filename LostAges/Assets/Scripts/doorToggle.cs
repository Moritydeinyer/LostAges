using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class doorToggle : MonoBehaviour
{
    public GameObject closedDoor; 
    public GameObject openDoor; 
    public bool atv = true;
    public PlayerInput playerInput;
    private InputAction interactAction;
    [SerializeField] private escMenuController escMC;

    public bool isPlayerInRange = false; 

    void Start()
    {
        SetDoorState(true);
        interactAction = playerInput.actions["Interact"];
    }

    void Update()
    {
        if (isPlayerInRange && interactAction.triggered) 
        {
           if (isPlayerInRange && atv)
            {
                ToggleDoor();
            } 
        }
    }


    private void ToggleDoor()
    {
        bool isClosed = closedDoor.activeSelf;
        SetDoorState(!isClosed);
    }

    public void SetDoorState(bool closed)
    {
        closedDoor.SetActive(closed);
        openDoor.SetActive(!closed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            var txt = "";
            var txt1 = "";
            if (atv)
            {
                if (closedDoor.activeSelf) 
                {
                    txt = "Press 'E' to open door";
                    txt1 = "Press     to open door";
                } 
                else 
                {
                    txt = "Press 'E' to close door";
                    txt1 = "Press     to close door";
                }
                escMC.UIinteractionPanelDataKeyboard.text = txt; 
                escMC.UIinteractionPanelDataXbox.text = txt1;
                escMC.UIinteractionPanelDataPS.text = txt1;
                escMC.UIinteractionPanel.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            escMC.UIinteractionPanel.gameObject.SetActive(false);
        }
    }
}
