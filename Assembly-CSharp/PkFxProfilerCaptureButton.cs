using System.Text;
using UnityEngine;

public class PkFxProfilerCaptureButton : MonoBehaviour
{
	public int FrameCountToCapture = 10;

	private bool _InCapture;

	private int _FrameCaptured;

	private void OnGUI()
	{
		if (_InCapture)
		{
			return;
		}
		while (true)
		{
			if (GUI.Button(new Rect(10f, 10f, 500f, 150f), "Profiler capture"))
			{
				_InCapture = true;
				PKFxManager.ProfilerSetEnable(true);
			}
			return;
		}
	}

	private void OnPostRender()
	{
		if (!_InCapture)
		{
			return;
		}
		while (true)
		{
			_FrameCaptured++;
			if (_FrameCaptured >= FrameCountToCapture)
			{
				while (true)
				{
					_FrameCaptured = 0;
					PKFxManager.WriteProfileReport(new StringBuilder().Append(Application.persistentDataPath).Append("/ProfileReport.pkpr").ToString());
					Debug.Log(new StringBuilder().Append("[PKFX] Profiling report written to ").Append(Application.persistentDataPath).Append("/ProfileReport.pkpr").ToString());
					_InCapture = false;
					PKFxManager.ProfilerSetEnable(false);
					return;
				}
			}
			return;
		}
	}
}
