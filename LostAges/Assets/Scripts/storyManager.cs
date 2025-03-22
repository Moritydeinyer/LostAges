using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using HeneGames.DialogueSystem;
using System.Linq;
using UnityEngine.Events;
using Cainos.PixelArtTopDown_Basic;
using System;
using Unity.Collections;
using UnityEditor;

public class storyManager : MonoBehaviour
{

    [SerializeField] private escMenuController escMC;
    [SerializeField] private GameObject titleScreenPanel;
    [SerializeField] private TextMeshProUGUI titleScreenPanelTxt1;
    [SerializeField] private TextMeshProUGUI titleScreenPanelTxt2;
    [SerializeField] private RectTransform credits;
    [SerializeField] private Transform creditScreenPanel;
    [SerializeField] private GameObject miniMap;
    [SerializeField] private GameObject taschenuhrUI;
    [SerializeField] private GameObject taschenuhrProlog;    
    [SerializeField] private DialogueCharacter chronosSWDC;
    [SerializeField] public DialogueManager chronosSWDM;
    [SerializeField] private GameObject CHRONOS;
    [SerializeField] public Enemy chronosEnemy;

    [Header("Trigger")]
    [SerializeField] private doorToggle doorSt3;
    [SerializeField] private doorToggle doorSt4;
    [SerializeField] private doorToggle doorSt5;

    [SerializeField] public Enemy kampfTutorialEnemy;
    [SerializeField] public GameObject kampfTutorialEnemyGO;
    private bool enemyAttackDetection = true;
    private bool enemyComboDetection = true;
    public bool session = false;
    [SerializeField] private BoxCollider2D chronosKampfTutorialEnemy;
    private Coroutine currentCoroutine;
    public bool checkDashKampfTutorialEnemy = false;

    public string collisionTag;


    [Header("End Fight")]
    [SerializeField] private GameObject K1;
    [SerializeField] private GameObject K2;
    [SerializeField] private GameObject K3;
    [SerializeField] private GameObject K4;
    [SerializeField] private GameObject portalZEUS;
    [SerializeField] private GameObject portalAfterworld;
    [SerializeField] private GameObject taschenuhrAfterworld;
    [SerializeField] private Enemy enemyZEUS;
    [SerializeField] public Enemy afterWorldEnemy;
    [SerializeField] public Enemy demoENEMY;
    [SerializeField] public GameObject afterWorldEnemyGO;

    [Header("Startwelt")]
    [SerializeField] private SpriteRenderer kristall;

    void Start() 
    {

    }

    void Update()
    {
        if (checkStoryID("3")) {st3Trigger();}
        if (checkStoryID("4")) {st4Trigger();}
        if (checkStoryID("5")) {st5Trigger();}
        if (checkStoryID("6")) {st6Trigger();}

        if ((checkStoryIDgraterThan("5") && checkStoryIDsmallerThan("1010")) || checkStoryIDgraterThan("1016"))
        {
            taschenuhrUI.SetActive(true);
            taschenuhrProlog.SetActive(false);
        }
        if (checkStoryIDsmallerThan("4") || (checkStoryIDgraterThan("1011") && checkStoryIDsmallerThan("1015"))) 
        {
            taschenuhrUI.SetActive(false);
            taschenuhrProlog.SetActive(true);
        }
        if (checkStoryIDsmallerThan("6")) {miniMap.SetActive(false);}
        if (checkStoryIDgraterThan("7")) {miniMap.SetActive(true);}

        if (checkStoryID("9") && !session) {
            st9Trigger();
        }

        if ((checkStoryID("10") && !session) || (checkStoryID("11") && !session)) 
        {
            st9Trigger();
        }

        if (checkStoryID("11")) {
            DialogueUI.instance.ShowInteractionUI(false);
        }



        if (checkStoryID("100")) {
            st100Trigger();
        }
        if (checkStoryIDgraterThan("100") && !session && checkStoryIDsmallerThan("109")) {
            st100Trigger();
        }
        if (checkStoryID("102")) {
            DialogueUI.instance.ShowInteractionUI(false);
        }



        if (checkStoryID("1000") && !session) {
            st1000Trigger();
        }
        if (checkStoryIDgraterThan("1000") && !session && checkStoryIDsmallerThan("1021") && !session) 
        {
            st1000Trigger();
        } 
        if (checkStoryID("1005"))
        {
            st1005Trigger();
        }
        if (checkStoryID("1007"))
        {
            st1007Trigger();
        }
        if (checkStoryID("1015"))
        {
            st1015Trigger();
        }
        if (checkStoryID("1017"))
        {
            st1016Trigger();
        }
        if (checkStoryID("1001"))
        {
            DialogueUI.instance.ShowInteractionUI(false);
        } 
        if (checkStoryID("1020"))
        {
            escMC.gameData.story_id = "1021";
            showCredit();
        }
        if (checkStoryID("1021"))
        {
            if (credits.anchoredPosition.y >= 3140)
            {
                creditScript cS = credits.GetComponent<creditScript>();
                cS.enabled = false;
                escMC.gameData.story_id = "1022";
                session = false;
                // END GAMEPLAY DEBUG
            }
        }


        if (escMC.gameData.health <= 0)
        {
            Debug.Log("DEBUG Game Over");
        }
    }

