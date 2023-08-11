namespace Level.Waves;

public sealed class Always : Leaf
{
	protected override bool Check(EnemyWave wave)
	{
		return true;
	}
}
