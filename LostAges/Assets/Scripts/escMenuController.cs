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
using System.Threading;
using UnityEngine.AI;
using System.Globalization;
using Unity.Collections;
using System.Dynamic;
using UnityEngine.Rendering;




[System.Serializable]
public class PayPalAuthResponse
{
    public string access_token;
}
[System.Serializable]
public class PayPalPaymentResponse
{
    public string id;
    public string intent;
    public string state;
    public string payer;
    public string transactions;
    public string description;
    public string create_time;
    public Link[] links;
}
[System.Serializable]
public class Link
{
    public string href;
    public string rel;
}

public class jsonGameData {
    public string id { get; set; }
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

public class requestPaymentData {
    public string approval_link { get; set; }
    public string access_token { get; set; }
}

public class jsonPlayerData
{
    public int id { get; set; }
    public string authentication_id { get; set; }
    public string add { get; set; }
    public string username { get; set; }
    public string games { get; set; }
    public int settingsVolume { get; set; }
    public string bindings { get; set; }
    public string moskito { get; set; }
    public string diagnostics { get; set; }
    public string autosave { get; set; }
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
    [SerializeField] public TextMeshProUGUI UIinteractionPanelDataKeyboard;
    [SerializeField] public TextMeshProUGUI UIinteractionPanelDataXbox;
    [SerializeField] public TextMeshProUGUI UIinteractionPanelDataPS;
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
    [SerializeField] public TextMeshProUGUI UIdataKey;
    [SerializeField] public TextMeshProUGUI UIdataXbox;
    [SerializeField] public TextMeshProUGUI UIdataPS;
    [SerializeField] public Image keyIconImage;
    [SerializeField] public Image xboxIconImage;
    [SerializeField] public Image psIconImage;
    [SerializeField] public TextMeshProUGUI UIdataKeyTalk;
    [SerializeField] public TextMeshProUGUI UIdataXboxTalk;
    [SerializeField] public TextMeshProUGUI UIdataPSTalk;
    [SerializeField] public Image keyIconImageTalk;
    [SerializeField] public Image xboxIconImageTalk;
    [SerializeField] public Image psIconImageTalk;
    [SerializeField] public TextMeshProUGUI UIdataKeyTalk2;
    [SerializeField] public TextMeshProUGUI UIdataXboxTalk2;
    [SerializeField] public TextMeshProUGUI UIdataPSTalk2;
    [SerializeField] public Image keyIconImageTalk2;
    [SerializeField] public Image xboxIconImageTalk2;
    [SerializeField] public Image psIconImageTalk2;
    [SerializeField] public InputIconLibrary inputIconLibrary;

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
    [SerializeField] private Toggle sendDiagnosticsToggle;
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
    [SerializeField] private Button shopBtn;
    [SerializeField] private Transform startPanel;

    [Header("Insert Game Name Panel")]
    
    [SerializeField] private Button playBtnII;
    [SerializeField] private TMP_InputField gameNameInput;
    [SerializeField] private Transform insertGameNamePanel;
    [SerializeField] public GameObject OSK;
    [SerializeField] private Button oskEnter;

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
    public jsonPlayerData playerData = new jsonPlayerData();
    public string authentication_id;                      
    private PlayerProfile playerProfile;
    private int y_rot = -45784578;

    [Header("Szene Switch")]
    [SerializeField] private Image switchPanel;
    private float fadeDuration = 1f;

    [Header("In Game Purchase")]
    private HttpListener httpListener;
    private Thread listenerThread;
    private bool isListening = false;

    requestPaymentData paymentData;

    [SerializeField] private Transform shopPanel;
    [SerializeField] private Button quitShopBtn;

    [SerializeField] private Button buyBtn1;
    [SerializeField] private Button buyBtn2;
    [SerializeField] private Button buyBtn3;
    [SerializeField] private Button buyBtn4;

    [SerializeField] private Button plusBtn1;
    [SerializeField] private Button plusBtn2;
    [SerializeField] private Button plusBtn3;
    [SerializeField] private Button plusBtn4;

