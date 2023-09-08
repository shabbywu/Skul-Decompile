using System.Collections;
using Characters.Abilities;
using Characters.Operations;
using Characters.Operations.Fx;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions;

public sealed class Heirloom : InscriptionInstance
{
	private sealed class HeirloomAbility : IAbility, IAbilityInstance
	{
		private readonly Character _owner;

		private readonly Heirloom _heirloom;

		Character IAbilityInstance.owner => _owner;

		public IAbility ability => this;

		public float remainTime { get; set; }

		public bool attached => true;

		public Sprite icon { get; set; }

		public float iconFillAmount => 0f;

		public bool iconFillInversed => false;

		public bool iconFillFlipped => false;

		public int iconStacks => 0;

		public bool expired => false;

		public float duration { get; set; }

		public int iconPriority => 0;

		public bool removeOnSwapWeapon => false;

		public IAbilityInstance CreateInstance(Character owner)
		{
			return this;
		}

		public HeirloomAbility(Character owner, Heirloom heirloom)
		{
			_owner = owner;
			_heirloom = heirloom;
		}

		public void Initialize()
		{
		}

		public void UpdateTime(float deltaTime)
		{
		}

		public void Refresh()
		{
		}

		void IAbilityInstance.Attach()
		{
			_owner.health.onTakeDamage.Add(200, OnTakeDamage);
			_heirloom.StartSpawningMotionTrail();
			_owner.onGiveDamage.Add(int.MaxValue, HanldeOnGiveDamage);
			((MonoBehaviour)_owner).StartCoroutine(_heirloom._onAttach.CRun(_owner));
		}

		private bool HanldeOnGiveDamage(ITarget target, ref Damage damage)
		{
			if (!_heirloom.keyword.isMaxStep)
			{
				return false;
			}
			if (damage.attackType == Damage.AttackType.None)
			{
				return false;
			}
			damage.percentMultiplier *= _heirloom._damagePercentMultiplier;
			return false;
		}

		void IAbilityInstance.Detach()
		{
			_heirloom.SpawnMotionTrail();
			_heirloom.StopSpawningMotionTrail();
			_owner.health.onTakeDamage.Remove(OnTakeDamage);
			_owner.onGiveDamage.Remove(HanldeOnGiveDamage);
			_owner.ability.Add(_heirloom._cooldownAbility);
			((MonoBehaviour)_owner).StartCoroutine(_heirloom.CCooldown());
			((MonoBehaviour)_owner).StartCoroutine(_heirloom._onDetach.CRun(_owner));
		}

		private bool OnTakeDamage(ref Damage damage)
		{
			if (damage.attackType == Damage.AttackType.None || damage.attackType == Damage.AttackType.Additional)
			{
				return false;
			}
			if (damage.amount < 1.0)
			{
				return false;
			}
			if (_owner.invulnerable.value || _owner.evasion.value || damage.@null)
			{
				return false;
			}
			damage.@null = true;
			_owner.ability.Remove(this);
			return true;
		}
	}

	[SerializeField]
	[Header("2세트 효과")]
	private float _cooldownTime;

	[SerializeField]
	private Sprite _icon;

	[SerializeField]
	[Space]
	private MotionTrail _motionTrailOperation;

	[SerializeField]
	private float _motionTrailInterval;

	[Space]
	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onAttach;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onDetach;

	[SerializeField]
	private Nothing _cooldownAbility;

	[SerializeField]
	[Header("4세트 효과")]
	private float _damagePercentMultiplier;

	private HeirloomAbility _ability;

	private bool _canUse;

	private CoroutineReference _attachReference;

	protected override void Initialize()
	{
		_ability = new HeirloomAbility(base.character, this);
		_ability.Initialize();
		_ability.icon = _icon;
		_cooldownAbility.Initialize();
	}

	public override void UpdateBonus(bool wasActive, bool wasOmen)
	{
		if (keyword.step < 1)
		{
			base.character.ability.Remove(_ability);
			base.character.ability.Remove(_cooldownAbility);
		}
	}

	public override void Attach()
	{
		_attachReference.Stop();
		_attachReference = ((MonoBehaviour)(object)this).StartCoroutineWithReference("CStartAttachLoop");
		_canUse = true;
	}

	public override void Detach()
	{
		_attachReference.Stop();
		base.character.ability.Remove(_ability);
		base.character.ability.Remove(_cooldownAbility);
		((MonoBehaviour)this).StopCoroutine("CStartAttachLoop");
		StopSpawningMotionTrail();
	}

	private IEnumerator CStartAttachLoop()
	{
		while (true)
		{
			yield return null;
			if (keyword.step >= 1 && _canUse)
			{
				base.character.ability.Add(_ability);
			}
		}
	}

	private IEnumerator CCooldown()
	{
		_canUse = false;
		while (base.character.ability.Contains(_cooldownAbility))
		{
			yield return null;
		}
		_canUse = true;
	}

	private void SpawnMotionTrail()
	{
		_motionTrailOperation.Run(base.character);
	}

	private void StartSpawningMotionTrail()
	{
		((MonoBehaviour)this).StartCoroutine("CSpawnTrail");
	}

	private void StopSpawningMotionTrail()
	{
		((MonoBehaviour)this).StopCoroutine("CSpawnTrail");
	}

	private IEnumerator CSpawnTrail()
	{
		while (true)
		{
			SpawnMotionTrail();
			yield return Chronometer.global.WaitForSeconds(_motionTrailInterval);
		}
	}
}
