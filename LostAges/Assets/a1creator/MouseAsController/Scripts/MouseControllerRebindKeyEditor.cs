#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace a1creator
{

    /// <summary>
    /// This script is a modified version of the script that Unity provides with their InputActionRebinding sample
    /// ---
    /// A custom inspector for <see cref="MouseControllerRebindKey"/> which provides a more convenient way for
    /// picking the binding which to rebind.
    /// </summary>
    [CustomEditor(typeof(MouseControllerRebindKey))]
    public class MouseControllerRebindKeyEditor : Editor
    {
        private SerializedProperty _localSave;
        private SerializedProperty _actionToRebind;
        private SerializedProperty _bindingID;
        private SerializedProperty _bindingNameText;
        private SerializedProperty _rebindOverlay;
        private SerializedProperty _rebindOverlayMessage;
        private SerializedProperty _onRebindStart;
        private SerializedProperty _onRebindEnded;
        private SerializedProperty _onUpdateBindingUIEvent;
        private SerializedProperty _onRebindAbortedMessage;
        //private SerializedProperty m_DisplayStringOptionsProperty;

        private GUIContent m_BindingLabel = new GUIContent("Binding");
        private GUIContent m_UILabel = new GUIContent("UI");
        private GUIContent m_EventsLabel = new GUIContent("Events");
        private GUIContent[] m_BindingOptions;
        private string[] m_BindingOptionValues;
        private int m_SelectedBindingOption;

        private static class Styles
        {
            public static GUIStyle boldLabel = new GUIStyle("MiniBoldLabel");
        }

        protected void OnEnable()
        {
            _localSave = serializedObject.FindProperty("_localSave");
            _actionToRebind = serializedObject.FindProperty("_actionToRebind");
            _bindingID = serializedObject.FindProperty("_bindingID");
            _bindingNameText = serializedObject.FindProperty("_bindingNameText");
            _rebindOverlay = serializedObject.FindProperty("_rebindOverlay");
            _rebindOverlayMessage = serializedObject.FindProperty("_rebindOverlayMessage");
            _onUpdateBindingUIEvent = serializedObject.FindProperty("_onUpdateBindingUIEvent");
            _onRebindStart = serializedObject.FindProperty("_onRebindStart");
            _onRebindEnded = serializedObject.FindProperty("_onRebindEnded");
            _onRebindAbortedMessage = serializedObject.FindProperty("_onRebindAbortedMessage");

            RefreshBindingOptions();
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            // Binding section.
            EditorGUILayout.LabelField(m_BindingLabel, Styles.boldLabel);
            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUILayout.PropertyField(_actionToRebind);

                var newSelectedBinding = EditorGUILayout.Popup(m_BindingLabel, m_SelectedBindingOption, m_BindingOptions);
                if (newSelectedBinding != m_SelectedBindingOption)
                {
                    var bindingId = m_BindingOptionValues[newSelectedBinding];
                    _bindingID.stringValue = bindingId;
                    m_SelectedBindingOption = newSelectedBinding;
                }
            }

            // UI section.
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(m_UILabel, Styles.boldLabel);
            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUILayout.PropertyField(_localSave);
                EditorGUILayout.PropertyField(_bindingNameText);
                EditorGUILayout.PropertyField(_rebindOverlayMessage);
                EditorGUILayout.PropertyField(_rebindOverlay);
            }

            // Events section.
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(m_EventsLabel, Styles.boldLabel);
            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUILayout.PropertyField(_onRebindStart);
                EditorGUILayout.PropertyField(_onRebindEnded);
                EditorGUILayout.PropertyField(_onRebindAbortedMessage);
                EditorGUILayout.PropertyField(_onUpdateBindingUIEvent);
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                RefreshBindingOptions();
            }
        }

        protected void RefreshBindingOptions()
        {
            var actionReference = (InputActionReference)_actionToRebind.objectReferenceValue;
            var action = actionReference?.action;

            if (action == null)
            {
                m_BindingOptions = new GUIContent[0];
                m_BindingOptionValues = new string[0];
                m_SelectedBindingOption = -1;
                return;
            }

            var bindings = action.bindings;
            var bindingCount = bindings.Count;

            m_BindingOptions = new GUIContent[bindingCount];
            m_BindingOptionValues = new string[bindingCount];
            m_SelectedBindingOption = -1;

            var currentBindingId = _bindingID.stringValue;
            for (var i = 0; i < bindingCount; ++i)
            {
                var binding = bindings[i];
                var bindingId = binding.id.ToString();
                var haveBindingGroups = !string.IsNullOrEmpty(binding.groups);

                // Create display string.
                var displayString = action.GetBindingDisplayString(i, InputBinding.DisplayStringOptions.IgnoreBindingOverrides);

                // If binding is part of a composite, include the part name.
                if (binding.isPartOfComposite)
                    displayString = $"{ObjectNames.NicifyVariableName(binding.name)}: {displayString}";

                // Some composites use '/' as a separator. When used in popup, this will lead to to submenus. Prevent
                // by instead using a backlash.
                displayString = displayString.Replace('/', '\\');

                // If the binding is part of control schemes, mention them.
                if (haveBindingGroups)
                {
                    var asset = action.actionMap?.asset;
                    if (asset != null)
                    {
                        var controlSchemes = string.Join(", ",
                            binding.groups.Split(InputBinding.Separator)
                                .Select(x => asset.controlSchemes.FirstOrDefault(c => c.bindingGroup == x).name));

                        displayString = $"{displayString} ({controlSchemes})";
                    }
                }

                m_BindingOptions[i] = new GUIContent(displayString);
                m_BindingOptionValues[i] = bindingId;

                if (currentBindingId == bindingId)
                    m_SelectedBindingOption = i;
            }
        }

    }


}
#endif