    [SerializeField] private Button minusBtn1;
    [SerializeField] private Button minusBtn2;
    [SerializeField] private Button minusBtn3;
    [SerializeField] private Button minusBtn4;

    [SerializeField] private TextMeshProUGUI amount1;
    [SerializeField] private TextMeshProUGUI amount2;
    [SerializeField] private TextMeshProUGUI amount3;
    [SerializeField] private TextMeshProUGUI amount4;

    [Header("Version Control")]
    [SerializeField] private GameObject versionControllPanel;
    [SerializeField] private Button versionControllPanelBtn;

    [SerializeField] public audioManager audioManager;
    public int autosave = 300; // Autosave alle 5 Minuten
    [SerializeField] private TMP_InputField autosaveintervall;
    [SerializeField] public saveManager saveManager;


    [SerializeField] public Button showSchriftrolle1Btn;
    [SerializeField] public Button showSchriftrolle2Btn;
    [SerializeField] public GameObject PanelSchriftrolle1;
    [SerializeField] public GameObject PanelSchriftrolle2;

    [SerializeField] private List<Button> InventoryButtons;
    private int selectedIndex = 0;
    private float lastInputTime = 0f;
    private float inputCooldown = 0.2f; // 200ms Schutz gegen Dauer-Trigger

    public int level;



