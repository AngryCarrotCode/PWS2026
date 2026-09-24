using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class Towers : Node2D
{
	// Called when the node enters the scene tree for the first time.

	private List<Enemy> enemyArray = new List<Enemy>();
	public Enemy enemy;
	public bool built = false;
	public bool ready = true;
	public string towerType;
	public string category;
	private GameData gameData;


	public override void _Ready()
    {
        if(built)
        {
			((CircleShape2D)GetNode<CollisionShape2D>("Range/CollisionShape2D").Shape).Radius = 0.5f * Convert.ToSingle(GetNode<GameData>("/root/GameData").towerData[towerType]["range"]);
        }
        gameData = GetNode<GameData>("/root/GameData");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
    public override void _PhysicsProcess(double delta)
    {
		if(enemyArray.Count != 0 && built){
			selectEnemy();
        	Turn();
			if (ready && enemy.GlobalPosition.X > 0){
				_ = fire();
			}
		}else{
			enemy = null;
		}
    }
	public void Turn()
	{
		var enemyPosition = enemy.Position;
		GetNode<Sprite2D>("turret").LookAt(enemyPosition);
	}

	public void onRangeBodyEntered(Node2D body)
	{
		enemyArray.Add(body.GetParent<Enemy>());
	}
	public void onRangeBodyExited(Node2D body)
	{
		enemyArray.Remove(body.GetParent<Enemy>());
	}
	private void selectEnemy()
	{
		List<float> enemyProgressArray = new List<float>(); // []
		foreach (Enemy enemy in enemyArray) // for enemy in enemyArray
		{
			enemyProgressArray.Add(enemy.Progress); // [20, 353, 512]
		}
		var maxOffset = enemyProgressArray.Max(); // 512
		var enemyIndex = enemyProgressArray.IndexOf(maxOffset); // 2
		enemy = enemyArray[enemyIndex]; // enemy = enemyArray[2] = 
	}
	private async Task fire()
	{
		ready = false;
		switch (category)
		{
			case "gun":
				fireGun();
				break;
			case "missile":
				fireMissile();
				break;
		}
		enemy.OnHit(Convert.ToInt32(gameData.towerData[towerType]["damage"]));
		await ToSignal(GetTree().CreateTimer(Convert.ToDouble(gameData.towerData[towerType]["fireRate"])), SceneTreeTimer.SignalName.Timeout);
		ready = true;
	}

	private void fireGun()
	{
		GetNode<AnimationPlayer>("AnimationPlayer").Play("Fire");
	}

	private void fireMissile()
	{

	}
}