    public void addStoryID (string story_id)
    {
        List<string> newStIDs = new List<string>();
        newStIDs.Add(escMC.gameData.story_id);
        newStIDs.Add(story_id);
        escMC.gameData.story_id = string.Join(";", newStIDs);
    }

    public void rmStoryID (string story_id)
    {
        string[] stIDs = escMC.gameData.story_id.Split(";");
        List<string> newStIDs = new List<string>();
        foreach (string st in stIDs)
        {
            if (st != story_id)
            {
                newStIDs.Add(st);
            }
        }
        escMC.gameData.story_id = string.Join(";", newStIDs);
    }

    public bool checkStoryIDgraterThan (string story_id)
    {
        bool data = false;
        foreach (string tempID in escMC.gameData.story_id.Split(";"))
        {
            try 
            {
                if (int.Parse(tempID) >= int.Parse(story_id))
                {
                    data = true;
                    break;
                }
            } 
            catch 
            {
                // DEBUG no integer
            }
        }
        return data;
    }

    public bool checkStoryIDsmallerThan (string story_id)
    {
        bool data = false;
        foreach (string tempID in escMC.gameData.story_id.Split(";"))
        {
            try 
            {
                if (int.Parse(tempID) <= int.Parse(story_id))
                {
                    data = true;
                    break;
                }
            } 
            catch 
            {
                // DEBUG no integer
            }
        }
        return data;
    }

    public bool checkStoryID (string story_id)
    {
        if (escMC.gameData.story_id.Split(";").Contains(story_id)){return true;} else {return false;}
    }

    public void ResetMap()
    {
        doorSt3.GetComponent<doorToggle>().SetDoorState(true);
        doorSt4.GetComponent<doorToggle>().SetDoorState(true);
        doorSt5.GetComponent<doorToggle>().SetDoorState(true);
    }

    void st3Trigger()
    {
        doorSt3.GetComponent<doorToggle>().SetDoorState(false);
    }

    void st4Trigger()
    {
        doorSt4.GetComponent<doorToggle>().SetDoorState(false);
        doorSt3.GetComponent<doorToggle>().SetDoorState(true);
    }

    void st5Trigger()
    {
        doorSt5.GetComponent<doorToggle>().SetDoorState(false);
        doorSt4.GetComponent<doorToggle>().SetDoorState(true);
        // DEBUG display taschenuhr auf UI 
    }

    void st6Trigger()
    {
        doorSt5.GetComponent<doorToggle>().SetDoorState(true);
        // DEBUG cutscene
        Debug.Log("PROLOG <end>");
        titleScreenPanel.SetActive(true);
        StartCoroutine(titleFadeOut());
        escMC.gameData.story_id = "7";
    }

