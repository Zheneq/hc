using System;
using UnityEngine;

public class WorkaroundLensFlareNativePluginBug : MonoBehaviour
{
	private void OnPostRender()
	{
		GL.InvalidateState();
	}
}
