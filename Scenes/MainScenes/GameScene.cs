using Godot;
using System;
using System.Collections.Generic;
using System.Net.Quic;
using System.Threading.Tasks;

public partial class GameScene : Node2D
{
	[Signal]
	public delegate void gameFinishedEventHandler(bool result);
	private Node2D mapNode;

	public bool buildMode = false;
	private bool buildValid = false;
	private bool cardPreviewActive = false;
	public bool CardPreviewActive => cardPreviewActive;
	private Vector2 buildLocation;
	private string buildType;
	private Towers hoveredTower = null;

	private Vector2I buildTile;
	public int currentWave;
	private int enemiesInWave;
	public int baseHealth = 100;

	public int cash = 100;

	public static int cardCost = -150;
	private bool waveActive = false;
	private int totalEnemiesKilled = 0;
	private int numberOfWaves = 10;
	private int wavesFinished = 0;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		updateLabels();
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

		if (enemiesInWave <= 0 && waveActive)
		{
			onWaveEnd();
		}
	}
	private void updateLabels()
	{
		var gameData = GetNode<GameData>("/root/GameData");
		GetNode<Label>("UI/HUD/InfoBar/H/Money").Text = cash.ToString();
		GetNode<Label>("UI/HUD/InfoBar/H/CardPrice").Text = "$"+(-cardCost).ToString();
		var gunCost = Convert.ToInt32(gameData.towerData["GunT1"]["cost"]);
		var missileCost = Convert.ToInt32(gameData.towerData["MissileT1"]["cost"]);
		GetNode<Label>("UI/HUD/BuildBar/GunPrice").Text = "$"+gunCost.ToString();
		GetNode<Label>("UI/HUD/BuildBar/Missileprice").Text = "$"+missileCost.ToString();
		GetNode<Label>("UI/HUD/InfoBar/H/Wave").Text = "Wave "+(currentWave + 1).ToString();
	}
	//
	// Building Functions
	//

	private void InitiateBuildMode(string towerType)
	{
		if (buildMode || cardPreviewActive)
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
		if (buildValid && changeCash(- cost)) // Check if the player has enough cash
		{
			newTower.Position = buildLocation;
			newTower.category = gameData.towerData[buildType]["category"].ToString();
			newTower.built = true;
			mapNode.AddChild(newTower);
			mapNode.GetNode<TileMapLayer>("TowerExclusion").SetCell(buildTile, 5, Vector2I.Zero); // Mark the tile as occupied
			newTower.GetNode<Sprite2D>("turret/fire").Visible = false;
			
		}
	}

	public void verifyAndCard()
	{
		if (GodotObject.IsInstanceValid(hoveredTower))
		{
			var cardData = Cards.cards[selectedCardKey];
			var cardType = cardData["card type"];
			var sideEffect = cardData["side effect"];
			switch (cardType)
			{
				case "BonusDamagePercent":
					hoveredTower.damageBonusPercent += Convert.ToSingle(cardData["card bonus"]);
					GD.Print(hoveredTower.damageBonusPercent);
					break;
				case "BonusDamage":
					hoveredTower.damageBonus += Convert.ToSingle(cardData["card bonus"]);
					break;
				case "BonusRange":
					hoveredTower.bonusRange += Convert.ToSingle(cardData["card bonus"]);
					hoveredTower.CalculateRange();
					break;
				case "BonusFireRate":
					hoveredTower.fireRateBonus += Convert.ToSingle(cardData["card bonus"]);
					break;
			}
			switch (sideEffect)
			{
				case "none":
					break;
				case "DecreaseDamagePercent":
					hoveredTower.damageBonusPercent -= Convert.ToSingle(cardData["side bonus"]);
					GD.Print(hoveredTower.damageBonusPercent);
					break;
			}
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
		public void startCardBoxPreview()
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
		waveActive = true;
		currentWave += 1;
		var waveData = WaveData.RetrieveWaveData(currentWave);
		enemiesInWave = waveData.Count;
		GD.Print("Enemies: " + enemiesInWave);
		await ToSignal(GetTree().CreateTimer(0.2f, false), SceneTreeTimer.SignalName.Timeout);
		await spawnEnemies(waveData);
	}
	
	private async Task spawnEnemies(List<(string sceneName, float delay)> waveData)
	{
		foreach (var enemyData in waveData)
		{
			var newEnemy = GD.Load<PackedScene>("res://Scenes/Enemies/" + enemyData.sceneName + ".tscn").Instantiate<Enemy>();
			newEnemy.BaseDamage += OnBaseDamage;
			mapNode.GetNode<Path2D>("Path").AddChild(newEnemy);
			await ToSignal(GetTree().CreateTimer(enemyData.delay, false), SceneTreeTimer.SignalName.Timeout);
		}
	}

	public void OnEnemyRemoved()
	{
		totalEnemiesKilled += 1;
		GD.Print("total enemies killed: "+ totalEnemiesKilled);
		enemiesInWave -= 1;
	}

	private async void onWaveEnd()
	{
		wavesFinished += 1;
		waveActive = false;
		await ToSignal(GetTree().CreateTimer(0.1f), SceneTreeTimer.SignalName.Timeout);
		GetTree().Paused = true;
    	GetNode<Button>("UI/HUD/GameControls/PausePlay").ButtonPressed = false;
		updateLabels();
		if(wavesFinished != numberOfWaves){
			startWave();
		} else
		{
			onWin();
		}
	}
	private void onWin()
	{
		var winScreen = GD.Load<PackedScene>("res://WinScreen.tscn").Instantiate<Node2D>();
		winScreen.Name = "WinScreen";
		var ui = GetNode<Ui>("UI");
		var viewportSize = GetViewport().GetVisibleRect().Size;
		winScreen.GlobalPosition = (viewportSize - new Vector2(600, 500)) / 2;
		ui.AddChild(winScreen);
		ui.MoveChild(winScreen, 0);
	}

	private void OnBaseDamage(int damage)
	{
    baseHealth -= damage;
	if(baseHealth <= 0){
		EmitSignal(SignalName.gameFinished, false);
	} else
	{
		GetNode<Ui>("UI").UpdateHealthBar(baseHealth);
	}
	}

	public bool changeCash(int amount)
{
    if (amount < 0 && cash + amount < 0)
    {
        return false;
    }

    cash += amount;
    GetNode<Label>("UI/HUD/InfoBar/H/Money").Text = cash.ToString();
    return true;
}

	private string selectedCardKey;

	public void SelectCard(string cardKey)
	{
		selectedCardKey = cardKey;
	}
}