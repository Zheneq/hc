using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatController : MonoBehaviour
{
	public TMP_InputField TMP_ChatInput;

	public TMP_Text TMP_ChatOutput;

	public Scrollbar ChatScrollbar;

	private void OnEnable()
	{
		TMP_ChatInput.onSubmit.AddListener(AddToChatOutput);
	}

	private void OnDisable()
	{
		TMP_ChatInput.onSubmit.RemoveListener(AddToChatOutput);
	}

	private void AddToChatOutput(string newText)
	{
		TMP_ChatInput.text = string.Empty;
		DateTime now = DateTime.Now;
		TMP_Text tMP_ChatOutput = TMP_ChatOutput;
		string text = tMP_ChatOutput.text;
		tMP_ChatOutput.text = new StringBuilder().Append(text).Append("[<#FFFF80>").Append(now.Hour.ToString("d2")).Append(":").Append(now.Minute.ToString("d2")).Append(":").Append(now.Second.ToString("d2")).Append("</color>] ").Append(newText).Append("\n").ToString();
		TMP_ChatInput.ActivateInputField();
		ChatScrollbar.value = 0f;
	}
}
