using System.Collections;
using Characters.Abilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Characters;

public class Dummy : MonoBehaviour
{
	private static Stat.Values cannotBeKnockbacked = new Stat.Values(new Stat.Value(Stat.Category.Percent, Stat.Kind.KnockbackResistance, 0.0));

	private static readonly string[] tauntScripts = new string[44]
	{
		"오징어볶음 다뒤졋다 ㅋㅋ", "와 샌즈! 언더테일 아시는구나!", "모르면 맞아야죠", "오늘 메뉴는 불고기 어떻습니까?", "오늘 메뉴는 찜닭 어떻습니까?", "오늘 메뉴는 닭도리탕 어떻습니까?", "오늘 메뉴는 스테이크 어떻습니까?", "오늘 메뉴는 보쌈 어떻습니까?", "오늘 메뉴는 족발 어떻습니까?", "오늘 메뉴는 삼겹살 어떻습니까?",
		"오늘 메뉴는 쌈밥 어떻습니까?", "오늘 메뉴는 치킨 어떻습니까?", "오늘 메뉴는 순두부찌개 어떻습니까?", "오늘 메뉴는 부대찌개 어떻습니까?", "오늘 메뉴는 김치찌개 어떻습니까?", "오늘 메뉴는 카레 어떻습니까?", "오늘 메뉴는 비빔밥 어떻습니까?", "오늘 메뉴는 김치볶음밥 어떻습니까?", "오늘 메뉴는 제육볶음 어떻습니까?", "오늘 메뉴는 치킨마요 어떻습니까?",
		"오늘 메뉴는 돈부리 어떻습니까?", "오늘 메뉴는 국밥 어떻습니까?", "오늘 메뉴는 뼈해장국 어떻습니까?", "오늘 메뉴는 스파게티 어떻습니까?", "오늘 메뉴는 냉면 어떻습니까?", "오늘 메뉴는 짜장면 어떻습니까?", "오늘 메뉴는 짬뽕 어떻습니까?", "오늘 메뉴는 탕수육 어떻습니까?", "오늘 메뉴는 꿔바로우 어떻습니까?", "오늘 메뉴는 라면 어떻습니까?",
		"오늘 메뉴는 초밥 어떻습니까?", "오늘 메뉴는 깐풍기 어떻습니까?", "오늘 메뉴는 쌀국수 어떻습니까?", "오늘 메뉴는 마파두부 어떻습니까?", "오늘 메뉴는 햄버거 어떻습니까?", "오늘 메뉴는 팟타이 어떻습니까?", "오늘 메뉴는 샌드위치 어떻습니까?", "오늘 메뉴는 떢볶이 어떻습니까?", "오늘 메뉴는 갈비탕 어떻습니까?", "오늘 메뉴는 밥버거 어떻습니까?",
		"오늘 메뉴는 서브웨이 어떻습니까?", "오늘 메뉴는 도시락 어떻습니까?", "오늘 메뉴는 돈까스 어떻습니까?", "오늘 메뉴는 그냥 굶는 게 어떻습니까?"
	};

	private readonly GetInvulnerable _getInvulnerable = new GetInvulnerable
	{
		duration = 4f
	};

	[SerializeField]
	private bool _immuneToCritical;

	[SerializeField]
	private bool _cannotBeKnockbacked;

	[SerializeField]
	private bool _dpsTextEmptyOnStart;

	[GetComponent]
	[SerializeField]
	private Character _character;

	[SerializeField]
	private TMP_Text _timeText;

	[SerializeField]
	private TMP_Text _dpsText;

	private bool _started;

	private float _time;

	private EnumArray<Damage.Attribute, int> _attackCountByAttribute = new EnumArray<Damage.Attribute, int>();

	private EnumArray<Damage.Attribute, double> _damageByAttribute = new EnumArray<Damage.Attribute, double>();

	private int _totalAttackCount;

	private double _totalDamage;

	private Vector3 _original;

	private void Awake()
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		_character.health.onTookDamage += OnTookDamage;
		if (_immuneToCritical)
		{
			_character.health.immuneToCritical = true;
		}
		if (_cannotBeKnockbacked)
		{
			_character.stat.AttachValues(cannotBeKnockbacked);
		}
		_original = ((Component)this).transform.position;
		if (_dpsTextEmptyOnStart)
		{
			_dpsText.text = string.Empty;
		}
		else
		{
			_dpsText.text = tauntScripts.Random();
		}
	}

	private void Initialize()
	{
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		_timeText.text = $"{_time:0.00}s";
		_dpsText.text = $"{_totalDamage:0.0}\n{_totalDamage / (double)_time:0.00}";
		_started = false;
		_time = 0f;
		_attackCountByAttribute.SetAll(0);
		_damageByAttribute.SetAll(0.0);
		_totalAttackCount = 0;
		_totalDamage = 0.0;
		((Component)this).transform.position = _original;
		_character.health.Revive();
		_character.movement.push.Expire();
		_character.health.ResetToMaximumHealth();
		_character.ability.Add(_getInvulnerable);
		((MonoBehaviour)this).StopAllCoroutines();
	}

	private void Update()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		if (_started)
		{
			((Graphic)_timeText).color = Color.yellow;
			((Graphic)_dpsText).color = Color.yellow;
		}
		else if (_character.cinematic.value)
		{
			((Graphic)_timeText).color = Color.red;
			((Graphic)_dpsText).color = Color.red;
		}
		else
		{
			((Graphic)_timeText).color = Color.white;
			((Graphic)_dpsText).color = Color.white;
		}
	}

	private void OnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		if (!_character.cinematic.value && !_character.ability.Contains(_getInvulnerable))
		{
			if (!_started)
			{
				_started = true;
				((MonoBehaviour)this).StartCoroutine(CMesure());
			}
			_totalDamage += tookDamage.amount;
			_damageByAttribute[tookDamage.attribute] += tookDamage.amount;
			_totalAttackCount++;
			_attackCountByAttribute[tookDamage.attribute]++;
			if (_character.health.currentHealth == 0.0)
			{
				Initialize();
			}
		}
	}

	private IEnumerator CMesure()
	{
		while (true)
		{
			yield return null;
			_time += Chronometer.global.deltaTime;
			_timeText.text = $"{_time:0.00}s";
			string dpsTextByAttribute = GetDpsTextByAttribute(Damage.Attribute.Physical, _time);
			string dpsTextByAttribute2 = GetDpsTextByAttribute(Damage.Attribute.Magic, _time);
			string dpsTextByAttribute3 = GetDpsTextByAttribute(Damage.Attribute.Fixed, _time);
			string dpsText = GetDpsText(_totalDamage, _totalAttackCount, _time);
			_dpsText.text = Colorize(dpsTextByAttribute, "#F25D1C") + "\n" + Colorize(dpsTextByAttribute2, "#1787D8") + "\n" + Colorize(dpsTextByAttribute3, "#959595") + "\n" + dpsText;
		}
	}

	private static string Colorize(string text, string color)
	{
		return "<color=" + color + ">" + text + "</color>";
	}

	private string GetDpsTextByAttribute(Damage.Attribute attribute, float time)
	{
		return GetDpsText(_damageByAttribute[attribute], _attackCountByAttribute[attribute], time);
	}

	private static string GetDpsText(double damage, int attackCount, float time)
	{
		return $"{attackCount}\n{damage:0.0}\n{damage / (double)time:0.00}/s";
	}
}
