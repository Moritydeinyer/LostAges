using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputIconLibrary", menuName = "Input/IconLibrary")]
public class InputIconLibrary : ScriptableObject
{
    [System.Serializable]
    public struct IconEntry
    {
        public string bindingPath; // z. B. "<Keyboard>/e", "<Gamepad>/buttonSouth"
        public Sprite icon;
        public bool xBox; // true, wenn das Icon für Xbox-Controller gedacht ist
    }

    public List<IconEntry> icons;

    private Dictionary<(string, bool), Sprite> _iconDict;

    public Sprite GetIconForBinding(string bindingPath, bool isXbox)
    {
        if (_iconDict == null)
        {
            _iconDict = new Dictionary<(string, bool), Sprite>();
            foreach (var entry in icons)
            {
                _iconDict[(entry.bindingPath, entry.xBox)] = entry.icon;
            }
        }

        return _iconDict.TryGetValue((bindingPath, isXbox), out var sprite) ? sprite : null;
    }
}
