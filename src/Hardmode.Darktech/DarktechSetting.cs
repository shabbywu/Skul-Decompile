using System;
using System.Collections.Generic;
using Level;
using UnityEngine;

namespace Hardmode.Darktech;

[CreateAssetMenu]
public sealed class DarktechSetting : ScriptableObject
{
	[Serializable]
	private class ValueByStage<T>
	{
		[SerializeField]
		private Chapter.Type _type;

		[SerializeField]
		private int _stage;

		[SerializeField]
		private T _value;

		public Chapter.Type type => _type;

		public int stage => _stage;

		public T value => _value;
	}

	[Serializable]
	public class ItemRotationEquipmentInfo
	{
		public DroppedPurchasableReward item;

		public int weight;

		public int basePrice;
	}

	[Serializable]
	public class ItemRotationEquipmentPriceInfo
	{
		public float multiplier;
	}

	[Serializable]
	public class LuckyMeasuringInstrument
	{
		[SerializeField]
		private RarityPossibilities _weightByRarity;

		[SerializeField]
		private int _lootableCount;

		[SerializeField]
		private int _refreshCount;

		[SerializeField]
		private int _refreshPrice;

		[SerializeField]
		private int _uniquePityCount;

		[SerializeField]
		private int _legendaryPityCount;

		public RarityPossibilities weightByRarity => _weightByRarity;

		public int lootableCount => _lootableCount;

		public int maxRefreshCount => _refreshCount;

		public int refreshPrice => _refreshPrice;

		public int uniquePityCount => _uniquePityCount;

		public int legendaryPityCount => _legendaryPityCount;
	}

	[Header("두개골 제조기")]
	[SerializeField]
	private int _두개골제조기가격;

	[Header("보급품 제조기")]
	[SerializeField]
	private int _보급품제조기가격;

	[Header("흉조 증폭기")]
	[SerializeField]
	private ValueByStage<CustomFloat>[] _흉조증폭기확률;

	private IDictionary<(Chapter.Type, float), CustomFloat> _흉조증폭기확률Dict;

	[Header("품목 순환 장치")]
	[SerializeField]
	private ItemRotationEquipmentInfo[] _품목순환장치아이템설정;

	[SerializeField]
	private ValueByStage<float>[] _품목순환장치스테이지설정;

	[SerializeField]
	private int _품목순환장치버프맵카운트;

	private IDictionary<(Chapter.Type, int), float> _품목순환장치상품별가격Dict;

	[Header("건강 보조 장치")]
	[SerializeField]
	private int[] _건강보조장치공격력버프가격;

	[SerializeField]
	private float[] _건강보조장치공격력버프스텟;

	[SerializeField]
	private int[] _건강보조장치체력버프가격;

	[SerializeField]
	private float[] _건강보조장치체력버프스텟;

	[SerializeField]
	private int[] _건강보조장치체력버프회복량;

	[SerializeField]
	private int[] _건강보조장치속도버프가격;

	[SerializeField]
	private float[] _건강보조장치속도버프스텟;

	[Header("행운 계측기")]
	[SerializeField]
	private LuckyMeasuringInstrument _행운계측기설정;

	[SerializeField]
	[Header("각인 합성 장치")]
	private ValueByStage<int>[] _각인합성장치사용가격;

	private IDictionary<(Chapter.Type, int), int> _각인합성장치사용가격Dict;

	[Header("뼈입자 검출기")]
	[SerializeField]
	private float _뼈입자검출기변량;

	[SerializeField]
	[Header("황금 계산기")]
	private float _황금계산기변량;

	[SerializeField]
	private ValueByStage<Vector2Int>[] _황금계산기골드바갯수;

	private IDictionary<(Chapter.Type, int), Vector2Int> _황금계산기골드바갯수Dict;

	public int 두개골제조기가격 => _두개골제조기가격;

	public int 보급품제조기가격 => _보급품제조기가격;

	public IDictionary<(Chapter.Type, float), CustomFloat> 흉조증폭기확률 => _흉조증폭기확률Dict;

	public ItemRotationEquipmentInfo[] 품목순화장치가중치 => _품목순환장치아이템설정;

	public int 품목순환장치버프맵카운트 => _품목순환장치버프맵카운트;

	public IDictionary<(Chapter.Type, int), float> 품목순환장치상품별가격Dict => _품목순환장치상품별가격Dict;

	public int[] 건강보조장치공격력버프가격 => _건강보조장치공격력버프가격;

	public float[] 건강보조장치공격력버프스텟 => _건강보조장치공격력버프스텟;

	public int[] 건강보조장치체력버프가격 => _건강보조장치체력버프가격;

	public float[] 건강보조장치체력버프스텟 => _건강보조장치체력버프스텟;

	public int[] 건강보조장치체력버프회복량 => _건강보조장치체력버프회복량;

	public int[] 건강보조장치속도버프가격 => _건강보조장치속도버프가격;

	public float[] 건강보조장치속도버프스텟 => _건강보조장치속도버프스텟;

	public LuckyMeasuringInstrument 행운계측기설정 => _행운계측기설정;

	public float 뼈입자검출기변량 => _뼈입자검출기변량;

	public float 황금계산기변량 => _황금계산기변량;

	public float 건강보조장치스탯증폭량 { get; set; } = 1f;


	public int GetInscriptionBonusCostByStage(Chapter.Type chapterType, int stageIndex)
	{
		if (_각인합성장치사용가격Dict.TryGetValue((chapterType, stageIndex), out var value))
		{
			return value;
		}
		return 123;
	}

	public int GetGoldCalculatorCount(Random random, Chapter.Type chapterType, int stageIndex)
	{
		if (_황금계산기골드바갯수Dict.TryGetValue((chapterType, stageIndex), out var value))
		{
			return random.Next(((Vector2Int)(ref value)).x, ((Vector2Int)(ref value)).y + 1);
		}
		return 1;
	}

	public void Initialize()
	{
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		_흉조증폭기확률Dict = new Dictionary<(Chapter.Type, float), CustomFloat>();
		ValueByStage<CustomFloat>[] array = _흉조증폭기확률;
		foreach (ValueByStage<CustomFloat> valueByStage in array)
		{
			_흉조증폭기확률Dict.Add((valueByStage.type, valueByStage.stage), valueByStage.value);
		}
		_각인합성장치사용가격Dict = new Dictionary<(Chapter.Type, int), int>();
		ValueByStage<int>[] 각인합성장치사용가격 = _각인합성장치사용가격;
		foreach (ValueByStage<int> valueByStage2 in 각인합성장치사용가격)
		{
			_각인합성장치사용가격Dict.Add((valueByStage2.type, valueByStage2.stage), valueByStage2.value);
		}
		_품목순환장치상품별가격Dict = new Dictionary<(Chapter.Type, int), float>();
		ValueByStage<float>[] 품목순환장치스테이지설정 = _품목순환장치스테이지설정;
		foreach (ValueByStage<float> valueByStage3 in 품목순환장치스테이지설정)
		{
			_품목순환장치상품별가격Dict.Add((valueByStage3.type, valueByStage3.stage), valueByStage3.value);
		}
		_황금계산기골드바갯수Dict = new Dictionary<(Chapter.Type, int), Vector2Int>();
		ValueByStage<Vector2Int>[] 황금계산기골드바갯수 = _황금계산기골드바갯수;
		foreach (ValueByStage<Vector2Int> valueByStage4 in 황금계산기골드바갯수)
		{
			_황금계산기골드바갯수Dict.Add((valueByStage4.type, valueByStage4.stage), valueByStage4.value);
		}
	}
}
