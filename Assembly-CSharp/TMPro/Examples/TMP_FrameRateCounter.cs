using System;
using UnityEngine;

namespace TMPro.Examples
{
	public class TMP_FrameRateCounter : MonoBehaviour
	{
		public float \u001D = 5f;

		private float \u000E;

		private int \u0012;

		public TMP_FrameRateCounter.FpsCounterAnchorPositions \u0015 = TMP_FrameRateCounter.FpsCounterAnchorPositions.\u0012;

		private string \u0016;

		private const string \u0013 = "{0:2}</color> FPS \n{1:2} <#8080ff>MS";

		private TextMeshPro \u0018;

		private Transform \u0009;

		private Camera \u0019;

		private TMP_FrameRateCounter.FpsCounterAnchorPositions \u0011;

		private void \u001A()
		{
			if (!base.enabled)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_FrameRateCounter.\u001A()).MethodHandle;
				}
				return;
			}
			this.\u0019 = Camera.main;
			Application.targetFrameRate = -1;
			GameObject gameObject = new GameObject("Frame Counter");
			this.\u0018 = gameObject.AddComponent<TextMeshPro>();
			this.\u0018.font = (Resources.Load("Fonts & Materials/LiberationSans SDF", typeof(TMP_FontAsset)) as TMP_FontAsset);
			this.\u0018.fontSharedMaterial = (Resources.Load("Fonts & Materials/LiberationSans SDF - Overlay", typeof(Material)) as Material);
			this.\u0009 = gameObject.transform;
			this.\u0009.SetParent(this.\u0019.transform);
			this.\u0009.localRotation = Quaternion.identity;
			this.\u0018.enableWordWrapping = false;
			this.\u0018.fontSize = 24f;
			this.\u0018.isOverlay = true;
			this.\u001A(this.\u0015);
			this.\u0011 = this.\u0015;
		}

		private void \u0004()
		{
			this.\u000E = Time.realtimeSinceStartup;
			this.\u0012 = 0;
		}

		private void \u000B()
		{
			if (this.\u0015 != this.\u0011)
			{
				this.\u001A(this.\u0015);
			}
			this.\u0011 = this.\u0015;
			this.\u0012++;
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			if (realtimeSinceStartup > this.\u000E + this.\u001D)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_FrameRateCounter.\u000B()).MethodHandle;
				}
				float num = (float)this.\u0012 / (realtimeSinceStartup - this.\u000E);
				float arg = 1000f / Mathf.Max(num, 1E-05f);
				if (num < 30f)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					this.\u0016 = "<color=yellow>";
				}
				else if (num < 10f)
				{
					this.\u0016 = "<color=red>";
				}
				else
				{
					this.\u0016 = "<color=green>";
				}
				this.\u0018.SetText(this.\u0016 + "{0:2}</color> FPS \n{1:2} <#8080ff>MS", num, arg);
				this.\u0012 = 0;
				this.\u000E = realtimeSinceStartup;
			}
		}

		private void \u001A(TMP_FrameRateCounter.FpsCounterAnchorPositions \u001D)
		{
			this.\u0018.margin = new Vector4(1f, 1f, 1f, 1f);
			switch (\u001D)
			{
			case TMP_FrameRateCounter.FpsCounterAnchorPositions.\u001D:
				this.\u0018.alignment = TextAlignmentOptions.TopLeft;
				this.\u0018.rectTransform.pivot = new Vector2(0f, 1f);
				this.\u0009.position = this.\u0019.ViewportToWorldPoint(new Vector3(0f, 1f, 100f));
				break;
			case TMP_FrameRateCounter.FpsCounterAnchorPositions.\u000E:
				this.\u0018.alignment = TextAlignmentOptions.BottomLeft;
				this.\u0018.rectTransform.pivot = new Vector2(0f, 0f);
				this.\u0009.position = this.\u0019.ViewportToWorldPoint(new Vector3(0f, 0f, 100f));
				break;
			case TMP_FrameRateCounter.FpsCounterAnchorPositions.\u0012:
				this.\u0018.alignment = TextAlignmentOptions.TopRight;
				this.\u0018.rectTransform.pivot = new Vector2(1f, 1f);
				this.\u0009.position = this.\u0019.ViewportToWorldPoint(new Vector3(1f, 1f, 100f));
				break;
			case TMP_FrameRateCounter.FpsCounterAnchorPositions.\u0015:
				this.\u0018.alignment = TextAlignmentOptions.BottomRight;
				this.\u0018.rectTransform.pivot = new Vector2(1f, 0f);
				this.\u0009.position = this.\u0019.ViewportToWorldPoint(new Vector3(1f, 0f, 100f));
				break;
			}
		}

		public enum FpsCounterAnchorPositions
		{
			\u001D,
			\u000E,
			\u0012,
			\u0015
		}
	}
}
