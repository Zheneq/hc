using System;
using UnityEngine;

namespace TMPro.Examples
{
	public class TMP_UiFrameRateCounter : MonoBehaviour
	{
		public float \u001D = 5f;

		private float \u000E;

		private int \u0012;

		public TMP_UiFrameRateCounter.FpsCounterAnchorPositions \u0015 = TMP_UiFrameRateCounter.FpsCounterAnchorPositions.\u0012;

		private string \u0016;

		private const string \u0013 = "{0:2}</color> FPS \n{1:2} <#8080ff>MS";

		private TextMeshProUGUI \u0018;

		private RectTransform \u0009;

		private TMP_UiFrameRateCounter.FpsCounterAnchorPositions \u0019;

		private void \u0011()
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_UiFrameRateCounter.\u0011()).MethodHandle;
				}
				return;
			}
			Application.targetFrameRate = 0x78;
			GameObject gameObject = new GameObject("Frame Counter");
			this.\u0009 = gameObject.AddComponent<RectTransform>();
			this.\u0009.SetParent(base.transform, false);
			this.\u0018 = gameObject.AddComponent<TextMeshProUGUI>();
			this.\u0018.font = (Resources.Load("Fonts & Materials/LiberationSans SDF", typeof(TMP_FontAsset)) as TMP_FontAsset);
			this.\u0018.fontSharedMaterial = (Resources.Load("Fonts & Materials/LiberationSans SDF - Overlay", typeof(Material)) as Material);
			this.\u0018.enableWordWrapping = false;
			this.\u0018.fontSize = 36f;
			this.\u0018.isOverlay = true;
			this.\u0011(this.\u0015);
			this.\u0019 = this.\u0015;
		}

		private void \u001A()
		{
			this.\u000E = Time.realtimeSinceStartup;
			this.\u0012 = 0;
		}

		private void \u0004()
		{
			if (this.\u0015 != this.\u0019)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_UiFrameRateCounter.\u0004()).MethodHandle;
				}
				this.\u0011(this.\u0015);
			}
			this.\u0019 = this.\u0015;
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
				float num = (float)this.\u0012 / (realtimeSinceStartup - this.\u000E);
				float arg = 1000f / Mathf.Max(num, 1E-05f);
				if (num < 30f)
				{
					this.\u0016 = "<color=yellow>";
				}
				else if (num < 10f)
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

		private void \u0011(TMP_UiFrameRateCounter.FpsCounterAnchorPositions \u001D)
		{
			switch (\u001D)
			{
			case TMP_UiFrameRateCounter.FpsCounterAnchorPositions.\u001D:
				this.\u0018.alignment = TextAlignmentOptions.TopLeft;
				this.\u0009.pivot = new Vector2(0f, 1f);
				this.\u0009.anchorMin = new Vector2(0.01f, 0.99f);
				this.\u0009.anchorMax = new Vector2(0.01f, 0.99f);
				this.\u0009.anchoredPosition = new Vector2(0f, 1f);
				break;
			case TMP_UiFrameRateCounter.FpsCounterAnchorPositions.\u000E:
				this.\u0018.alignment = TextAlignmentOptions.BottomLeft;
				this.\u0009.pivot = new Vector2(0f, 0f);
				this.\u0009.anchorMin = new Vector2(0.01f, 0.01f);
				this.\u0009.anchorMax = new Vector2(0.01f, 0.01f);
				this.\u0009.anchoredPosition = new Vector2(0f, 0f);
				break;
			case TMP_UiFrameRateCounter.FpsCounterAnchorPositions.\u0012:
				this.\u0018.alignment = TextAlignmentOptions.TopRight;
				this.\u0009.pivot = new Vector2(1f, 1f);
				this.\u0009.anchorMin = new Vector2(0.99f, 0.99f);
				this.\u0009.anchorMax = new Vector2(0.99f, 0.99f);
				this.\u0009.anchoredPosition = new Vector2(1f, 1f);
				break;
			case TMP_UiFrameRateCounter.FpsCounterAnchorPositions.\u0015:
				this.\u0018.alignment = TextAlignmentOptions.BottomRight;
				this.\u0009.pivot = new Vector2(1f, 0f);
				this.\u0009.anchorMin = new Vector2(0.99f, 0.01f);
				this.\u0009.anchorMax = new Vector2(0.99f, 0.01f);
				this.\u0009.anchoredPosition = new Vector2(1f, 0f);
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
