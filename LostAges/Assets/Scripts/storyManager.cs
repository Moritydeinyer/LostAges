using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using HeneGames.DialogueSystem;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Cainos.PixelArtTopDown_Basic;
using System;
using Unity.Collections;
using UnityEditor;
using Pathfinding;
using UnityEngine.UIElements;

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
    [SerializeField] public Enemy chronosEnemy;

    [Header("Trigger")]
    [SerializeField] private doorToggle doorSt3;
    [SerializeField] private doorToggle doorSt4;
    [SerializeField] private doorToggle doorSt5;

    
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
    [SerializeField] private GameObject portalZEUS;
    [SerializeField] public GameObject afterWorldSPZEUS;
    [SerializeField] private GameObject afterWorldSPCHRONOS_I;
    [SerializeField] private GameObject afterWorldSPCHRONOS_II;
    [SerializeField] private GameObject afterWorldSPZEUS_I;
    [SerializeField] private GameObject playerSPkathedraleafterWorld;
    [SerializeField] public GameObject afterWorldSPplayerCHRONOS;
    [SerializeField] private GameObject portalAfterworld;
    [SerializeField] private GameObject taschenuhrAfterworld;
    [SerializeField] private Enemy enemyZEUS;
    [SerializeField] public Enemy afterWorldEnemy;
    [SerializeField] public Enemy demoENEMY;
    [SerializeField] public GameObject afterWorldEnemyGO;
    [SerializeField] public GameObject CHwp1combatTutorial;
    [SerializeField] public GameObject CHwp2combatTutorial;
    [SerializeField] public GameObject CHwp1endfight;
    [SerializeField] public GameObject CHwp2endfight;

    [SerializeField] public InteractionScript intKugel1;
    [SerializeField] public InteractionScript intKugel2;
    [SerializeField] public InteractionScript intKugel3;

    [SerializeField] public Enemy mo1;
    [SerializeField] public Enemy mo2;


    [Header("Startwelt")]
    [SerializeField] private SpriteRenderer kristall;

    [Header("Prolog")]
    [SerializeField] private GameObject mapPROLOG;
    [SerializeField] public GameObject SpawnPoint;
    [SerializeField] public GameObject SpawnPointH2;
    [SerializeField] public GameObject entranceDoorH3;
    [SerializeField] public GameObject entranceDoorH4;
    [SerializeField] public GameObject SpawnPoint4;
    [SerializeField] public GameObject SpawnPoint2;
    [SerializeField] public GameObject SpawnPoint3;
    [SerializeField] private BoxCollider2D TriggerI;
    [SerializeField] private BoxCollider2D TriggerII;
    [SerializeField] private GameObject Wanduhr;
    [SerializeField] private BoxCollider2D TriggerIII;
    [SerializeField] private GameObject Taschenuhr;
    [SerializeField] private BoxCollider2D TriggerIV;
    [SerializeField] private GameObject H1;
    [SerializeField] private GameObject H2;
    [SerializeField] private GameObject H3;
    [SerializeField] private GameObject H4;
    [SerializeField] private GameObject obj1;
    [SerializeField] private GameObject obj2;
    [SerializeField] private GameObject obj3;
    [SerializeField] private GameObject obj4;
    [SerializeField] private GameObject obj5;
    [SerializeField] private GameObject obj6;
    [SerializeField] private GameObject obj7;
    [SerializeField] private GameObject obj8;
    [SerializeField] private GameObject obj9;
    [SerializeField] private GameObject obj10;
    [SerializeField] private Vector3 obj1Pos;
    [SerializeField] private Vector3 obj2Pos;
    [SerializeField] private Vector3 obj3Pos;
    [SerializeField] private Vector3 obj4Pos;
    [SerializeField] private Vector3 obj5Pos;
    [SerializeField] private Vector3 obj6Pos;
    [SerializeField] private Vector3 obj7Pos;
    [SerializeField] private Vector3 obj8Pos;
    [SerializeField] private Vector3 obj9Pos;
    [SerializeField] private Vector3 obj10Pos;

    [Header("Startwelt")]
    [SerializeField] public GameObject mapSTARTWELT;
    [SerializeField] private GameObject SP10;
    [SerializeField] private GameObject SPChronosCombatTutorial; 
    [SerializeField] private GameObject SPChronosKristall; 
    [SerializeField] private GameObject SPEnemyCombatTutorial;

    [SerializeField] private GameObject CHRONOS;
    [SerializeField] public Enemy kampfTutorialEnemy;
    [SerializeField] public GameObject kampfTutorialEnemyGO;

    [SerializeField] private GameObject glowingM1;
    [SerializeField] private GameObject glowingM2;
    [SerializeField] private GameObject glowingM3;
    [SerializeField] private GameObject glowingM4;
    
    [SerializeField] private SpriteRenderer kristallStart;
    [SerializeField] private Sprite k0;
    [SerializeField] private Sprite k1;
    [SerializeField] private Sprite k2;
    [SerializeField] private Sprite k3;
    [SerializeField] private Sprite k4;

    [SerializeField] private GameObject k1marker;
    [SerializeField] private GameObject k2marker;
    [SerializeField] private GameObject k3marker;

    [SerializeField] private GameObject SRm1;
    [SerializeField] private List<Vector3> SRm1originalPosition = new List<Vector3>();

    [SerializeField] private BoxCollider2D barrierK1;
    [SerializeField] private BoxCollider2D barrierK2;
    [SerializeField] private BoxCollider2D barrierK3;

    [SerializeField] public GameObject SPTBC;

    [Header("TBC")]
    [SerializeField] public GameObject SPTBC1;
    [SerializeField] public GameObject mapTBC;
    [SerializeField] public bool updateNav = true;
    [SerializeField] private GameObject K2KugelItem;

    [Header("KE")]
        [SerializeField] private DialogueCharacter Leader_K1_DC;
        [SerializeField] private DialogueCharacter Leader_K2_DC;
        [SerializeField] private DialogueCharacter Leader_K3_DC;
        [SerializeField] private DialogueManager Leader_K1_DM;
        [SerializeField] private DialogueManager Leader_K2_DM;
        [SerializeField] private DialogueManager Leader_K3_DM;
        [SerializeField] private DialogueCharacter KE_DC;
        [SerializeField] private DialogueManager KE_DM;

    [Header("K1")]
        [SerializeField] private GameObject schriftrolle;
        [SerializeField] private GameObject schriftrolleUI;
        [SerializeField] private Enemy Monster;
        [SerializeField] private GameObject RunenEntziffernUIPanel;
        [SerializeField] private UnityEngine.UI.Image[] runeImages;
        [SerializeField] private RuneDatabase runeDatabase;
        [SerializeField] private TMP_InputField runeInputField;
        [SerializeField] private UnityEngine.UI.Button runeSubmitButton;
        [SerializeField] private GameObject geschafftAnzeige;
        [SerializeField] private GameObject nichtGeschafftAnzeige;
        [SerializeField] private GameObject pflanze;

    [Header("K2")]
        [Header("Verhaftung")]
        [SerializeField] private GameObject verhaftungsTriggerK2;
        [SerializeField] private GameObject zelleTriggerK2;
        [SerializeField] private Enemy Wache1K2;
        [SerializeField] private Enemy Wache2K2;
        [SerializeField] private Enemy playerDummyK2;
        [SerializeField] private GameObject wp1_1w1;
        [SerializeField] private GameObject wp1_2w2;
        [SerializeField] private GameObject wp1_3pl;
        [SerializeField] private GameObject wp2_1w1;
        [SerializeField] private GameObject wp2_2w2;
        [SerializeField] private GameObject wp2_3pl;
        [SerializeField] private BoxCollider2D triggerTPTBC;

    [Header("K3")]
        [Header("Verhaftung")]
        [SerializeField] public GameObject VerhaftungGO;
        [SerializeField] private CameraFollow mainCameraFollow;
        [SerializeField] private GameObject verhaftungsTrigger;
        [SerializeField] private GameObject zelleTrigger;
        [SerializeField] private Enemy Wache1;
        [SerializeField] private Enemy Wache2;
        [SerializeField] private Enemy playerDummy;
        [SerializeField] private GameObject wp1_1;
        [SerializeField] private GameObject wp1_2;
        [SerializeField] private GameObject wp1_3;
        [SerializeField] private GameObject wp2_1;
        [SerializeField] private GameObject wp2_2;
        [SerializeField] private GameObject wp2_3;

        [Header("ARENA Fight")]
        [SerializeField] private GameObject wp_arena_player;
        [SerializeField] private TextMeshProUGUI arenaRundenText;
        private int arenaRunden = 3;
        private int arenaCurrentRunde = 0;
        [SerializeField] private GameObject[] enemyArenaK3;
        [SerializeField] private GameObject initEnemyArenaK3;
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private GameObject arenaWinTP;
        [SerializeField] private GameObject arenaVisitors;

    [Header("System")]
    [SerializeField] private GameObject Kugel1UI;
    [SerializeField] public GameObject Kugel2UI;
    [SerializeField] private GameObject Kugel3UI;
    [SerializeField] private GameObject K3VerhaftedText;
    public string nameMain = "Gresmond Zimmermann";
    public string vornameMain;
    public string nachnameMain;

        [Header("Afterworld")]
        [SerializeField] public GameObject AfterWorldMap;
        [SerializeField] public GameObject SPAfterWorld;
    public float h, s, v;


    void Start()
    {
        nachnameMain = nameMain.Split(' ').Last();
        vornameMain = nameMain.Substring(0, nameMain.Length - nachnameMain.Length - 1);
        initRunes();
        obj1Pos = obj1.transform.position;
        obj2Pos = obj2.transform.position;
        obj3Pos = obj3.transform.position;
        obj4Pos = obj4.transform.position;
        obj5Pos = obj5.transform.position;
        obj6Pos = obj6.transform.position;
        obj7Pos = obj7.transform.position;
        obj8Pos = obj8.transform.position;
        obj9Pos = obj9.transform.position;
        obj10Pos = obj10.transform.position;
        SRm1originalPosition.Clear();
        foreach (Transform child in SRm1.transform)
        {
            SRm1originalPosition.Add(child.position);
        }
        runeSubmitButton.onClick.AddListener(SubmitRuneInput);
    }

    void Update()
    {
        if (updateNav)
        {
            string filename;
            if (mapTBC.activeSelf) filename = "tbc_nav";
            else filename = "start_nav";

            escMC.level = 0;
            if (checkStoryID("K1_done")) escMC.level++;
            if (checkStoryID("K2_done")) escMC.level++;
            if (checkStoryID("K3_done")) escMC.level++;

            //if (AfterWorldMap.activeSelf) filename = "afterworld_nav";
            TextAsset graphData = Resources.Load<TextAsset>(filename);
            if (graphData != null)
            {
                AstarPath.active.data.DeserializeGraphs(graphData.bytes);
                Debug.Log("Graph geladen: " + filename);
            }
            else
            {
                Debug.LogError("Graph-Datei nicht gefunden: " + filename);
            }
            updateNav = false;
        }




        // DEBUG Task UI Management
        if (checkStoryID("116")) { escMC.taskPanelData.text = "\n\nKampftutorial abgeschlossen"; }

        if (checkStoryID("116_1m")) { escMC.taskPanelData.text += "Monument 1 aktiviert\n"; }
        if (checkStoryID("116_2m")) { escMC.taskPanelData.text += "Monument 2 aktiviert\n"; }
        if (checkStoryID("116_3m")) { escMC.taskPanelData.text += "Monument 3 aktiviert\n"; }
        if (checkStoryID("116_4m")) { escMC.taskPanelData.text += "Monument 4 aktiviert\n"; }

        if (checkStoryID("K1")) { escMC.taskPanelData.text = ""; } //DEBUG
        if (checkStoryID("K2")) { escMC.taskPanelData.text = ""; } //DEBUG
        if (checkStoryID("K3")) { escMC.taskPanelData.text = ""; } //DEBUG

        if (checkStoryID("126")) { escMC.taskPanelData.text = "\n\nLade den Kristall durch Aktivieren der Monumente auf"; session = true; }
        if (checkStoryIDgraterThan("129") && !(checkStoryID("K1") || checkStoryID("K2") || checkStoryID("K3"))) { escMC.taskPanelData.text = "\n\nSuche die Kugeln der Zeit"; }





        if (checkStoryID("0")) { st0Trigger(); }
        if (checkStoryID("1") && !session)
        {
            kampfTutorialEnemy.player.transform.position = SpawnPoint.transform.position;
            st0Trigger();
        }
        if (checkStoryID("2")) { st2Trigger(); }
        if (checkStoryID("3") && !session)
        {
            kampfTutorialEnemy.player.transform.position = SpawnPointH2.transform.position;
            st2Trigger();
        }
        if (checkStoryID("4")) { st4Trigger(); }
        if ((checkStoryID("5") || checkStoryID("6")) && !session)
        {
            st4Trigger();
        }
        if (checkStoryID("6") && session) { st6Trigger(); }
        if (checkStoryID("7")) { st7Trigger(); }
        if ((checkStoryID("8") || checkStoryID("9")) && !session)
        {
            st7Trigger();
        }
        if (checkStoryID("9") && session) { st9Trigger(); }
        if (checkStoryID("10")) { st10Trigger(); }
        if (checkStoryID("11") && !session)
        {
            st10Trigger();
        }
        if (checkStoryIDsmallerThan("10"))
        {
            miniMap.SetActive(false);
            mapPROLOG.SetActive(true);
            mapSTARTWELT.SetActive(false);
            CHRONOS.SetActive(false);
        }
        if (checkStoryIDsmallerThan("8"))
        {
            taschenuhrUI.SetActive(false);
        }
        if (checkStoryIDgraterThan("11"))
            {
                mapPROLOG.SetActive(false);
                if (checkStoryIDgraterThan("8011") && checkStoryIDsmallerThan("8017"))
                {
                    taschenuhrUI.SetActive(false);
                }
                else
                {
                    taschenuhrUI.SetActive(true);
                }
            }

        if (!checkStoryIDgraterThan("8018"))
        {
            mo1.gameObject.SetActive(false);
            mo2.gameObject.SetActive(false);
        }


        if (checkStoryIDgraterThan("116") || checkStoryIDsmallerThan("100"))
        {
            kampfTutorialEnemyGO.gameObject.SetActive(false);
        }
        if (checkStoryID("100"))
        {
            st100Trigger();
        }
        if ((checkStoryID("101") && !session) || (checkStoryID("102") && !session))
        {
            st100Trigger();
        }
        if (checkStoryID("102") || checkStoryID("118"))
        {
            DialogueUI.instance.ShowInteractionUI(false);
        }

        if (checkStoryID("116"))
        {
            st117Trigger();
        }
        if ((checkStoryID("118") || checkStoryID("117")) && !session)
        {
            st117Trigger();
        }

        if (checkStoryID("127")) { st127Trigger(); }
        if (checkStoryID("128") && !session) { st127Trigger(); }

        int tempCount = 0;
        if (checkStoryID("116_1m") || checkStoryIDgraterThan("127")) { glowingM1.SetActive(true); tempCount++; } else { glowingM1.SetActive(false); }
        if (checkStoryID("116_2m") || checkStoryIDgraterThan("127")) { glowingM2.SetActive(true); tempCount++; } else { glowingM2.SetActive(false); }
        if (checkStoryID("116_3m") || checkStoryIDgraterThan("127")) { glowingM3.SetActive(true); tempCount++; } else { glowingM3.SetActive(false); }
        if (checkStoryID("116_4m") || checkStoryIDgraterThan("127")) { glowingM4.SetActive(true); tempCount++; } else { glowingM4.SetActive(false); }

        Color.RGBToHSV(kristallStart.color, out h, out s, out v);
        if (tempCount == 4)
        {
            v = 1f;
            if (checkStoryID("126")) { escMC.gameData.story_id = "127"; }
        }
        else
        {
            barrierK1.enabled = true;
            barrierK2.enabled = true;
            barrierK3.enabled = true;
        }
        if (tempCount == 3) { v = 0.9f; } else if (tempCount == 2) { v = 0.8f; } else if (tempCount == 1) { v = 0.7f; } else if (tempCount == 0) { v = 0.6f; }
        kristallStart.color = Color.HSVToRGB(h, s, v);

        if (checkStoryIDgraterThan("100") && !session && checkStoryIDsmallerThan("127"))
        {
            int i = 0;
            // reset schieberaetsel Monument 1
            foreach (Transform child in SRm1.transform)
            {
                if (i < SRm1originalPosition.Count)
                {
                    child.position = SRm1originalPosition[i];
                    i++;
                }
            }
        }



        if (checkStoryIDgraterThan("129"))
        {
            barrierK1.enabled = false;
            barrierK2.enabled = false;
            barrierK3.enabled = false;
            k1marker.gameObject.SetActive(true);
            k2marker.gameObject.SetActive(true);
            k3marker.gameObject.SetActive(true);
            miniMap.SetActive(true);
            escMC.playerController.fogStartMitK.SetActive(true);
            escMC.playerController.fogStartOhneK.SetActive(false);

            //DEBUG
            if (!checkStoryID("K1") && !checkStoryID("K2") && !checkStoryID("K3"))
            {
                if (!checkStoryID("K3_done"))
                {
                    VerhaftungGO.SetActive(true);
                    verhaftungsTrigger.SetActive(true);
                }
            }
        }
        else
        {
            miniMap.SetActive(false);
            k1marker.gameObject.SetActive(false);
            k2marker.gameObject.SetActive(false);
            k3marker.gameObject.SetActive(false);
            escMC.playerController.fogStartMitK.SetActive(false);
            escMC.playerController.fogStartOhneK.SetActive(true);
        }

        if (checkStoryID("18700"))
        {
            AfterWorldMap.SetActive(true);
            afterWorldEnemyGO.SetActive(true);
        }
        else if (!checkStoryIDgraterThan("8000")) { AfterWorldMap.SetActive(false); }






        if (checkStoryID("K1_done") && checkStoryID("K2_done") && checkStoryID("K3_done") && !checkStoryID("7999"))
        {
            addStoryID("7999");
        }





        if (checkStoryID("K1_temp"))
        {
            if (!(checkStoryID("K2") || checkStoryID("K3")))
            {
                addStoryID("K1");
                addStoryID("K1_1");
            }
            rmStoryID("K1_temp");
        }

        if (checkStoryID("K1_1"))
        {
            if (!checkStoryID("KE") && !checkStoryID("KE_temp"))
            {
                addStoryID("KE_temp");
                session = true;
                startDialogueKE();
            }
            if (!checkStoryID("K1_2"))
            {
                addStoryID("K1_2");
            }
        }

        if (checkStoryID("K1_2") && checkStoryID("KE"))
        {
            rmStoryID("K1_1");
            rmStoryID("K1_2");
            stK1_1Trigger();
        }

        if (checkStoryID("K1_3") && !session)
        {
            rmStoryID("K1_3");
            addStoryID("K1_2");
        }

        if (checkStoryID("K1_10"))
        {
            //DEUBG Labyrinth ...
            schriftrolle.gameObject.SetActive(true);
            
            
            Monster.health = Monster.maxHealth;
        }
        else
        {
            schriftrolle.gameObject.SetActive(false);
            
            
        }

        if (checkStoryID("K1_11"))
        {
            Monster.gameObject.SetActive(true);
            schriftrolleUI.gameObject.SetActive(true);
        }
        else
        {
            Monster.gameObject.SetActive(false);
        }

        if (checkStoryID("K1_13"))
        {
            RunenEntziffernUIPanel.SetActive(true);
            if (runeInputField.isFocused)
            {
                escMC.playerController.act = false;
            }
            else
            {
                escMC.playerController.act = true;
            }
        }
        else
        {
            RunenEntziffernUIPanel.SetActive(false);
        }
        if (checkStoryID("K1_11") || checkStoryID("K1_13") || checkStoryID("K1_14") || checkStoryID("K1_15") || checkStoryID("K1_12") || checkStoryID("K1_done"))
        {
            schriftrolleUI.gameObject.SetActive(true);
        }
        else
        {
            schriftrolleUI.gameObject.SetActive(false);
        }

        if (checkStoryID("K1_14"))
        {
            escMC.playerController.act = true;
            pflanze.SetActive(true);
            // rmStoryID("K1_14");
            // addStoryID("K1_15");
            // addStoryID("K1_done");
        }
        else
        {
            pflanze.SetActive(false);
        }

        if (checkStoryID("K1_15"))
        {
            rmStoryID("K1_14");
            rmStoryID("K1_15");
            addStoryID("K1_done");
        }

        if (checkStoryID("K1_done"))
        {
            rmStoryID("K1");
            Kugel1UI.SetActive(true);
        }
        else
        {
            if (!checkStoryIDgraterThan("8000"))
            {
                Kugel1UI.SetActive(false);
            }
            }

        // DEBUG TEST FEHLT ^^^










        if (!(checkStoryID("K2_done") || checkStoryID("K2_51") || checkStoryID("K2_52") || checkStoryID("K2_53") || checkStoryID("K2_54") || checkStoryID("K2_55") || checkStoryID("K2_61")))
        {
            if (!checkStoryIDgraterThan("8000"))
            {
                Kugel2UI.SetActive(false);
            }
        }

        if (checkStoryID("K2_temp") && !(checkStoryID("K1") || checkStoryID("K3")))
        {
            addStoryID("K2");
            addStoryID("K2_1");
        }
        if (checkStoryID("K2"))
        {
            verhaftungsTriggerK2.SetActive(false);
            Leader_K2_DM.gameObject.SetActive(true); 
            escMC.gameData.respw = SpawnPoint.transform.position.x + ";" + SpawnPoint.transform.position.y; 
        }
        else
        {
            Leader_K2_DM.gameObject.SetActive(false);
            verhaftungsTriggerK2.SetActive(true);
            if (checkStoryID("K2_done"))
            {
                verhaftungsTriggerK2.SetActive(false);
            }
            Wache1K2.gameObject.SetActive(false);
            Wache2K2.gameObject.SetActive(false);
            playerDummyK2.gameObject.SetActive(false);
        }
        if (checkStoryID("K2_1"))
        {
            stK2_1Trigger();
            mapSTARTWELT.SetActive(false);
        }

        if (checkStoryID("K2_2") && !session)
        {
            rmStoryID("K2_2");
            addStoryID("K2_1");
        }

        if (checkStoryID("K2_3") && !checkStoryID("KE_temp"))
        {
            rmStoryID("K2_2");
            Wache1K2.active = false;
            Wache2K2.active = false;
            playerDummyK2.active = false;
            escMC.playerController.transform.position = playerDummyK2.gameObject.transform.position;
            escMC.playerController.spRenderer.enabled = true;
            escMC.playerController.act = true;
            playerDummyK2.gameObject.SetActive(false);
            mainCameraFollow.target = escMC.playerController.transform;
            addStoryID("K2_4");
        }

        if (checkStoryID("K2_4"))
        {
            stK2_4Trigger();
            rmStoryID("K2_4");
            rmStoryID("K2_3");
            addStoryID("K2_5");
        }

        if (checkStoryID("K2_5") && !session)
        { 
            rmStoryID("K2_5");
            addStoryID("K2_3");
        }

        if (checkStoryID("K2_6"))
        {
            if (!checkStoryID("KE") && !checkStoryID("KE_temp"))
            {
                addStoryID("KE_temp");
                session = true;
                startDialogueKE();
            }
            if (!checkStoryID("K2_40"))
            {
                addStoryID("K2_40");
            }
        }

        if (checkStoryID("KE") && checkStoryID("K2_40"))
        {
            rmStoryID("K2_40");
            rmStoryID("K2_6");
            Wache1K2.gameObject.SetActive(false);
            Wache2K2.gameObject.SetActive(false);
            addStoryID("K2_41");
        }

        if (checkStoryID("K2_41"))
        {
            stK2_41Trigger();
            addStoryID("K2_42");
            rmStoryID("K2_41");
        }

        if (checkStoryID("K2_42") && !session)
        {
            rmStoryID("K2_42");
            addStoryID("K2_41");
        }

        if (checkStoryID("K2_51"))
        {
            rmStoryID("K2_42");
            triggerTPTBC.isTrigger = true;
        }
        else
        {
            triggerTPTBC.isTrigger = false;
        }

        if (checkStoryID("K2_52"))
        {
            rmStoryID("K2_51");
            if (!session)
            {
                K2KugelItem.SetActive(true);
                Kugel2UI.SetActive(false);        
                mapSTARTWELT.SetActive(false);
                mapTBC.SetActive(true);
                updateNav = true;
            }
        }

        if (checkStoryID("K2_53"))
        {
            rmStoryID("K2_52");
            Leader_K2_DM.gameObject.SetActive(true);
        }

        if (checkStoryID("K2_54"))
        {
            rmStoryID("K2_53");
            Leader_K2_DM.gameObject.SetActive(true);
            stK2_54Trigger();
        }

        if (checkStoryID("K2_55") && !session)
        {
            rmStoryID("K2_55");
            addStoryID("K2_54");
        }

        if (checkStoryID("K2_61"))
        {
            Leader_K2_DM.gameObject.SetActive(false);
            rmStoryID("K2_61");
            addStoryID("K2_done");
            rmStoryID("K2");
        }
        
        if (checkStoryID("K2_done"))
        {
            Kugel2UI.SetActive(true);
            Leader_K2_DM.gameObject.SetActive(false);
        }














        if (checkStoryID("K3_temp") && !(checkStoryID("K2") || checkStoryID("K1")))
        {
            addStoryID("K3");
            addStoryID("K3_1");
        }
        if (checkStoryID("K3"))
        {
            VerhaftungGO.SetActive(true);
            verhaftungsTrigger.SetActive(false);
            Leader_K3_DM.gameObject.SetActive(true);
            escMC.gameData.respw = SpawnPoint.transform.position.x + ";" + SpawnPoint.transform.position.y; 
        }
        else
        {
            Leader_K3_DM.gameObject.SetActive(false);
            VerhaftungGO.SetActive(false);
            verhaftungsTrigger.SetActive(true);
            if (checkStoryID("K3_done"))
            {
                VerhaftungGO.SetActive(false);
                verhaftungsTrigger.SetActive(false);
            }
            Wache1.gameObject.SetActive(false);
            Wache2.gameObject.SetActive(false);
            playerDummy.gameObject.SetActive(false);
        }
        if (checkStoryID("K3_1"))
        {
            stK3_1Trigger();
        }

        if (checkStoryID("K3_2") && !session)
        {
            rmStoryID("K3_2");
            addStoryID("K3_1");
        }

        if (checkStoryID("K3_3") && !checkStoryID("KE_temp"))
        {
            rmStoryID("K3_2");
            Wache1.active = false;
            Wache2.active = false;
            playerDummy.active = false;
            escMC.playerController.transform.position = playerDummy.gameObject.transform.position;
            escMC.playerController.spRenderer.enabled = true;
            escMC.playerController.act = true;
            playerDummy.gameObject.SetActive(false);
            mainCameraFollow.target = escMC.playerController.transform;
            //DIALOG
            //ANFUEHRER K3 + KE?
            // im Gefangnis...
            if (!checkStoryID("KE") && !checkStoryID("KE_temp"))
            {
                addStoryID("KE_temp");
                session = true;
                startDialogueKE();
            }
            if (!checkStoryID("K3_40"))
            {
                addStoryID("K3_40");
            }
        }

        if (checkStoryID("KE_temp") && !session)
        {
            rmStoryID("KE_temp");
        }









        if (checkStoryID("K3_40") && checkStoryID("KE"))
        {
            rmStoryID("K3_3");
            rmStoryID("K3_40");
            Wache1.gameObject.SetActive(false);
            Wache2.gameObject.SetActive(false);
            escMC.TeleportWithFade(wp_arena_player.transform.position, escMC.playerController.gameObject);
            arenaCurrentRunde = 0;
            addStoryID("K3_41");
            //RESPOWN POINT get_killed ARENA... ++ STORYLINE ?? DEBUG
        }

        // ARENA FIGHT
        if (checkStoryID("K3_41"))
        {
            stK3_41Trigger();
            arenaVisitors.SetActive(true);
        }
        else
        {
            arenaRundenText.gameObject.SetActive(false);
            arenaVisitors.SetActive(false);
        }

        if (checkStoryID("K3_42"))
        {
            Debug.Log("DEBUG K3_42");
            escMC.playerController.transform.position = arenaWinTP.transform.position;
            //DEBUG gewonnen ARENA --> DIALOG leader ??
            addStoryID("K3_done");
            rmStoryID("K3_42");
            rmStoryID("K3");
        }

        if (checkStoryID("K3_done"))
        {
            Kugel3UI.SetActive(true);
        }
        else
        {
            if (!checkStoryIDgraterThan("8000"))
            {
                Kugel3UI.SetActive(false);
            }
        }













        if (checkStoryIDgraterThan("8016"))
        {
            portalAfterworld.SetActive(false);
        }

        if (checkStoryIDsmallerThan("7999"))
            {
                chronosEnemy.active = false;
                chronosEnemy.attackable = false;
                portalZEUS.SetActive(false);
                portalAfterworld.SetActive(false);
                afterWorldEnemyGO.SetActive(true);
                taschenuhrAfterworld.SetActive(false);
                enemyZEUS.gameObject.SetActive(false);
            }

        if (checkStoryIDgraterThan("8019") && checkStoryIDsmallerThan("8021"))
        {
            CHRONOS.GetComponent<Enemy>().patrolPoints[0] = CHwp1endfight.transform;
            CHRONOS.GetComponent<Enemy>().patrolPoints[1] = CHwp2endfight.transform;
        }
        else
        {
            CHRONOS.GetComponent<Enemy>().patrolPoints[0] = CHwp1combatTutorial.transform;
            CHRONOS.GetComponent<Enemy>().patrolPoints[1] = CHwp2combatTutorial.transform;
        }

        if (checkStoryID("8000"))
        {
            st8000Trigger();
        }
        if (checkStoryIDgraterThan("8000") && !session && checkStoryIDsmallerThan("8021") && !session) 
        {
            st8000Trigger();
        } 
        if (checkStoryID("8002"))
        {
            Kugel1UI.SetActive(true);
            Kugel2UI.SetActive(true);
            Kugel3UI.SetActive(true);
            intKugel1.enabled = true;
            intKugel2.enabled = false;
            intKugel3.enabled = false;
        }
        if (checkStoryID("8003"))
        {
            Kugel1UI.SetActive(false);
            intKugel1.enabled = false;
            intKugel2.enabled = true;
            intKugel3.enabled = false;
        }
        if (checkStoryID("8004"))
        {
            Kugel2UI.SetActive(false);
            intKugel1.enabled = false;
            intKugel2.enabled = false;
            intKugel3.enabled = true;
        }
        if (checkStoryID("8005"))
        {
            st8005Trigger();
            Kugel3UI.SetActive(false);
        }
        if (checkStoryID("8007"))
        {
            st8007Trigger();
        }
        if (checkStoryID("8015"))
        {
            st8015Trigger();
        }
        if (checkStoryID("8017"))
        {
            st8016Trigger();
        }
        if (checkStoryID("8001"))
        {
            DialogueUI.instance.ShowInteractionUI(false);
        } 
        if (checkStoryID("8020"))
        {
            escMC.gameData.story_id = "8021";
            escMC.playerController.act = false;
            showCredit();
        }
        if (checkStoryID("8021"))
        {
            if (credits.anchoredPosition.y >= 3140)
            {
                creditScript cS = credits.GetComponent<creditScript>();
                cS.enabled = false;
                escMC.gameData.story_id = "8022";
                session = false;
                // END GAMEPLAY DEBUG
            }
        }
    }

    Vector3 GetRandomPointInTransformArea(Transform areaTransform)
    {
        Vector3 scale = areaTransform.localScale;
        Vector3 localOffset = new Vector3(
            UnityEngine.Random.Range(-0.5f, 0.5f) * scale.x,
            UnityEngine.Random.Range(-0.5f, 0.5f) * scale.y,
            UnityEngine.Random.Range(-0.5f, 0.5f) * scale.z
        );
        return areaTransform.TransformPoint(localOffset);
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

    IEnumerator titleFadeOut()
    {
        escMC.audioManager.PlayTitleRevealSFX();
        yield return new WaitForSeconds(2.65f);
        titleScreenPanelTxt1.alpha = 1f;
        titleScreenPanelTxt2.alpha = 1f;
        titleScreenPanel.SetActive(true);
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
    /// PROLOG
    /// </summary>
    /// <returns></returns>
  
    void st0Trigger()
    {
        Debug.Log("DEBUG st0Trigger()");
        session = true;
        H1.SetActive(true);
        H2.SetActive(false);
        H3.SetActive(false);
        H4.SetActive(false);
        TriggerI.isTrigger = true;
        kampfTutorialEnemy.player.transform.position = SpawnPoint.transform.position;
        obj1.transform.position = obj1Pos;
        obj2.transform.position = obj2Pos;
        obj3.transform.position = obj3Pos;
        obj4.transform.position = obj4Pos;
        obj5.transform.position = obj5Pos;
        obj6.transform.position = obj6Pos;
        obj7.transform.position = obj7Pos;
        escMC.gameData.story_id = "1";
    }

    void st2Trigger()
    {
        session = true;
        H1.SetActive(false);
        H2.SetActive(true);
        H3.SetActive(false);
        H4.SetActive(false);
        TriggerII.isTrigger = true;
        kampfTutorialEnemy.player.transform.position = SpawnPoint2.transform.position;
        obj8.transform.position = obj8Pos;
        obj9.transform.position = obj9Pos;
        obj10.transform.position = obj10Pos;
        escMC.gameData.story_id = "3";
    }

    void st4Trigger()
    {
        session = true;
        H1.SetActive(false);
        H2.SetActive(false);
        H3.SetActive(true);
        H4.SetActive(false);
        TriggerIII.isTrigger = false;
        entranceDoorH3.SetActive(false);
        kampfTutorialEnemy.player.transform.position = SpawnPoint3.transform.position;
        escMC.gameData.story_id = "5";
    }

    void st6Trigger()
    {   
        if (TriggerIII.isTrigger != true) 
        {
            TriggerIII.isTrigger = true;
            entranceDoorH3.SetActive(true);
            Debug.Log("DEBUG TriggerIII ist aktiv");
        }
    }

    void st7Trigger()
    {
        session = true;
        H1.SetActive(false);
        H2.SetActive(false);
        H3.SetActive(false);
        H4.SetActive(true);
        TriggerIV.isTrigger = false;
        entranceDoorH4.SetActive(false);
        kampfTutorialEnemy.player.transform.position = SpawnPoint4.transform.position;
        escMC.gameData.story_id = "8";
    }

    void st9Trigger()
    {
        taschenuhrUI.SetActive(true);
        if (TriggerIV.isTrigger != true)
        {
            TriggerIV.isTrigger = true;
            entranceDoorH4.SetActive(true);
            //deaktivate interaction ui
            //remove taschenuhr...
        }
    }

    void st10Trigger()
    {
        escMC.gameData.story_id = "11";
        StartCoroutine(titleFadeOut());
        kampfTutorialEnemy.player.transform.position = SP10.transform.position;
        Debug.Log("DEBUG prolog ende");
        escMC.gameData.story_id = "100"; //DEBUG
    }





    /// <summary>   
    /// KAMPFTUTORIAL     
    /// </summary>
    /// <returns></returns>

    void enemyFreeze(bool freeze)
    {
        kampfTutorialEnemy.active = !freeze;
    }


    void st100Trigger()
    {
        CHRONOS.SetActive(true);
        chronosSWDM.StopDialogue();
        chronosSWDM.ext = true;
        List<NPC_Centence> sentences = new List<NPC_Centence>();
        NPC_Centence sentence = new NPC_Centence();
        chronosSWDM.sentences = sentences;
        session = true;
        escMC.gameData.story_id = "101";
        mapSTARTWELT.SetActive(true);
        CHRONOS.transform.position = SPChronosCombatTutorial.transform.position;
        kampfTutorialEnemyGO.gameObject.SetActive(false);
        escMC.gameData.respw = SpawnPoint.transform.position.x + ";" + SpawnPoint.transform.position.y; 
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Ein Krieger lernt nicht durch Worte, sondern durch Kampf. Also, verschwenden wir keine Zeit mit Reden - du wirst überleben oder untergehen. Deine Prüfung beginnt ... jetzt!", chronosSWDM, false, st101Trigger)));
    }

    void st101Trigger()
    {    
        escMC.gameData.story_id = "102";
        kampfTutorialEnemyGO.gameObject.SetActive(true);                                  
        kampfTutorialEnemyGO.gameObject.transform.position = SPEnemyCombatTutorial.transform.position;
        kampfTutorialEnemy.health = 1f; 
        chronosSWDM.ext = false;                       
        enemyFreeze(true);
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Dein erster Feind - ein einfacher Trainingsgegner. Aber unterschätze ihn nicht!", chronosSWDM, true, st103Trigger)));
    }

    void st103Trigger()
    {    
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Weiche aus! Ein echter Gegner wartet nicht, dass du dich in Position bringst!", chronosSWDM, true, st104Trigger)));
        kampfTutorialEnemy.patrolPoints[1].position = kampfTutorialEnemy.player.position;         
    }

    void st104Trigger()
    {
        string action = "ctrl";
        if (escMC.useController) {if (escMC.controllerType == "Xbox") {action = "RB";} else {action = "RB";}}
        string data = "Dash mit " + action;
        kampfTutorialEnemy.patrolPoints[1].position = kampfTutorialEnemy.player.position; 
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

    void st105Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Zu langsam! Versuch es nochmal - konzentriere dich auf die Bewegung deines Gegners!", chronosSWDM, true, stDashWdh)));
    }

    void stDashWdh()
    {
        kampfTutorialEnemyGO.gameObject.transform.position = SPEnemyCombatTutorial.transform.position;
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
            kampfTutorialEnemy.patrolPoints[1].position = kampfTutorialEnemy.player.position; 
            if (tempHealth > escMC.gameData.health)
            {
                detectionDashBool = true;
                st105Trigger();
            }
            else if (checkDashKampfTutorialEnemy)
            {
                detectionDashBool = true;
                st106Trigger();
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void st106Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Gut. Bewegung rettet Leben. Aber ein Kampf wird nicht durch das Wegrennen gewonnen.", chronosSWDM, true, st107Trigger)));
        enemyFreeze(true);
    }

    void st107Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Jetzt bist du am Zug, " + vornameMain + "! Greif an, bevor sich dein Feind erholt!", chronosSWDM, true, st108Trigger)));
    }

    void st108Trigger()
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
            kampfTutorialEnemy.patrolPoints[1].position = kampfTutorialEnemy.player.position; 
            if (enemyHealth > kampfTutorialEnemy.health)
            {
                st109Trigger();
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void st109Trigger()
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
            kampfTutorialEnemy.patrolPoints[1].position = kampfTutorialEnemy.player.position; 
            if (tempHealth > kampfTutorialEnemy.health)
            {
                tempHealth = kampfTutorialEnemy.health;
                count++;
                if (count >= 3)
                {
                    st110Trigger();
                    break;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void st110Trigger()
    {
        enemyComboDetection = false;
        kampfTutorialEnemyGO.SetActive(false);
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Ja! Spüre den Rhythmus des Kampfes - Angriff, Bewegung, Angriff!”", chronosSWDM, true, st111Trigger)));
    }

    void st111Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Verletzungen sind unvermeidlich. Aber du regenerierst mit der Zeit…", chronosSWDM, true, st112Trigger)));
    }

    void st112Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Fünf Leben. Das ist alles, was du hast.", chronosSWDM, true, st113Trigger)));
    }

    void st113Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Wenn du sie alle verlierst, wirst du in die Afterworld gezogen. Dort erwartet dich der Wächter. Dein letzter Gegner… und deine einzige Chance auf Rückkehr.", chronosSWDM, true, st114Trigger)));
    }

    void st114Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Besiege ihn, und du wirst wiedergeboren. Scheiterst du… dann war es das.", chronosSWDM, true, st115Trigger)));
    }

    void st115Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Aber wenn du weiter so kämpfst, wirst du hoffentlich nie herausfinden, wie es dort unten ist. Nun geh " + nameMain + " - deine wahre Aufgabe beginnt erst jetzt.", chronosSWDM, true, st116Trigger)));
    }

    void st116Trigger()
    {
        List<NPC_Centence> sentences = new List<NPC_Centence>();
        NPC_Centence sentence = new NPC_Centence();
        chronosSWDM.sentences = sentences;
        escMC.gameData.story_id = "116";
        chronosSWDM.ext = true;    
        Debug.Log("DEBUG Kampftutorial abgeschlossen"); //DEBUG
    }
    // END KAMPFTUTORIAL



    // /// <summary>   
    // /// STARTWELT     
    // /// </summary>
    // /// <returns></returns>

    void st117Trigger()
    {
        chronosSWDM.StopDialogue();
        chronosSWDM.ext = true;
        List<NPC_Centence> sentences = new List<NPC_Centence>();
        NPC_Centence sentence = new NPC_Centence();
        chronosSWDM.sentences = sentences;
        escMC.gameData.story_id = "117";
        session = true;
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, vornameMain + ", du stehst an einem vergessenen Ort, einem Schatten der Welt da draußen. Einst war dies eine Zuflucht, doch nun... ist es ein Käfig. Mein Käfig.", chronosSWDM, false, st118Trigger)));
    }

    void st118Trigger()
    {
        escMC.gameData.story_id = "118";
        chronosSWDM.ext = false; 
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Wofür? Für das Volk, das jenseits dieser Nebel lebt. Man erzählt ihnen, dieser Ort sei eine Barriere gegen das Böse, gegen die Zeitanomalien, die die Welt zerreißen. Doch die Wahrheit ist... Zeus hat mich hierher verbannt.", chronosSWDM, true, st119Trigger)));
    }

    void st119Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Mich - den einzigen, der sein wahres Gesicht kennt. Und nun bist du hier. Das Schicksal hat uns zusammengeführt.", chronosSWDM, true, st120Trigger)));
    }

    void st120Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Er ist nicht der, für den er sich ausgibt. Er hat die Zeit selbst missbraucht, sie verdreht und gebrochen, um seine Herrschaft zu sichern. Die Anomalien da draußen? Seine Schuld. Doch niemand sieht es - niemand außer mir... und jetzt auch du.", chronosSWDM, true, st121Trigger)));
    }

    void st121Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Weil du die Macht dazu hast. Du bist anders als die anderen. Dein Schicksal ist noch nicht geschrieben. Aber bevor du Zeus gegenübertreten kannst, musst du den Kristall aufladen. Er ist der Schlüssel, der uns hier hinausführt.", chronosSWDM, true, st122Trigger)));
    }

    void st122Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Vier Monumente, verstreut in dieser Welt. Jedes von ihnen wird deine Fähigkeiten prüfen, jedes fordert dein Können heraus. Wenn du sie aktivierst, wird der Kristall seine volle Kraft entfalten - und der Nebel wird sich lichten.", chronosSWDM, true, st123Trigger)));
    }

    void st123Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Dann beginnt deine wahre Reise. Drei Gebiete, drei Kugeln der Zeit: Vergangenheit, Gegenwart, Zukunft. Erst wenn du sie alle vereinst, öffnet sich das Tor zu Zeus.", chronosSWDM, true, st124Trigger)));
    }

    void st124Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Ich werde am Kristall, in der Zitadelle auf dich warten. Deine Entscheidung, dein Schicksal. Wähle weise... und vergeude keine Zeit", chronosSWDM, true, st125Trigger)));
    }

    void st125Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Nun geh. " + nameMain + " Die Zeit mag auf deiner Seite sein - aber sie wartet auf niemanden.", chronosSWDM, true, st126Trigger)));
    }

    void st126Trigger()
    {
        List<NPC_Centence> sentences = new List<NPC_Centence>();
        NPC_Centence sentence = new NPC_Centence();
        chronosSWDM.sentences = sentences;
        escMC.gameData.story_id = "126";
        chronosSWDM.ext = true;
        CHRONOS.transform.position = SPChronosKristall.transform.position;
    }


    void st127Trigger()
    { 
        chronosSWDM.StopDialogue();
        chronosSWDM.ext = true;
        List<NPC_Centence> sentences = new List<NPC_Centence>();
        NPC_Centence sentence = new NPC_Centence();
        chronosSWDM.sentences = sentences;
        escMC.gameData.story_id = "128";
        session = true;
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "... Es ist vollbracht, " + vornameMain + " Du hast die Monumente erweckt, den Kristall aufgeladen... und mich befreit.", chronosSWDM, false, st129Trigger)));
    }

    void st129Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Lange war ich gefangen - verbannt, vergessen. Und doch... hat das Schicksal dich hergeführt. Das war kein Zufall.", chronosSWDM, true, st130Trigger)));
    }

    void st130Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Jetzt beginnt deine wahre Reise. Der Nebel ist gefallen, aber Zeus bleibt. Seine Herrschaft, seine Lügen... sie zerfressen die Welt.", chronosSWDM, true, st131Trigger)));
    }

    void st131Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Um ihm entgegentreten zu können, brauchst du mehr als Mut. Du brauchst... die drei Kugeln der Zeit: Vergangenheit, Gegenwart und Zukunft.", chronosSWDM, true, st132Trigger)));
    }

    void st132Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Wo sie sind, wirst du erfahren. Wer sie beschützt, wirst du selbst entdecken müssen. Doch wisse: Keine Prüfung wird leicht sein - jede wird dich fordern.", chronosSWDM, true, st133Trigger)));
    }

    void st133Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Finde sie. Bringe sie zum Kristall zurück. Nur dann kann das Portal zu Zeus geöffnet werden.", chronosSWDM, true, st134Trigger)));
    }

    void st134Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Ich wünschte, ich könnte dir helfen. Aber meine Zeit… sie ist fast aufgebraucht. Ich werde hier auf dich warten - am Kristall.", chronosSWDM, true, st135Trigger)));
    }

    void st135Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Drei Wege liegen vor dir. Drei Prüfungen. Wähle weise. Und denk daran: Die Zeit mag auf deiner Seite sein - aber sie wartet auf niemanden, " + vornameMain, chronosSWDM, true, st136Trigger)));
    }

    void st136Trigger()
    {
        escMC.gameData.story_id = "129";
        List<NPC_Centence> sentences = new List<NPC_Centence>();
        NPC_Centence sentence = new NPC_Centence();
        chronosSWDM.sentences = sentences;
        chronosSWDM.ext = true;
        //CHRONOS.transform.position = DEBUG
    }

    // // END STARTWELT








    /// <summary>   
    /// K1  
    /// </summary>
    /// <returns></returns>

    void stK1_1Trigger() // DIALOG Gemeindehaus Leader
    {
        List<NPC_Centence> sentences = new List<NPC_Centence>();
        NPC_Centence sentence = new NPC_Centence();
        KE_DC = Leader_K1_DC;
        KE_DM = Leader_K1_DM;
        KE_DM.sentences = sentences;
        KE_DM.ext = false;
        session = true;
        addStoryID("K1_3");
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Das kann nicht sein… Diese Haltung… diese Augen…", KE_DM, true, stK1_2Trigger)));
    }

    void stK1_2Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Du bist es. Nach all den Jahren… du bist wirklich zurück.", KE_DM, true, stK1_3Trigger)));
    }

    void stK1_3Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Willkommen in Futura, " + nameMain + ". Ich hörte von deinen Taten. Doch was du suchst - die Kugel der Zukunft - ist nicht leicht zu erlangen.", KE_DM, true, stK1_4Trigger)));
    }

    void stK1_4Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Unsere Vorfahren hinterließen ein Rezept - ein mächtiges Elixier, das die Körper der Menschen stärkte, ihren Geist schärfte, ihr Schicksal veränderte. Doch wir… wir können es nicht mehr entziffern.", KE_DM, true, stK1_5Trigger)));
    }

    void stK1_5Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Die Runen… wir nennen sie die Zeichen von Leni - benannt nach der letzten, die sie verstand. Doch die Entzifferungsschriftrolle… ging vor Jahrzehnten verloren. Ohne sie bleibt das Rezept stumm - ein Lied ohne Melodie.", KE_DM, true, stK1_6Trigger)));
    }

    void stK1_6Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Wir vermuten, dass die Schriftrolle in den Tiefen des Dschungels verborgen liegt… Einem Ort, aus dem keiner zurückkam.", KE_DM, true, stK1_7Trigger)));
    }

    void stK1_7Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Der Dschungel ist ein Labyrinth, von Natur selbst geformt - Wege aus morschem Holz über dunklen Wassern… und dort unten - hungrige Schatten. Krokodile, sagt man… vielleicht mehr.", KE_DM, true, stK1_8Trigger)));
    }

    void stK1_8Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Finde die Schriftrolle. Nur dann wirst du das Rezept entschlüsseln können. Nur dann wirst du würdig sein, die Kugel der Zukunft zu erhalten.", KE_DM, true, stK1_9Trigger)));
    }

    void stK1_9Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Geh, " + vornameMain + " - und trage das Licht der Vergangenheit in den Schatten des Dschungels.", KE_DM, true, stK1_10Trigger)));
    }

    void stK1_10Trigger()
    {
        List<NPC_Centence> sentences = new List<NPC_Centence>();
        NPC_Centence sentence = new NPC_Centence();
        KE_DM.sentences = sentences;
        KE_DM.ext = true;
        rmStoryID("K1_3");
        addStoryID("K1_10");
    }

    void SubmitRuneInput()
    {
        if (checkStoryID("K1_13"))
        {
            string uebersetzung = runeInputField.text;
            string loesung = "";
            foreach (UnityEngine.UI.Image i in runeImages)
            {
                foreach (RuneMapping mapping in runeDatabase.runeMappings)
                {
                    if (mapping.runeIcon == i.sprite)
                    {
                        loesung += mapping.translation.ToLower();
                        break;
                    }
                }
            }
            Debug.LogWarning("DEBUG K1_13: Uebersetzung: " + uebersetzung + " | Loesung: " + loesung);
            if (uebersetzung.ToLower() == loesung)
            {
                rmStoryID("K1_13");
                addStoryID("K1_14");
                StartCoroutine(ZeigeGeschafftAnzeige());
            }
            else
            {
                StartCoroutine(ZeigeNichtGeschafftAnzeige());
                initRunes();
            }
        }
    }

    private void initRunes()
    {
        if (runeDatabase == null || runeDatabase.runeMappings.Count == 0)
        {
            Debug.LogWarning("RuneDatabase ist leer oder fehlt!");
            return;
        }
        string loesung = "karllauderbac";
        for (int i = 0; i < loesung.Length; i++)
        {
            for (int j = 0; j < runeDatabase.runeMappings.Count; j++)
            {
                if (runeDatabase.runeMappings[j].translation.ToLower() == loesung[i].ToString())
                {
                    runeImages[i].sprite = runeDatabase.runeMappings[j].runeIcon;
                    break;
                }
            }
        }
    }

    

    private IEnumerator ZeigeGeschafftAnzeige()
    {
        geschafftAnzeige.SetActive(true);
        yield return new WaitForSeconds(3f);
        geschafftAnzeige.SetActive(false);
    }

    private IEnumerator ZeigeNichtGeschafftAnzeige()
    {
        nichtGeschafftAnzeige.SetActive(true);
        yield return new WaitForSeconds(3f);
        nichtGeschafftAnzeige.SetActive(false);
    }


    /// END K1








    /// <summary>   
    /// K2    
    /// </summary>
    /// <returns></returns>

    void stK2_1Trigger()
    {
        rmStoryID("K2_temp");
        K3VerhaftedText.gameObject.SetActive(true);
        StartCoroutine(HideVerhaftedText());
        Wache1K2.gameObject.SetActive(true);
        Wache2K2.gameObject.SetActive(true);
        playerDummyK2.gameObject.SetActive(true);
        //DEBUG orientation fixen
        Vector3 p1 = escMC.playerController.transform.position;
        Vector3 p2 = wp1_3pl.transform.position;
        Vector3 dir = (p2 - p1).normalized;
        Vector3 perpendicular = new Vector3(-dir.z, 0, dir.x); 
        float distance = 2.0f;
        wp2_1w1.transform.position = p1 + perpendicular * distance;
        wp2_2w2.transform.position = p1 - perpendicular * distance;

        Wache1K2.gameObject.transform.position = wp2_1w1.transform.position;
        Wache2K2.gameObject.transform.position = wp2_2w2.transform.position;

        wp2_3pl.transform.position = escMC.playerController.transform.position;
        escMC.playerController.act = false;
        escMC.playerController.spRenderer.enabled = false;       
        mainCameraFollow.target = playerDummyK2.gameObject.transform;
        playerDummyK2.gameObject.transform.position = wp2_3pl.transform.position;
        Wache1K2.active = true;
        Wache2K2.active = true;
        playerDummyK2.active = true;
        session = true;
        addStoryID("K2_2");
        rmStoryID("K2_1");
    }


    void stK2_4Trigger() // DIALOG VERHOER // DURCHSUCHUNG
    {
        List<NPC_Centence> sentences = new List<NPC_Centence>();
        NPC_Centence sentence = new NPC_Centence();
        KE_DC = Leader_K2_DC;
        KE_DM = Leader_K2_DM;
        KE_DM.sentences = sentences;
        KE_DM.ext = false;
        session = true;
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "So also sieht er aus - der Eindringling, der zur Stunde unserer größten Not die Schwelle Instantias betritt. Was habt Ihr zu verbergen? Zeigt mir, wer Ihr seid… ", KE_DM, true, stK2_5Trigger)));
    }

    void stK2_5Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "… Eine Uhr, alt wie die Zeit selbst. Solche Relikte findet man nicht in den Händen gewöhnlicher Diebe. Erzählt, Fremder - woher stammt dieses Werkzeug der Chroniken? Wer hat es Euch gegeben?", KE_DM, true, stK2_6Trigger)));
    }

    void stK2_6Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Ihr schweigt - das ist klug… oder töricht. Doch Eure Augen… sie sprechen von mehr. Ich sehe keine Gier in Euch. Keine Bosheit. Nur Verwirrung - und ein Hauch von Bestimmung.", KE_DM, true, stK2_7Trigger)));
    }

    void stK2_7Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Seht her, Männer! Dieser hier trägt das Zeichen des Auserwählten - auch wenn er es selbst nicht weiß. Bindet ihn los. Nicht jeder, der fremd erscheint, ist ein Feind. Manchmal ist das Schicksal nur eine Reise ohne Einladung", KE_DM, true, stK2_8Trigger)));
    }

    void stK2_8Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Vergebt mir, Reisender. Unsere Zeiten sind schwer, unsere Herzen misstrauisch. Doch ich fürchte, Eure Ankunft ist kein Zufall… und schon gar nicht ungefährlich.", KE_DM, true, stK2_9Trigger)));
    }

    void stK2_9Trigger()
    { 
        List<NPC_Centence> sentences = new List<NPC_Centence>();
        NPC_Centence sentence = new NPC_Centence();
        KE_DM.sentences = sentences;
        KE_DM.ext = true;
        rmStoryID("K2_5");
        addStoryID("K2_6");
    }


    void stK2_41Trigger() // DIALOG Bande Z hat geklaut...
    {
        List<NPC_Centence> sentences = new List<NPC_Centence>();
        NPC_Centence sentence = new NPC_Centence();
        KE_DC = Leader_K2_DC;
        KE_DM = Leader_K2_DM;
        KE_DM.sentences = sentences;
        KE_DM.ext = false;
        session = true;
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Vor drei Nächten nur - unter dem Mantel des Sturms - drangen sie ein: Schattenhafte Gestalten, schnell wie der Wind, stumm wie der Tod.", KE_DM, true, stK2_42Trigger)));
    }

    void stK2_42Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Die Bande Z nennen sie sich - ein dunkler Schwarm von Getreuen des alten Bösen, das wir längst tot glaubten.", KE_DM, true, stK2_43Trigger)));
    }

    void stK2_43Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Sie kamen nicht, um zu plündern… sondern um zu stehlen, was nicht zu fassen ist: unsere Zeitkugel.", KE_DM, true, stK2_44Trigger)));
    }

    void stK2_44Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Die Kugel der Gegenwart - Quelle unseres Gleichgewichts, Anker unserer Zeitachse - ist fort. Ohne sie beginnt unser Reich zu flackern wie eine Kerze im Wind. Stunden vergehen wie Minuten, Erinnerungen zerfallen.", KE_DM, true, stK2_45Trigger)));
    }

    void stK2_45Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Deshalb - verzeiht meine Härte vorhin - sind wir gezwungen, jeden zu prüfen, der unsere Mauern durchschreitet. Die Gefahr sitzt uns im Nacken.", KE_DM, true, stK2_46Trigger)));
    }

    void stK2_46Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Doch nun, da Ihr hier seid - und da diese Uhr in Eurem Besitz ist…", KE_DM, true, stK2_47Trigger)));
    }

    void stK2_47Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Vielleicht ist es wahr, was die Alten flüsterten: Dass einer kommt, geboren aus den Splittern der Zeit, der das Zerrissene heilt.", KE_DM, true, stK2_48Trigger)));
    }

    void stK2_48Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Ich will Euch vertrauen. Ihr sollt erfahren, wo die Bande Z ihr Nest aufgeschlagen hat.", KE_DM, true, stK2_49Trigger)));
    }

    void stK2_49Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Reist dorthin - lautlos, wachsam, schnell. Holt zurück, was unser ist. Und vielleicht - nur vielleicht - verdient Ihr damit unsere Kugel… und unsere Hoffnung.", KE_DM, true, stK2_50Trigger)));
    }

    void stK2_50Trigger()
    {
        List<NPC_Centence> sentences = new List<NPC_Centence>();
        NPC_Centence sentence = new NPC_Centence();
        KE_DM.sentences = sentences;
        KE_DM.ext = true;
        addStoryID("K2_51");
    }

    void stK2_54Trigger() // DIALOG K2 Bericht erstatten
    {
        List<NPC_Centence> sentences = new List<NPC_Centence>();
        NPC_Centence sentence = new NPC_Centence();
        KE_DC = Leader_K2_DC;
        KE_DM = Leader_K2_DM;
        KE_DM.sentences = sentences;
        KE_DM.ext = false;
        session = true;
        addStoryID("K2_55");
        rmStoryID("K2_54");
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, vornameMain + ", du bist zurück. Und… ich spüre es schon - die Zeit fließt wieder klarer durch diese Hallen. Du hast sie, nicht wahr?", KE_DM, true, stK2_56Trigger)));
    }

    void stK2_56Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Bei allen Ahnen… die Kugel. Du hast vollbracht, woran meine besten Späher scheiterten. Die Bande Z - zerschlagen oder zerstreut?", KE_DM, true, stK2_57Trigger)));
    }

    void stK2_57Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Mutig, präzise und ohne Aufsehen… ganz wie es ein Auserwählter tun sollte.", KE_DM, true, stK2_58Trigger)));
    }

    void stK2_58Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Du hast unsere Hoffnung gerettet - und mehr noch: du hast bewiesen, dass die Prophezeiungen mehr als nur Staub auf Pergament sind.", KE_DM, true, stK2_59Trigger)));
    }

    void stK2_59Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Die Kugel gehört nun wieder ihrem wahren Platz - doch sie erkennt dich. Denn nicht ich… sondern du hast sie verdient.", KE_DM, true, stK2_60Trigger)));
    }

    void stK2_60Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Nimm sie, " + nameMain + ". Trage sie sicher - und trage unsere Geschichte mit dir. Möge der Strom der Gegenwart dich leiten… bis zur Schwelle des Schicksals.", KE_DM, true, stK2_61Trigger)));
    }

    void stK2_61Trigger()
    {
        List<NPC_Centence> sentences = new List<NPC_Centence>();
        NPC_Centence sentence = new NPC_Centence();
        KE_DM.sentences = sentences;
        KE_DM.ext = true;
        rmStoryID("K2_55");
        addStoryID("K2_61");
    }

    /// END K2










    /// <summary>   
    /// K3     
    /// </summary>
    /// <returns></returns>

    void stK3_1Trigger()
    {
        rmStoryID("K3_temp");
        K3VerhaftedText.gameObject.SetActive(true);
        StartCoroutine(HideVerhaftedText());
        Wache1.gameObject.SetActive(true);
        Wache2.gameObject.SetActive(true);
        playerDummy.gameObject.SetActive(true);
        //DEBUG orientation fixen
        Vector3 p1 = escMC.playerController.transform.position;
        Vector3 p2 = wp2_1.transform.position;
        Vector3 dir = (p2 - p1).normalized;
        Vector3 perpendicular = new Vector3(-dir.z, 0, dir.x); 
        float distance = 2.0f;
        wp1_2.transform.position = p1 + perpendicular * distance;
        wp1_3.transform.position = p1 - perpendicular * distance;

        Wache1.gameObject.transform.position = wp1_2.transform.position;
        Wache2.gameObject.transform.position = wp1_3.transform.position;

        wp1_1.transform.position = escMC.playerController.transform.position;
        escMC.playerController.act = false;
        escMC.playerController.spRenderer.enabled = false;       
        mainCameraFollow.target = playerDummy.gameObject.transform;
        playerDummy.gameObject.transform.position = wp1_1.transform.position;
        Wache1.active = true;
        Wache2.active = true;
        playerDummy.active = true;
        session = true;
        addStoryID("K3_2");
        rmStoryID("K3_1");
    }

    IEnumerator HideVerhaftedText()
    {
        yield return new WaitForSeconds(2f);
        K3VerhaftedText.gameObject.SetActive(false);
    }

    void stK3_41Trigger()
    {
        arenaRundenText.gameObject.SetActive(true);
        arenaRundenText.text = "Round " + arenaCurrentRunde + "/" + arenaRunden;
        enemyArenaK3 = enemyArenaK3.Where(e => e != null).ToArray();
        if (enemyArenaK3.Length == 0) 
        {
            if (arenaCurrentRunde < arenaRunden)
            {
                arenaCurrentRunde++;
                int x = UnityEngine.Random.Range(3, 10);
                enemyArenaK3 = new GameObject[x];
                while (x > 0) 
                {
                    Transform randomSpawn = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
                    BoxCollider2D box = randomSpawn.GetComponent<BoxCollider2D>();
                    if (box != null)
                    {
                        Vector3 position = GetRandomPointInBoxCollider2D(box);
                        enemyArenaK3[x - 1] = Instantiate(initEnemyArenaK3, position, Quaternion.identity);
                        enemyArenaK3[x - 1].SetActive(true);
                    }
                    else
                    {
                        Debug.LogWarning("SpawnTransform " + randomSpawn.name + " has no BoxCollider2D!");
                    }
                    x--;
                } 
            }
            else 
            {
                addStoryID("K3_42");
                rmStoryID("K3_41");
            }
        }
    }

    /// END K3



    /// KE <start>
    void startDialogueKE()
    {
        List<NPC_Centence> sentences = new List<NPC_Centence>();
        NPC_Centence sentence = new NPC_Centence();
        if (checkStoryID("K3"))
        {
            KE_DC = Leader_K3_DC;
            KE_DM = Leader_K3_DM;
        }
        else if (checkStoryID("K2"))
        {
            KE_DC = Leader_K2_DC;
            KE_DM = Leader_K2_DM;
        }
        else if (checkStoryID("K1"))
        {
            KE_DC = Leader_K1_DC;
            KE_DM = Leader_K1_DM;
        }
        KE_DM.sentences = sentences;
        KE_DM.ext = false;
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, vornameMain + ", du bist gekommen. Die Prophezeiungen sprachen von einem, der die Zeichen der Zeit lesen kann. Vor zehn Generationen teilten unsere Ahnen die Macht der Drei Kugeln auf - Feuer, Wasser und Sturm -, um die Rückkehr des Bösen zu verhindern. ", KE_DM, true, stKE_01Trigger)));
    }

    void stKE_01Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Jedes Königreich bewacht eine Kugel, doch unser Bündnis bröckelt.", KE_DM, true, stKE_02Trigger)));
    }

    void stKE_02Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Das Böse, das die Zeit selbst verdreht, hungerte nach ihrer vereinten Kraft. Damals schmiedeten wir einen Pakt:", KE_DM, true, stKE_03Trigger)));
    }

    void stKE_03Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Nur der Auserwählte, der alle drei Reiche durchläuft und ihre Prüfungen besteht, darf die Kugeln vereinen - und das Böse für immer verbannen.", KE_DM, true, stKE_04Trigger)));
    }

    void stKE_04Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Doch die Jahrhunderte ließen die Erinnerung verblassen. Manche Königreiche misstrauen einander, andere glauben, die Kugeln seien Legenden.", KE_DM, true, stKE_05Trigger)));
    }

    void stKE_05Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Hier, in diesen Hallen, bewahren wir die letzte Schriftrolle, die den wahren Zweck dokumentiert.", KE_DM, true, stKE_06Trigger)));
    }

    void stKE_06Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Jede Kugel ist in einem Tempel verborgen, geschützt durch Rätsel, die nur mit Mut, Weisheit und Opferbereitschaft gelöst werden können.", KE_DM, true, stKE_07Trigger)));
    }

    void stKE_07Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Doch Vorsicht: Die Schatten des Bösen lauern bereits. Sie manipulieren die Schwachen, suchen nach Lücken in unserer Wachsamkeit.", KE_DM, true, stKE_08Trigger)));
    }

    void stKE_08Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Welches der drei Königreiche du zuerst bereist ist dir überlassen, aber wenn du schon mal da bist…", KE_DM, true, stKE_09Trigger)));
    }

    void stKE_09Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, KE_DC, "Geh - und beweise, dass du nicht nur die Kugeln sammelst, sondern auch ihre Wächter vereinst.", KE_DM, true, stKE_doneTrigger)));
    }

    void stKE_doneTrigger()
    {
        List<NPC_Centence> sentences = new List<NPC_Centence>();
        NPC_Centence sentence = new NPC_Centence();
        KE_DM.sentences = sentences;
        KE_DM.ext = true;
        Debug.Log("DEBUG startDialogueKE() done");
        rmStoryID("KE_temp");
        addStoryID("KE");
    }


    /// END KE





















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

    public void resetEndFight () // RESET MAP DEBUG
    {
        //RESET DEBUG
        escMC.gameData.story_id = "7999";
        K1.SetActive(false);
        K2.SetActive(false);
        K3.SetActive(false);
        barrierK1.isTrigger = false;
        barrierK2.isTrigger = false;
        barrierK3.isTrigger = false;
        chronosEnemy.active = false;
        portalZEUS.SetActive(false);
        portalAfterworld.SetActive(false);
        afterWorldEnemyGO.SetActive(true);
        taschenuhrAfterworld.SetActive(false);
        CHRONOS.transform.position = SPChronosKristall.transform.position; //DEBUG KIRCHE...
        escMC.gameData.respw = playerPositionEndgameRespanw.x + ";" + playerPositionEndgameRespanw.y;
    }

    Vector3 playerPositionEndgameRespanw;

    // if box trigger by chronos ... 
    public void st8000Trigger()
    {
        escMC.gameData.story_id = "8001";
        chronosSWDM.StopDialogue();
        chronosSWDM.ext = true;
        List<NPC_Centence> sentences = new List<NPC_Centence>();
        NPC_Centence sentence = new NPC_Centence();
        chronosSWDM.sentences = sentences;
        if (!session)
        { 
            kampfTutorialEnemy.player.transform.position = playerSPkathedraleafterWorld.transform.position;
        }
        session = true;
        K1.SetActive(true);
        K2.SetActive(true);
        K3.SetActive(true);
        chronosEnemy.active = false;
        portalZEUS.SetActive(false);
        portalAfterworld.SetActive(false);
        afterWorldEnemyGO.SetActive(true);
        taschenuhrAfterworld.SetActive(false);
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Endlich, " + vornameMain + ". Du hast es geschafft. Alle Königreiche bereist, alle Rätsel gemeistert... und mir genau das gebracht, was ich brauche. Lege die Kugeln in den Kristall ein.", chronosSWDM, true, st8001Trigger)));
    }

    void st8001Trigger()
    {
        chronosSWDM.ext = false; 
        playerPositionEndgameRespanw = new Vector3(kampfTutorialEnemy.player.position.x, kampfTutorialEnemy.player.position.y, kampfTutorialEnemy.player.position.z);
        clearDialoguechronosSWDM();
        escMC.gameData.story_id = "8002";
    }
    // EINSETZEN DER KUGELN DEBUG

    void st8005Trigger()
    {
        escMC.gameData.story_id = "8006";
        portalZEUS.SetActive(true);
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Perfekt... Das Portal ist offen. Unsere Reise endet hier nicht, mein Freund - sie beginnt erst. Ich wusste, dass du es schaffst. Dass du mir hilfst. Auch wenn du es nicht bemerkt hast.", chronosSWDM, true, st8006Trigger)));
    }

    void st8006Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Siehst du, wie es pulsiert? Wie die Macht der Zeit selbst sich entfaltet? Und du dachtest, du kämpfst für das Richtige...", chronosSWDM, true, clearDialoguechronosSWDM)));
    }

    void st8007Trigger() 
    {
        // KAMPF ZEUS
        //escMC.gameData.health = 400;
        CHRONOS.transform.position = afterWorldSPCHRONOS_I.transform.position;
        enemyZEUS.gameObject.SetActive(true);
        AfterWorldMap.SetActive(true);
        afterWorldEnemyGO.SetActive(false);
        enemyZEUS.gameObject.transform.position = afterWorldSPZEUS_I.transform.position;
        escMC.gameData.story_id = "8008";
        StartCoroutine(attackDetectionZEUS());
    }

    IEnumerator attackDetectionZEUS()
    {
        enemyAttackDetection = true;
        float enemyHealth = enemyZEUS.health;
        while (enemyAttackDetection)
        {
            if (enemyZEUS.health <= 0)
            {
                enemyAttackDetection = false;
                st8008Trigger();
                yield break;
            }
            if (checkStoryID("8001"))
            {
                enemyAttackDetection = false;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void st8008Trigger()
    {
        Debug.Log("DEBUG Sieg über Zeus"); //DEBUG
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Hervorragend! Wahrlich ein Anblick für die Ewigkeit - der letzte Wächter, gefallen durch deine Hand.", chronosSWDM, true, st8009Trigger)));
    }

    void st8009Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Du hast dich gut geschlagen... zu gut. Ich hätte es selbst nicht besser machen können. Aber siehst du nicht? Ich habe dich gelenkt. Ich habe dich gebraucht. Und du... hast mir alles gegeben, was ich wollte.", chronosSWDM, true, st8010Trigger)));
    }

    void st8010Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Die Kugeln? Ein Schlüssel. Zeus? Ein Hindernis. Du? Mein Werkzeug.", chronosSWDM, true, st8011Trigger)));
    }

    void st8011Trigger()
    {
        // ANIM Chronos klaut Taschenuhr DEBUG
        escMC.gameData.story_id = "8011";
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "DIESE Macht... lange habe ich darauf gewartet! Du hast nicht den letzten Feind besiegt - du hast ihn befreit!", chronosSWDM, true, st8012Trigger)));
    }

    void st8012Trigger()
    {
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Zeus war der letzte, der mich hätte aufhalten können. Und du hast ihn für mich beseitigt. Dafür danke ich dir... doch jetzt bist du überflüssig", chronosSWDM, true, st8013Trigger)));
    }
    void st8013Trigger()
    {
        CHRONOS.transform.position = afterWorldSPCHRONOS_II.transform.position;
        CHRONOS.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Komm... ich will deine letzten Atemzüge selbst erleben.", chronosSWDM, true, st8014Trigger)));
    }

    void st8014Trigger()
    {
        portalAfterworld.SetActive(true);
    }

    void st8015Trigger()
    {
        CHRONOS.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        escMC.gameData.story_id = "8016";
        taschenuhrAfterworld.SetActive(true);
        afterWorldEnemyGO.SetActive(false);
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Da bist du, " + nameMain + ". Noch atmend. Noch hoffend. Lächerlich.", chronosSWDM, true, clearDialoguechronosSWDM)));
    }

    void st8016Trigger() 
    {
        escMC.gameData.story_id = "8018";
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Oh? Ein letzter Funke Widerstand? Dann lass mich ihn auslöschen!", chronosSWDM, true, st8017Trigger)));
    }

    void st8017Trigger()
    {
        escMC.gameData.story_id = "8019";
        StartCoroutine(attackDetectionAfterWorld());
        chronosEnemy.active = true;
        chronosEnemy.attackable = true;
        mo1.gameObject.SetActive(true);
        mo2.gameObject.SetActive(true);
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
                st8018Trigger();
                yield break;
            }
            if (checkStoryID("8001"))
            {
                enemyAttackDetectionA = false;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void st8018Trigger()
    {
        mo1.active = false;
        mo2.active = false;
        stopCoroutine(StartCoroutine(setDialogue(0.5f, chronosSWDC, "Nein... Nein! ICH bin die Zeit! ICH kann nicht...", chronosSWDM, true, st8019Trigger)));
    }

    void st8019Trigger()
    {
        escMC.gameData.story_id = "8020";
        chronosSWDM.ext = true; 
        K1.SetActive(false);
        K2.SetActive(false);
        K3.SetActive(false);
        barrierK1.isTrigger = false;
        barrierK2.isTrigger = false;
        barrierK3.isTrigger = false;
        portalZEUS.SetActive(false);
        portalAfterworld.SetActive(false);
        taschenuhrAfterworld.SetActive(false);
        afterWorldEnemyGO.SetActive(true);
    }

    // // END ENDGAMEPLAY







    Vector3 GetRandomPointInBoxCollider2D(BoxCollider2D box)
    {
        Bounds bounds = box.bounds;

        float x = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
        float y = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);

        return new Vector3(x, y, 0f); // Z bleibt bei 0 für 2D
    }




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
            }
        }
    }
}
