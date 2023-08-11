using System;
using System.Collections.Generic;
using Characters.Operations;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class SymbolOfToughness : Ability
{
	public class Histroy
	{
		public Instance instance;

		public Histroy(Instance instance)
		{
			this.instance = instance;
		}
	}

	public class Instance : AbilityInstance<SymbolOfToughness>
	{
		private double _multiplier;

		private static short spriteLayer = short.MinValue;

		private bool _precondition;

		private float _lastAttackTime;

		public Instance(Character owner, SymbolOfToughness ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.health.onTakeDamage.Add(int.MaxValue, (TakeDamageDelegate)HandleOnTakeDamage);
			owner.health.onTookDamage += OnTookDamage;
			((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Add(int.MaxValue, (GiveDamageDelegate)OnGiveDamage);
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(HanldeOnGaveDamage));
			_lastAttackTime = Time.time;
			_instances.Add(this);
		}

		private bool HandleOnTakeDamage(ref Damage damage)
		{
			if (Time.time - _lastAttackTime < ability._attackCooldownTime)
			{
				_precondition = false;
				return false;
			}
			if (owner.health.shield.hasAny && owner.health.shield.amount > 0.0)
			{
				_precondition = true;
				_multiplier = Mathf.Min(ability._damageMaxMultiplier, 1f + (float)(owner.health.shield.amount * ability._damageConversionRatio));
				return false;
			}
			_precondition = false;
			return false;
		}

		private void OnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			if (!_precondition || damageDealt < 1.0)
			{
				return;
			}
			Character character = originalDamage.attacker.character;
			if (!((Object)(object)character == (Object)null))
			{
				_lastAttackTime = Time.time;
				Vector3 one = Vector3.one;
				if (((Component)character).transform.position.x > ((Component)owner).transform.position.x)
				{
					ability._summonTransform.position = Vector2.op_Implicit(new Vector2(((Component)owner).transform.position.x + ability._offset.x, ((Component)owner).transform.position.y));
				}
				else
				{
					ability._summonTransform.position = Vector2.op_Implicit(new Vector2(((Component)owner).transform.position.x - ability._offset.x, ((Component)owner).transform.position.y));
					((Vector3)(ref one))._002Ector(-1f, 1f, 1f);
				}
				OperationRunner operationRunner = ability._operationRunner.Spawn();
				OperationInfos operationInfos = operationRunner.operationInfos;
				((Component)operationInfos).transform.SetPositionAndRotation(ability._summonTransform.position, Quaternion.identity);
				SortingGroup component = ((Component)operationRunner).GetComponent<SortingGroup>();
				if ((Object)(object)component != (Object)null)
				{
					component.sortingOrder = spriteLayer++;
				}
				((Component)operationInfos).transform.localScale = one;
				operationInfos.Run(owner);
			}
		}

		private bool OnGiveDamage(ITarget target, ref Damage damage)
		{
			if (!string.IsNullOrWhiteSpace(ability._counterAttackKey) && !damage.key.Equals(ability._counterAttackKey, StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			if (!_instances[attackIndex].Equals(this))
			{
				return false;
			}
			damage.percentMultiplier *= _multiplier;
			return false;
		}

		private void HanldeOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			if ((string.IsNullOrWhiteSpace(ability._counterAttackKey) || gaveDamage.key.Equals(ability._counterAttackKey, StringComparison.OrdinalIgnoreCase)) && _instances[attackIndex].Equals(this))
			{
				attackIndex++;
				if (attackIndex >= _instances.Count)
				{
					attackIndex = 0;
				}
			}
		}

		protected override void OnDetach()
		{
			_instances.Remove(this);
			owner.health.onTakeDamage.Remove((TakeDamageDelegate)HandleOnTakeDamage);
			owner.health.onTookDamage -= OnTookDamage;
			((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Remove((GiveDamageDelegate)OnGiveDamage);
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(HanldeOnGaveDamage));
		}
	}

	[SerializeField]
	private string _counterAttackKey;

	[Header("최대 데미지 증폭값 (percent) > 1")]
	[SerializeField]
	private float _damageMaxMultiplier;

	[SerializeField]
	[Header("데미지 증폭값 (percent) = 1 + shield양 * damageConversionRatio")]
	private double _damageConversionRatio;

	[SerializeField]
	private Transform _summonTransform;

	[SerializeField]
	private Vector2 _offset = new Vector2(1f, 0.5f);

	[SerializeField]
	[Tooltip("오퍼레이션 프리팹")]
	[Header("Summon")]
	internal OperationRunner _operationRunner;

	[SerializeField]
	private float _attackCooldownTime = 1f;

	internal static List<Instance> _instances = new List<Instance>();

	internal static int attackIndex = 0;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
