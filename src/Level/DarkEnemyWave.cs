namespace Level;

public sealed class DarkEnemyWave : EnemyWave
{
	public override void Initialize()
	{
		base.Initialize();
		DarkEnemySelector.instance.ElectIn(base.characters);
	}
}
