using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class GameScene : Node2D
{
	private Node2D mapNode;

	private bool buildMode = false;
	private bool buildValid = false;
	private Vector2 buildLocation;
	private string buildType;

	private Vector2I buildTile;
	private int currentWave;
	private int enemiesInWave;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		mapNode = GetNode<Node2D>("Map 1");
		foreach (Node i in GetTree().GetNodesInGroup("buildButtons"))
		{
			i.Connect("pressed", Callable.From(() => InitiateBuildMode(i.Name)));
		}
		startWave();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (buildMode)
		{
			updateTowerPreview();
		}
	}
	//
	// Building Functions
	//

	private void InitiateBuildMode(string towerType)
	{
		if (buildMode)
		{
			cancelBuildMode();
		}
		buildType = towerType + "T1";
		buildMode = true;
		GetNode("UI").Call("setTowerPreview", buildType, GetGlobalMousePosition());
	}

	private void verifyAndBuild()
	{
		
		if (buildValid) // Check if the player has enough cash (not implemented yet)
		{
			var towerScene = GD.Load<PackedScene>("res://Scenes/Towers/" + buildType + ".tscn");
			var towerInstance = towerScene.Instantiate<Towers>();
			towerInstance.Position = buildLocation;
			mapNode.AddChild(towerInstance);
			mapNode.GetNode<TileMapLayer>("TowerExclusion").SetCell(buildTile, 5, Vector2I.Zero); // Mark the tile as occupied
			//deduct cash from player (not implemented yet)
			// update cash label (not implemented yet)
		}
	}

	private void updateTowerPreview()
	{
		var mousePos = GetGlobalMousePosition();
		var towerExclusion = mapNode.GetNode<TileMapLayer>("TowerExclusion");
		var currentTile = towerExclusion.LocalToMap(towerExclusion.ToLocal(mousePos));
		var tilePos = towerExclusion.ToGlobal(towerExclusion.MapToLocal(currentTile));
	
		if (towerExclusion.GetCellSourceId(currentTile) == -1)
		{
			GetNode<Ui>("UI").updateTowerPreview(tilePos, "54ff3caa");
			buildValid = true;
			buildLocation = tilePos;
			buildTile = currentTile;
		}
		else
		{
			GetNode<Ui>("UI").updateTowerPreview(tilePos, "ff3c3caa");
			buildValid = false;
		}
	}

	public override void _UnhandledInput(InputEvent inputEvent)
	{
		if (inputEvent.IsActionPressed("ui_cancel") && buildMode)
		{
			cancelBuildMode();
		}
		if (inputEvent.IsActionPressed("ui_accept") && buildMode)
		{
			verifyAndBuild();
			cancelBuildMode();
		}
	}
	
	private void cancelBuildMode()
	{
		buildMode = false;
		buildValid = false;
		GetNode<Ui>("UI").GetNode<Control>("TowerPreview").Free();
	}
	//
	// Wave Functions
	//

	private async void startWave()
	{
		var waveData = retrieveWaveData();
		await ToSignal(GetTree().CreateTimer(0.2f), SceneTreeTimer.SignalName.Timeout);
		await spawnEnemies(waveData);

	}
	private List<(string sceneName, float delay)> retrieveWaveData()
	{
		var waveData = new List<(string sceneName, float delay)>
		{
			("blueTank", 0.7f),
			("blueTank", 0.1f),
		};
		currentWave += 1;
		enemiesInWave = waveData.Count;
		return waveData;
	}
	private async Task spawnEnemies(List<(string sceneName, float delay)> waveData)
	{
		foreach (var enemyData in waveData)
		{
			var newEnemy = GD.Load<PackedScene>("res://Scenes/Enemies/" + enemyData.sceneName + ".tscn").Instantiate<BlueTank>();
			mapNode.GetNode<Path2D>("Path").AddChild(newEnemy);
			await ToSignal(GetTree().CreateTimer(enemyData.delay), SceneTreeTimer.SignalName.Timeout);
		}
	}


}
