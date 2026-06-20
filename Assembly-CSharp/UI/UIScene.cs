using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIScene : MonoBehaviour, IUIScene
{
	[Serializable]
	public class CloseObjectInfo
	{
		public UIScene m_SceneReference;

		public Animator m_AnimatorToClose;

		public string m_animationToPlay;

		public GameObject[] m_GameObjectsToDisableOnClick;

		public GameObject[] m_GameObjectsToIgnoreCloseCall;

		public bool m_checkParentObjectsOfClickedObject;
	}

	public delegate void RebuildDelegate();

	protected Dictionary<int, RebuildDelegate> RebuildCalls = new Dictionary<int, RebuildDelegate>();

	public virtual SceneStateParameters GetCurrentState()
	{
		return null;
	}

	public virtual void HandleNewSceneStateParameter(SceneStateParameters parameters)
	{
	}

	public virtual bool DoesHandleParameter(SceneStateParameters parameters)
	{
		return false;
	}

	public virtual CloseObjectInfo[] GetMouseClickObjects()
	{
		return null;
	}

	public virtual void Awake()
	{
		RegisterWithUIManager();
	}

	protected void RegisterWithUIManager()
	{
		if (!(UIManager.Get() != null))
		{
			return;
		}
		UIManager.Get().RegisterUIScene(this);
	}

	public virtual Transform[] GetSceneContainers()
	{
		return new Transform[1]
		{
			base.gameObject.transform
		};
	}

	public abstract SceneType GetSceneType();

	public virtual void SetVisible(bool visible, SceneVisibilityParameters parameters)
	{
	}

	public virtual void NotifyGameStateChange(SceneStateParameters newState)
	{
	}
}