    void Start()
    {
        GraphicsSettings.transparencySortMode = TransparencySortMode.CustomAxis;
        GraphicsSettings.transparencySortAxis = new Vector3(0, 1, 1);

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
        loggedOutPanel.gameObject.SetActive(true); //DEBUG build true
        taskPanel.gameObject.SetActive(false);
        UIminiMapPanel.gameObject.SetActive(false); //DEBUG build false
        UIinteractionPanel.gameObject.SetActive(false);
        //UIdialogOverlayPanel.gameObject.SetActive(false);
        storyManager.enabled = false;   //DEBUG build
        active = false;
        playerController.act = false; //DEBUG build false
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
        shopBtn.onClick.AddListener(shopListener);
        quitShopBtn.onClick.AddListener(quitShopListener);



        plusBtn1.onClick.AddListener(() => { amount1.text = (playerData.moskito == "bought") ? "0" : "1"; });
        plusBtn2.onClick.AddListener(() => { amount2.text = (int.Parse(amount2.text) + 1).ToString(); });
        plusBtn3.onClick.AddListener(() => { amount3.text = (int.Parse(amount3.text) + 1).ToString(); });
        plusBtn4.onClick.AddListener(() => { amount4.text = (int.Parse(amount4.text) + 1).ToString(); });

        minusBtn1.onClick.AddListener(() => { amount1.text = Math.Max(0, int.Parse(amount1.text) - 1).ToString(); });
        minusBtn2.onClick.AddListener(() => { amount2.text = Math.Max(0, int.Parse(amount2.text) - 1).ToString(); });
        minusBtn3.onClick.AddListener(() => { amount3.text = Math.Max(0, int.Parse(amount3.text) - 1).ToString(); });
        minusBtn4.onClick.AddListener(() => { amount4.text = Math.Max(0, int.Parse(amount4.text) - 1).ToString(); });

        buyBtn1.onClick.AddListener(() => { BuyItem(1); });
        buyBtn2.onClick.AddListener(() => { BuyItem(2); });
        buyBtn3.onClick.AddListener(() => { BuyItem(3); });
        buyBtn4.onClick.AddListener(() => { BuyItem(4); });


        GeneralSettingsBtn.onClick.AddListener(() =>
        {
            GeneralPanel.gameObject.SetActive(true);
            ControllerPanel.gameObject.SetActive(false);
            KeyboardPanel.gameObject.SetActive(false);
        });

        ControllerSettingsBtn.onClick.AddListener(() =>
        {
            GeneralPanel.gameObject.SetActive(false);
            ControllerPanel.gameObject.SetActive(true);
            KeyboardPanel.gameObject.SetActive(false);
            actions.LoadBindingOverridesFromJson("");
            loadBindings();
        });

        KeyboardSettingsBtn.onClick.AddListener(() =>
        {
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


        version();
        tryAutoLogin();
        Debug.LogWarning("Esc Menu Controller started");
        showSchriftrolle1Btn.onClick.AddListener(() =>
        {
            showSchriftrolle1();
        });
        showSchriftrolle2Btn.onClick.AddListener(() =>
        {
            showSchriftrolle2();
        });
        Debug.LogWarning("Esc Menu Controller initialized");
    }
    
    public void ChangeItem(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Vector2 input = context.ReadValue<Vector2>();
        float y = input.y;

        // Cooldown gegen zu viele Inputs
        if (Time.time - lastInputTime < inputCooldown) return;

        if (Mathf.Abs(y) > 0.5f)
        {
            if (y < 0) // Scroll/Mausrad runter → nächstes Item
                selectedIndex++;
            else        // Scroll/Mausrad hoch → vorheriges Item
                selectedIndex--;

            // Index begrenzen
            if (selectedIndex < 0) selectedIndex = InventoryButtons.Count - 1;
            if (selectedIndex >= InventoryButtons.Count) selectedIndex = 0;

            // Fokus setzen
            EventSystem.current.SetSelectedGameObject(InventoryButtons[selectedIndex].gameObject);
            lastInputTime = Time.time;
        }
    }

    async private void version()
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync("https://iab-services.ddns.net/api/gta_speichersdorf/version");
        string responseBody = await response.Content.ReadAsStringAsync();
        if (!responseBody.Contains("b737ca7ee563ae80e457bb3d1dfe64edd2b4c015a8f88b6f87d5c113b68897fd"))
        {
            versionControllPanel.gameObject.SetActive(true);
            versionControllPanelBtn.onClick.AddListener(() =>
            {
                versionControllPanel.gameObject.SetActive(false);
                Application.Quit();
            });
        }
    }

    async private void tryAutoLogin()
    {
        playerData = await saveManager.getPlayerData(null);
        if (playerData.authentication_id != null && playerData.authentication_id != "")
        {
            authentication_id = playerData.authentication_id;
            loggedOutPanel.gameObject.SetActive(false);
            startPanel.gameObject.SetActive(true);
            audioManager.StartMoskitoSFX();
            playBtn.Select();
            loadVolume();
            loadBindings();
            sync();
        }
        else
        {

        }
    }

    async private void sync()
    {
        await saveManager.ProcessSyncQueue(authentication_id);
    }

    private void BuyItem(int item)
    {
        int quantity = 0;
        String price = "0";
        if (item == 1)
        {
            quantity = int.Parse(amount1.text);
            price = "1.99";
            Debug.Log("Item 1 gekauft");
        }
        else if (item == 2)
        {
            quantity = int.Parse(amount2.text);
            price = "2.99";
            Debug.Log("Item 2 gekauft");
        }
        else if (item == 3)
        {
            quantity = int.Parse(amount3.text);
            price = "3.99";
            Debug.Log("Item 3 gekauft");
        }
        else if (item == 4)
        {
            quantity = int.Parse(amount4.text);
            price = "4.99";
            Debug.Log("Item 4 gekauft");
        }
        if (quantity > 0)
        {
            Debug.Log("Kauf wird verarbeitet...");
            requestPayment(quantity, price, item);
        }
        else
        {
            Debug.Log("Keine Menge ausgewählt!");
        }
    }

    public async void requestPayment(int quantity, String price, int item) 
    {
        //DEBUG
        try 
        {
            string data = "{\"quantity\":\"" + quantity + "\",\"authentication_id\":\"" + authentication_id + "\",\"price\":\"" + price + "\",\"item_id\":\"" + item + "\"}";                              
            HttpClient client = new HttpClient();
            StringContent queryString = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://iab-services.ddns.net/api/gta_speichersdorf/request_payment", queryString);
            string responseBody = await response.Content.ReadAsStringAsync();
            Debug.Log(responseBody);
            paymentData = JsonConvert.DeserializeObject<requestPaymentData>(responseBody);
            Application.OpenURL(paymentData.approval_link);
        } 
        catch (Exception e) 
        {
            Debug.LogError(e);
        }
    } 


    public void shopListener()
    {
        startPanel.gameObject.SetActive(false);
        shopPanel.gameObject.SetActive(true);
        StartServer();
    }

    public void quitShopListener()
    {
        shopPanel.gameObject.SetActive(false);
        startPanel.gameObject.SetActive(true);
        playBtn.Select();
        StopServer();
    }

    public void StartServer()
    {
        if (!HttpListener.IsSupported)
        {
            Debug.LogError("HTTP Listener wird nicht unterstützt!");
            return;
        }

        httpListener = new HttpListener();
        httpListener.Prefixes.Add("http://localhost:5000/success/");  // URL für Rückgabe von PayPal
        httpListener.Start();

        isListening = true;
        listenerThread = new Thread(ListenForRequests);
        listenerThread.Start();
    }

    public void StopServer()
    {
        isListening = false;
        httpListener?.Stop();
        listenerThread?.Abort();
    }

    private void ListenForRequests()
    {
        while (httpListener.IsListening)
        {
            HttpListenerContext context = httpListener.GetContext();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            if (request.Url.Query.Contains("paymentId") && request.Url.Query.Contains("PayerID") && request.Url.Query.Contains("token"))
            {
                string paymentId = request.QueryString["paymentId"];
                string payerId = request.QueryString["PayerID"];
                string token = request.QueryString["token"];
                Debug.Log($"Zahlung bestätigt! PaymentId: {paymentId}, PayerId: {payerId}");
                exec_payment(paymentId, payerId, token);
                response.StatusCode = 302; 
                response.Headers["Location"] = "https://iab-services.ddns.net/api/gta_speichersdorf/payment/success"; 
             }
            response.Close();
        }
    }

    private async void exec_payment(String paymentId, String payerId, String token)
    {
        try {
            Debug.Log($"Zahlung bestätigt! PaymentId: {paymentId}, PayerId: {payerId}, Token: {token}");
            Debug.Log(authentication_id);
            string data = "{\"paymentID\":\"" + paymentId + "\",\"payerID\":\"" + payerId + "\",\"token\":\"" + token + "\",\"authentication_id\":\"" + authentication_id + "\"}";
            HttpClient client = new HttpClient();
            StringContent queryString = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage res = await client.PostAsync("https://iab-services.ddns.net/api/gta_speichersdorf/exec_payment", queryString);
            string responseBody = await res.Content.ReadAsStringAsync();
            Debug.Log(responseBody);
        } 
        catch (Exception e) 
        {
            Debug.LogError(e);
        }
    }

    private string GetQueryValue(string query, string key)
    {
        var parts = query.Trim('?').Split('&');
        foreach (var part in parts)
        {
            var kv = part.Split('=');
            if (kv.Length == 2 && kv[0] == key)
                return kv[1];
        }
        return null;
    }

    private GameObject lastSelectedObject;

    void FixedUpdate()
    {
        //DEBUG TEST START

        //DEBUG TEST ENDE

        if (insertGameNamePanel.gameObject.activeSelf)
        {
            if (useController)
            {
                OSK.gameObject.SetActive(true);
                RectTransform rt = gameNameInput.GetComponent<RectTransform>();
                Vector2 pos = rt.anchoredPosition;
                pos.y = 205f;
                rt.anchoredPosition = pos;
                if (lastSelectedObject != oskEnter.gameObject)
                {
                    oskEnter.Select();
                    lastSelectedObject = oskEnter.gameObject;
                }
            }
            else
            {
                RectTransform rt = gameNameInput.GetComponent<RectTransform>();
                Vector2 pos = rt.anchoredPosition;
                pos.y = 41f;
                rt.anchoredPosition = pos;
                OSK.gameObject.SetActive(false);
                if (lastSelectedObject != gameNameInput.gameObject)
                {
                    gameNameInput.Select();
                    lastSelectedObject = gameNameInput.gameObject;
                }
            }
        }
        else
        {
            lastSelectedObject = null; 
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

        string keyboardScheme = "Keyboard&Mouse";
        string gamepadScheme = "Gamepad";
        var action = actions.FindAction("Interact");
        var action2 = actions.FindAction("Talk");
        if (action != null) 
        {
            ShowBinding(keyIconImage, UIdataKey, action, keyboardScheme, false);
            ShowBinding(xboxIconImage, UIdataXbox, action, gamepadScheme, true);
            ShowBinding(psIconImage, UIdataPS, action, gamepadScheme, false);
        }
        if (action2 != null) 
        {
            ShowBinding(keyIconImageTalk, UIdataKeyTalk, action2, keyboardScheme, false);
            ShowBinding(xboxIconImageTalk, UIdataXboxTalk, action2, gamepadScheme, true);
            ShowBinding(psIconImageTalk, UIdataPSTalk, action2, gamepadScheme, false);

            ShowBinding(keyIconImageTalk2, UIdataKeyTalk2, action2, keyboardScheme, false);
            ShowBinding(xboxIconImageTalk2, UIdataXboxTalk2, action2, gamepadScheme, true);
            ShowBinding(psIconImageTalk2, UIdataPSTalk2, action2, gamepadScheme, false);
        }
    }

    private void ShowBinding(Image image, TextMeshProUGUI text, InputAction action, string schemeName, bool xBox)
    {
        image.enabled = false;
        text.enabled = false;
        text.text = "";
        foreach (var binding in action.bindings)
        {
            if (binding.groups != null && binding.groups.Contains(schemeName))
            {
                string path = binding.effectivePath;
                Sprite icon = inputIconLibrary.GetIconForBinding(path, xBox);
                if (icon != null)
                {
                    image.sprite = icon;
                    image.enabled = true;
                }
                else
                {
                    image.enabled = false;
                    text.text = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);
                    text.enabled = true;
                }
                return;
            }
        }
        text.text = "N/A";
        text.enabled = true;
        image.enabled = false;
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
                sync();
                yield return new WaitForSeconds(2f);
                savingText.enabled = false;
            }
            yield return new WaitForSeconds(autosave); // DEBUG 5 min 
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

    public void TeleportWithFade(Vector3 targetPosition, GameObject go)
    {
        StartCoroutine(TeleportRoutine(targetPosition, go));
    }

    public void showSchriftrolle1()
    {
        if (PanelSchriftrolle1.activeSelf)
        {
            PanelSchriftrolle1.SetActive(false);
        }
        else
        {
            PanelSchriftrolle1.SetActive(true);
        }
    }
    public void showSchriftrolle2()
    { 
        if (PanelSchriftrolle2.activeSelf)
        {
            PanelSchriftrolle2.SetActive(false);
        }
        else
        {
            PanelSchriftrolle2.SetActive(true);
        }
    }

    private IEnumerator TeleportRoutine(Vector3 targetPosition, GameObject go)
    {
        // Schritt 1: Fade to Black
        yield return StartCoroutine(Fade(0f, 1f, false));

        // Schritt 2: Teleportation
        go.transform.position = targetPosition;

        // Optional: kleine Wartezeit beim Blackout
        yield return new WaitForSeconds(0.1f);

        // Schritt 3: Fade Back
        yield return StartCoroutine(Fade(1f, 0f, false));
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
        deleteGame(game1, true);
    }
    public void deleteG2Listener() 
    {
        deleteGame(game2, true);
    }
    public void deleteG3Listener() 
    {
        deleteGame(game3, true);
    }

    public async void loginAuthListener()
    {
        await loginController.InitSignIn();
    }

    private async void LoginController_OnSignedIn(PlayerProfile profile)
    {
        Debug.Log("Signed in: " + profile.playerInfo.Id + "\n" + profile.playerInfo.Username + "\n" + profile.playerInfo.CreatedAt + "\n" + profile.playerInfo.GetType());
        authentication_id = profile.playerInfo.Id;
        playerData = await saveManager.getPlayerData(authentication_id);
        Debug.LogError(playerData);
        if (playerData == null)
        {
            Debug.LogWarning("Server nicht erreichbar - Lokale PlayerData wird neu erstellt.");
            
            playerData = new jsonPlayerData
            {
                authentication_id = authentication_id,
                username = profile.playerInfo.Username ?? "",
                games = "",
                settingsVolume = 0,
                bindings = "",
                moskito = "",
                diagnostics = "0",
                autosave = "500"
            };

            saveManager.SavePlayerDataLocally(playerData);

            var formattedSettings = new FormattedPlayerSettings
            {
                authentication_id = playerData.authentication_id,
                volume = playerData.settingsVolume.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture).Replace(".", ","),
                bindings = playerData.bindings,
                diagnostics = playerData.diagnostics,
                autosave = playerData.autosave
            };

            saveManager.AddToQueue("account", "0", JsonConvert.SerializeObject(formattedSettings));
        }

        loggedOutPanel.gameObject.SetActive(false);
        startPanel.gameObject.SetActive(true);
        audioManager.StartMoskitoSFX();
        playBtn.Select();
        loadVolume();
        loadBindings();
    }

