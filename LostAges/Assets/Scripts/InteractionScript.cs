using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.UI; //DEBUG Build = // Remove this line
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionScript : MonoBehaviour
{
    public PlayerInput playerInput;
    private InputAction interactAction;
    [SerializeField] private escMenuController escMC;
    public bool isPlayerInRange = false;
    [SerializeField] private string ConditionStID;
    [SerializeField] private string stID;
    [SerializeField] public string interactionText;

    [SerializeField] private bool additiveStID = false;
    [SerializeField] private bool stIDforbidden= false;

    [SerializeField] private bool deaktivate = false;
    [SerializeField] private bool rmStoryID = false;
    

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
                if (additiveStID) 
                {
                    escMC.storyManager.addStoryID(stID);
                } else {
                    escMC.gameData.story_id = stID;
                }
                if (deaktivate)
                {
                    gameObject.SetActive(false);
                }
                if (rmStoryID)
                {
                    escMC.storyManager.rmStoryID(ConditionStID);
                }
                if (!escMC.storyManager.checkStoryID(ConditionStID))
                {
                    isPlayerInRange = false;
                    escMC.UIinteractionPanel.gameObject.SetActive(false);
                }
                if (stIDforbidden && escMC.storyManager.checkStoryID(stID)) 
                {
                    isPlayerInRange = false;
                    escMC.UIinteractionPanel.gameObject.SetActive(false);
                }
            } 
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isActiveAndEnabled)
        {
            if (stIDforbidden && escMC.storyManager.checkStoryID(stID)) 
            {
                isPlayerInRange = false;
                escMC.UIinteractionPanel.gameObject.SetActive(false);
                return;
            }
            if (escMC.storyManager.checkStoryID(ConditionStID))
            {
                isPlayerInRange = true;
                var txt = interactionText; // "Press 'E' to interact";
                escMC.UIinteractionPanelDataKeyboard.text = txt;
                escMC.UIinteractionPanelDataXbox.text = txt;
                escMC.UIinteractionPanelDataPS.text = txt;
                escMC.UIinteractionPanel.gameObject.SetActive(true);
            }
            else
            {
                isPlayerInRange = false;
                escMC.UIinteractionPanel.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isActiveAndEnabled)
        {
            isPlayerInRange = false;
            escMC.UIinteractionPanel.gameObject.SetActive(false);
        }
    }
}
