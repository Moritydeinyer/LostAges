using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace a1creator
{
    #if UNITY_EDITOR
    [CustomEditor(typeof(ControllerMouse))]
    [CanEditMultipleObjects]
    public class CustomInspectorControllerMouse : Editor
    {

        private bool showevents;

        private GUIStyle labelBackgroundStyle;

        SerializedProperty MoveCursor_Begin;
        SerializedProperty MoveCursor_While;
        SerializedProperty MoveCursor_End;

        SerializedProperty LeftClick_Down;
        SerializedProperty LeftClick_Up;

        SerializedProperty RightClick_Down;
        SerializedProperty RightClick_Up;

        SerializedProperty MiddleClick_Down;
        SerializedProperty MiddleClick_Up;

        void OnEnable()
        {
            labelBackgroundStyle = new GUIStyle();
            labelBackgroundStyle.normal = new GUIStyleState();
            labelBackgroundStyle.normal.textColor = Color.white;
            var texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, new Color(1f, 1f, 1f, 0.1f));
            texture.Apply();
            labelBackgroundStyle.normal.background = texture;

            // Fetch the objects from the GameObject script to display in the inspector
            MoveCursor_Begin = serializedObject.FindProperty("OnStartedCursorMove_WithController");
            MoveCursor_While = serializedObject.FindProperty("OnCursorMove_WithController");
            MoveCursor_End = serializedObject.FindProperty("OnEndedCursorMove_WithController");

            LeftClick_Down = serializedObject.FindProperty("OnLeftClickDown_WithController");
            LeftClick_Up = serializedObject.FindProperty("OnLeftClickUp_WithController");

            RightClick_Down = serializedObject.FindProperty("OnRightClickDown_WithController");
            RightClick_Up = serializedObject.FindProperty("OnRightClickUp_WithController");

            MiddleClick_Down = serializedObject.FindProperty("OnMiddleClickDown_WithController");
            MiddleClick_Up = serializedObject.FindProperty("OnMiddleClickUp_WithController");
        }

        private void BarLabel(string labelText)
        {
            var defaultColor = GUI.contentColor;
            GUILayout.BeginHorizontal(labelBackgroundStyle);
            GUI.contentColor = Color.white;
            GUILayout.Label(labelText, EditorStyles.boldLabel);
            GUI.contentColor = defaultColor;
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
        }

        public override void OnInspectorGUI()
        {
            //DrawDefaultInspector();

            ControllerMouse _controllerMouseScript = (ControllerMouse)target;

            GUILayout.Space(5);
            _controllerMouseScript._dontDestroy = EditorGUILayout.ToggleLeft(new GUIContent("Never destroy this object", "Sets DontDestroy(this) and destroys all other instances of this component, should there be more than one in the scene."), _controllerMouseScript._dontDestroy, EditorStyles.boldLabel);
            GUILayout.Space(5);
            _controllerMouseScript._debugComments = EditorGUILayout.ToggleLeft(new GUIContent("Write debug to console", "Enable this to debug the component. Will not output in a build, no matter the value."), _controllerMouseScript._debugComments, EditorStyles.boldLabel);
            GUILayout.Space(5);
            _controllerMouseScript._cursorBehaviour = (ControllerMouse.ControllerMouseSpeedBehaviour)GUILayout.Toolbar((int)_controllerMouseScript._cursorBehaviour, new string[] { ((ControllerMouse.ControllerMouseSpeedBehaviour)0).ToString(), ((ControllerMouse.ControllerMouseSpeedBehaviour)1).ToString() });
            GUILayout.Space(5);
            _controllerMouseScript.ControllerMouseDefaultSpeed = EditorGUILayout.FloatField(new GUIContent("Controller Mouse Speed"), _controllerMouseScript.ControllerMouseDefaultSpeed);
            GUILayout.Space(5);
            _controllerMouseScript.StickDriftThreshold = EditorGUILayout.FloatField(new GUIContent("Stick Drift Threshold"), _controllerMouseScript.StickDriftThreshold);
            GUILayout.Space(5);
            _controllerMouseScript.TimeBeforeStopMovingStick = EditorGUILayout.FloatField(new GUIContent("Seconds to wait before stopping movement on release"), _controllerMouseScript.TimeBeforeStopMovingStick);


            GUILayout.Space(5);
            showevents = EditorGUILayout.Foldout(showevents, "Mouse Simulation Events");
            if (showevents)
            {
                GUILayout.Space(10);
                BarLabel("Cursor Movement");
                EditorGUILayout.PropertyField(MoveCursor_Begin, new GUIContent("On Started Moving Cursor With Controller"));
                EditorGUILayout.PropertyField(MoveCursor_While, new GUIContent("While Moving Cursor With Controller"));
                EditorGUILayout.PropertyField(MoveCursor_End, new GUIContent("On Ended Moving Cursor With Controller"));

                GUILayout.Space(10);
                BarLabel("Left Click");
                EditorGUILayout.PropertyField(LeftClick_Down, new GUIContent("On LeftMouseDown With Controller"));
                EditorGUILayout.PropertyField(LeftClick_Up, new GUIContent("On LeftMouseUp With Controller"));

                GUILayout.Space(10);
                BarLabel("Right Click");
                EditorGUILayout.PropertyField(RightClick_Down, new GUIContent("On RightMouseDown With Controller"));
                EditorGUILayout.PropertyField(RightClick_Up, new GUIContent("On RightMouseUp With Controller"));

                GUILayout.Space(10);
                BarLabel("Middle Click");
                EditorGUILayout.PropertyField(MiddleClick_Down, new GUIContent("On MiddleMouseDown With Controller"));
                EditorGUILayout.PropertyField(MiddleClick_Up, new GUIContent("On MiddleMouseUp With Controller"));

            }
            GUILayout.Space(5);

            if (GUI.changed)
            {
                EditorUtility.SetDirty(_controllerMouseScript);
                serializedObject.ApplyModifiedProperties();
            }
        }

    }
    #endif


    // I haven't implemented MacOS mouse yet. This is the reference I plan to use:
    // https://discussions.unity.com/t/how-can-i-control-the-mouse-move-and-send-mouse-click-signals-via-the-keyboard/85297/3
    // If you beat me to it, please send your script to me @ customerhelp.a1a3@gmail.com ! I will add it as an update.
    // Also, feel free to suggest improvements to the code !

    [System.Serializable]
    public class ControllerMouse : MonoBehaviour
    {
        public static bool IsRebinding { get; set; }
        public static float ControllerMouseSpeedMultiplier { get; set; } = 1f;
        // The reason this is set to false is so that you can use the script
        // in level scenes when testing, and not have it cause errors
        // when you load the scene from your menu where you also have the script.
        [SerializeField] public bool _dontDestroy = false;

        // This is just for debugging if you can't get it to work.
        // Since it should work, this should theoretically never have to be used.
        [SerializeField] public bool _debugComments = false;
        private bool DebugComments
        {
            get
            {
                // It's your choice if you want console output in the editor
    #if UNITY_EDITOR
                return _debugComments;
    #endif
                // When we aren't using the editor (fx. in a build) it will never output to console
                return false;
            }
        }

        // All of this is just to make the inspector more readable. It could literally just be one bool.
        public enum ControllerMouseSpeedBehaviour { SensitiveSpeed, ConstantSpeed, }
        [SerializeField] public ControllerMouseSpeedBehaviour _cursorBehaviour;
        private bool ConstantMouseSpeed { get => _cursorBehaviour == ControllerMouseSpeedBehaviour.ConstantSpeed; }
        [field: SerializeField] public float ControllerMouseDefaultSpeed { get; set; } = 10f;

        // How many seconds after releasing the stick should we stop moving?
        // (Stick issues can trigger release on random frames if this isn't above 0)
        [field: SerializeField] public float TimeBeforeStopMovingStick { get; set; } = 0.05f;

        // How large must the magnitude of the stick be before we count it as an intentional move?
        [field: SerializeField] public float StickDriftThreshold { get; set; } = 0.05f;

        // This is only used in one calculation. I'm not sure if it saves memory to reuse it or if it could just be replace with var ? - Better safe than sorry
        private Vector2 mousePosition;
        // Controller stick direction
        private Vector2 mouseDirection;


        // ---------------------------------------------------------------------------------------
        // These should be properties but Custom Editor can't display properties for some reason
        // ---------------------------------------------------------------------------------------

        // Cursor Move With Controller
        [field: SerializeField] public UnityEvent<Vector2> OnStartedCursorMove_WithController = new UnityEvent<Vector2>();
        [field: SerializeField] public UnityEvent<Vector2> OnCursorMove_WithController = new UnityEvent<Vector2>();
        [field: SerializeField] public UnityEvent OnEndedCursorMove_WithController = new UnityEvent();

        // Left Click With Controller
        [field: SerializeField] public UnityEvent OnLeftClickDown_WithController = new UnityEvent();
        [field: SerializeField] public UnityEvent OnLeftClickUp_WithController = new UnityEvent();

        // Right Click With Controller
        [field: SerializeField] public UnityEvent OnRightClickDown_WithController = new UnityEvent();
        [field: SerializeField] public UnityEvent OnRightClickUp_WithController = new UnityEvent();

        // Middle Click With Controller
        [field: SerializeField] public UnityEvent OnMiddleClickDown_WithController = new UnityEvent();
        [field: SerializeField] public UnityEvent OnMiddleClickUp_WithController = new UnityEvent();


        // Singleton that isn't really a singleton.. hmm...
        private static ControllerMouse instance;
        private void Awake()
        {
            // Keep this instance in all scenes
            if (_dontDestroy)
                DontDestroyOnLoad(this);
            // Remove other instances if we are keeping one instance
            if (instance == null)
                instance = this;
            else
            {
                Destroy(this.gameObject);
                // reload the scripts that were removed from priority by this.gameObject
                instance.gameObject.SetActive(false);
                instance.gameObject.SetActive(true);
            }
        }


        private void Update()
        {
            if (IsRebinding) return;
            if (stoppedMovingStick)
            {
                stoppedMovingStickTimer += Time.unscaledDeltaTime;
                if (stoppedMovingStickTimer >= TimeBeforeStopMovingStick)
                {
                    stoppedMovingStick = false;
                    stoppedMovingStickTimer = 0f;
                    // behaviour
                    isMovingControllerMouse = false;
                    OnEndedCursorMove_WithController.Invoke();
                    if (DebugComments)
                        Debug.Log("Stopped moving cursor by controller.");
                }
            }

            if (isMovingControllerMouse)
            {
                // This is Windows cursor. it triggers all events as a mouse would
                var mouseTravel = mouseDirection * ControllerMouseDefaultSpeed * ControllerMouseSpeedMultiplier * Time.unscaledDeltaTime * 100f;
                MouseOperations.MouseMoveEvent(new MouseOperations.MousePoint((int)mouseTravel.x, -(int)mouseTravel.y));
            }
        }

        // to prevent activation by stick drift i do this manually instead of with context.started (doesn't work for some reason)
        // if you have stick drift it's always receiving a number
        private bool isMovingControllerMouse = false;
        // broken controllers set passthrough to 0 for weird frames, this counteracts that issue
        private bool stoppedMovingStick = false;
        private float stoppedMovingStickTimer = 0f;

        public void OnSimulateMouse(InputAction.CallbackContext context)
        {
            if (IsRebinding) return;
            var vec = context.ReadValue<Vector2>();
            if (ConstantMouseSpeed)
                vec = vec.normalized;

            if (vec.magnitude > StickDriftThreshold)
            {
                if (!isMovingControllerMouse)
                {
                    OnStartedCursorMove_WithController.Invoke(vec);
                    if (DebugComments)
                        Debug.Log("Started moving cursor by controller | " + vec);
                }
                isMovingControllerMouse = true;
                stoppedMovingStick = false;
                stoppedMovingStickTimer = 0f;
                mouseDirection = vec;
                OnCursorMove_WithController.Invoke(vec);
                // No point in outputting every frame when moving
            }
            else
            {
                if (isMovingControllerMouse)
                {
                    stoppedMovingStick = true;
                }
            }
        }

        public void OnLeftClick(InputAction.CallbackContext context)
        {
            if (IsRebinding) return;

            if (context.started)
            {
                OnLeftClickDown_WithController.Invoke();
                MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftDown);
                if (DebugComments)
                    Debug.Log("OnLeftClickDown as controller, context=" + context.phase);
            }
            if (context.canceled)
            {
                OnLeftClickUp_WithController.Invoke();
                MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp);
                if (DebugComments)
                    Debug.Log("OnLeftClickUp as controller, context=" + context.phase);
            }
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            if (IsRebinding) return;
            if (context.started)
            {
                OnRightClickDown_WithController.Invoke();
                MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.RightDown);
                if (DebugComments)
                    Debug.Log("OnRightClickDown as controller, context=" + context.phase);
            }
            if (context.canceled)
            {
                OnRightClickUp_WithController.Invoke();
                MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.RightUp);
                if (DebugComments)
                    Debug.Log("OnRightClickUp as controller, context=" + context.phase);
            }
        }




        public void OnMiddleClick(InputAction.CallbackContext context)
        {
            if (IsRebinding) return;
            if (context.started)
            {
                OnMiddleClickDown_WithController.Invoke();
                MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.MiddleDown);
                if (DebugComments)
                    Debug.Log("OnMiddleClickDown as controller, context=" + context.phase);
            }
            if (context.canceled)
            {
                OnMiddleClickUp_WithController.Invoke();
                MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.MiddleUp);
                if (DebugComments)
                    Debug.Log("OnMiddleClickUp as controller, context=" + context.phase);
            }
        }


    }

}