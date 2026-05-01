using System.Collections.Generic;
using System.Linq;
using Evos.ActorStatus;
using UnityEngine;

#if EVOS
public class EvosOptions
{
    public delegate bool StateGetter(SettingsState state);
    public delegate void StateSetter(SettingsState state, bool value);
    public delegate void StateApplier();
    
    public class Option
    {
        public readonly string gameObjectName;
        public readonly string key;
        public readonly bool defaultValue;
        public readonly StateGetter stateGetter;
        public readonly StateSetter stateSetter;
        public readonly StateApplier stateApplier;
        public readonly int position;
        public readonly string termTitle;
        public readonly string termEnable;
        public readonly string termDisable;
        
        public _SelectableBtn m_btnEnable;
        public _SelectableBtn m_btnDisable;

        public Option(
            string gameObjectName,
            string key,
            bool defaultValue,
            StateGetter stateGetter,
            StateSetter stateSetter,
            StateApplier stateApplier,
            string termTitle,
            string termEnable = "On@Global",
            string termDisable = "Off@Global",
            int position = -1)
        {
            this.gameObjectName = gameObjectName;
            this.key = key;
            this.defaultValue = defaultValue;
            this.stateGetter = stateGetter;
            this.stateSetter = stateSetter;
            this.stateApplier = stateApplier;
            this.position = position;
            this.termTitle = termTitle;
            this.termEnable = termEnable;
            this.termDisable = termDisable;
        }

        public void AssignButtons(_SelectableBtn btnEnable, _SelectableBtn btnDisable)
        {
            m_btnEnable = btnEnable;
            m_btnDisable = btnDisable;
        }
        
        public void UpdateButtons(bool isEnabled)
        {
            m_btnEnable?.SetSelected(isEnabled);
            m_btnDisable?.SetSelected(!isEnabled);
        }
    }

    public const string AllowResettingWaypoints = "OptionsAllowResettingWaypoints";
    public const string ExtendedCooldownView = "OptionsExtendedCooldownView";
    public const string EnableGamepadControls = "EnableGamepadControls";
    public const string EnableUniqueStatusEffectIcons = "EnableUniqueStatusEffectIcons";

    public readonly List<Option> m_options = new List<Option>
    {
        new Option(
            "allowResettingWaypoints",
            AllowResettingWaypoints,
            true,
            pendingState => pendingState.allowResettingWaypoints,
            (pendingState, value) => pendingState.allowResettingWaypoints = value,
            () => { },
            "AllowResettingWaypoints@EvosOptions",
            "AllowResettingWaypointsYes@EvosOptions",
            "AllowResettingWaypointsNo@EvosOptions",
            11),
        new Option(
            "extendedCooldownView",
            ExtendedCooldownView,
            false,
            pendingState => pendingState.extendedCooldownView,
            (pendingState, value) => pendingState.extendedCooldownView = value,
            () => UIMainScreenPanel.Get()?.m_playerDisplayPanel?.UpdateExtendedCooldownView(),
            "ExtendedCooldownView@EvosOptions",
            position: 13),
        new Option(
            "enableUniqueStatusEffectIcons",
            EnableUniqueStatusEffectIcons,
            true,
            pendingState => pendingState.enableUniqueStatusEffectIcons,
            (pendingState, value) => pendingState.enableUniqueStatusEffectIcons = value,
            () => EvosActorStatusManager.Get()?.OnToggle(),
            "EnableUniqueStatusEffectIcons@EvosOptions",
            position: 14),
        new Option(
            "enableGamepadControls",
            EnableGamepadControls,
            false,
            pendingState => pendingState.enableGamepadControls,
            (pendingState, value) => pendingState.enableGamepadControls = value,
            () => { },
            "EnableGamepadControls@EvosOptions",
            position: 15)
    };
    private readonly Dictionary<string, Option> m_optionDict;
    
    private static EvosOptions _instance;

    public static EvosOptions Get()
    {
        return _instance ?? (_instance = new EvosOptions());
    }

    public EvosOptions()
    {
        m_optionDict = m_options.ToDictionary(o => o.key);
    }

    public bool GetOption(string key)
    {
        if (m_optionDict.TryGetValue(key, out Option value))
        {
            Options_UI optionsUI = Options_UI.Get();
            return optionsUI != null ? optionsUI.GetOption(value.stateGetter) : value.defaultValue;
        }
        
        Log.Error($"Custom option {key} does not exist!");
        return false;
    }

    public void InitDefaults(SettingsState state)
    {
        foreach (Option option in m_options)
        {
            option.stateSetter(state, option.defaultValue);
        }
    }

    public void LoadFromPrefs(SettingsState state)
    {
        foreach (Option option in m_options)
        {
            option.stateSetter(state, PlayerPrefs.GetInt(option.key, option.defaultValue ? 1 : 0) != 0);
        }
    }

    public void SaveToPrefs(SettingsState state)
    {
        foreach (Option option in m_options)
        {
            PlayerPrefs.SetInt(option.key, option.stateGetter(state) ? 1 : 0);
        }
    }

    public void UpdateButtons(SettingsState state)
    {
        foreach (Option option in m_options)
        {
            option.UpdateButtons(option.stateGetter(state));
        }
    }
}
#endif