using System;
using UnityEngine;

namespace I2.Loc
{
	public class RealTimeTranslation : MonoBehaviour
	{
		private string OriginalText = "This is an example showing how to use the google translator to translate chat messages within the game.\nIt also supports multiline translations.";

		private string TranslatedText = string.Empty;

		private bool IsTranslating;

		public void OnGUI()
		{
			GUILayout.Label("Translate:", new GUILayoutOption[0]);
			this.OriginalText = GUILayout.TextArea(this.OriginalText, new GUILayoutOption[]
			{
				GUILayout.Width((float)Screen.width)
			});
			GUILayout.Space(10f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (GUILayout.Button("English -> Español", new GUILayoutOption[]
			{
				GUILayout.Height(100f)
			}))
			{
				this.StartTranslating("en", "es");
			}
			if (GUILayout.Button("Español -> English", new GUILayoutOption[]
			{
				GUILayout.Height(100f)
			}))
			{
				this.StartTranslating("es", "en");
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			GUILayout.TextArea(this.TranslatedText, new GUILayoutOption[]
			{
				GUILayout.Width((float)Screen.width)
			});
			GUILayout.Space(10f);
			if (this.IsTranslating)
			{
				GUILayout.Label("Contacting Google....", new GUILayoutOption[0]);
			}
		}

		private void StartTranslating(string fromCode, string toCode)
		{
			this.IsTranslating = true;
			GoogleTranslation.Translate(this.OriginalText, fromCode, toCode, new Action<string>(this.OnTranslationReady));
		}

		private void OnTranslationReady(string Translation)
		{
			this.TranslatedText = Translation;
			this.IsTranslating = false;
		}
	}
}
