using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionScript : MonoBehaviour
{
    public PlayerInput playerInput;
    private InputAction interactAction;
    [SerializeField] private escMenuController escMC;
    public bool isPlayerInRange = false;
    [SerializeField] private string stID;
    

    void Start()
    {
        interactAction = playerInput.actions["Interact"];
    }

    void Update()
    {
        if (isPlayerInRange && interactAction.triggered) 
        {
           if (isPlayerInRange)
            {
                escMC.gameData.story_id = stID;
            } 
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            var txt = "Press 'E' to interact";
            escMC.UIinteractionPanelData.text = txt;
            escMC.UIinteractionPanel.gameObject.SetActive(true);
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
