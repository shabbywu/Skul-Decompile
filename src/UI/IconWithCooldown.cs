using System.Collections;
using Characters.Cooldowns;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

public class IconWithCooldown : MonoBehaviour
{
	[SerializeField]
	private Image _icon;

	[SerializeField]
	private Image _cooldownMask;

	[SerializeField]
	private Image _streakMask;

	[SerializeField]
	private TMP_Text _remainStreaks;

	[SerializeField]
	private Animator _effect;

	private float _effectLength;

	private CooldownSerializer _cooldown;

	private CoroutineReference _cPlayEffectReference;

	public Image icon => _icon;

	public CooldownSerializer cooldown
	{
		get
		{
			return _cooldown;
		}
		set
		{
			if ((Object)(object)_effect != (Object)null && _cooldown != value)
			{
				if (_cooldown != null)
				{
					_cooldown.onReady -= SpawnEffect;
				}
				if (value != null)
				{
					value.onReady += SpawnEffect;
				}
			}
			_cooldown = value;
		}
	}

	private void Awake()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		((Component)_effect).gameObject.SetActive(true);
		AnimatorStateInfo currentAnimatorStateInfo = _effect.GetCurrentAnimatorStateInfo(0);
		_effectLength = ((AnimatorStateInfo)(ref currentAnimatorStateInfo)).length;
		((Component)_effect).gameObject.SetActive(false);
	}

	protected virtual void Update()
	{
		if (cooldown == null)
		{
			return;
		}
		_cooldownMask.fillAmount = cooldown.remainPercent;
		if (cooldown.type == CooldownSerializer.Type.Time)
		{
			if ((Object)(object)_remainStreaks != (Object)null)
			{
				if (cooldown.streak.remains > 0)
				{
					_remainStreaks.text = cooldown.streak.remains.ToString();
					_streakMask.fillAmount = cooldown.time.streak.remainPercent;
				}
				else if (cooldown.stacks > 1)
				{
					_remainStreaks.text = cooldown.stacks.ToString();
					_streakMask.fillAmount = 0f;
				}
				else
				{
					_remainStreaks.text = string.Empty;
					_streakMask.fillAmount = 0f;
				}
			}
		}
		else
		{
			_remainStreaks.text = string.Empty;
			_streakMask.fillAmount = 0f;
		}
	}

	private void OnDisable()
	{
		((Component)_effect).gameObject.SetActive(false);
	}

	private void SpawnEffect()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		if (((Behaviour)this).isActiveAndEnabled)
		{
			((CoroutineReference)(ref _cPlayEffectReference)).Stop();
			_cPlayEffectReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, CPlayEffect());
		}
	}

	private IEnumerator CPlayEffect()
	{
		((Component)_effect).gameObject.SetActive(true);
		_effect.Play(0, 0, 0f);
		yield return Chronometer.global.WaitForSeconds(_effectLength);
		((Component)_effect).gameObject.SetActive(false);
	}
}
