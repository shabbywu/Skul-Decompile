using System;
using FX;
using Level;
using UnityEngine;

namespace Characters.Abilities.Customs;

[Serializable]
public sealed class MagicWand : Ability
{
	public class Instance : AbilityInstance<MagicWand>
	{
		public int stack { get; set; }

		public Instance(Character owner, MagicWand ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.playerComponents.inventory.weapon.onSwap += Weapon_onSwap;
		}

		private void Weapon_onSwap()
		{
			stack++;
			if (stack >= ability._maxStack)
			{
				ChangeToDummy();
				stack = 0;
			}
		}

		protected override void OnDetach()
		{
		}

		private void ChangeToDummy()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			Character randomTarget = TargetFinder.GetRandomTarget(ability._findRange, LayerMask.op_Implicit(1024));
			if (!((Object)(object)randomTarget == (Object)null))
			{
				Vector3 position = ((Component)randomTarget).transform.position;
				randomTarget.health.Kill();
				Object.Instantiate<GameObject>(ability._summonPrefab, ((Component)Map.Instance.waveContainer.summonWave).transform).transform.position = position;
				ability._effect.Spawn(position);
			}
		}
	}

	[SerializeField]
	private CharacterTypeBoolArray _targetType;

	[SerializeField]
	private Collider2D _findRange;

	[SerializeField]
	private GameObject _summonPrefab;

	[SerializeField]
	private EffectInfo _effect;

	[SerializeField]
	private int _maxStack;

	private Instance _instance;

	public int stack
	{
		get
		{
			return _instance.stack;
		}
		set
		{
			_instance.stack = value;
		}
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return _instance = new Instance(owner, this);
	}
}
