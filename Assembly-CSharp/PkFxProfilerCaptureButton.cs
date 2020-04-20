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
			this._FrameCaptured++;
			if (this._FrameCaptured >= this.FrameCountToCapture)
			{
				this._FrameCaptured = 0;
				PKFxManager.WriteProfileReport(Application.persistentDataPath + "/ProfileReport.pkpr");
				Debug.Log("[PKFX] Profiling report written to " + Application.persistentDataPath + "/ProfileReport.pkpr");
				this._InCapture = false;
				PKFxManager.ProfilerSetEnable(false);
			}
		}
	}
}
