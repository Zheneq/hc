using System;
using System.Collections.Generic;
using UnityEngine;

public class DisableGameObjectWithAnimOutInfo : MonoBehaviour
{
	[Serializable]
	public class SetGameObjectEnableInfo
	{
		public string m_AnimationNameToPlay;

		public string m_AnimationNameForDoneCallback;

		public int m_AnimLayer;

		public float m_AnimStartTimeNormalized;
	}

	public Animator m_animator;

	public SetGameObjectEnableInfo[] m_EnableGameObjectInfo;

	public SetGameObjectEnableInfo[] m_DisableGameObjectInfo;

	public static HashSet<int> s_attachedObjectInstanceIds = new HashSet<int>();

	private void Awake()
	{
		s_attachedObjectInstanceIds.Add(base.gameObject.GetInstanceID());
	}

	private void OnDestroy()
	{
		s_attachedObjectInstanceIds.Remove(base.gameObject.GetInstanceID());
	}
}
