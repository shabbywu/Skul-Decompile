using BehaviorDesigner.Runtime;
using Services;
using Singletons;
using UnityEngine;

namespace Characters;

public class BDEnemyCharacterSpecificator : MonoBehaviour
{
	[SerializeField]
	private Character _character;

	[SerializeField]
	private Vector2 _movementSpeedRange = new Vector2(-0.2f, 0.2f);

	[SerializeField]
	private float _speedBonusAtChaseTarget = 0.3f;

	[SerializeField]
	private string _targetName = "Target";

	private BehaviorDesignerCommunicator _bdCommunicator;

	private Stat.Values _statValue;

	private bool _movementSpeedAttached;

	private void Awake()
	{
		_bdCommunicator = ((Component)_character).GetComponentInChildren<BehaviorDesignerCommunicator>();
		_statValue = new Stat.Values(new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.MovementSpeed, 0.0), new Stat.Value(Stat.Category.Constant, Stat.Kind.MovementSpeed, Random.Range(_movementSpeedRange.x, _movementSpeedRange.y)), new Stat.Value(Stat.Category.Percent, Stat.Kind.Health, Singleton<Service>.Instance.levelManager.currentChapter.currentStage.healthMultiplier));
		_character.stat.AttachValues(_statValue);
	}

	private void Update()
	{
		if ((Object)(object)_bdCommunicator == (Object)null)
		{
			return;
		}
		if ((Object)(object)((SharedVariable<Character>)_bdCommunicator.GetVariable<SharedCharacter>(_targetName)).Value == (Object)null)
		{
			if (_movementSpeedAttached)
			{
				((ReorderableArray<Stat.Value>)_statValue).values[0].value = 0.0;
				_movementSpeedAttached = false;
				_character.stat.SetNeedUpdate();
			}
		}
		else if (!_movementSpeedAttached)
		{
			((ReorderableArray<Stat.Value>)_statValue).values[0].value = _speedBonusAtChaseTarget;
			_movementSpeedAttached = true;
			_character.stat.SetNeedUpdate();
		}
	}
}
