using Platforms;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions;

public sealed class Sin : InscriptionInstance
{
	[SerializeField]
	[Header("1세트 효과")]
	private float[] _damageReducePercentPoints;

	protected override void Initialize()
	{
	}

	public override void UpdateBonus(bool wasActive, bool wasOmen)
	{
	}

	public override void Attach()
	{
		ExtensionMethods.Set((Type)66);
		base.character.stat.adaptiveAttribute = true;
		base.character.onGiveDamage.Add(int.MaxValue, HandleOnGiveDamage);
	}

	private bool HandleOnGiveDamage(ITarget target, ref Damage damage)
	{
		Character character = target.character;
		if (character.type == Character.Type.Player || character.type == Character.Type.PlayerMinion)
		{
			return false;
		}
		damage.multiplier -= 0.30000001192092896;
		return false;
	}

	public override void Detach()
	{
		base.character.stat.adaptiveAttribute = false;
		base.character.onGiveDamage.Remove(HandleOnGiveDamage);
	}
}
