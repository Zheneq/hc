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
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			_FrameCaptured++;
			if (_FrameCaptured >= FrameCountToCapture)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					_FrameCaptured = 0;
					PKFxManager.WriteProfileReport(Application.persistentDataPath + "/ProfileReport.pkpr");
					Debug.Log("[PKFX] Profiling report written to " + Application.persistentDataPath + "/ProfileReport.pkpr");
					_InCapture = false;
					PKFxManager.ProfilerSetEnable(false);
					return;
				}
			}
			return;
		}
	}
}
