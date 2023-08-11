using System.Collections.Generic;
using GameResources;
using UnityEngine;

namespace Runnables;

public sealed class ShowLineText : Runnable
{
	[SerializeField]
	private LineText _lineText;

	[SerializeField]
	private Transform _spawnPosition;

	[SerializeField]
	private string _textKey;

	[SerializeField]
	private float _duration = 2f;

	[SerializeField]
	private bool _force;

	public override void Run()
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_lineText == (Object)null)
		{
			_lineText = ((Component)this).GetComponentInChildren<LineText>();
			if ((Object)(object)_lineText == (Object)null)
			{
				return;
			}
		}
		if (_force || _lineText.finished)
		{
			string[] localizedStringArray = Localization.GetLocalizedStringArray(_textKey);
			if (localizedStringArray.Length != 0)
			{
				string text = ExtensionMethods.Random<string>((IEnumerable<string>)localizedStringArray);
				Vector3 position = _spawnPosition.position;
				((Component)_lineText).transform.position = position;
				_lineText.Display(text, _duration);
			}
		}
	}
}
