using System.Text;
using TMPro;
using UnityEngine;

public class DebugCanvas : MonoBehaviour
{
	[SerializeField]
	private TMP_Text _text;

	private StringBuilder _stringBuilder;

	private void Awake()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		Object.DontDestroyOnLoad((Object)(object)((Component)this).gameObject);
		_stringBuilder = new StringBuilder();
		Application.logMessageReceived += new LogCallback(Application_logMessageReceived);
	}

	private void OnDestroy()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		Application.logMessageReceived -= new LogCallback(Application_logMessageReceived);
	}

	private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		if ((int)type == 4 || (int)type == 0)
		{
			_stringBuilder.AppendLine(condition);
			_stringBuilder.AppendLine(stackTrace);
			_text.text = _stringBuilder.ToString();
		}
	}
}
