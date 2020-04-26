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
			GUILayout.Label("Translate:");
			OriginalText = GUILayout.TextArea(OriginalText, GUILayout.Width(Screen.width));
			GUILayout.Space(10f);
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("English -> Español", GUILayout.Height(100f)))
			{
				StartTranslating("en", "es");
			}
			if (GUILayout.Button("Español -> English", GUILayout.Height(100f)))
			{
				StartTranslating("es", "en");
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			GUILayout.TextArea(TranslatedText, GUILayout.Width(Screen.width));
			GUILayout.Space(10f);
			if (IsTranslating)
			{
				GUILayout.Label("Contacting Google....");
			}
		}

		private void StartTranslating(string fromCode, string toCode)
		{
			IsTranslating = true;
			GoogleTranslation.Translate(OriginalText, fromCode, toCode, OnTranslationReady);
		}

		private void OnTranslationReady(string Translation)
		{
			TranslatedText = Translation;
			IsTranslating = false;
		}
	}
}
