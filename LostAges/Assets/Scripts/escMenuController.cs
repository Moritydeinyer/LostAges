using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.CSharp;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using UnityEditor;
using Unity.VisualScripting;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using Cainos.PixelArtTopDown_Basic;

using UnityEngine.AI;
using System.Globalization;

public class jsonGameData {
    public int id { get; set; }
    public float x { get; set; }
    public float y { get; set; }
    public int z { get; set; }
    public int y_rot { get; set; }
    public string name { get; set; }
    public string ltp { get; set; }
    public int unlocked_areas { get; set; }
    public string story_id { get; set; }
    public string waypoints { get; set; }
    public int health { get; set;}
    public string respw { get; set; }
}

public class jsonPlayerData {
    public int id { get; set; }
    public string auth_id { get; set; }
    public string add { get; set; }
    public string username { get; set; }
    public string games { get; set; }
    public int settingsVolume { get; set; }
    public string bindings { get; set; }
}

public class escMenuController : MonoBehaviour
{
    [Header("Story Management")]
    [SerializeField] public storyManager storyManager;

    [Header("Tasks Panel")]
    [SerializeField] private Transform taskPanel;
    [SerializeField] public TextMeshProUGUI taskPanelData;

    [Header("Overlay Panel")]
    [SerializeField] public Transform gameOverScreenPanel;
    [SerializeField] public TextMeshProUGUI gospTxt1;
    [SerializeField] public TextMeshProUGUI gospTxt2;
    [SerializeField] public Transform afterWorldScreenPanel;
    [SerializeField] public TextMeshProUGUI awspTxt1;
    [SerializeField] public TextMeshProUGUI awspTxt2;
    [SerializeField] public Transform resawnScreenPanel; 
    [SerializeField] public TextMeshProUGUI rpspTxt1;
    [SerializeField] public TextMeshProUGUI rpspTxt2;   

    [Header("UI Panel")]
    [SerializeField] private Transform UIminiMapPanel;
    [SerializeField] private Transform UIdialogOverlayPanel;
    [SerializeField] public Transform UIinteractionPanel;
    [SerializeField] public TextMeshProUGUI UIinteractionPanelData;
    [SerializeField] public GameObject InputKeyboardDialogUI;
    [SerializeField] public GameObject InputGamepadXboxDialogUI;
    [SerializeField] public GameObject InputGamepadPSDialogUI;
    [SerializeField] public GameObject InputKeyboardInteractionUI;
    [SerializeField] public GameObject InputGamepadXboxInteractionUI;
    [SerializeField] public GameObject InputGamepadPSInteractionUI;
    [SerializeField] public GameObject InteractionUIKeyboardTXT;
    [SerializeField] public GameObject InteractionUIGamepadXboxTXT;
    [SerializeField] public GameObject InteractionUIGamepadPSTXT;
    [SerializeField] public GameObject mapPanel;

    [Header("Esc Panel")]
    [SerializeField] private Button quitSaveBtn;
    [SerializeField] private Button backGameBtn;
    [SerializeField] private Button loadDebugBtn;
    [SerializeField] private Button loadDemoBtn;
    [SerializeField] private Button settingsBtn;
    [SerializeField] private Transform escMenuPanel;

    [Header("Settings Panel")]
    [SerializeField] private Button settingsSaveBtn;
    [SerializeField] private Transform settingsPanel;
    public AudioMixer mainMixer;
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;
    public Slider volumeSlider;

    [SerializeField] private Button GeneralSettingsBtn;
    [SerializeField] private Button ControllerSettingsBtn;
    [SerializeField] private Button KeyboardSettingsBtn;
    [SerializeField] private Transform GeneralPanel;
    [SerializeField] private Transform ControllerPanel;
    [SerializeField] private Transform KeyboardPanel;
    [SerializeField] public InputActionAsset actions;

    [Header("Start Panel")]
    [SerializeField] private Button quitGameBtn;
    [SerializeField] private Button playBtn;
    [SerializeField] private Button logoutBtn;
    [SerializeField] private Transform startPanel;

    [Header("Insert Game Name Panel")]
    [SerializeField] private Button playBtnII;
    [SerializeField] private TMP_InputField gameNameInput;
    [SerializeField] private Transform insertGameNamePanel;

