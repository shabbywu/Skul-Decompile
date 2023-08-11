using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("Character의 Chronometer.Time을 가져옵니다.")]
public sealed class GetCharacterChronometer : Action
{
	[SerializeField]
	private SharedCharacter _character;

	[SerializeField]
	private SharedFloat storeResult;

	private Character _characterValue;

	private ChronometerTime _time;

	public override void OnAwake()
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		_characterValue = ((SharedVariable<Character>)_character).Value;
		if (((Component)_characterValue).gameObject.activeInHierarchy)
		{
			_time = new ChronometerTime(_characterValue.chronometer.master, (MonoBehaviour)(object)_characterValue);
		}
	}

	public override TaskStatus OnUpdate()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		if (!((Object)(object)_characterValue == (Object)null))
		{
			if (_time == null)
			{
				_time = new ChronometerTime(_characterValue.chronometer.master, (MonoBehaviour)(object)_characterValue);
			}
			((SharedVariable)storeResult).SetValue((object)_time.time);
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}
}
