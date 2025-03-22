using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Events;
using System.Collections;

namespace a1creator
{

    /// <summary>
    /// This script is a heavily modified version of the script that Unity provides with their InputActionRebinding sample
    /// </summary>
    public class MouseControllerRebindKey : MonoBehaviour
    {
        [SerializeField] private LocalSave_MouseControllerBindings _localSave;

        [SerializeField] private InputActionReference _actionToRebind;
        public InputActionReference ActionToRebind
        {
            get => _actionToRebind;
            set
            {
                _actionToRebind = value;
                UpdateBindingDisplay();
            }
        }

        [SerializeField] private string _bindingID;
        public string BindingID
        {
            get => _bindingID;
            set
            {
                _bindingID = value;
                UpdateBindingDisplay();
            }
        }

        [SerializeField] private TMP_Text _bindingNameText;
        public TMP_Text BindingNameText { get => _bindingNameText; }

        [SerializeField] private GameObject _rebindOverlay;
        [SerializeField] private TMP_Text _rebindOverlayMessage;

        [SerializeField] private UpdateBindingUIEvent _onUpdateBindingUIEvent;
        public UpdateBindingUIEvent OnUpdateBindingUIEvent { get => _onUpdateBindingUIEvent; }

        [SerializeField] private InteractiveRebindEvent _onRebindStart;
        [SerializeField] private InteractiveRebindEvent _onRebindEnded;
        [SerializeField] private UnityEvent<string> _onRebindAbortedMessage;

        private InputActionRebindingExtensions.RebindingOperation _rebindOperation;
        private static List<MouseControllerRebindKey> _rebindActionUIs;

        [Serializable]
        public class UpdateBindingUIEvent : UnityEvent<string, string, string> { }
        [Serializable]
        public class InteractiveRebindEvent : UnityEvent<MouseControllerRebindKey, InputActionRebindingExtensions.RebindingOperation> { }


    #if UNITY_EDITOR
        // Refresh button name outside of play mode
        protected void OnValidate() { UpdateBindingDisplay(); }
    #endif

        public void UpdateBindingDisplay()
        {
            var displayString = string.Empty;
            var deviceLayoutName = default(string);
            var controlPath = default(string);

            // Get display string from action.
            var action = _actionToRebind?.action;
            if (action != null)
            {
                var bindingIndex = action.bindings.IndexOf(x => x.id.ToString() == _bindingID);
                if (bindingIndex != -1)
                    displayString = action.GetBindingDisplayString(bindingIndex, out deviceLayoutName, out controlPath, /*displayStringOptions*/InputBinding.DisplayStringOptions.DontUseShortDisplayNames);
            }

            // Set on label
            if (_bindingNameText != null)
                _bindingNameText.text = displayString;

            // Give listeners a chance to configure UI in response.
            _onUpdateBindingUIEvent?.Invoke(displayString, deviceLayoutName, controlPath);
        }


        /// <summary>
        /// Return the action and binding index for the binding that is targeted by the component
        /// according to
        /// </summary>
        public bool ResolveActionAndBinding(out InputAction action, out int bindingIndex)
        {
            bindingIndex = -1;

            action = _actionToRebind?.action;
            if (action == null)
                return false;

            if (string.IsNullOrEmpty(_bindingID))
                return false;

            // Look up binding index.
            var bindingId = new Guid(_bindingID);
            bindingIndex = action.bindings.IndexOf(x => x.id == bindingId);
            if (bindingIndex == -1)
            {
                Debug.LogError($"Cannot find binding with ID '{bindingId}' on '{action}'", this);
                return false;
            }

            return true;
        }

        public void ResetToDefault()
        {
            if (!ResolveActionAndBinding(out var action, out var bindingIndex))
                return;

            if (action.bindings[bindingIndex].isComposite)
            {
                // It's a composite. Remove overrides from part bindings.
                for (var i = bindingIndex + 1; i < action.bindings.Count && action.bindings[i].isPartOfComposite; ++i)
                    action.RemoveBindingOverride(i);
            }
            else
            {
                action.RemoveBindingOverride(bindingIndex);
            }
            UpdateBindingDisplay();
        }

        public void StartInteractiveRebind()
        {
            if (!ResolveActionAndBinding(out var action, out var bindingIndex))
                return;
            // Disable cursor simulation when waiting on keybind
            EnableCursorSimulation(false, action);

            // If the binding is a composite, we need to rebind each part in turn.
            if (action.bindings[bindingIndex].isComposite)
            {
                var firstPartIndex = bindingIndex + 1;
                if (firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isPartOfComposite)
                    PerformInteractiveRebind(action, firstPartIndex, true);
            }
            else
            {
                PerformInteractiveRebind(action, bindingIndex);
            }
        }

        private void EnableCursorSimulation(bool doEnable, InputAction action)
        {
            // Don't let us use controller movement while rebinding
            ControllerMouse.IsRebinding = !doEnable;
            // Disable the action when we begin because it's not possible to edit an active action
            if (doEnable)
                // We skip a few milliseconds because sometimes the keyUp registers when rebinding, which causes a new rebind to start
                StartCoroutine(WaitThenEnableCursorSim(action));
            else
                action.Disable();
        }

