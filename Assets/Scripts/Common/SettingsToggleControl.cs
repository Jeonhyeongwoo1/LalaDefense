using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsToggleControl : ToggleGroup
{
    public enum Type { Quality, Resoultion }
    public Type m_Type;
    public List<Toggle> GetToogles() { return m_Toggles; }

}
