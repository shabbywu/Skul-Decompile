using System.Collections;
using UnityEngine;

namespace Characters;

public class CharacterHealthBar : MonoBehaviour
{
	public enum ActionWhenCharacterNull
	{
		Deactivate,
		ShowZero
	}

	[SerializeField]
	protected RectTransform _container;

	[SerializeField]
	protected bool _alwaysVisible;

	[SerializeField]
	protected bool _roundUp;

	[SerializeField]
	private ActionWhenCharacterNull _actionWhenCharacterNull;

	[SerializeField]
	protected RectTransform _healthBar;

	[SerializeField]
	protected RectTransform _grayHealthBar;

	[SerializeField]
	protected RectTransform _canHealBar;

	[SerializeField]
	protected RectTransform _shieldBar;

	[SerializeField]
	protected RectTransform _damageLerpBar;

	[SerializeField]
	protected RectTransform _healLerpBar;

	protected Character _character;

	protected CharacterHealth _health;

	protected float _percent;

	protected float _percentWithGrayHealth;

	protected float _percentWithCanHeal;

	protected float _percentWithShield;

	protected Vector3 _defaultHealthScale;

	protected Vector3 _defaultGrayHealthScale;

	protected Vector3 _defaultCanHealScale;

	protected Vector3 _defaultShieldScale;

	protected Vector3 _defaultDamageLerpScale;

	protected Vector3 _defaultHealLerpScale;

	protected Vector3 _healthScale = Vector3.one;

	protected Vector3 _grayHealthScale = Vector3.one;

	protected Vector3 _canHealScale = Vector3.one;

	protected Vector3 _shieldScale = Vector3.one;

	protected Vector3 _damageLerpScale = Vector3.one;

	protected Vector3 _healLerpScale = Vector3.one;

	private const float _lifeTime = 3f;

	protected float _remainLifetime;

