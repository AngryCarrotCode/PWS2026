using Godot;
using System;

public partial class Ui : CanvasLayer
{

	int towerRange = 350;
	private TextureProgressBar hpBar;
	private GameData gameData;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;
		gameData = GetNode<GameData>("/root/GameData");
		hpBar = GetNode<TextureProgressBar>("HUD/InfoBar/H/HP");
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void setTowerPreview(string towerType, Vector2 mousePosition)
	{
		var dragTower = GD.Load<PackedScene>("res://Scenes/Towers/" + towerType + ".tscn").Instantiate<Node2D>();
		dragTower.Name = "DragTower";
		dragTower.Modulate = new Color("54ff3caa");

        var control = new Control
        {
            Position = mousePosition,
            Name = "TowerPreview"
        };
		control.AddChild(dragTower, true);
		AddChild(control, true);
		MoveChild(control, 0);

		var rangeTexture = new Sprite2D();
		rangeTexture.Name = "Sprite";
		rangeTexture.Position = new Vector2(0,0);
		var scaling = Convert.ToSingle(gameData.towerData[towerType]["range"]) / 600.0f;
		rangeTexture.Scale = new Vector2(scaling, scaling);
		var texture = GD.Load<Texture2D>("res://Assets/UI/range_overlay.png");
		rangeTexture.Texture = texture;
		rangeTexture.Modulate = new Color("54ff3caa");
		control.AddChild(rangeTexture);
	}

	public void updateTowerPreview(Vector2 tilePosition, string color)
	{
		var preview = GetNode<Control>("TowerPreview");
		preview.Position = tilePosition;
		var dragTower = preview.GetNode<Node2D>("DragTower");
		var rangeTexture = preview.GetNode<Sprite2D>("Sprite");
		var newColor = new Color(color);
		if (dragTower.Modulate != newColor)
		{
			dragTower.Modulate = newColor;
			rangeTexture.Modulate = newColor;
		}
	}

	//
	// Game Control Functions
	//
	public void OnPausePlayPressed()
{
    GetTree().Paused = !GetTree().Paused;
		var gameScene = GetParent<GameScene>();
		if (gameScene.currentWave == 0)
		{
			gameScene.startWave();
		}
		if (GetParent<GameScene>().buildMode){
			GetParent<GameScene>().cancelBuildMode();
		}
		var buildBar = GetNode<Control>("HUD/BuildBar");
		buildBar.ProcessMode = GetTree().Paused
			? ProcessModeEnum.Pausable
			: ProcessModeEnum.Always;
		if (gameScene.currentWave == 0)
		{
			buildBar.ProcessMode = ProcessModeEnum.Always;
		}
}
	public void onSpeedUpPressed()
	{
		if (Engine.TimeScale == 2.0f){
			Engine.TimeScale = 1.0f;
		}else{
			Engine.TimeScale = 2.0f;
		}
	}
	public void UpdateHealthBar(int baseHealth){
		var hpBarTween = CreateTween();
		hpBarTween.TweenProperty(hpBar, "value", baseHealth, 0.1f);
		if (baseHealth >= 60){
			hpBar.TintProgress = new Color("4eff15");
		}else if(baseHealth <= 60 && baseHealth >=25){
			hpBar.TintProgress = new Color("e1be32");
		}else{
			hpBar.TintProgress = new Color("e11e1e");
		}
	}
}
