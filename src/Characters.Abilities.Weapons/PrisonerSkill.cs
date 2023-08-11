using UnityEngine;

namespace Characters.Abilities.Weapons;

public class PrisonerSkill : MonoBehaviour
{
	public PrisonerSkillInfosByGrade parent;

	public int level;

	public static PrisonerSkill AddComponent(GameObject gameObject, PrisonerSkillInfosByGrade parent, int level)
	{
		PrisonerSkill prisonerSkill = gameObject.AddComponent<PrisonerSkill>();
		prisonerSkill.parent = parent;
		prisonerSkill.level = level;
		return prisonerSkill;
	}
}
