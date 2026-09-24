using Godot;
using System;

public partial class SceneHandler : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		loadMainMenu();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	private void OnNewGamePressed()
	{
		GetNode<Control>("MainMenu").QueueFree();
		// 1. Load the PackedScene resource
		PackedScene gameSceneResource = GD.Load<PackedScene>("res://Scenes/MainScenes/GameScene.tscn");

		// 2. Instantiate the scene and cast it to your expected node type (e.g., Node2D, Control, Node)
		Node2D gameScene = gameSceneResource.Instantiate<Node2D>();

		gameScene.Connect("gameFinished", Callable.From<bool>(UnloadGame));
		// 3. Add it to the tree
		AddChild(gameScene);
	}
	private void OnQuitGamePressed()
	{
		GetTree().Quit();
	}
	private void UnloadGame(bool result)
	{
		GetNode<Node2D>("GameScene").QueueFree();
		var mainMenu = GD.Load<PackedScene>("res://Scenes/MainScenes/main_menu.tscn").Instantiate<Control>();
		AddChild(mainMenu);
		loadMainMenu();
	}
	private void loadMainMenu()
	{
		GetNode<Button>("MainMenu/M/VB/NewGame").Pressed += OnNewGamePressed;
		GetNode<Button>("MainMenu/M/VB/QuitGame").Pressed += OnQuitGamePressed;
	}
}
