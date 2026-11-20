using Godot;
using System;
using System.Collections.Generic;

public partial class Cardbox : Node2D
{
	public string[] CardKeys { get; set; } = Array.Empty<string>();
	private string ChosenCard;
	public string cardType;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
	private void onCard1Pressed(){
		ChosenCard = CardKeys[0];
		cardPressed(ChosenCard);
	}
	private void onCard2Pressed(){
		ChosenCard = CardKeys[1];
		cardPressed(ChosenCard);
	}
	private void onCard3Pressed(){
		ChosenCard = CardKeys[2];
		cardPressed(ChosenCard);
	}
	private void cardPressed(string card)
	{
		var cardData = Cards.cards[card];
		var cardScene = GD.Load<PackedScene>("res://Cards/card.tscn");
		var preview = cardScene.Instantiate<Node2D>();
		
		preview.GetNode<Sprite2D>("Sprite2D").Texture = GD.Load<Texture2D>("res://Assets/Cards/" + cardData["image"]);
		preview.GetNode<Label>("Sprite2D/Label").Text = cardData["desc short"];

		var ui = GetParent<Ui>();
		preview.Name = "CardPreview";
		preview.GlobalPosition = GetViewport().GetMousePosition();
		ui.AddChild(preview);
		ui.MoveChild(preview, 0);
		ui.cardPreview = preview;
		ui.GetParent<GameScene>().SelectCard(card);

		QueueFree();
	}
}
