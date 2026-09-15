using Godot;
using System;

public partial class Towers : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
    public override void _PhysicsProcess(double delta)
    {
        Turn();
    }
	public void Turn()
	{
		var enemyPosition = GetGlobalMousePosition();
		GetNode<Sprite2D>("turret").LookAt(enemyPosition);
	}
}
