namespace Characters.Gear.Quintessences.Constraints;

public static class ConstraintExtension
{
	public static bool Pass(this Constraint[] constraints)
	{
		for (int i = 0; i < constraints.Length; i++)
		{
			if (!constraints[i].Pass())
			{
				return false;
			}
		}
		return true;
	}
}