    [Header("Logged Out Panel")]
    [SerializeField] private Button loginBtn;
    [SerializeField] private Transform loggedOutPanel;
    [SerializeField] private loginController loginController;

    [Header("Select Game Panel")]
    [SerializeField] private Transform selectGamePanel;
    [SerializeField] private Button quitSelectBtn;
    [SerializeField] private Button playGame_1;
    [SerializeField] private Button playGame_2;
    [SerializeField] private Button playGame_3;
    [SerializeField] private Button deleteGame_1;
    [SerializeField] private Button deleteGame_2;
    [SerializeField] private Button deleteGame_3;
    [SerializeField] private TextMeshProUGUI data1;
    [SerializeField] private TextMeshProUGUI data2;
    [SerializeField] private TextMeshProUGUI data3;
    private jsonGameData game1;
    private jsonGameData game2;
    private jsonGameData game3;

    [Header("Player Data")]
    [SerializeField]  PlayerInput playerInput;
    public bool useController;
    public string controllerType;
    private IEnumerator SaveRoutine;
    [SerializeField] private TextMeshProUGUI savingText;

    [SerializeField] public TopDownCharacterController playerController;
    [SerializeField] public Transform playerTransform;
    [SerializeField] private SpriteRenderer playerRenderer;
    [SerializeField] private SpriteRenderer playerShadow;
    private bool active = false;
    private bool canPressEscKey = true;
    private bool canPressTKey = true;
    private bool canPressMKey = true;
    public jsonGameData gameData = new jsonGameData();
    private jsonPlayerData playerData = new jsonPlayerData();
    public string authentication_id = "gh45";                           // DEBUG
    //public string authentication_id = "CfdR2IGZEQZUoApxK93Sm5nvLBUx";   // DEBUG (google)
    private PlayerProfile playerProfile;
    private int y_rot = -45784578;

    [Header("Szene Switch")]
    [SerializeField] private Image switchPanel;
    private float fadeDuration = 1f;

    void Start()
    {
        savingText.enabled = false;
        StartCoroutine(SaveRoutine = AutoSave());
        FadeOut();
        Application.targetFrameRate = 60;
        mapPanel.gameObject.SetActive(false);
        escMenuPanel.gameObject.SetActive(false);
        settingsPanel.gameObject.SetActive(false);
        startPanel.gameObject.SetActive(false);
        selectGamePanel.gameObject.SetActive(false);
        insertGameNamePanel.gameObject.SetActive(false);
        loggedOutPanel.gameObject.SetActive(false);
        taskPanel.gameObject.SetActive(false);
        UIminiMapPanel.gameObject.SetActive(false);
        UIinteractionPanel.gameObject.SetActive(false);
        //UIdialogOverlayPanel.gameObject.SetActive(false);
        storyManager.enabled = false;
        active = false;
        backGameBtn.onClick.AddListener(backGameListener);
        quitSaveBtn.onClick.AddListener(quitSaveListenerGateway);
        loadDebugBtn.onClick.AddListener(loadDebugListener);
        loadDemoBtn.onClick.AddListener(loadDemoListener);
        settingsBtn.onClick.AddListener(settingsListener);
        settingsSaveBtn.onClick.AddListener(settingsSaveListener);
        quitGameBtn.onClick.AddListener(quitGameListener);
        playBtn.onClick.AddListener(playListener);
        quitSelectBtn.onClick.AddListener(quitSelectListener);
        logoutBtn.onClick.AddListener(logoutListener);

        GeneralSettingsBtn.onClick.AddListener(() => {
            GeneralPanel.gameObject.SetActive(true); 
            ControllerPanel.gameObject.SetActive(false); 
            KeyboardPanel.gameObject.SetActive(false);
        });

        ControllerSettingsBtn.onClick.AddListener(() => {
            GeneralPanel.gameObject.SetActive(false); 
            ControllerPanel.gameObject.SetActive(true); 
            KeyboardPanel.gameObject.SetActive(false);
            actions.LoadBindingOverridesFromJson("");
            loadBindings();
        });

        KeyboardSettingsBtn.onClick.AddListener(() => {
            GeneralPanel.gameObject.SetActive(false); 
            ControllerPanel.gameObject.SetActive(false); 
            KeyboardPanel.gameObject.SetActive(true);
            actions.LoadBindingOverridesFromJson("");
            loadBindings();
        });

        loginBtn.onClick.AddListener(loginAuthListener);

        playGame_1.onClick.AddListener(playGame1Listener);
        playGame_2.onClick.AddListener(playGame2Listener);
        playGame_3.onClick.AddListener(playGame3Listener);

        deleteGame_1.onClick.AddListener(deleteG1Listener);
        deleteGame_2.onClick.AddListener(deleteG2Listener);
        deleteGame_3.onClick.AddListener(deleteG3Listener);

        playBtnII.onClick.AddListener(saveGameNameListener);
        taskPanelData.SetText("\n\nCurrently no tasks available");

        loginController.OnSignedIn += LoginController_OnSignedIn;
        loginController.OnAvatarUpdate += LoginController_OnAvatarUpdate;

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) 
            {
                currentResolutionIndex = i;
            } 
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        mainMixer.SetFloat("masterVolume", 0);

        
        /// DEBUG <start>
        mainMixer.SetFloat("masterVolume", -80);
        /// DEBUG <end>
        /// 

