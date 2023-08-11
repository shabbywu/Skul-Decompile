namespace Level.Waves;

public sealed class ReturnFail : Leaf
{
	protected override bool Check(EnemyWave wave)
	{
		return false;
	}
}
