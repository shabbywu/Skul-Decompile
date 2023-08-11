using System.Collections.Generic;
using GameResources;
using UnityEngine;

namespace Level.Npc.FieldNpcs;

public class FieldNpcLineText : MonoBehaviour
{
	[SerializeField]
	private FieldNpc _fieldNpc;

	[SerializeField]
	[Range(0f, 100f)]
	private float _duration;

	[SerializeField]
	[MinMaxSlider(0f, 100f)]
	private Vector2 _coolTimeRange;

	[SerializeField]
	private LineText _lineText;

	private float _cooltime;

	private float _elapsed;

	private bool _canRun;

	[SerializeField]
	private string _overrideCageDestroyedLineKey;

	[SerializeField]
	private string _overrideNormalLineKey;

	private void Start()
	{
		if ((Object)(object)_lineText == (Object)null)
		{
			_lineText = ((Component)this).GetComponentInChildren<LineText>();
		}
		ResetTime();
		_fieldNpc.onCageDestroyed += delegate
		{
			if (string.IsNullOrEmpty(_overrideCageDestroyedLineKey))
			{
				ShowLineText(_fieldNpc.cageDestroyedLine);
			}
			else
			{
				ShowLineText(Localization.GetLocalizedString(_overrideCageDestroyedLineKey));
			}
		};
	}

	private void ShowLineText(string text)
	{
		_lineText.Display(text, _duration);
		ResetTime();
	}

	private void ResetTime()
	{
		_cooltime = Random.Range(_coolTimeRange.x, _coolTimeRange.y);
		_elapsed = 0f;
	}

	private void Update()
	{
		if (!_fieldNpc.release)
		{
			return;
		}
		_elapsed += ((ChronometerBase)Chronometer.global).deltaTime;
		if (_elapsed > _cooltime)
		{
			_canRun = true;
		}
		else
		{
			_canRun = false;
		}
		if (_canRun)
		{
			if (string.IsNullOrEmpty(_overrideNormalLineKey))
			{
				ShowLineText(_fieldNpc.normalLine);
			}
			else
			{
				ShowLineText(ExtensionMethods.Random<string>((IEnumerable<string>)Localization.GetLocalizedStringArray(_overrideNormalLineKey)));
			}
		}
	}
}
