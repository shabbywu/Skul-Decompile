using System;
using Characters.Operations;
using FX;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Essences;

[Serializable]
public class EvileEye : Ability
{
	public class Instance : AbilityInstance<EvileEye>
	{
		private float _interval = 0.5f;

		private float _elapsed;

		private int _stacks;

		public override int iconStacks => _stacks;

		public override Sprite icon => SavableAbilityResource.instance.curseIcon;

		public Instance(Character owner, EvileEye ability)
			: base(owner, ability)
		{
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_elapsed += deltaTime;
			if (_elapsed >= _interval)
			{
				TakeDamage();
				_elapsed -= _interval;
			}
		}

		protected override void OnAttach()
		{
			_elapsed = 0f;
		}

		protected override void OnDetach()
		{
		}

		public override void Refresh()
		{
			base.Refresh();
			_elapsed = 0f;
		}

		private void TakeDamage()
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			Character player = Singleton<Service>.Instance.levelManager.player;
			Damage damage = player.stat.GetDamage(ability.attackDamage, Vector2.op_Implicit(((Component)owner).transform.position), ability.hitInfo);
			player.Attack(owner, ref damage);
			PersistentSingleton<SoundManager>.Instance.PlaySound(ability.soundInfo, ((Component)owner).transform.position);
		}
	}

	[SerializeField]
	private float attackDamage;

	[SerializeField]
	private SoundInfo soundInfo;

	[SerializeField]
	private HitInfo hitInfo;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