        private IEnumerator WaitThenEnableCursorSim(InputAction action)
        {
            yield return new WaitForSecondsRealtime(0.2f);
            action.Enable();
        }

        private void PerformInteractiveRebind(InputAction action, int bindingIndex, bool allCompositeParts = false)
        {
            // Set the previous operation to null
            _rebindOperation?.Cancel();

            // checking for the same keypress twice in a composite
            string lastKeysName = "";
            if (allCompositeParts)
                lastKeysName = action.bindings[bindingIndex - 1].name;
        

            void CleanUp()
            {
                _rebindOperation?.Dispose();
                _rebindOperation = null;
            }

            _rebindOperation = action.PerformInteractiveRebinding(bindingIndex)
                                      .WithControlsExcluding("<Pointer>/delta")
                                      .WithControlsExcluding("<Pointer>/position")
                                      .WithControlsExcluding("<Touchscreen>/touch*/position")
                                      .WithControlsExcluding("<Touchscreen>/touch*/delta")
                                      .WithControlsExcluding("<Mouse>/clickCount")
                                      .OnPotentialMatch((rebindOperation) =>
                                      {
                                          if (rebindOperation == null || !allCompositeParts) return;
                                          foreach (var canditate in rebindOperation.candidates)
                                          {
                                              // If we are making a composite and use the same key twice
                                              if (canditate.name.Equals(lastKeysName))
                                              {
                                                  _onRebindAbortedMessage.Invoke("Tried to use the same key twice in a composite.");
                                                  rebindOperation.Cancel();
                                              }
                                          }
                                      });
                                      //.WithMatchingEventsBeingSuppressed();

            if (allCompositeParts)
                _rebindOperation = _rebindOperation.OnMatchWaitForAnother(0.35f);

            _rebindOperation = _rebindOperation.OnCancel(operation =>
                    {
                        _onRebindEnded?.Invoke(this, operation);
                        _rebindOverlay?.SetActive(false);
                        UpdateBindingDisplay();
                        EnableCursorSimulation(true, action);
                        CleanUp();
                    });

            _rebindOperation = _rebindOperation.OnComplete(
                    operation =>
                    {
                        _onRebindEnded?.Invoke(this, operation);
                        _rebindOverlay?.SetActive(false);
                        UpdateBindingDisplay();
                        _localSave.QueueSaveAll();
                        CleanUp();

                        // If there's more composite parts we should bind, initiate a rebind
                        // for the next part.
                        if (allCompositeParts)
                        {
                            var nextBindingIndex = bindingIndex + 1;
                            if (nextBindingIndex < action.bindings.Count && action.bindings[nextBindingIndex].isPartOfComposite)
                                PerformInteractiveRebind(action, nextBindingIndex, true);
                            else // once we have gone through all keys in the composite
                            {
                                EnableCursorSimulation(true, action);

                            }
                        }
                        else // if we are not a composite
                        {
                            EnableCursorSimulation(true, action);
                        }
                    
                    });

            // If it's a part binding, show the name of the part in the UI.
            var partName = default(string);
            if (action.bindings[bindingIndex].isPartOfComposite)
                partName = $"Binding '{action.bindings[bindingIndex].name}'. ";

            // Bring up rebind overlay, if we have one.
            _rebindOverlay?.SetActive(true);
            if (_rebindOverlayMessage != null)
                _rebindOverlayMessage.text = $"{partName}Waiting for input...";
            _bindingNameText.text = "< Waiting... >";

            // Give listeners a chance to act on the rebind starting.
            _onRebindStart?.Invoke(this, _rebindOperation);

            _rebindOperation.Start();
        }

        protected void OnEnable()
        {
            if (_rebindActionUIs == null)
                _rebindActionUIs = new List<MouseControllerRebindKey>();
            _rebindActionUIs.Add(this);
            if (_rebindActionUIs.Count == 1)
                InputSystem.onActionChange += OnActionChange;
        }

        protected void OnDisable()
        {
            _rebindOperation?.Dispose();
            _rebindOperation = null;

            _rebindActionUIs.Remove(this);
            if (_rebindActionUIs.Count == 0)
            {
                _rebindActionUIs = null;
                InputSystem.onActionChange -= OnActionChange;
            }
        }

        // When the action system re-resolves bindings, we want to update our UI in response. While this will
        // also trigger from changes we made ourselves, it ensures that we react to changes made elsewhere. If
        // the user changes keyboard layout, for example, we will get a BoundControlsChanged notification and
        // will update our UI to reflect the current keyboard layout.
        private static void OnActionChange(object obj, InputActionChange change)
        {
            if (change != InputActionChange.BoundControlsChanged)
                return;

            var action = obj as InputAction;
            var actionMap = action?.actionMap ?? obj as InputActionMap;
            var actionAsset = actionMap?.asset ?? obj as InputActionAsset;

            for (var i = 0; i < _rebindActionUIs.Count; ++i)
            {
                var component = _rebindActionUIs[i];
                var referencedAction = component.ActionToRebind?.action;
                if (referencedAction == null)
                    continue;

                if (referencedAction == action ||
                    referencedAction.actionMap == actionMap ||
                    referencedAction.actionMap?.asset == actionAsset)
                    component.UpdateBindingDisplay();
            }
        }


    }

}