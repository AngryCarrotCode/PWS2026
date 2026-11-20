using Godot;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public partial class Ui : CanvasLayer
{

	int towerRange = 350;
	private TextureProgressBar hpBar;
	private GameData gameData;
	public Node2D cardPreview;
	private Cardbox cardBoxPreview;
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
		if (GodotObject.IsInstanceValid(cardPreview))
		{
			cardPreview.GlobalPosition = GetViewport().GetMousePosition();
		}
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

	public void onQuitPressed()
	{
		GetTree().Quit();
	}
		
	public void onCardPressed()
	{
		if (!GetParent<GameScene>().CardPreviewActive)
		{
			if (GetParent<GameScene>().changeCash(GameScene.cardCost))
			{
				if (GodotObject.IsInstanceValid(cardBoxPreview))
				{
					cardBoxPreview.QueueFree();
				}
				var cardBox = GD.Load<PackedScene>("res://Scenes/Cards/cardbox.tscn");
				cardBoxPreview = cardBox.Instantiate<Cardbox>();

				var keys = new List<string>(Cards.cards.Keys);
				var card1 = keys[Random.Shared.Next(keys.Count)];
				var card2 = keys[Random.Shared.Next(keys.Count)];
				var card3 = keys[Random.Shared.Next(keys.Count)];
				while (card2 == card1)
				{
					card2 = keys[Random.Shared.Next(keys.Count)];
				}
				while (card3 == card1 || card3 == card2)
				{
					card3 = keys[Random.Shared.Next(keys.Count)];
				}
				cardBoxPreview.CardKeys = new[] { card1, card2, card3 };
				cardBoxPreview.GetNode<TextureButton>("CardBox/Card1").TextureNormal = GD.Load<Texture2D>("res://Assets/Cards/" + Cards.cards[card1]["image"]);
				cardBoxPreview.GetNode<TextureButton>("CardBox/Card2").TextureNormal = GD.Load<Texture2D>("res://Assets/Cards/" + Cards.cards[card2]["image"]);
				cardBoxPreview.GetNode<TextureButton>("CardBox/Card3").TextureNormal = GD.Load<Texture2D>("res://Assets/Cards/" + Cards.cards[card3]["image"]);
				cardBoxPreview.GetNode<Label>("CardBox/Card1/Description").Text = Cards.cards[card1]["description"];
				cardBoxPreview.GetNode<Label>("CardBox/Card2/Description").Text = Cards.cards[card2]["description"];
				cardBoxPreview.GetNode<Label>("CardBox/Card3/Description").Text = Cards.cards[card3]["description"];
				cardBoxPreview.Name = "CardBoxPreview";
				var viewportSize = GetViewport().GetVisibleRect().Size;
				cardBoxPreview.GlobalPosition = (viewportSize - new Vector2(600, 500)) / 2;
				AddChild(cardBoxPreview);
				MoveChild(cardBoxPreview, 0);
				GetParent<GameScene>().startCardBoxPreview();
			}
		}
	}
	/*public void onCardPressed()
	{
		if (GodotObject.IsInstanceValid(cardPreview))
			cardPreview.QueueFree();

		var cardScene = GD.Load<PackedScene>("res://Cards/KH.tscn");
		cardPreview = cardScene.Instantiate<Node2D>();
		cardPreview.Name = "CardPreview";
		cardPreview.GlobalPosition = GetViewport().GetMousePosition();
		AddChild(cardPreview);
		MoveChild(cardPreview, 0);
		GetParent<GameScene>().startCardPreview();
	}*/
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

	public void updateCardPreview(Vector2 tilePosition, string color)
	{
		if (!GodotObject.IsInstanceValid(cardPreview)) return;
		cardPreview.GlobalPosition = GetViewport().GetMousePosition();
		cardPreview.Modulate = new Color(color);
	}

	public void clearCardPreview()
	{
		if (GodotObject.IsInstanceValid(cardPreview))
			cardPreview.QueueFree();
		cardPreview = null;
	}
}
