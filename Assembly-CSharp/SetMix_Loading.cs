using System;
using UnityEngine;

public class SetMix_Loading : MonoBehaviour
{
	private void Start()
	{
	}

	private void Destroy()
	{
		AudioManager.GetMixerSnapshotManager().SetMix_LoadingScreen();
	}
}
