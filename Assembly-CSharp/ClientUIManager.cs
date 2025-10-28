using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if EVOS
public class ClientUIManager : MonoBehaviour
{
    private static ClientUIManager s_instance;
    
    public static ClientUIManager Get()
    {
        return s_instance;
    }

    private void Awake()
    {
        s_instance = this;
    }

    private void OnDestroy()
    {
        s_instance = null;
    }

    public void OnDisable()
    {
        Log.Info("ClientUIManager disabled");
    }

    public void OnEnable()
    {
        Log.Info("ClientUIManager enabled");
    }

    public void UpdateExtendedCooldownView(List<UIPlayerStatus> icons)
    {
        StartCoroutine(UpdateExtendedCooldownViewAsync(icons));
    }
	
    private IEnumerator UpdateExtendedCooldownViewAsync(List<UIPlayerStatus> icons)
    {
        yield return null;
        for (int i = 0; i < icons.Count; i++)
        {
            UIPlayerStatus icon = icons[i];
            Log.Info($"UPDATING PLAYERSTATUSICON {i}");
            try
            {
                icon.UpdateExtendedCooldownView();
            }
            catch (Exception e)
            {
                Log.Info($"PLAYERSTATUSICON {i} EXCEPTION: {e}");
            }
            Log.Info($"PLAYERSTATUSICON {i} DONE");
        }
		
        HUD_UI.Get().m_mainScreenPanel.m_alertDisplay.UpdateExtendedCooldownView();
        HUD_UI.Get().m_mainScreenPanel.m_notificationPanel.UpdateExtendedCooldownView();
		
        // Moving UI elements can introduce a hitch which can cause Unity to skip animation events
        UIMainScreenPanel.Get()?.m_nameplatePanel?.RefreshNameplates();
    }
}
#endif