using System.Collections.Generic;
using Characters.Gear.Weapons;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters.Abilities.Weapons;

public class PrisonerSkillInfosByGrade : MonoBehaviour
{
	[SerializeField]
	private string _key;

	[FormerlySerializedAs("_skills")]
	[SerializeField]
	private SkillInfo[] _skillInfos;

	public string key => _key;

	public IReadOnlyList<SkillInfo> skillInfos => _skillInfos;

	private void Awake()
	{
		for (int i = 0; i < _skillInfos.Length; i++)
		{
			PrisonerSkill.AddComponent(((Component)_skillInfos[i]).gameObject, this, i);
		}
	}
}
