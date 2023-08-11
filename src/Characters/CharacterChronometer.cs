namespace Characters;

public class CharacterChronometer
{
	public readonly Chronometer master;

	public readonly Chronometer effect;

	public readonly Chronometer projectile;

	public readonly Chronometer animation;

	public CharacterChronometer()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		master = new Chronometer();
		effect = new Chronometer((ChronometerBase)(object)master);
		projectile = new Chronometer((ChronometerBase)(object)master);
		animation = new Chronometer((ChronometerBase)(object)master);
	}
}
