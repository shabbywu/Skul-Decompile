using Data;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Hardmode.Darktech;

public sealed class BoneParticleMagnetoScope : MonoBehaviour
{
	[SerializeField]
	private Grave _grave;

	[SerializeField]
	private PoolObject _particle;

	private float _multiplier;

	private void Awake()
	{
		_grave.onLoot += Drop;
		_multiplier = Singleton<DarktechManager>.Instance.setting.뼈입자검출기변량;
	}

	private void Drop()
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		Rarity rarity = _grave.rarity;
		int num = (int)((float)Singleton<Service>.Instance.levelManager.currentChapter.currentStage.boneRangeByRarity.Evaluate(rarity) * _multiplier);
		int num2 = Random.Range(15, 17);
		int currencyAmount = num / num2;
		for (int i = 0; i < num2; i++)
		{
			CurrencyParticle component = ((Component)_particle.Spawn(((Component)this).transform.position, true)).GetComponent<CurrencyParticle>();
			component.currencyType = GameData.Currency.Type.Bone;
			component.currencyAmount = currencyAmount;
			if (i == 0)
			{
				component.currencyAmount += num % num2;
			}
		}
	}
}