    private void LoginController_OnAvatarUpdate(PlayerProfile profile)
    {
        playerProfile = profile;
    }

    public void deleteGame(jsonGameData gameDataTemp, bool play)
    {
        try
        {
            saveManager.updateSpielstand(authentication_id, gameDataTemp, 0);
            if (play) { playListener(); }
        } 
        catch (Exception e)
        {
            Debug.Log("dl1 " + e);
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
        sync();
        saveManager.logout();
    }

    public void setVolume(float volume) 
    {
        mainMixer.SetFloat("masterVolume", volume);
        playerData.settingsVolume = (int)volume;
        updatePlayerDataDB();
    }

    public void updatePlayerDataDB()
    {
        saveManager.updatePlayerData(authentication_id, playerData);
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

    public void saveGameNameListener()
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
                storyManager.enabled = true;
                if (playerController.act)
                {
                    if (storyManager.checkStoryIDgraterThan("100"))
                    {
                        bool inK1 = playerTransform.position.x < playerController.StartK1Trigger.transform.position.x;
                        bool inK3 = playerTransform.position.x > playerController.StartK3Trigger.transform.position.x;
                        bool inK2 = !inK1 && !inK3 && playerTransform.position.y > playerController.StartK2Trigger.transform.position.y;

                        playerController.K1.SetActive(inK1);
                        playerController.K2.SetActive(inK2);
                        playerController.K3.SetActive(inK3);

                        //DEBUG reset map // monolith tutorials...

                        if (inK1)
                        {
                            playerController.mainMap.SetActive(!(playerTransform.position.x <= playerController.K1K1Trigger.transform.position.x));
                        }
                        else if (inK3)
                        {
                            playerController.mainMap.SetActive(!(playerTransform.position.x >= playerController.K3K3Trigger.transform.position.x));
                        }
                        else if (inK2)
                        {
                            playerController.mainMap.SetActive(!(playerTransform.position.y >= playerController.K2K2Trigger.transform.position.y));
                        }
                        else
                        {
                            playerController.mainMap.SetActive(true);
                        }
                    }
                }
                UIminiMapPanel.gameObject.SetActive(true);
                foreach (GameObject waypoint in playerController.waypoints.ToArray())
                {
                    playerController.DeleteWaypoint(waypoint);
                }
                if (gameData.waypoints != null && gameData.waypoints != "")
                {
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
                }
                //UIdialogOverlayPanel.gameObject.SetActive(true); DEBUG
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
            gameData.id = "0";
            
            gameData.unlocked_areas = 0;
            gameData.story_id = "0";
            gameData.health = 400;
            gameData.respw = storyManager.SpawnPoint.transform.position.x + ";" + storyManager.SpawnPoint.transform.position.y;            
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
        if (storyManager.RunenEntziffernUIPanel.gameObject.activeSelf)
        {
            storyManager.RunenEntziffernUIPanel.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            canPressEscKey = true; 
            yield break;
        }
        if (PanelSchriftrolle1.gameObject.activeSelf || PanelSchriftrolle2.gameObject.activeSelf)
        {
            PanelSchriftrolle1.SetActive(false);
            PanelSchriftrolle2.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            canPressEscKey = true;
            yield break;
        }
        if (escMenuPanel.gameObject.activeSelf || playerController.act)
        {
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
        if (storyManager.checkStoryIDgraterThan("128")) 
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
        yield return null;
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
        bool ready = false;
        sync();
        while (!ready)
        {
            try
            {
                Debug.Log(authentication_id);
                playerData = await saveManager.getPlayerData(authentication_id);
                audioManager.StartMoskitoSFX();
                if (playerData.games == null || playerData.games == "") { ready = true; }
                else
                {
                    List<string> tempGameIds = playerData.games.Split(';').ToList();
                    ready = true;
                    try
                    {
                        string game_id = tempGameIds[0];
                        game1 = await saveManager.getSpielstand(game_id, authentication_id);
                        if (game1.name == null || game1.name == "") { deleteGame(game1, false); ready = false; }
                        else if (game1.name == "")
                            data1.SetText("available");
                        else
                            data1.SetText(game1.name + "<br>" + game1.ltp);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                    try
                    {
                        string game_id = tempGameIds[1];
                        game2 = await saveManager.getSpielstand(game_id, authentication_id);
                        if (game2.name == null || game2.name == "") { deleteGame(game2, false); ready = false; }
                        else
                            data2.SetText(game2.name + "<br>" + game2.ltp);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                    try
                    {
                        string game_id = tempGameIds[2];
                        game3 = await saveManager.getSpielstand(game_id, authentication_id);
                        if (game3.name == null || game3.name == "") { deleteGame(game3, false); ready = false; }
                        else
                            data3.SetText(game3.name + "<br>" + game3.ltp);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            await Task.Delay(500);
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
        autosave = playerData.autosave != null && playerData.autosave != "" ? int.Parse(playerData.autosave) : 300;
        autosaveintervall.text = autosave.ToString();
        sendDiagnosticsToggle.isOn = playerData.diagnostics == "1";

    }

    private void settingsSaveListener()
    {
        settingsPanel.gameObject.SetActive(false);
        escMenuPanel.gameObject.SetActive(true);
        backGameBtn.Select();
        playerData.bindings = actions.SaveBindingOverridesAsJson();
        if (sendDiagnosticsToggle.isOn)
        {
            playerData.diagnostics = "1";
        }
        else
        {
            playerData.diagnostics = "0";
        }
        if (autosaveintervall.text == null || autosaveintervall.text == "")
        {
            autosave = 300; // Default 5 min
        }
        else
        {
            try
            {
                autosave = int.Parse(autosaveintervall.text);
                playerData.autosave = "" + autosave;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                autosave = 300; // Default 5 min
                playerData.autosave = "" + autosave;
            }
        }
        updatePlayerDataDB();
    }

    private void quitSaveListenerGateway()
    {
        quitSaveListener(false);
    }

    public void quitSaveListener(bool autosave) 
    {
        try 
        {
            if (gameData.name != null && gameData.name != "")
            {
                gameData.respw = playerTransform.position.x + ";" + playerTransform.position.y;
                string waypointsToSave = "";
                foreach (GameObject waypoint in playerController.waypoints)
                {
                    waypointsToSave += waypoint.transform.position.x + ":" + waypoint.transform.position.y + ";";
                }

                gameData.waypoints = waypointsToSave.TrimEnd(';');
                gameData.ltp = System.DateTime.Now.ToString("dd/MM/yyyy");

                if (gameData.id == "0")
                {
                    gameData.id = "create";
                }

                gameData.x = playerTransform.position.x;
                gameData.y = playerTransform.position.y;
                gameData.z = playerTransform.gameObject.layer;
                gameData.y_rot = playerRenderer.sortingLayerID;

                saveManager.updateSpielstand(authentication_id, gameData, 1);
                sync();
            } 
        } 
        catch (Exception e) 
        {
            Debug.LogError(e);
        }
        if (!autosave) 
        {
            //storyManager.ResetMap();
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
                        Debug.LogException(e);
                    }
                }
            } catch (Exception e) {
                Debug.LogException(e);
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
        return;
        playerController.act = true;
        try
        {
            int game_id = 1324;                                
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
