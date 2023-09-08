using Characters.Abilities.Constraints;
using GameResources;
using UnityEngine;

namespace Level.Npc;

public class NpcLineText : MonoBehaviour
{
	[SerializeField]
	private string _commonTextKey;

	[Range(0f, 100f)]
	[SerializeField]
	private float _duration;

	[MinMaxSlider(0f, 100f)]
	[SerializeField]
	private Vector2 _preCoolTimeRange;

	[MinMaxSlider(0f, 100f)]
	[SerializeField]
	private Vector2 _coolTimeRange;

	[SerializeField]
	[Constraint.Subcomponent]
	private Constraint.Subcomponents _constraints;

	[SerializeField]
	private LineText _lineText;

	private float _cooltime;

	private float _elapsed;

	private bool _canRun;

	private void Start()
	{
		if ((Object)(object)_lineText == (Object)null)
		{
			_lineText = ((Component)this).GetComponentInChildren<LineText>();
		}
		_cooltime = Random.Range(_preCoolTimeRange.x, _preCoolTimeRange.y);
	}

	public void Run()
	{
		string[] localizedStringArray = Localization.GetLocalizedStringArray(_commonTextKey);
		if (localizedStringArray.Length < 0)
		{
			_elapsed = 0f;
			return;
		}
		string text = localizedStringArray.Random();
		_lineText.Display(text, _duration);
		_cooltime = Random.Range(_coolTimeRange.x, _coolTimeRange.y);
		_elapsed = 0f;
	}

	public void Run(string text)
	{
		_lineText.Display(text, _duration);
		_cooltime = Random.Range(_coolTimeRange.x, _coolTimeRange.y);
		_elapsed = 0f;
	}

	private void Update()
	{
		if (_constraints == null || _constraints.Pass())
		{
			_elapsed += Chronometer.global.deltaTime;
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
				Run();
			}
		}
	}
}
