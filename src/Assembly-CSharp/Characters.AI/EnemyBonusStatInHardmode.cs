using Data;
using Level;

namespace Characters.AI;

public sealed class EnemyBonusStatInHardmode
{
	public void AttachTo(Character character)
	{
		if (GameData.HardmodeProgress.hardmode)
		{
			HardmodeLevelInfo.EnemyStatInfo enemyStatInfoByType = HardmodeLevelInfo.instance.GetEnemyStatInfoByType(character.type);
			Stat.Values values = new Stat.Values(new Stat.Value(Stat.Category.Percent, Stat.Kind.AttackDamage, enemyStatInfoByType.attackMultiplier), new Stat.Value(Stat.Category.Percent, Stat.Kind.Health, enemyStatInfoByType.healthMultiplier));
			character.stat.AttachValues(values);
			character.stat.SetNeedUpdate();
		}
	}
}