	public bool visible
	{
		get
		{
			return ((Component)this).gameObject.activeSelf;
		}
		set
		{
			((Component)this).gameObject.SetActive(value);
		}
	}

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		_defaultHealthScale = ((Transform)_healthBar).localScale;
		if ((Object)(object)_canHealBar != (Object)null)
		{
			_defaultCanHealScale = ((Transform)_canHealBar).localScale;
			_defaultGrayHealthScale = ((Transform)_grayHealthBar).localScale;
		}
		_defaultShieldScale = ((Transform)_shieldBar).localScale;
		_defaultDamageLerpScale = ((Transform)_damageLerpBar).localScale;
		_defaultHealLerpScale = ((Transform)_healLerpBar).localScale;
	}

	private void OnEnable()
	{
		_healthScale.x = 0f;
		_grayHealthScale.x = 0f;
		_canHealScale.x = 0f;
		_shieldScale.x = 0f;
		_damageLerpScale.x = 0f;
		_healLerpScale.x = 0f;
		((MonoBehaviour)this).StartCoroutine(CLerp());
	}

	public void Initialize(Character character)
	{
		_character = character;
		_health = _character.health;
		if (!_alwaysVisible)
		{
			((Component)_container).gameObject.SetActive(false);
			_remainLifetime = 3f;
			_health.onTookDamage += OnTookDamage;
		}
		else
		{
			((Component)_container).gameObject.SetActive(true);
		}
	}

	private void OnDestroy()
	{
		if ((Object)(object)_health != (Object)null)
		{
			_health.onTookDamage -= OnTookDamage;
		}
	}

	public void SetWidth(float width)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		RectTransform healthBar = _healthBar;
		RectTransform shieldBar = _shieldBar;
		RectTransform damageLerpBar = _damageLerpBar;
		RectTransform healLerpBar = _healLerpBar;
		Vector2 val = default(Vector2);
		((Vector2)(ref val))._002Ector(0.5f, 0.5f);
		healLerpBar.pivot = val;
		Vector2 val3 = (damageLerpBar.pivot = val);
		Vector2 pivot = (shieldBar.pivot = val3);
		healthBar.pivot = pivot;
		if ((Object)(object)_grayHealthBar != (Object)null)
		{
			_grayHealthBar.pivot = new Vector2(0.5f, 0.5f);
			_canHealBar.pivot = new Vector2(0.5f, 0.5f);
		}
		_container.SetSizeWithCurrentAnchors((Axis)0, width);
		RectTransform healthBar2 = _healthBar;
		RectTransform shieldBar2 = _shieldBar;
		RectTransform damageLerpBar2 = _damageLerpBar;
		RectTransform healLerpBar2 = _healLerpBar;
		((Vector2)(ref val))._002Ector(0f, 0.5f);
		healLerpBar2.pivot = val;
		val3 = (damageLerpBar2.pivot = val);
		pivot = (shieldBar2.pivot = val3);
		healthBar2.pivot = pivot;
		if ((Object)(object)_grayHealthBar != (Object)null)
		{
			_grayHealthBar.pivot = new Vector2(0f, 0.5f);
			_canHealBar.pivot = new Vector2(0f, 0.5f);
		}
	}

	private void OnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		((Component)_container).gameObject.SetActive(true);
		_remainLifetime = 3f;
	}

	private void Update()
	{
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_character == (Object)null && _actionWhenCharacterNull == ActionWhenCharacterNull.Deactivate)
		{
			((Component)_container).gameObject.SetActive(false);
			return;
		}
		double num;
		double num2;
		double num3;
		double num4;
		double num5;
		if ((Object)(object)_character == (Object)null || (Object)(object)_character.health == (Object)null || _character.health.dead)
		{
			num = 0.0;
			num2 = (((Object)(object)_health == (Object)null) ? 0.0 : _health.maximumHealth);
			num3 = 0.0;
			num4 = 0.0;
			num5 = 0.0;
		}
		else
		{
			num = _health.currentHealth;
			num2 = _health.maximumHealth;
			num3 = _health.grayHealth.maximum;
			num4 = _health.grayHealth.canHeal;
			num5 = _health.shield.amount;
		}
		if (num2 == 0.0)
		{
			_percent = 0f;
			_percentWithGrayHealth = 0f;
			_percentWithCanHeal = 0f;
			_percentWithShield = 0f;
		}
		else if (num3 > 0.0)
		{
			double num6 = num + num3;
			double num7 = num + num4;
			if (num6 + num5 <= num2)
			{
				_percent = (_roundUp ? _health.displayPercent : ((float)_health.percent));
				_percentWithGrayHealth = (float)(num6 / num2);
				_percentWithCanHeal = (float)(num7 / num2);
				_percentWithShield = (float)((num6 + num5) / num2);
			}
			else
			{
				_percent = (float)(num / (num6 + num5));
				_percentWithGrayHealth = (float)(num6 / (num6 + num5));
				_percentWithCanHeal = (float)(num7 / (num6 + num5));
				_percentWithShield = 1f;
			}
		}
		else
		{
			_percentWithGrayHealth = 0f;
			_percentWithCanHeal = 0f;
			if (num + num5 <= num2)
			{
				_percent = (_roundUp ? _health.displayPercent : ((float)_health.percent));
				if (_roundUp)
				{
					_percentWithShield = Mathf.Round((float)(num + num5)) / Mathf.Round((float)num2);
				}
				else
				{
					_percentWithShield = (float)((num + num5) / num2);
				}
			}
			else
			{
				_percent = (float)(num / (num + num5));
				_percentWithShield = 1f;
			}
		}
		_healLerpScale.x = _percentWithShield;
		((Transform)_healLerpBar).localScale = Vector3.Scale(_healLerpScale, _defaultHealLerpScale);
	}

	private IEnumerator CLerp()
	{
		while (true)
		{
			if (_percentWithShield < _damageLerpScale.x)
			{
				_damageLerpScale.x = Mathf.Lerp(_damageLerpScale.x, _percentWithShield, 0.1f);
			}
			else
			{
				_damageLerpScale.x = _shieldScale.x;
			}
			if (_percentWithShield < _shieldScale.x)
			{
				_shieldScale.x = _percentWithShield;
			}
			else
			{
				_shieldScale.x = Mathf.Lerp(_shieldScale.x, _percentWithShield, 0.1f);
			}
			_healthScale.x = _shieldScale.x - (_percentWithShield - _percent);
			_grayHealthScale.x = _shieldScale.x - (_percentWithShield - _percentWithGrayHealth);
			_canHealScale.x = _shieldScale.x - (_percentWithShield - _percentWithCanHeal);
			if (_healthScale.x < 0f)
			{
				_healthScale.x = 0f;
			}
			if (_canHealScale.x < 0f)
			{
				_canHealScale.x = 0f;
			}
			if (_grayHealthScale.x < 0f)
			{
				_grayHealthScale.x = 0f;
			}
			((Transform)_damageLerpBar).localScale = Vector3.Scale(_damageLerpScale, _defaultDamageLerpScale);
			((Transform)_healthBar).localScale = Vector3.Scale(_healthScale, _defaultHealthScale);
			if ((Object)(object)_canHealBar != (Object)null)
			{
				((Transform)_grayHealthBar).localScale = Vector3.Scale(_grayHealthScale, _defaultGrayHealthScale);
				((Transform)_canHealBar).localScale = Vector3.Scale(_canHealScale, _defaultCanHealScale);
			}
			((Transform)_shieldBar).localScale = Vector3.Scale(_shieldScale, _defaultShieldScale);
			_remainLifetime -= Time.deltaTime;
			if (!_alwaysVisible && _remainLifetime <= 0f)
			{
				_damageLerpScale.x = _shieldScale.x;
				_shieldScale.x = _percentWithShield;
				((Component)_container).gameObject.SetActive(false);
			}
			yield return null;
		}
	}
}
