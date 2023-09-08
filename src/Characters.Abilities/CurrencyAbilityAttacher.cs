using Data;
using UnityEngine;

namespace Characters.Abilities;

public sealed class CurrencyAbilityAttacher : AbilityAttacher
{
	private enum Comparer
	{
		Equal,
		Greater,
		GreaterThanOrEqual,
		LessThan,
		LessThanOrEqual
	}

	[SerializeField]
	private Comparer _comparer;

	[SerializeField]
	private GameData.Currency.Type _type;

	[SerializeField]
	private int _count;

	[SerializeField]
	private float _checkInterval = 1f;

	[AbilityComponent.Subcomponent]
	[SerializeField]
	private AbilityComponent _abilityComponent;

	private bool _startAttachCheck;

	private bool _attached;

	private float _elapsed;

	public override void OnIntialize()
	{
		_abilityComponent.Initialize();
	}

	public override void StartAttach()
	{
		_startAttachCheck = true;
		Check();
	}

	private void Update()
	{
		if (_startAttachCheck)
		{
			_elapsed += Chronometer.global.deltaTime;
			if (!(_elapsed < _checkInterval))
			{
				_elapsed -= _checkInterval;
				Check();
			}
		}
	}

	public override void StopAttach()
	{
		if (!((Object)(object)base.owner == (Object)null))
		{
			_attached = false;
			_startAttachCheck = false;
			base.owner.ability.Remove(_abilityComponent.ability);
		}
	}

	private void Check()
	{
		switch (_comparer)
		{
		case Comparer.Equal:
			if (_count == GameData.Currency.currencies[_type].balance)
			{
				Attach();
			}
			else
			{
				Detach();
			}
			break;
		case Comparer.GreaterThanOrEqual:
			if (_count <= GameData.Currency.currencies[_type].balance)
			{
				Attach();
			}
			else
			{
				Detach();
			}
			break;
		case Comparer.Greater:
			if (_count < GameData.Currency.currencies[_type].balance)
			{
				Attach();
			}
			else
			{
				Detach();
			}
			break;
		case Comparer.LessThanOrEqual:
			if (_count >= GameData.Currency.currencies[_type].balance)
			{
				Attach();
			}
			else
			{
				Detach();
			}
			break;
		case Comparer.LessThan:
			if (_count > GameData.Currency.currencies[_type].balance)
			{
				Attach();
			}
			else
			{
				Detach();
			}
			break;
		}
	}

	private void Attach()
	{
		if (!_attached)
		{
			_attached = true;
			base.owner.ability.Add(_abilityComponent.ability);
		}
	}

	private void Detach()
	{
		if (_attached)
		{
			_attached = false;
			base.owner.ability.Remove(_abilityComponent.ability);
		}
	}

	public override string ToString()
	{
		return this.GetAutoName();
	}
}
