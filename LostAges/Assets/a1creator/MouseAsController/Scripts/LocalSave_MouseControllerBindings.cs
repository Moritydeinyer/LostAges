
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace a1creator
{


    // This class is meant for you to be able to edit yourself to fit into your own system.
    // You can also use as is, playerprefs is a fine solution for keybinds.
    public class LocalSave_MouseControllerBindings : MonoBehaviour
    {

        [SerializeField] private InputActionAsset _actions;
        [Space(10)]
        [SerializeField] Slider mouseSpeedMultiplier_Slider;

        private Coroutine saveAllRoutine = null;

        private void OnEnable()
        {
            // Load saved custom keybind
            LoadAndSetKeyOverride();
            mouseSpeedMultiplier_Slider.value = GetSavedControllerMouseSpeedMultiplier();
        }

        public void SetControllerMouseSpeedMultilpier(float multiplier)
        {
            SaveControllerMouseSpeedMultiplier(multiplier);
            LoadAndSetControllerMouseSpeedMultiplier();
        }
        public void LoadAndSetControllerMouseSpeedMultiplier()
        {
            ControllerMouse.ControllerMouseSpeedMultiplier = GetSavedControllerMouseSpeedMultiplier();
        }
        private void SaveControllerMouseSpeedMultiplier(float multiplier)
        {//                                                                                 min   max
            PlayerPrefs.SetFloat("CONTROLLERMOUSE_SPEEDMULTIPLIER", Mathf.Clamp(multiplier, 0.1f, 3f));
        }
        public float GetSavedControllerMouseSpeedMultiplier()
        {
            return Mathf.Clamp(PlayerPrefs.GetFloat("CONTROLLERMOUSE_SPEEDMULTIPLIER", 1f), 0.1f, 3f);
        }




        public void LoadAndSetKeyOverride()
        {
            string keybindsAsJson = GetAllBinds();
            if (keybindsAsJson != null)
                _actions.LoadBindingOverridesFromJson(keybindsAsJson);
        }
        public void QueueSaveAll()
        {
            if (saveAllRoutine != null)
                StopCoroutine(saveAllRoutine);

            saveAllRoutine = StartCoroutine(WaitThenSaveAll());
        }
        // I don't like waiting until the game closes to save the keybinds, and this prevents spamming
        private IEnumerator WaitThenSaveAll()
        {
            yield return new WaitForSecondsRealtime(2f);
            //Debug.LogWarning("Waited, then saved all bindings to playerprefs");
            //var rebinds = _actions.ToJson();
            //PlayerPrefs.SetString("MOUSECONTROLLER_KEYBINDS", rebinds);
            PlayerPrefs.SetString("CONTROLLERMOUSE_ALL_KEYBINDS", _actions.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();
        }
        public string GetAllBinds()
        {
            return PlayerPrefs.GetString("CONTROLLERMOUSE_ALL_KEYBINDS");
        }

    }

}