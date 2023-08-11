using GameResources;
using UnityEngine;

namespace Level.Npc.FieldNpcs;

public class PlebbyNpcLineText : MonoBehaviour
{
	[SerializeField]
	private FieldNpc _fieldNpc;

	[SerializeField]
	[Range(0f, 100f)]
	private float _duration;

	[MinMaxSlider(0f, 100f)]
	[SerializeField]
	private Vector2 _coolTimeRange;

	[SerializeField]
	private LineText _ALineText;

	[SerializeField]
	private LineText _BLineText;

	private float _cooltime;

	private float _elapsed;

	private bool _canRun;

	private int _normalLineLenght;

	private string _AcageDestroyedLine => Localization.GetLocalizedString($"npc/{_fieldNpc.type}/A/line/resqued");

	private string _BcageDestroyedLine => Localization.GetLocalizedString($"npc/{_fieldNpc.type}/B/line/resqued");

	private void Start()
	{
		_normalLineLenght = Localization.GetLocalizedStringArray($"npc/{_fieldNpc.type}/A/line").Length;
		ResetTime();
		_fieldNpc.onCageDestroyed += delegate
		{
			ShowLineText(_ALineText, _AcageDestroyedLine);
			ShowLineText(_BLineText, _BcageDestroyedLine);
		};
	}

	private void ShowLineText(LineText lineText, string text)
	{
		lineText.Display(text, _duration);
		ResetTime();
	}

	private void ResetTime()
	{
		_cooltime = Random.Range(_coolTimeRange.x, _coolTimeRange.y);
		_elapsed = 0f;
	}

	private void Update()
	{
		if (_fieldNpc.release)
		{
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
				int num = Random.Range(0, _normalLineLenght);
				ShowLineText(_ALineText, Localization.GetLocalizedString($"npc/{_fieldNpc.type}/A/line/{num}"));
				ShowLineText(_BLineText, Localization.GetLocalizedString($"npc/{_fieldNpc.type}/B/line/{num}"));
			}
		}
	}
}
