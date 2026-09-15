using Godot;
using System;

public partial class Ui : CanvasLayer
{

	int towerRange = 350;
	private GameData gameData;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameData = GetNode<GameData>("/root/GameData");
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

}
