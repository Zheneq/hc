using System;
using UnityEngine;

public class PkFxProfilerCaptureButton : MonoBehaviour
{
	public int FrameCountToCapture = 0xA;

	private bool _InCapture;

	private int _FrameCaptured;

	private void OnGUI()
	{
		if (!this._InCapture)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(PkFxProfilerCaptureButton.OnGUI()).MethodHandle;
			}
			if (GUI.Button(new Rect(10f, 10f, 500f, 150f), "Profiler capture"))
			{
				this._InCapture = true;
				PKFxManager.ProfilerSetEnable(true);
			}
		}
	}

	private void OnPostRender()
	{
		if (this._InCapture)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(PkFxProfilerCaptureButton.OnPostRender()).MethodHandle;
			}
			this._FrameCaptured++;
			if (this._FrameCaptured >= this.FrameCountToCapture)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				this._FrameCaptured = 0;
				PKFxManager.WriteProfileReport(Application.persistentDataPath + "/ProfileReport.pkpr");
				Debug.Log("[PKFX] Profiling report written to " + Application.persistentDataPath + "/ProfileReport.pkpr");
				this._InCapture = false;
				PKFxManager.ProfilerSetEnable(false);
			}
		}
	}
}
