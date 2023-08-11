using Characters;
using Services;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TestingTool;

public sealed class PlayerStatElement : MonoBehaviour
{
	[SerializeField]
	private TMP_Text _name;

	[SerializeField]
	private TMP_InputField _percent;

	[SerializeField]
	private TMP_InputField _percentPoint;

	[SerializeField]
	private TMP_InputField _constant;

	[SerializeField]
	private TMP_Text _final;

	private Stat.Kind _kind;

	private Stat.Values _stat;

	public void Set(Stat.Kind kind)
	{
		Character player = Singleton<Service>.Instance.levelManager.player;
		_kind = kind;
		_stat = new Stat.Values(new Stat.Value(Stat.Category.Percent, kind, 1.0), new Stat.Value(Stat.Category.PercentPoint, kind, 0.0), new Stat.Value(Stat.Category.Constant, kind, 0.0));
		_name.text = kind.name;
		_percent.text = "1";
		_percentPoint.text = "0";
		_constant.text = "0";
		switch (kind.valueForm)
		{
		case Stat.Kind.ValueForm.Percent:
			((Selectable)_constant).interactable = false;
			break;
		case Stat.Kind.ValueForm.Product:
			((Selectable)_constant).interactable = false;
			break;
		}
		UpdateFinal();
		player.stat.AttachValues(_stat);
	}

	private void UpdateFinal()
	{
		Character player = Singleton<Service>.Instance.levelManager.player;
		string text = "";
		switch (_kind.valueForm)
		{
		case Stat.Kind.ValueForm.Percent:
			text = $"{player.stat.GetFinal(_kind) * 100.0:0}";
			break;
		case Stat.Kind.ValueForm.Constant:
			text = $"{player.stat.GetFinal(_kind)}";
			break;
		case Stat.Kind.ValueForm.Product:
			text = $"x{player.stat.GetFinal(_kind):0.00}";
			break;
		}
		_final.text = text;
	}

	public void Apply()
	{
		Character player = Singleton<Service>.Instance.levelManager.player;
		double result2;
		double result3;
		if (!double.TryParse(_percent.text, out var result))
		{
			_percent.text = $"{((ReorderableArray<Stat.Value>)_stat).values[0].value:0}";
			_percentPoint.text = $"{((ReorderableArray<Stat.Value>)_stat).values[1].value:0}";
			_constant.text = $"{((ReorderableArray<Stat.Value>)_stat).values[2].value:0}";
		}
		else if (!double.TryParse(_percentPoint.text, out result2))
		{
			_percent.text = $"{((ReorderableArray<Stat.Value>)_stat).values[0].value:0}";
			_percentPoint.text = $"{((ReorderableArray<Stat.Value>)_stat).values[1].value:0}";
			_constant.text = $"{((ReorderableArray<Stat.Value>)_stat).values[2].value:0}";
		}
		else if (!double.TryParse(_constant.text, out result3))
		{
			_percent.text = $"{((ReorderableArray<Stat.Value>)_stat).values[0].value:0}";
			_percentPoint.text = $"{((ReorderableArray<Stat.Value>)_stat).values[1].value:0}";
			_constant.text = $"{((ReorderableArray<Stat.Value>)_stat).values[2].value:0}";
		}
		else
		{
			((ReorderableArray<Stat.Value>)_stat).values[0].value = result;
			((ReorderableArray<Stat.Value>)_stat).values[1].value = result2;
			((ReorderableArray<Stat.Value>)_stat).values[2].value = result3;
			player.stat.Update();
			UpdateFinal();
		}
	}
}
