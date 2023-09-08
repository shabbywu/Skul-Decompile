using System;
using Services;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions;

public abstract class InscriptionInstance : MonoBehaviour, IComparable<InscriptionInstance>
{
	public Inscription keyword;

	public Character character { get; private set; }

	public void Initialize(Character character)
	{
		this.character = character;
		Initialize();
	}

	protected abstract void Initialize();

	public abstract void UpdateBonus(bool wasActive, bool wasOmen);

	public abstract void Attach();

	public abstract void Detach();

	public int CompareTo(InscriptionInstance other)
	{
		if (keyword.key == other.keyword.key)
		{
			return 0;
		}
		if (keyword.key <= other.keyword.key)
		{
			return -1;
		}
		return 1;
	}

	private void OnDestroy()
	{
		if (!Service.quitting)
		{
			Detach();
		}
	}
}
