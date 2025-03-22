
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace a1creator
{
    public class ControllerIconRebindKey : MonoBehaviour
    {
        [SerializeField] private GamePadIcons_SO _gamepadIcons;

        [SerializeField] private MouseControllerRebindKey _mouseControllerRebind;

        [SerializeField] private TMP_Text _bindingNameText;
        [SerializeField] private Image _bindingIconImage;

        protected void OnEnable()
        {
            _mouseControllerRebind.OnUpdateBindingUIEvent.AddListener(OnUpdateBindingDisplay);
            _mouseControllerRebind.UpdateBindingDisplay();
        }

        protected void OnUpdateBindingDisplay(string bindingDisplayString, string deviceLayoutName, string controlPath)
        {
            if (string.IsNullOrEmpty(deviceLayoutName) || string.IsNullOrEmpty(controlPath))
                return;

            var icon = default(Sprite);
            if (InputSystem.IsFirstLayoutBasedOnSecond(deviceLayoutName, "DualShockGamepad"))
                icon = _gamepadIcons.ps4.GetSprite(controlPath);
            else if (InputSystem.IsFirstLayoutBasedOnSecond(deviceLayoutName, "Gamepad"))
                icon = _gamepadIcons.xbox.GetSprite(controlPath);

            if (icon != null)
            {
                _bindingNameText.gameObject.SetActive(false);
                _bindingIconImage.sprite = icon;
                _bindingIconImage.gameObject.SetActive(true);
            }
            else
            {
                _bindingNameText.gameObject.SetActive(true);
                _bindingIconImage.gameObject.SetActive(false);
            }
        }

    }
}


