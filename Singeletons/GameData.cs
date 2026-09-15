using Godot;
using System;
using System.Collections.Generic;

public partial class GameData : Node
{
	public Dictionary<string, Dictionary<string, object>> towerData = new Dictionary<string, Dictionary<string, object>>()
	{
		{ "GunT1", new Dictionary<string, object>()
			{
				{ "damage", 20 },
				{ "range", 350 },
				{ "fireRate", 1.0f }
			}
		},
		{ "GunT2", new Dictionary<string, object>()
			{
				{ "damage", 25 },
				{ "range", 400 },
				{ "fireRate", 0.8f }
			}
		},
		{ "MissileT1", new Dictionary<string, object>()
			{
				{ "damage", 100 },
				{ "range", 550 },
				{ "fireRate", 3f }
			}
		}
	};
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
