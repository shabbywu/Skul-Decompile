using System;
using System.Collections;
using Characters.Gear.Items;
using Characters.Operations;
using FX;
using Level;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class OmenMisfortune : Ability
{
	public sealed class Instance : AbilityInstance<OmenMisfortune>
	{
		private readonly int _attackKey = Animator.StringToHash("SymbolOfDeath_Attack");

		private readonly EffectInfo _effect;

		private EffectPoolInstance[] _effectInstances;

		private int _stack;

		private bool _attackRunning;

		public Instance(Character owner, OmenMisfortune ability)
			: base(owner, ability)
		{
			_effect = ability._mark;
			_effectInstances = new EffectPoolInstance[ability._angles.Length];
		}

		protected override void OnAttach()
		{
			_stack = 1;
			UpdateStack();
		}

		private IEnumerator CAttack()
		{
			Character attacker = ability._ownerItem.owner;
			PersistentSingleton<SoundManager>.Instance.PlaySound(ability._hitSound, ((Component)owner).transform.position);
			if (_effectInstances != null)
			{
				ability._hitInfo.ChangeAdaptiveDamageAttribute(attacker);
				if (_effectInstances != null)
				{
					EffectPoolInstance[] effectInstances = _effectInstances;
					for (int i = 0; i < effectInstances.Length; i++)
					{
						effectInstances[i].animator.Play(_attackKey);
						Damage damage = attacker.stat.GetDamage(ability._attackDamage.value, MMMaths.RandomPointWithinBounds(((Collider2D)owner.collider).bounds), ability._hitInfo);
						attacker.Attack(owner, ref damage);
						ability._hitEffect.Spawn(Vector2.op_Implicit(MMMaths.RandomPointWithinBounds(((Collider2D)owner.collider).bounds)));
						yield return Chronometer.global.WaitForSeconds(ability._attackInterval);
					}
				}
			}
			yield return Chronometer.global.WaitForSeconds(0.6f - ability._attackInterval);
			if (_effectInstances != null)
			{
				for (int num = _effectInstances.Length - 1; num >= 0; num--)
				{
					EffectPoolInstance effectPoolInstance = _effectInstances[num];
					if ((Object)(object)effectPoolInstance != (Object)null)
					{
						effectPoolInstance.Stop();
						_effectInstances[num] = null;
					}
				}
			}
			base.remainTime = 0f;
		}

		public override void Refresh()
		{
			base.Refresh();
			if (!_attackRunning)
			{
				_stack++;
				UpdateStack();
				if (_stack >= ability._maxStack)
				{
					_attackRunning = true;
					((MonoBehaviour)Map.Instance).StartCoroutine(CAttack());
				}
			}
		}

		private void UpdateStack()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			Bounds bounds = ((Collider2D)owner.collider).bounds;
			float x = ((Bounds)(ref bounds)).center.x;
			bounds = ((Collider2D)owner.collider).bounds;
			Vector2 val = new Vector2(x, ((Bounds)(ref bounds)).max.y) + Vector2.op_Implicit(Quaternion.Euler(0f, 0f, ability._angles[_stack - 1]) * Vector3.right) * ability._markDistanceFromTop;
			EffectPoolInstance effectPoolInstance = ((_effect == null) ? null : _effect.Spawn(Vector2.op_Implicit(val), owner));
			((Component)effectPoolInstance).transform.parent = owner.attach.transform;
			((Component)effectPoolInstance).transform.rotation = Quaternion.AngleAxis(ability._lookAngles[_stack - 1], Vector3.forward);
			_effectInstances[_stack - 1] = effectPoolInstance;
			PersistentSingleton<SoundManager>.Instance.PlaySound(ability._attackSound, ((Component)owner).transform.position);
		}

		protected override void OnDetach()
		{
			for (int num = _effectInstances.Length - 1; num >= 0; num--)
			{
				EffectPoolInstance effectPoolInstance = _effectInstances[num];
				if ((Object)(object)effectPoolInstance != (Object)null)
				{
					effectPoolInstance.Stop();
					_effectInstances[num] = null;
				}
			}
		}
	}

	[SerializeField]
	private Item _ownerItem;

	[SerializeField]
	private float[] _angles = new float[5] { 90f, 45f, 135f, 0f, 180f };

	[SerializeField]
	private float[] _lookAngles = new float[5] { 270f, 245f, 295f, 220f, 320f };

	[SerializeField]
	private float _markDistanceFromTop = 1.5f;

	[SerializeField]
	private float _attackInterval = 0.05f;

	[SerializeField]
	private EffectInfo _mark;

	[SerializeField]
	private SoundInfo _attachSound;

	[SerializeField]
	private EffectInfo _attackEffect;

	[SerializeField]
	private SoundInfo _attackSound;

	[SerializeField]
	private EffectInfo _hitEffect;

	[SerializeField]
	private SoundInfo _hitSound;

	[SerializeField]
	private CustomFloat _attackDamage;

	[SerializeField]
	private HitInfo _hitInfo;

	[SerializeField]
	private int _maxStack;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
