using System;
using Characters;
using Characters.Operations;
using Services;
using Singletons;
using UnityEngine;

namespace Level.Traps;

[RequireComponent(typeof(Character))]
public class Fall : Trap
{
	[GetComponent]
	[SerializeField]
	private Character _character;

	[Tooltip("이미 설정된 값으로, 참조용으로 사용중")]
	[SerializeField]
	private double _damage;

	[Range(0f, 100f)]
	[SerializeField]
	private int _darkenemyDamagePercent;

	[SerializeField]
	private Transform _escapePoint;

	[SerializeField]
	[CharacterOperation.Subcomponent]
	private CharacterOperation.Subcomponents _onEscape;

	private IAttackDamage _attackDamage;

	private CharacterStatus.ApplyInfo _stun;

	private void Awake()
	{
		_onEscape.Initialize();
		_attackDamage = ((Component)this).GetComponent<IAttackDamage>();
		_stun = new CharacterStatus.ApplyInfo(CharacterStatus.Kind.Stun);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		Character component = ((Component)collision).GetComponent<Character>();
		if ((Object)(object)component == (Object)null)
		{
			return;
		}
		switch (component.type)
		{
		case Character.Type.Player:
		{
			float num = ((_damage == 0.0) ? 0f : _attackDamage.amount);
			Damage damage2 = new Damage(_character, num, Vector2.op_Implicit(((Component)component).transform.position), Damage.Attribute.Fixed, Damage.AttackType.Additional, Damage.MotionType.Basic, 1.0, 0f, 0.0, 1.0, 1.0, canCritical: true, @null: false, 0.0, 0.0, 0);
			if (Math.Floor(component.health.currentHealth) <= _damage)
			{
				damage2.@base = component.health.currentHealth - 1.0;
			}
			component.health.TakeDamage(ref damage2);
			((Component)component).transform.position = _escapePoint.position;
			Singleton<Service>.Instance.floatingTextSpawner.SpawnPlayerTakingDamage(damage2.amount, Vector2.op_Implicit(((Component)component).transform.position));
			_onEscape.Run(component);
			break;
		}
		case Character.Type.PlayerMinion:
			((Component)component).transform.position = _escapePoint.position;
			break;
		case Character.Type.TrashMob:
		case Character.Type.Summoned:
			component.health.Kill();
			break;
		case Character.Type.Named:
		{
			double @base = component.health.maximumHealth * (double)_darkenemyDamagePercent * 0.009999999776482582;
			Damage damage = new Damage(_character, @base, Vector2.op_Implicit(((Component)component).transform.position), Damage.Attribute.Fixed, Damage.AttackType.Additional, Damage.MotionType.Basic, 1.0, 0f, 0.0, 1.0, 1.0, canCritical: true, @null: false, 0.0, 0.0, 0);
			_character.GiveStatus(component, _stun);
			component.health.TakeDamage(ref damage);
			((Component)component).transform.position = _escapePoint.position;
			break;
		}
		case Character.Type.Adventurer:
		case Character.Type.Boss:
		case Character.Type.Trap:
		case Character.Type.Dummy:
			break;
		}
	}
}
