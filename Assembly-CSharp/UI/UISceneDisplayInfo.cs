using System;

[Serializable]
public class UISceneDisplayInfo
{
	public string SceneName;

	public string UnitySceneLoadName;

	public SceneType m_SceneType;

	public bool m_InFrontEnd;

	public bool m_InGame;

	public int SceneLoadPriority = 100;
}
