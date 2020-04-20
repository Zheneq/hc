using System;
using UnityEngine;

namespace TMPro.Examples
{
	public class TMP_FrameRateCounter : MonoBehaviour
	{
		public float symbol_001D = 5f;

		private float symbol_000E;

		private int symbol_0012;

		public TMP_FrameRateCounter.FpsCounterAnchorPositions symbol_0015 = TMP_FrameRateCounter.FpsCounterAnchorPositions.symbol_0012;

		private string symbol_0016;

		private const string symbol_0013 = "{0:2}</color> FPS \n{1:2} <#8080ff>MS";

		private TextMeshPro symbol_0018;

		private Transform symbol_0009;

		private Camera symbol_0019;

		private TMP_FrameRateCounter.FpsCounterAnchorPositions symbol_0011;

		private void symbol_001A()
		{
			if (!base.enabled)
			{
				return;
			}
			this.symbol_0019 = Camera.main;
			Application.targetFrameRate = -1;
			GameObject gameObject = new GameObject("Frame Counter");
			this.symbol_0018 = gameObject.AddComponent<TextMeshPro>();
			this.symbol_0018.font = (Resources.Load("Fonts & Materials/LiberationSans SDF", typeof(TMP_FontAsset)) as TMP_FontAsset);
			this.symbol_0018.fontSharedMaterial = (Resources.Load("Fonts & Materials/LiberationSans SDF - Overlay", typeof(Material)) as Material);
			this.symbol_0009 = gameObject.transform;
			this.symbol_0009.SetParent(this.symbol_0019.transform);
			this.symbol_0009.localRotation = Quaternion.identity;
			this.symbol_0018.enableWordWrapping = false;
			this.symbol_0018.fontSize = 24f;
			this.symbol_0018.isOverlay = true;
			this.symbol_001A(this.symbol_0015);
			this.symbol_0011 = this.symbol_0015;
		}

		private void symbol_0004()
		{
			this.symbol_000E = Time.realtimeSinceStartup;
			this.symbol_0012 = 0;
		}

		private void symbol_000B()
		{
			if (this.symbol_0015 != this.symbol_0011)
			{
				this.symbol_001A(this.symbol_0015);
			}
			this.symbol_0011 = this.symbol_0015;
			this.symbol_0012++;
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			if (realtimeSinceStartup > this.symbol_000E + this.symbol_001D)
			{
				float num = (float)this.symbol_0012 / (realtimeSinceStartup - this.symbol_000E);
				float arg = 1000f / Mathf.Max(num, 1E-05f);
				if (num < 30f)
				{
					this.symbol_0016 = "<color=yellow>";
				}
				else if (num < 10f)
				{
					this.symbol_0016 = "<color=red>";
				}
				else
				{
					this.symbol_0016 = "<color=green>";
				}
				this.symbol_0018.SetText(this.symbol_0016 + "{0:2}</color> FPS \n{1:2} <#8080ff>MS", num, arg);
				this.symbol_0012 = 0;
				this.symbol_000E = realtimeSinceStartup;
			}
		}

		private void symbol_001A(TMP_FrameRateCounter.FpsCounterAnchorPositions symbol_001D)
		{
			this.symbol_0018.margin = new Vector4(1f, 1f, 1f, 1f);
			switch (symbol_001D)
			{
			case TMP_FrameRateCounter.FpsCounterAnchorPositions.symbol_001D:
				this.symbol_0018.alignment = TextAlignmentOptions.TopLeft;
				this.symbol_0018.rectTransform.pivot = new Vector2(0f, 1f);
				this.symbol_0009.position = this.symbol_0019.ViewportToWorldPoint(new Vector3(0f, 1f, 100f));
				break;
			case TMP_FrameRateCounter.FpsCounterAnchorPositions.symbol_000E:
				this.symbol_0018.alignment = TextAlignmentOptions.BottomLeft;
				this.symbol_0018.rectTransform.pivot = new Vector2(0f, 0f);
				this.symbol_0009.position = this.symbol_0019.ViewportToWorldPoint(new Vector3(0f, 0f, 100f));
				break;
			case TMP_FrameRateCounter.FpsCounterAnchorPositions.symbol_0012:
				this.symbol_0018.alignment = TextAlignmentOptions.TopRight;
				this.symbol_0018.rectTransform.pivot = new Vector2(1f, 1f);
				this.symbol_0009.position = this.symbol_0019.ViewportToWorldPoint(new Vector3(1f, 1f, 100f));
				break;
			case TMP_FrameRateCounter.FpsCounterAnchorPositions.symbol_0015:
				this.symbol_0018.alignment = TextAlignmentOptions.BottomRight;
				this.symbol_0018.rectTransform.pivot = new Vector2(1f, 0f);
				this.symbol_0009.position = this.symbol_0019.ViewportToWorldPoint(new Vector3(1f, 0f, 100f));
				break;
			}
		}

		public enum FpsCounterAnchorPositions
		{
			symbol_001D,
			symbol_000E,
			symbol_0012,
			symbol_0015
		}
	}
}
