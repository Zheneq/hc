using System;
using System.Collections.Generic;
using UnityEngine;

public class ActorModelAnimData : MonoBehaviour
{
	[HideInInspector]
	public float[] m_savedCamStartEventDelays = new float[0x15];

	[HideInInspector]
	public float[] m_savedTauntCamStartEventDelays = new float[0x15];

	[HideInInspector]
	public List<int> m_savedAnimatorStateNameHashes;

	[HideInInspector]
	public List<string> m_savedAnimatorStateNames;
}
