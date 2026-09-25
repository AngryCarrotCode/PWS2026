using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class GameScene : Node2D
{
	[Signal]
	public delegate void gameFinishedEventHandler(bool result);
	private Node2D mapNode;

	public bool buildMode = false;
	private bool buildValid = false;
	private bool cardPreviewActive = false;
	private Vector2 buildLocation;
	private string buildType;
	private Towers hoveredTower = null;

	private Vector2I buildTile;
	public int currentWave;
	private int enemiesInWave;
	public int baseHealth = 100;

	public int cash = 100;

	public int cardCost = -50;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;
		mapNode = GetNode<Node2D>("Map 1");
		mapNode.ProcessMode = ProcessModeEnum.Pausable;
		GetNode<Control>("UI/HUD/BuildBar").ProcessMode = ProcessModeEnum.Always;
		foreach (Node i in GetTree().GetNodesInGroup("buildButtons"))
		{
			i.Connect("pressed", Callable.From(() => InitiateBuildMode(i.Name)));
		}
		GetTree().Paused = true;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (buildMode)
		{
			updateTowerPreview();
		}
		if (cardPreviewActive)
		{
			updateCardPreview();
		}
	}
	//
	// Building Functions
	//

	private void InitiateBuildMode(string towerType)
	{
		if (cardPreviewActive)
		{
			cancelCardMode();
		}
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
		var towerScene = GD.Load<PackedScene>("res://Scenes/Towers/" + buildType + ".tscn");
		var newTower = towerScene.Instantiate<Towers>();
		newTower.towerType = buildType;
		var gameData = GetNode<GameData>("/root/GameData");
		var cost = Convert.ToInt32(gameData.towerData[buildType]["cost"]);
		if (buildValid && cash >= cost) // Check if the player has enough cash
		{
			newTower.Position = buildLocation;
			newTower.category = gameData.towerData[buildType]["category"].ToString();
			newTower.built = true;
			mapNode.AddChild(newTower);
			mapNode.GetNode<TileMapLayer>("TowerExclusion").SetCell(buildTile, 5, Vector2I.Zero); // Mark the tile as occupied
			newTower.GetNode<Sprite2D>("turret/fire").Visible = false;
			changeCash(- cost);
		}
	}

	private void verifyAndCard()
	{
		if (GodotObject.IsInstanceValid(hoveredTower) && cash >= cardCost)
		{
			hoveredTower.damageModifier += 20;
			changeCash(cardCost);
		}
	}

	private void updateTowerPreview()
	{
		GetNode<Sprite2D>("UI/TowerPreview/DragTower/turret/fire").Visible = false;
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

	public void updateCardPreview()
	{
		var mousePos = GetGlobalMousePosition();
		var towerExclusion = mapNode.GetNode<TileMapLayer>("TowerExclusion");
		var currentTile = towerExclusion.LocalToMap(towerExclusion.ToLocal(mousePos));
		var tilePos = towerExclusion.ToGlobal(towerExclusion.MapToLocal(currentTile));
		var aboveTurret = false;
		foreach (Node child in mapNode.GetChildren())
		{
			if (child is Towers tower && towerExclusion.LocalToMap(towerExclusion.ToLocal(tower.GlobalPosition)) == currentTile)
			{
				hoveredTower = tower;
				aboveTurret = true;
				break;
			}
			else
			{
				hoveredTower = null;
			}
		}
		GetNode<Ui>("UI").updateCardPreview(tilePos, aboveTurret ? "54ff3caa" : "ff3c3caa");
	}

	public void startCardPreview()
	{
		cardPreviewActive = true;
		if (buildMode)
		{
			cancelBuildMode();
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

		if (inputEvent.IsActionPressed("ui_cancel") && cardPreviewActive)
		{
			cancelCardMode();
		}
		if (inputEvent.IsActionPressed("ui_accept") && cardPreviewActive)
		{
			verifyAndCard();
			cancelCardMode();
		}
	}
	
	public void cancelBuildMode()
	{
		buildMode = false;
		buildValid = false;
		GetNode<Ui>("UI").GetNode<Control>("TowerPreview").Free();
	}

	public void cancelCardMode()
	{
		cardPreviewActive = false;
		GetNode<Ui>("UI").clearCardPreview();
		hoveredTower = null;
	}
	//
	// Wave Functions
	//

	public async void startWave()
	{
		var waveData = retrieveWaveData();
		await ToSignal(GetTree().CreateTimer(0.2f), SceneTreeTimer.SignalName.Timeout);
		await spawnEnemies(waveData);

	}
	private List<(string sceneName, float delay)> retrieveWaveData()
	{
		var waveData = new List<(string sceneName, float delay)>
		{
			("blueTank", 3.0f),
			("blueTank", 0.2f),
			("blueTank", 2.0f),
			("blueTank", 0.2f),
			("blueTank", 0.7f),
			("blueTank", 1.3f),
			("blueTank", 1.0f),
			("blueTank", 0.5f),
			("blueTank", 0.1f)
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
			newEnemy.BaseDamage += OnBaseDamage;
			mapNode.GetNode<Path2D>("Path").AddChild(newEnemy);
			await ToSignal(GetTree().CreateTimer(enemyData.delay), SceneTreeTimer.SignalName.Timeout);
		}
	}

	private void OnBaseDamage(int damage)
{
    baseHealth -= damage;
	if(baseHealth <= 0){
		EmitSignal(SignalName.gameFinished, false);
	}else{
		GetNode<Ui>("UI").UpdateHealthBar(baseHealth);
	}
}

 public void changeCash(int amount)
 {
	 cash += amount;
	 GetNode<Label>("UI/HUD/InfoBar/H/Money").Text = cash.ToString();
	
 }

}
