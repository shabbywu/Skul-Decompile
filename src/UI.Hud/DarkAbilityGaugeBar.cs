using Characters.Abilities.Darks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud;

public sealed class DarkAbilityGaugeBar : MonoBehaviour
{
	[SerializeField]
	private RectTransform _container;

	[SerializeField]
	private Image _mainBar;

	[SerializeField]
	private Vector3 _defaultMainBarScale = Vector3.one;

	private Vector3 _mainScale = Vector3.one;

	private DarkAbilityAttacher _attacher;

	private DarkAbilityGauge _gauge;

	public DarkAbilityGauge gauge
	{
		get
		{
			return _gauge;
		}
		set
		{
			_gauge = value;
			Update();
			((Component)this).gameObject.SetActive((Object)(object)_gauge != (Object)null);
		}
	}

	public void Initialize(DarkAbilityAttacher abilityAttacher)
	{
		_attacher = abilityAttacher;
		_gauge = _attacher.gauge;
		if ((Object)(object)_gauge == (Object)null)
		{
			((Component)_container).gameObject.SetActive(false);
		}
	}

	private void OnEnable()
	{
		_mainScale.x = 0f;
	}

	private void Update()
	{
		if (!((Object)(object)gauge == (Object)null))
		{
			float targetPercent = math.clamp(gauge.gaugePercent, 0f, 1f);
			Lerp(targetPercent);
		}
	}

	private void Lerp(float targetPercent)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		_mainScale.x = Mathf.Lerp(_mainScale.x, targetPercent, 0.25f);
		((Component)_mainBar).transform.localScale = Vector3.Scale(_mainScale, _defaultMainBarScale);
	}
}
