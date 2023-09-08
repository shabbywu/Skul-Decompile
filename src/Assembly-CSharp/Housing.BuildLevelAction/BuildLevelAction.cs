using UnityEngine;

namespace Housing.BuildLevelAction;

[RequireComponent(typeof(BuildLevel))]
public abstract class BuildLevelAction : MonoBehaviour
{
	protected enum Type
	{
		OnBuild,
		OnNew
	}

	[GetComponent]
	[SerializeField]
	private BuildLevel _buildLevel;

	[SerializeField]
	protected Type _type;

	protected void Initialize()
	{
		_buildLevel = ((Component)this).GetComponent<BuildLevel>();
	}

	private void Awake()
	{
		if (_type == Type.OnBuild)
		{
			_buildLevel.onBuild += Run;
		}
		else if (_type == Type.OnNew)
		{
			_buildLevel.onNew += Run;
		}
	}

	protected abstract void Run();
}
