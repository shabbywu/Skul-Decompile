using Services;
using Singletons;
using UnityEngine;

namespace Characters.Actions.Constraints;

public class HealthConstraint : Constraint
{
	private enum Type
	{
		Constnat,
		Percent
	}

	[SerializeField]
	private Type _type;

	[SerializeField]
	private float _amount;

	[SerializeField]
	[Tooltip("이 Constraint를 통과하고 액션이 실행될 때 체력을 소모시킬지")]
	private bool _loseHealthOnConsume;

	[Tooltip("피해입었을 때 나타나는 숫자를 띄울지")]
	[SerializeField]
	private bool _spawnFloatingText;

	public override bool Pass()
	{
		if ((Object)(object)_action.owner.health == (Object)null)
		{
			return false;
		}
		switch (_type)
		{
		case Type.Constnat:
			if (_action.owner.health.currentHealth < (double)_amount)
			{
				return false;
			}
			break;
		case Type.Percent:
			if (_action.owner.health.percent < (double)_amount * 0.01)
			{
				return false;
			}
			break;
		}
		return true;
	}

	public override void Consume()
	{
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		base.Consume();
		if (!_loseHealthOnConsume)
		{
			return;
		}
		double num = 0.0;
		num = ((_type != Type.Percent) ? ((double)_amount) : (num * _action.owner.health.maximumHealth * 0.01));
		if (!(num < 1.0))
		{
			_action.owner.health.TakeHealth(num);
			if (_spawnFloatingText)
			{
				Singleton<Service>.Instance.floatingTextSpawner.SpawnPlayerTakingDamage(num, Vector2.op_Implicit(((Component)_action.owner).transform.position));
			}
		}
	}
}
