using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimEventToAttachedVfxData
{
	public string m_name;

	[AnimEventPicker]
	public UnityEngine.Object m_persistentVfxStartEvent;

	[AnimEventPicker]
	public UnityEngine.Object m_persistentVfxStopEvent;

	public List<AdditionalAttachedActorVfx.JointToVfx> m_persistentVfxList;
}