    IEnumerator titleFadeOut()
    {
        yield return new WaitForSeconds(2);
        while (titleScreenPanelTxt1.alpha > 0)
        {
            titleScreenPanelTxt1.alpha -= 0.01f;
            titleScreenPanelTxt2.alpha -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        titleScreenPanel.SetActive(false);
    }

    
    IEnumerator setDialogue(float time, DialogueCharacter dc, String txt, DialogueManager dm, bool startDialogue, UnityAction call)
    {
        yield return new WaitForSeconds(time);
        List<NPC_Centence> sentences = new List<NPC_Centence>();
        NPC_Centence sentence = new NPC_Centence();
        sentence.dialogueCharacter = dc;
        sentence.sentence = txt;
        sentence.sentenceEvent = new UnityEvent();
        sentences.Add(sentence);
        if (dm.sentences != sentences)
        {
            dm.endDialogueEvent.RemoveAllListeners();
            dm.endDialogueEvent.AddListener(call);
            dm.sentences = sentences;
            if (startDialogue) {DialogueUI.instance.StartDialogue(dm); dm.dialogueIsOn = true;}
        }
    }

    void stopCoroutine(Coroutine tempCoroutine)
    {
        //if (currentCoroutine != null)
        //{
        //    stopCoroutine(currentCoroutine);
        //}
        //currentCoroutine = tempCoroutine;
    }








    /// <summary>   
    /// KAMPFTUTORIAL     
    /// </summary>
    /// <returns></returns>

    void enemyFreeze(bool freeze)
    {
        kampfTutorialEnemy.active = !freeze;
    }


    void st9Trigger()
    {
        chronosSWDM.StopDialogue();
        chronosSWDM.ext = true;
        List<NPC_Centence> sentences = new List<NPC_Centence>();
        NPC_Centence sentence = new NPC_Centence();
        chronosSWDM.sentences = sentences;
        session = true;
        escMC.gameData.story_id = "10";
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Ein Krieger lernt nicht durch Worte, sondern durch Kampf. Also, verschwenden wir keine Zeit mit Reden - du wirst überleben oder untergehen. Deine Prüfung beginnt ... jetzt!", chronosSWDM, false, st10Trigger)));
    }

    void st10Trigger()
    {    
        escMC.gameData.story_id = "11";
        kampfTutorialEnemyGO.gameObject.SetActive(true);                                  
        kampfTutorialEnemyGO.gameObject.transform.position = new Vector3(-112.07f, -8.44f, 0);       
        kampfTutorialEnemy.health = 1f; 
        chronosSWDM.ext = false;                       
        enemyFreeze(true);
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Dein erster Feind - ein einfacher Trainingsgegner. Aber unterschätze ihn nicht!", chronosSWDM, true, st11Trigger)));
    }

    void st11Trigger()
    {    
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Weiche aus! Ein echter Gegner wartet nicht, dass du dich in Position bringst!", chronosSWDM, true, st12Trigger)));
        kampfTutorialEnemy.patrolPoints[1].position = kampfTutorialEnemy.player.position;         
    }

    void st12Trigger()
    {
        string action = "ctrl";
        if (escMC.useController) {if (escMC.controllerType == "Xbox") {action = "RB";} else {action = "RB";}}
        string data = "Dash mit " + action;
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, data, chronosSWDM, true, stDetectionDash)));
    }

    void stDetectionDash()
    {
        enemyFreeze(false);
        List<NPC_Centence> sentences = new List<NPC_Centence>();
        NPC_Centence sentence = new NPC_Centence();
        chronosSWDM.sentences = sentences;
        stopCoroutine(StartCoroutine(detectionDash()));
    }

    void st13Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Zu langsam! Versuch es nochmal - konzentriere dich auf die Bewegung deines Gegners!", chronosSWDM, true, stDashWdh)));
    }

    void stDashWdh()
    {
        kampfTutorialEnemyGO.gameObject.transform.position = new Vector3(-112.07f, -8.44f, 0);
        kampfTutorialEnemy.patrolPoints[1].position = kampfTutorialEnemy.player.position;
        escMC.gameData.health = 400;
        stDetectionDash();
    }

    IEnumerator detectionDash()
    {
        int tempHealth;
        bool detectionDashBool = false;
        checkDashKampfTutorialEnemy = false;
        tempHealth = escMC.gameData.health;
        while (!detectionDashBool)
        {
            if (tempHealth > escMC.gameData.health)
            {
                detectionDashBool = true;
                st13Trigger();
            } else if (checkDashKampfTutorialEnemy)
            {
                detectionDashBool = true;
                st14Trigger();
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void st14Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Gut. Bewegung rettet Leben. Aber ein Kampf wird nicht durch das Wegrennen gewonnen.", chronosSWDM, true, st15Trigger)));
        enemyFreeze(true);
    }

    void st15Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Jetzt bist du am Zug! Greif an, bevor sich dein Feind erholt!", chronosSWDM, true, st16Trigger)));
    }

    void st16Trigger()
    {
        string action = "linkem Mausklick";
        if (escMC.useController) {if (escMC.controllerType == "Xbox") {action = "X";} else {action = "□";}}
        string data = "Greife mit " + action + " an.";
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, data, chronosSWDM, true, stAttackDetection)));
        enemyFreeze(false);
    }

    void stAttackDetection()
    {
        stopCoroutine(StartCoroutine(attackDetection()));
    }

    IEnumerator attackDetection()
    {
        enemyAttackDetection = true;
        float enemyHealth = kampfTutorialEnemy.health;
        while (enemyAttackDetection)
        {
            if (enemyHealth > kampfTutorialEnemy.health)
            {
                st17Trigger();
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void st17Trigger()
    {
        enemyAttackDetection = false;
        enemyFreeze(true);
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Nicht schlecht. Aber ein einzelner Schlag tötet selten - kombiniere deine Angriffe!", chronosSWDM, true, stDetectionCombo)));
    }

    void stDetectionCombo()
    {
        enemyFreeze(false);
        stopCoroutine(StartCoroutine(detectionCombo()));
    }

    IEnumerator detectionCombo()
    {
        enemyComboDetection = true;
        int count = 0;
        float tempHealth = kampfTutorialEnemy.health;
        while (enemyComboDetection)
        {
            if (tempHealth > kampfTutorialEnemy.health)
            {
                tempHealth = kampfTutorialEnemy.health;
                count++;
                if (count >= 3)
                {
                    st18Trigger();
                    break;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void st18Trigger()
    {
        enemyComboDetection = false;
        kampfTutorialEnemyGO.SetActive(false);
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Ja! Spüre den Rhythmus des Kampfes - Angriff, Bewegung, Angriff!”", chronosSWDM, true, st19Trigger)));
    }

    void st19Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Verletzungen sind unvermeidlich. Aber du regenerierst mit der Zeit…", chronosSWDM, true, st20Trigger)));
    }

    void st20Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Fünf Leben. Das ist alles, was du hast.", chronosSWDM, true, st21Trigger)));
    }

    void st21Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Wenn du sie alle verlierst, wirst du in die Afterworld gezogen. Dort erwartet dich der Wächter. Dein letzter Gegner… und deine einzige Chance auf Rückkehr.", chronosSWDM, true, st22Trigger)));
    }

    void st22Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Besiege ihn, und du wirst wiedergeboren. Scheiterst du… dann war es das.", chronosSWDM, true, st23Trigger)));
    }

    void st23Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Aber wenn du weiter so kämpfst, wirst du hoffentlich nie herausfinden, wie es dort unten ist. Nun geh - deine wahre Aufgabe beginnt erst jetzt.", chronosSWDM, true, st24Trigger)));
    }

    void st24Trigger()
    {
        List<NPC_Centence> sentences = new List<NPC_Centence>();
        NPC_Centence sentence = new NPC_Centence();
        chronosSWDM.sentences = sentences;
        escMC.taskPanelData.text = "\n\nDEBUG Kampftutorial abgeschlossen"; //DEBUG
        escMC.gameData.story_id = "12";
        chronosSWDM.ext = true;    
        session = false;
        Debug.Log("DEBUG Kampftutorial abgeschlossen");
        escMC.gameData.story_id = "100"; //DEBUG
    }


    // END KAMPFTUTORIAL








    /// <summary>   
    /// STARTWELT     
    /// </summary>
    /// <returns></returns>

    void st100Trigger()
    {
        chronosSWDM.StopDialogue();
        chronosSWDM.ext = true;
        List<NPC_Centence> sentences = new List<NPC_Centence>();
        NPC_Centence sentence = new NPC_Centence();
        chronosSWDM.sentences = sentences;
        escMC.gameData.story_id = "101";
        session = true;
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Du stehst an einem vergessenen Ort, einem Schatten der Welt da draußen. Einst war dies eine Zuflucht, doch nun... ist es ein Käfig. Mein Käfig.", chronosSWDM, false, st102Trigger)));
    }

    void st102Trigger()
    {
        escMC.gameData.story_id = "102";
        chronosSWDM.ext = false; 
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Wofür? Für das Volk, das jenseits dieser Nebel lebt. Man erzählt ihnen, dieser Ort sei eine Barriere gegen das Böse, gegen die Zeitanomalien, die die Welt zerreißen. Doch die Wahrheit ist... Zeus hat mich hierher verbannt.", chronosSWDM, true, st103Trigger)));
    }

    void st103Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Mich - den einzigen, der sein wahres Gesicht kennt. Und nun bist du hier. Das Schicksal hat uns zusammengeführt.", chronosSWDM, true, st104Trigger)));
    }

    void st104Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Er ist nicht der, für den er sich ausgibt. Er hat die Zeit selbst missbraucht, sie verdreht und gebrochen, um seine Herrschaft zu sichern. Die Anomalien da draußen? Seine Schuld. Doch niemand sieht es - niemand außer mir... und jetzt auch du.", chronosSWDM, true, st105Trigger)));
    }

    void st105Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Weil du die Macht dazu hast. Du bist anders als die anderen. Dein Schicksal ist noch nicht geschrieben. Aber bevor du Zeus gegenübertreten kannst, musst du den Kristall aufladen. Er ist der Schlüssel, der uns hier hinausführt.", chronosSWDM, true, st106Trigger)));
    }

    void st106Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Vier Monumente, verstreut in dieser Welt. Jedes von ihnen wird deine Fähigkeiten prüfen, jedes fordert dein Können heraus. Wenn du sie aktivierst, wird der Kristall seine volle Kraft entfalten - und der Nebel wird sich lichten.", chronosSWDM, true, st107Trigger)));
    }

    void st107Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Dann beginnt deine wahre Reise. Drei Gebiete, drei Kugeln der Zeit: Vergangenheit, Gegenwart, Zukunft. Erst wenn du sie alle vereinst, öffnet sich das Tor zu Zeus.", chronosSWDM, true, st108Trigger)));
    }

    void st108Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Ich werde hier auf dich warten, am Kristall. Deine Entscheidung, dein Schicksal. Wähle weise... und vergeude keine Zeit", chronosSWDM, true, st109Trigger)));
    }

    void st109Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Nun geh. Die Zeit mag auf deiner Seite sein - aber sie wartet auf niemanden.", chronosSWDM, true, st110Trigger)));
    }

    void st110Trigger()
    {
        List<NPC_Centence> sentences = new List<NPC_Centence>();
        NPC_Centence sentence = new NPC_Centence();
        chronosSWDM.sentences = sentences;
        escMC.gameData.story_id = "110";
        chronosSWDM.ext = true;
        session = false;
        Debug.Log("DEBUG Startwelt abgeschlossen");
        escMC.taskPanelData.text = "\n\nLade den Kristall durch Aktivieren der Monumente auf"; //DEBUG
        escMC.gameData.story_id = "1000"; //DEBUG
    }

    //Monumente aktivieren DETECTION
    // TRIGGER MONUMENTE --> Kristall aufladen
    // Kristall aufladen --> Nebel lichtet sich MINIMAP K1-3 anzeugen + Weg offen

    // END STARTWELT








    /// <summary>   
    /// ENDGAMEPLAY     
    /// </summary>
    /// <returns></returns>

    void clearDialoguechronosSWDM()
    {
        List<NPC_Centence> sentences = new List<NPC_Centence>();
        NPC_Centence sentence = new NPC_Centence();
        chronosSWDM.sentences = sentences;
    }

    Vector3 playerPositionEndgameRespanw;

    public void resetEndFight () // RESET MAP DEBUG
    {
        //RESET DEBUG
        escMC.gameData.story_id = "1000";
        K1.SetActive(false);
        K2.SetActive(false);
        K3.SetActive(false);
        K4.SetActive(false);
        chronosEnemy.active = false;
        portalZEUS.SetActive(false);
        portalAfterworld.SetActive(false);
        afterWorldEnemyGO.SetActive(true);
        taschenuhrAfterworld.SetActive(false);
        CHRONOS.transform.position = new Vector3(-101.35f, -8.86f, 0);
        escMC.gameData.respw = playerPositionEndgameRespanw.x + ";" + playerPositionEndgameRespanw.y;
    }

    public void st1000Trigger()
    {
        chronosSWDM.StopDialogue();
        chronosSWDM.ext = true;
        List<NPC_Centence> sentences = new List<NPC_Centence>();
        NPC_Centence sentence = new NPC_Centence();
        chronosSWDM.sentences = sentences;
        session = true;
        K1.SetActive(true);
        K2.SetActive(true);
        K3.SetActive(true);
        K4.SetActive(true);
        chronosEnemy.active = false;
        portalZEUS.SetActive(false);
        portalAfterworld.SetActive(false);
        afterWorldEnemyGO.SetActive(true);
        taschenuhrAfterworld.SetActive(false);
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Endlich. Du hast es geschafft. Alle Königreiche bereist, alle Rätsel gemeistert... und mir genau das gebracht, was ich brauche.", chronosSWDM, false, st1001Trigger)));
    }

    void st1001Trigger()
    {
        chronosSWDM.ext = false; 
        playerPositionEndgameRespanw = new Vector3(kampfTutorialEnemy.player.position.x, kampfTutorialEnemy.player.position.y, kampfTutorialEnemy.player.position.z);
        clearDialoguechronosSWDM();
        Debug.Log("DEBUG Lege alle Kugeln in die Vertiefungen");
        escMC.gameData.story_id = "1001";
    }
    // EINSETZEN DER KUGELN DEBUG

    void st1005Trigger()
    {
        escMC.gameData.story_id = "1006";
        portalZEUS.SetActive(true);
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Perfekt... Das Portal ist offen. Unsere Reise endet hier nicht, mein Freund - sie beginnt erst. Ich wusste, dass du es schaffst. Dass du mir hilfst. Auch wenn du es nicht bemerkt hast.", chronosSWDM, true, st1006Trigger)));
    }

    void st1006Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Siehst du, wie es pulsiert? Wie die Macht der Zeit selbst sich entfaltet? Und du dachtest, du kämpfst für das Richtige...", chronosSWDM, true, clearDialoguechronosSWDM)));
    }

    void st1007Trigger() 
    {
        // KAMPF ZEUS
        //escMC.gameData.health = 400;
        CHRONOS.transform.position = new Vector3(-64.61f, 57.29f, 0);
        escMC.gameData.story_id = "1008";
        StartCoroutine(attackDetectionZEUS());
    }

    IEnumerator attackDetectionZEUS()
    {
        enemyAttackDetection = true;
        float enemyHealth = enemyZEUS.health;
        while (enemyAttackDetection)
        {
            if (enemyZEUS == null)
            {
                enemyAttackDetection = false;
                st1008Trigger();
                yield break;
            }
            if (checkStoryID("1001"))
            {
                enemyAttackDetection = false;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void st1008Trigger()
    {
        Debug.Log("DEBUG Sieg über Zeus");
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Hervorragend! Wahrlich ein Anblick für die Ewigkeit - der letzte Wächter, gefallen durch deine Hand.", chronosSWDM, true, st1009Trigger)));
    }

    void st1009Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Du hast dich gut geschlagen... zu gut. Ich hätte es selbst nicht besser machen können. Aber siehst du nicht? Ich habe dich gelenkt. Ich habe dich gebraucht. Und du... hast mir alles gegeben, was ich wollte.", chronosSWDM, true, st1010Trigger)));
    }

    void st1010Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Die Kugeln? Ein Schlüssel. Zeus? Ein Hindernis. Du? Mein Werkzeug.", chronosSWDM, true, st1011Trigger)));
    }

    void st1011Trigger()
    {
        // ANIM Chronos klaut Taschenuhr DEBUG
        escMC.gameData.story_id = "1011";
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "DIESE Macht... lange habe ich darauf gewartet! Du hast nicht den letzten Feind besiegt - du hast ihn befreit!", chronosSWDM, true, st1012Trigger)));
    }

    void st1012Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Zeus war der letzte, der mich hätte aufhalten können. Und du hast ihn für mich beseitigt. Dafür danke ich dir... doch jetzt bist du überflüssig", chronosSWDM, true, st1013Trigger)));
    }

    void st1013Trigger()
    {
        CHRONOS.transform.position = new Vector3(-34.43f, 61.29f, 0);
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Komm... ich will deine letzten Atemzüge selbst erleben.", chronosSWDM, true, st1014Trigger)));
    }

    void st1014Trigger()
    {
        portalAfterworld.SetActive(true);
    }

    void st1015Trigger()
    {
        escMC.gameData.story_id = "1016";
        taschenuhrAfterworld.SetActive(true);
        afterWorldEnemyGO.SetActive(false);
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Da bist du. Noch atmend. Noch hoffend. Lächerlich.", chronosSWDM, true, clearDialoguechronosSWDM)));
    }

    void st1016Trigger() 
    {
        escMC.gameData.story_id = "1018";
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Oh? Ein letzter Funke Widerstand? Dann lass mich ihn auslöschen!", chronosSWDM, true, st1017Trigger)));
    }

    // DEBUG 12.03.2025 v v v

    void st1017Trigger()
    {
        escMC.gameData.story_id = "1019";
        StartCoroutine(attackDetectionAfterWorld());
        chronosEnemy.active = true;

    }

    IEnumerator attackDetectionAfterWorld()
    {
        bool enemyAttackDetectionA = true;
        float enemyHealth = chronosEnemy.health;
        while (enemyAttackDetectionA)
        {
            if (chronosEnemy.health <= 0)
            {
                enemyAttackDetectionA = false;
                st1018Trigger();
                yield break;
            }
            if (checkStoryID("1001"))
            {
                enemyAttackDetectionA = false;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void st1018Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Nein... Nein! ICH bin die Zeit! ICH kann nicht...", chronosSWDM, true, st1019Trigger)));
    }

    void st1019Trigger()
    {
        escMC.gameData.story_id = "1020";
        chronosSWDM.ext = true; 
        K1.SetActive(false);
        K2.SetActive(false);
        K3.SetActive(false);
        K4.SetActive(false);
        portalZEUS.SetActive(false);
        portalAfterworld.SetActive(false);
        taschenuhrAfterworld.SetActive(false);
        afterWorldEnemyGO.SetActive(true);
    }

    // END ENDGAMEPLAY











    void showCredit()
    {
        creditScreenPanel.gameObject.SetActive(true);
    }

    public void OnTriggerEnter2DGateway(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collisionTag == "DEBUG raetsel") 
            {
                escMC.gameData.story_id = "3";
            }
            if (collisionTag == "taschenuhrProlog")
            {
                escMC.gameData.story_id = "5";
            }
            if (collisionTag == "cutSzeneTrigger")
            {
                escMC.gameData.story_id = "6";
            }
            if (collisionTag == "tpStartgebietTrigger")
            {
                escMC.gameData.story_id = "8";
            }
        }
    }
}