        ///  DEBUG BUILD <start>
        ///    loggedOutPanel.gameObject.SetActive(true);
        ///    loginBtn.Select();
        /// DEBUG BUILD <end>
    }

    void FixedUpdate()
    {
        //DEBUG TEST START
        if (Input.GetKeyDown(KeyCode.L))
        {
            //gameData.story_id = "9";
            //FadeInOut();
            //UIminiMapPanel.gameObject.SetActive(false);
            //UIdialogOverlayPanel.gameObject.SetActive(false);
            //gameData.story_id = "999";
            Debug.Log(gameData.story_id);}
        if (Input.GetKeyDown(KeyCode.C))
        {
            //Debug.Log(actions.SaveBindingOverridesAsJson());    
            //Debug.Log(gameData.respw + "\nRestoring Respwn Point");
            //float x = float.Parse(gameData.respw.Split(';')[0].Replace(",", "."), CultureInfo.InvariantCulture);
            //float y = float.Parse(gameData.respw.Split(';')[1].Replace(",", "."), CultureInfo.InvariantCulture);
            //playerTransform.position = new Vector3(x, y, 0);
            //gameData.story_id = "6";       
            //storyManager.kampfTutorialEnemy.gameObject.SetActive(false);   
            storyManager.st1000Trigger();
        }
        //DEBUG TEST ENDE
        if (storyManager.enabled)
        {
            if (storyManager.checkStoryID("8"))
            {
                //FadeInOut();
                playerTransform.position = new Vector3(playerTransform.position.x - 73.33997f, playerTransform.position.y - 29, 0);
                Debug.Log(storyManager.session);
                gameData.story_id = "9";
            }
        }
        if (Gamepad.current != null) 
        {
            if (Gamepad.current.displayName.ToLower().Contains("xbox")) 
            {
                controllerType = "Xbox";
            }
            if (Gamepad.current.displayName.ToLower().Contains("dualsense")) 
            {
                controllerType = "PS";
            }
            if (Gamepad.current.displayName.ToLower().Contains("playstation")) 
            {
                controllerType = "PS";
            }
        }
        if (playerInput.currentControlScheme == "Gamepad") {useController = true;} else {useController = false;}
        if (useController)
        {   
            SetCursorToCenter();
            if (controllerType == "Xbox") 
            {
                InputGamepadXboxDialogUI.SetActive(true);
                InputGamepadXboxInteractionUI.SetActive(true);
                InteractionUIGamepadXboxTXT.SetActive(true);
                InputGamepadPSDialogUI.SetActive(false);
                InputGamepadPSInteractionUI.SetActive(false);
                InteractionUIGamepadPSTXT.SetActive(false);
            }        
            if (controllerType == "PS") 
            {
                InputGamepadPSDialogUI.SetActive(true);
                InputGamepadPSInteractionUI.SetActive(true);
                InteractionUIGamepadPSTXT.SetActive(true);
                InputGamepadXboxDialogUI.SetActive(false);
                InputGamepadXboxInteractionUI.SetActive(false);
                InteractionUIGamepadXboxTXT.SetActive(false);

            }         
            InputKeyboardDialogUI.SetActive(false);
            InputKeyboardInteractionUI.SetActive(false);
            InteractionUIKeyboardTXT.SetActive(false);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            InputGamepadXboxDialogUI.SetActive(false);
            InputGamepadXboxInteractionUI.SetActive(false);
            InteractionUIGamepadXboxTXT.SetActive(false);
            InputGamepadPSDialogUI.SetActive(false);
            InputGamepadPSInteractionUI.SetActive(false);
            InteractionUIGamepadPSTXT.SetActive(false);

            InputKeyboardDialogUI.SetActive(true);
            InputKeyboardInteractionUI.SetActive(true);
            InteractionUIKeyboardTXT.SetActive(true);
        }
    }

    private void SetCursorToCenter()
    {
        Vector3 centerPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Mouse.current.WarpCursorPosition(centerPosition);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    public void FadeIn()
    {
        StartCoroutine(Fade(0f, 1f, false));
    }
    
    public void FadeOut()
    {
        StartCoroutine(Fade(1f, 0f, false));
    }
    public void FadeInOut()
    {
        StartCoroutine(Fade(0f, 1f, true));
    }

    private IEnumerator AutoSave()
    {
        while (true)
        {
            if (playerController.act)
            {
                savingText.enabled = true;
                savingText.text = "Saving...";
                //Debug.Log("saving...");
                quitSaveListener(true);
                yield return new WaitForSeconds(2f);
                savingText.enabled = false;
            }
            yield return new WaitForSeconds(300f); // DEBUG 5 min 
        }
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, bool fadeBack)
    {
        float elapsedTime = 0f;
        switchPanel.gameObject.SetActive(true);
        Color color = switchPanel.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            switchPanel.color = color;
            yield return null;
        }
        color.a = endAlpha;
        switchPanel.color = color;
        if (endAlpha == 0f)
        {
            switchPanel.gameObject.SetActive(false);
        }
        if (fadeBack)
        {
            while (elapsedTime < (fadeDuration+0.25f))
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            elapsedTime = 0f;
            switchPanel.gameObject.SetActive(true);
            color = switchPanel.color;
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Lerp(endAlpha, startAlpha, elapsedTime / fadeDuration);
                switchPanel.color = color;
                yield return null;
            }
            color.a = startAlpha;
            switchPanel.color = color;
            if (startAlpha == 0f)
            {
                switchPanel.gameObject.SetActive(false);
            }
        }
    }

    public void EscMenu(InputAction.CallbackContext context)
    {
        if (canPressEscKey) {StartCoroutine(HandleEscapeInput());}
    }

    public void TaskMenu(InputAction.CallbackContext context)
    {
        if (canPressTKey) {StartCoroutine(HandleTInput());}
    }

    public void MapMenu(InputAction.CallbackContext context)
    {
        if (canPressMKey) {StartCoroutine(HandleMInput());}
    }

    public void loadVolume()
    {

        mainMixer.SetFloat("masterVolume", playerData.settingsVolume);
        volumeSlider.value = playerData.settingsVolume;
    }

    public void loadBindings()
    {
        try 
        {
        var rebinds = playerData.bindings.Replace("^$^", "\"");
        if (!string.IsNullOrEmpty(rebinds))
        {
            actions.LoadBindingOverridesFromJson(rebinds);
        }
        } catch {}
    }

    public void deleteG1Listener() 
    {
        deleteGame(game1.id, true);
    }
    public void deleteG2Listener() 
    {
        deleteGame(game2.id, true);
    }
    public void deleteG3Listener() 
    {
        deleteGame(game3.id, true);
    }

    public async void loginAuthListener()
    {
        await loginController.InitSignIn();
    }

    private async void LoginController_OnSignedIn(PlayerProfile profile)
    {
        //Debug.Log("Signed in: " + profile.playerInfo.Id);
        authentication_id = profile.playerInfo.Id;
        string data = "{\"authentication_id\":\"" + authentication_id + "\"}";
        HttpClient client = new HttpClient();
        StringContent queryString = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync("https://iab-services.ddns.net/api/gta_speichersdorf/get_user_data", queryString);
        string responseBody = await response.Content.ReadAsStringAsync();
        playerData = JsonConvert.DeserializeObject<jsonPlayerData>(responseBody);
        loggedOutPanel.gameObject.SetActive(false);
        startPanel.gameObject.SetActive(true);
        playBtn.Select();
        loadVolume();
        loadBindings();
    }

    private void LoginController_OnAvatarUpdate(PlayerProfile profile)
    {
        playerProfile = profile;
    }

    public async void deleteGame(int gameID, bool play)
    {
        try
        {
            string data = "{\"authentication_id\":\"" + authentication_id + "\", \"spielstand_id\":\"delete\", \"deleteID\":\"" + gameID + "\"}";
            HttpClient client = new HttpClient();
            StringContent queryString = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://iab-services.ddns.net/api/gta_speichersdorf/save_spielstand", queryString);
            string responseBody = await response.Content.ReadAsStringAsync(); 
            Debug.Log(responseBody);
            Debug.Log(gameID);
            if (play) {playListener();}
        } 
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void logoutListener()
    {
        game1 = new jsonGameData();
        game2 = new jsonGameData();
        game3 = new jsonGameData();
        game1.y_rot = y_rot;
        game2.y_rot = y_rot;
        game3.y_rot = y_rot;
        gameData = new jsonGameData();
        playerData = new jsonPlayerData();
        authentication_id = "";
        startPanel.gameObject.SetActive(false);
        loggedOutPanel.gameObject.SetActive(true);
        loginBtn.Select();
        mainMixer.SetFloat("masterVolume", -80);
        // RESET KEY BINDINGS
        actions.LoadBindingOverridesFromJson("");
    }

    public void setVolume(float volume) 
    {
        mainMixer.SetFloat("masterVolume", volume);
        playerData.settingsVolume = (int)volume;
        updatePlayerDataDB();
    }

    public async void updatePlayerDataDB()
    {
        string data = "{\"volume\":\"" + playerData.settingsVolume + "\",\"authentication_id\":\"" + authentication_id + "\",\"bindings\":\"" + playerData.bindings.Replace("\"", "^$^") + "\"}";                        
        HttpClient client = new HttpClient();
        StringContent queryString = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync("https://iab-services.ddns.net/api/gta_speichersdorf/update_account", queryString);
        string responseBody = await response.Content.ReadAsStringAsync();
        Debug.Log(responseBody);
        Debug.Log(playerData.bindings.Replace("\"", "^$^"));
    }


    public void setQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void setFullscreen (bool isFullscreen) 
    {
        Screen.fullScreen = isFullscreen;
    }

    public void setResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    private void playGame1Listener()
    {
        gameData = game1;
        checkName();
    }
    private void playGame2Listener()
    {
        gameData = game2;
        checkName();
    }
    private void playGame3Listener()
    {
        gameData = game3;
        checkName();
    }

    private void saveGameNameListener()
    {
        setPlayerPosition();
        insertGameNamePanel.gameObject.SetActive(false);
        selectGamePanel.gameObject.SetActive(false);
        gameData.name = gameNameInput.text;
        gameNameInput.text = "";
    }

    private void checkName()
    {
        try 
        {
            if (gameData.name == null || gameData.name == "")
            {
                selectGamePanel.gameObject.SetActive(false);
                insertGameNamePanel.gameObject.SetActive(true);
                gameNameInput.Select();
            }
            else 
            {
                setPlayerPosition();
                selectGamePanel.gameObject.SetActive(false);
            }
        }
        catch
        {
            selectGamePanel.gameObject.SetActive(false);
            insertGameNamePanel.gameObject.SetActive(true);
            gameNameInput.Select();
        }
        
    }

    private void setPlayerPosition()
    {
        FadeOut();
        try {
            if (gameData.y_rot != y_rot) {
                playerTransform.position = new Vector3(gameData.x, gameData.y, 0);
                playerTransform.gameObject.layer = gameData.z;
                playerRenderer.sortingLayerID = gameData.y_rot;
                playerShadow.sortingLayerID = gameData.y_rot;
                playerController.act = true;
                UIminiMapPanel.gameObject.SetActive(true);
                foreach (GameObject waypoint in playerController.waypoints.ToArray())
                {
                    playerController.DeleteWaypoint(waypoint);
                }
                foreach (string part in gameData.waypoints.Split(';'))
                {
                    try 
                    {
                        string[] coords = part.Split(':');
                        float x = float.Parse(coords[0].Replace(",", "."), CultureInfo.InvariantCulture);
                        float y = float.Parse(coords[1].Replace(",", "."), CultureInfo.InvariantCulture);
                        //Debug.Log(x + " " + y);
                        //Debug.Log(coords[0] + " " + coords[1]);
                        Vector3 tempPosition = new Vector3(x, y, 0);
                        GameObject wp = playerController.CreateWaypoint(tempPosition);
                        wp.transform.position = tempPosition;
                    } catch (Exception e) {
                        Debug.Log(e);
                    }
                }
                //UIdialogOverlayPanel.gameObject.SetActive(true); DEBUG
                storyManager.enabled = true;
                storyManager.chronosSWDM.StopDialogue();
                storyManager.chronosSWDM.ext = true;
                storyManager.session = false;
                Debug.Log("New Game Init2");
            }
            else
            {
                throw new Exception();
            }
        } catch {
            // NEW GAME INIT DEBUG
            try 
            {
                foreach (GameObject waypoint in playerController.waypoints.ToArray())
                {
                    playerController.DeleteWaypoint(waypoint);
                }
            } catch (Exception e) {
                Debug.Log(e);
            }
            gameData = new jsonGameData();
            gameData.id = 0;
            
            gameData.unlocked_areas = 0;
            gameData.story_id = "0";
            gameData.health = 400;
            gameData.respw = "-34.4;-11.28";

            playerTransform.position = new Vector3(-34.4f, -11.28f, 0);
            
            playerTransform.gameObject.layer = 20;
            playerRenderer.sortingLayerID = -1869315837;
            playerShadow.sortingLayerID = -1869315837;
            playerController.act = true;
            storyManager.enabled = true;
            storyManager.session = false;
            storyManager.chronosSWDM.ext = true;
            Debug.Log("New Game Init1");

            UIminiMapPanel.gameObject.SetActive(true);
        }
    }

    private IEnumerator HandleEscapeInput()
    {
        canPressEscKey = false;
        if (escMenuPanel.gameObject.activeSelf || playerController.act) {
            if (!active)
            {
                escMenuPanel.gameObject.SetActive(true);
                backGameBtn.Select();
                playerController.act = false;
                active = true;
            }
            else
            {
                escMenuPanel.gameObject.SetActive(false);
                playerController.act = true;
                active = false;
            }
        }
        if (settingsPanel.gameObject.activeSelf) 
        {
            settingsPanel.gameObject.SetActive(false);
            escMenuPanel.gameObject.SetActive(true);
            backGameBtn.Select();
        }
        if (mapPanel.gameObject.activeSelf) 
        {
            mapPanel.gameObject.SetActive(false);
            playerController.act = true;
        }
        yield return new WaitForSeconds(0.5f);
        canPressEscKey = true; 
    }

    private IEnumerator HandleTInput()
    {
        canPressTKey = false;
        if (playerController.act && !(taskPanel.gameObject.activeSelf)) {
            taskPanel.gameObject.SetActive(true);
        }
        else if (playerController.act && taskPanel.gameObject.activeSelf) 
        {
            taskPanel.gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(0.5f);
        canPressTKey = true; 
    }

    private IEnumerator HandleMInput()
    {
        canPressMKey = false;
        if (playerController.act && !(mapPanel.gameObject.activeSelf)) {
            mapPanel.gameObject.SetActive(true);
            playerController.act = false;
        }
        else if (!playerController.act && mapPanel.gameObject.activeSelf) 
        {
            mapPanel.gameObject.SetActive(false);
            playerController.act = true;
        }
        yield return new WaitForSeconds(0.5f);
        canPressMKey = true; 
    }

    private void quitGameListener()
    {
        Application.Quit();
    }

    private async void playListener()
    {
        startPanel.gameObject.SetActive(false);
        selectGamePanel.gameObject.SetActive(true);
        playGame_1.Select();
        data1.SetText("available");
        data2.SetText("available");
        data3.SetText("available");
        game1 = null;
        game2 = null;
        game3 = null;
        try
        {
            string data = "{\"authentication_id\":\"" + authentication_id + "\"}";
            HttpClient client = new HttpClient();
            StringContent queryString = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://iab-services.ddns.net/api/gta_speichersdorf/get_user_data", queryString);
            string responseBody = await response.Content.ReadAsStringAsync();
            playerData = JsonConvert.DeserializeObject<jsonPlayerData>(responseBody);
            List<int> tempGameIds = playerData.games.Split(';').Select(int.Parse).ToList();
            try 
            {
                int game_id = tempGameIds[0];                                
                data = "{\"spielstand_id\":\"" + game_id + "\",\"authentication_id\":\"" + authentication_id + "\"}";
                client = new HttpClient();
                queryString = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
                response = await client.PostAsync("https://iab-services.ddns.net/api/gta_speichersdorf/get_spielstand", queryString);
                responseBody = await response.Content.ReadAsStringAsync();
                game1 = JsonConvert.DeserializeObject<jsonGameData>(responseBody);
                if (game1.name == null || game1.name == "") {deleteGame(game1.id, true);} else
                data1.SetText(game1.name + "<br>" + game1.ltp);
            } 
            catch //(Exception e) 
            {
                //Debug.Log(e);
            }
            try 
            {
                int game_id = tempGameIds[1];                                
                data = "{\"spielstand_id\":\"" + game_id + "\",\"authentication_id\":\"" + authentication_id + "\"}";
                client = new HttpClient();
                queryString = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
                response = await client.PostAsync("https://iab-services.ddns.net/api/gta_speichersdorf/get_spielstand", queryString);
                responseBody = await response.Content.ReadAsStringAsync();
                game2 = JsonConvert.DeserializeObject<jsonGameData>(responseBody);
                if (game2.name == null || game2.name == "") {deleteGame(game2.id, true);} else
                data2.SetText(game2.name + "<br>" + game2.ltp);
            }
            catch //(Exception e) 
            {
                //Debug.Log(e);
            }
            try 
            {
                int game_id = tempGameIds[2];                                
                data = "{\"spielstand_id\":\"" + game_id + "\",\"authentication_id\":\"" + authentication_id + "\"}";
                client = new HttpClient();
                queryString = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
                response = await client.PostAsync("https://iab-services.ddns.net/api/gta_speichersdorf/get_spielstand", queryString);
                responseBody = await response.Content.ReadAsStringAsync();
                game3 = JsonConvert.DeserializeObject<jsonGameData>(responseBody);
                if (game3.name == null || game3.name == "") {deleteGame(game3.id, true);} else
                data3.SetText(game3.name + "<br>" + game3.ltp);
            } 
            catch //(Exception e) 
            {
                //Debug.Log(e);
            }
        } 
        catch (Exception e) 
        {
            Debug.LogError(e);
        }
    }

    private void quitSelectListener()
    {
        selectGamePanel.gameObject.SetActive(false);
        startPanel.gameObject.SetActive(true);
        playBtn.Select();
    }

    private void backGameListener() 
    {
        if (active) {
            active = false;
            playerController.act = true;
            escMenuPanel.gameObject.SetActive(false);
        }
    }

    private void settingsListener()
    {
        escMenuPanel.gameObject.SetActive(false);
        settingsPanel.gameObject.SetActive(true);
        settingsSaveBtn.Select();

    }

    private void settingsSaveListener()
    {
        settingsPanel.gameObject.SetActive(false);
        escMenuPanel.gameObject.SetActive(true);
        backGameBtn.Select();
        playerData.bindings = actions.SaveBindingOverridesAsJson();
        updatePlayerDataDB();
    }

    private void quitSaveListenerGateway()
    {
        quitSaveListener(false);
    }

    public async void quitSaveListener(bool autosave) 
    {
        try 
        {
            gameData.respw = playerTransform.position.x + ";" + playerTransform.position.y;
            int game_id = gameData.id;  
            string waypointsToSave = "";
            foreach (GameObject waypoint in playerController.waypoints)
            {
                waypointsToSave += waypoint.transform.position.x + ":" + waypoint.transform.position.y + ";";
            }
            string lastTimePlayed = System.DateTime.Now.ToString("dd/MM/yyyy");
            string data = "{\"x\":\"" + playerTransform.position.x + "\",\"y\":\"" + playerTransform.position.y + "\",\"respw\":\"" + gameData.respw + "\",\"health\":\"" + gameData.health + "\",\"waypoints\":\"" + waypointsToSave + "\",\"z\":\"" + playerTransform.gameObject.layer + "\",\"y_rot\":\"" + playerRenderer.sortingLayerID + "\",\"spielstand_id\":\"" + game_id + "\",\"authentication_id\":\"" + authentication_id + "\",\"name\":\"" + gameData.name + "\",\"ltp\":\"" + lastTimePlayed + "\",\"unlocked_areas\":\"" + gameData.unlocked_areas + "\",\"story_id\":\"" + gameData.story_id + "\"}";      
            if (game_id == 0) {
                data = "{\"x\":\"" + playerTransform.position.x + "\",\"y\":\"" + playerTransform.position.y + "\",\"respw\":\"" + gameData.respw + "\",\"health\":\"" + gameData.health + "\",\"waypoints\":\"" + waypointsToSave + "\",\"z\":\"" + playerTransform.gameObject.layer + "\",\"y_rot\":\"" + playerRenderer.sortingLayerID + "\",\"spielstand_id\":\"" + "create" + "\",\"authentication_id\":\"" + authentication_id + "\",\"name\":\"" + gameData.name + "\",\"ltp\":\"" + lastTimePlayed + "\",\"unlocked_areas\":\"" + gameData.unlocked_areas + "\",\"story_id\":\"" + gameData.story_id + "\"}";
            }                         
            HttpClient client = new HttpClient();
            StringContent queryString = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://iab-services.ddns.net/api/gta_speichersdorf/save_spielstand", queryString);
            string responseBody = await response.Content.ReadAsStringAsync();
        } 
        catch (Exception e) 
        {
            Debug.LogError(e);
        }
        if (!autosave) 
        {
            storyManager.ResetMap();
            storyManager.enabled = false;
            playerController.act = false;
            UIminiMapPanel.gameObject.SetActive(false);
            //UIdialogOverlayPanel.gameObject.SetActive(false);
            escMenuPanel.gameObject.SetActive(false);
            startPanel.gameObject.SetActive(true);
            playBtn.Select();
            taskPanel.gameObject.SetActive(false);
            taskPanelData.SetText("\n\nCurrently no tasks available");
            try 
            {
                foreach (GameObject waypoint in playerController.waypoints.ToArray())
                {
                    try 
                    {
                        playerController.waypoints.Remove(waypoint);
                        Destroy(waypoint);
                    } catch (Exception e) {
                        //Debug.Log(e);
                    }
                }
            } catch (Exception e) {
                //Debug.Log(e);
            }
        }
    }

    private void loadDemoListener()
    {
        playerTransform.position = new Vector3(42.4f, -7.4f, 0);
        playerTransform.gameObject.layer = 20;
        gameData.health = 400;
        gameData.respw = "42.4;-7.4";
        gameData.story_id = "demo";

    }

    private async void loadDebugListener()                      // DEBUG
    {
        playerController.act = true;
        try
        {
            int game_id = 1026;                                
            string data = "{\"spielstand_id\":\"" + game_id + "\",\"authentication_id\":\"" + authentication_id + "\"}";
            HttpClient client = new HttpClient();
            StringContent queryString = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://iab-services.ddns.net/api/gta_speichersdorf/get_spielstand", queryString);
            string responseBody = await response.Content.ReadAsStringAsync();
            gameData = JsonConvert.DeserializeObject<jsonGameData>(responseBody);

            foreach (string part in gameData.waypoints.Split(';'))
            {
                try 
                {
                    string[] coords = part.Split(':');
                    float x = float.Parse(coords[0].Replace(",", "."), CultureInfo.InvariantCulture);
                    float y = float.Parse(coords[1].Replace(",", "."), CultureInfo.InvariantCulture);
                    Debug.Log(x + " " + y);
                    Debug.Log(coords[0] + " " + coords[1]);
                    Vector3 tempPosition = new Vector3(x, y, 0);
                    GameObject wp = playerController.CreateWaypoint(tempPosition);
                    wp.transform.position = tempPosition;
                } catch (Exception e) {
                    Debug.Log(e);
                }
            }
            
            playerTransform.position = new Vector3(gameData.x, gameData.y, 0);
            playerTransform.gameObject.layer = gameData.z;
            playerRenderer.sortingLayerID = gameData.y_rot;
            playerShadow.sortingLayerID = gameData.y_rot;
            storyManager.enabled = true;
            UIminiMapPanel.gameObject.SetActive(true);
            //UIdialogOverlayPanel.gameObject.SetActive(true);
        } 
        catch (Exception e) 
        {
            Debug.LogError(e);
        }
    }
}
