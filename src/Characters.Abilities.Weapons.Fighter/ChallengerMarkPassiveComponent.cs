using System.Collections.Generic;
using Services;
using Singletons;

namespace Characters.Abilities.Weapons.Fighter;

public class ChallengerMarkPassiveComponent : AbilityComponent<ChallengerMarkPassive>
{
	private List<ChallengerMarkPassive.Instance> _challengerMarks;

	public override void Initialize()
	{
		base.Initialize();
		_challengerMarks = new List<ChallengerMarkPassive.Instance>(1);
		Singleton<Service>.Instance.levelManager.onMapLoaded += Clear;
	}

	private void Clear()
	{
		_challengerMarks.Clear();
	}

	public void Add(ChallengerMarkPassive.Instance instance)
	{
		_challengerMarks.Add(instance);
	}

	public int Count()
	{
		return _challengerMarks.Count;
	}

	public void Remove(ChallengerMarkPassive.Instance instance)
	{
		_challengerMarks.Remove(instance);
	}

	private void OnDestroy()
	{
		Singleton<Service>.Instance.levelManager.onMapLoaded -= Clear;
	}
}
