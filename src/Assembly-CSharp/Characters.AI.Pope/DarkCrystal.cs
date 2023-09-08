using System;
using System.Collections;
using Characters.Abilities;
using Hardmode;
using Scenes;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.AI.Pope;

public sealed class DarkCrystal : MonoBehaviour
{
	[Serializable]
	private class DarkCrystalBarrier : Ability
	{
		public class Instance : AbilityInstance<DarkCrystalBarrier>
		{
			public Instance(Character owner, DarkCrystalBarrier ability)
				: base(owner, ability)
			{
			}

			protected override void OnAttach()
			{
				//IL_0048: Unknown result type (might be due to invalid IL or missing references)
				//IL_004d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0065: Unknown result type (might be due to invalid IL or missing references)
				//IL_0076: Unknown result type (might be due to invalid IL or missing references)
				//IL_007b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0093: Unknown result type (might be due to invalid IL or missing references)
				owner.invulnerable.Attach(this);
				((Component)ability._front).gameObject.SetActive(true);
				((Component)ability._behind).gameObject.SetActive(true);
				Color color = ability._frontRenderer.color;
				color.a = 1f;
				ability._frontRenderer.color = color;
				color = ability._behindRenderer.color;
				color.a = 1f;
				ability._behindRenderer.color = color;
				ability._front.Play(_startHash);
				ability._behind.Play(_startHash);
			}

			protected override void OnDetach()
			{
				owner.invulnerable.Detach(this);
				((MonoBehaviour)owner).StartCoroutine(CFadeOut());
			}

			private IEnumerator CFadeOut()
			{
				float elapsed = 0f;
				int duration = 1;
				while (elapsed < (float)duration)
				{
					yield return null;
					elapsed += owner.chronometer.master.deltaTime;
					Color color = ability._frontRenderer.color;
					color.a = Mathf.Lerp(1f, 0f, elapsed / (float)duration);
					ability._frontRenderer.color = color;
					color = ability._behindRenderer.color;
					color.a = Mathf.Lerp(1f, 0f, elapsed / (float)duration);
					ability._behindRenderer.color = color;
				}
				((Component)ability._front).gameObject.SetActive(false);
				((Component)ability._behind).gameObject.SetActive(false);
			}
		}

		private static readonly int _startHash = Animator.StringToHash("Start");

		[SerializeField]
		private SpriteRenderer _frontRenderer;

		[SerializeField]
		private SpriteRenderer _behindRenderer;

		[SerializeField]
		private Animator _front;

		[SerializeField]
		private Animator _behind;

		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Instance(owner, this);
		}
	}

	[Header("하드모드")]
	[SerializeField]
	private Character _owner;

	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private DarkCrystal _other;

	[SerializeField]
	private bool _host;

	[SerializeField]
	private float _doubleAbilityInterval;

	[SerializeField]
	private float _singleAbilityInterval;

	[SerializeField]
	private DarkCrystalBarrier _doubleBarrierAbility;

	[SerializeField]
	private DarkCrystalBarrier _singleBarrierAbility;

	private Character owner => _owner;

	private void Start()
	{
		if (!Singleton<HardmodeManager>.Instance.hardmode)
		{
			Object.Destroy((Object)(object)this);
		}
		_doubleBarrierAbility.Initialize();
		_singleBarrierAbility.Initialize();
	}

	private void Update()
	{
	}

	public void StartProcess()
	{
		((MonoBehaviour)this).StartCoroutine(CProcess());
	}

	public void AttachDoubleAbility()
	{
		_owner.ability.Add(_doubleBarrierAbility);
	}

	private IEnumerator CProcess()
	{
		if (!((Object)(object)Singleton<Service>.Instance.levelManager.player == (Object)null))
		{
			while (Scene<GameBase>.instance.uiManager.letterBox.visible)
			{
				yield return null;
			}
			if (_host)
			{
				yield return StartAttachDoubleAbility();
			}
			while (!_other.owner.health.dead)
			{
				yield return null;
			}
			yield return StartAttachSingleAbility();
		}
	}

	private IEnumerator StartAttachDoubleAbility()
	{
		float elapsed = _doubleAbilityInterval;
		while (!_other.owner.health.dead)
		{
			elapsed += Chronometer.global.deltaTime;
			if (elapsed >= _doubleAbilityInterval)
			{
				elapsed -= _doubleAbilityInterval;
				if (MMMaths.RandomBool())
				{
					AttachDoubleAbility();
				}
				else
				{
					_other.AttachDoubleAbility();
				}
			}
			yield return null;
		}
	}

	private IEnumerator StartAttachSingleAbility()
	{
		float elapsed = 0f;
		while (!owner.health.dead)
		{
			elapsed += Chronometer.global.deltaTime;
			if (elapsed >= _singleAbilityInterval)
			{
				elapsed -= _singleAbilityInterval;
				_owner.ability.Add(_singleBarrierAbility);
			}
			yield return null;
		}
	}
}
