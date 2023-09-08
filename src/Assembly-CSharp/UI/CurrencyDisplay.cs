using Data;
using TMPro;
using UnityEngine;

namespace UI;

public class CurrencyDisplay : MonoBehaviour
{
	private static readonly int _earnAnimationHash = Animator.StringToHash("Earn");

	private static readonly int _idleAnimationHash = Animator.StringToHash("Idle");

	[SerializeField]
	private GameData.Currency.Type _type;

	[SerializeField]
	private TextMeshProUGUI _text;

	[Header("Effects")]
	[SerializeField]
	private CurrencyEffect _effect;

	[SerializeField]
	private Animator _animator;

	private int _balanceCache;

	private void Awake()
	{
		_balanceCache = GameData.Currency.currencies[_type].balance;
		((TMP_Text)_text).text = _balanceCache.ToString();
	}

	private void OnEnable()
	{
		((Behaviour)_animator).enabled = false;
		((Behaviour)_animator).enabled = true;
		_animator.Play(_idleAnimationHash);
	}

	private void Update()
	{
		int balance = GameData.Currency.currencies[_type].balance;
		if (_balanceCache != balance)
		{
			_balanceCache = balance;
			((TMP_Text)_text).text = balance.ToString();
			if ((Object)(object)_animator != (Object)null)
			{
				_animator.Play(_earnAnimationHash);
			}
			if ((Object)(object)_effect != (Object)null)
			{
				_effect.Play();
			}
		}
	}
}
