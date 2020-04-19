using System;
using UnityEngine;

public class _AnimEventPlaySound : MonoBehaviour
{
	public void PlayUIFrontEndSound(FrontEndButtonSounds soundType)
	{
		UIFrontEnd.PlaySound(soundType);
	}
}
