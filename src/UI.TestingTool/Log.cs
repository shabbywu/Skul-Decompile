using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.TestingTool;

public class Log : MonoBehaviour
{
	[SerializeField]
	private TMP_Text _text;

	[SerializeField]
	private Button _copy;

	[SerializeField]
	private Button _clear;

	private readonly StringBuilder _sb = new StringBuilder();

	public void StartLog()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		Application.logMessageReceived += new LogCallback(ApplicationLogMessageReceived);
	}

	private void Awake()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		((UnityEvent)_copy.onClick).AddListener(new UnityAction(Copy));
		((UnityEvent)_clear.onClick).AddListener(new UnityAction(Clear));
	}

	private void OnEnable()
	{
		_text.text = _sb.ToString();
	}

	private void Copy()
	{
		GUIUtility.systemCopyBuffer = _text.text;
	}

	private void Clear()
	{
		_sb.Clear();
		_text.text = string.Empty;
	}

	private void ApplicationLogMessageReceived(string condition, string stackTrace, LogType type)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Invalid comparison between Unknown and I4
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if ((int)type != 3 && (int)type != 2 && _sb.Length <= 10000)
		{
			_sb.AppendFormat("[{0}] {1}\n{2}\n\n", type, condition, stackTrace);
		}
	}
}
