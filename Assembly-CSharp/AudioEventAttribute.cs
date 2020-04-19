using System;
using UnityEngine;

public class AudioEventAttribute : PropertyAttribute
{
	public readonly bool AudioManagerMasterListOnly;

	public AudioEventAttribute(bool audioMasterListOnly)
	{
		this.AudioManagerMasterListOnly = audioMasterListOnly;
	}
}
