using Godot;

public partial class Enemy : PathFollow2D
{
    [Signal]
    public delegate void BaseDamageEventHandler(int damage);
    protected virtual int StartingHealth => 50;
    protected virtual int StartingDamage => 21;
    protected virtual int StartingSpeed => 100;
    protected virtual int StartingCash => 50;
    protected int health;
    protected int damage;
    protected int speed;
    protected int grantCash;

    public override void _Ready()
    {
        health = StartingHealth;
        damage = StartingDamage;
        speed = StartingSpeed;
        grantCash = StartingCash;

        var healthBar = GetNode<TextureProgressBar>("HealthBar");
        healthBar.TopLevel = true;
        if (healthBar != null)
        {
            healthBar.MaxValue = StartingHealth;
            healthBar.Value = health;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if(ProgressRatio == 1.0f){
            EmitSignal(SignalName.BaseDamage, damage);
            QueueFree();
        }
        move(delta);
    }

    public virtual void OnHit(int damage)
    {
        if (GlobalPosition.X > 0)
        {
            health -= damage;

            var healthBar = GetNode<TextureProgressBar>("HealthBar");
            if (healthBar != null)
            {
                healthBar.Value = health;
            }
        }
        if (health <= 0)
        {
            OnDestroy();
        }
    }
    public virtual void OnDestroy()
    {
        var gameScene = GetNode<GameScene>("../../..");
        gameScene.changeCash(grantCash);
        QueueFree();
    } 
    private void move(double delta)
	{
		Progress += speed * (float)delta;
		GetNode<TextureProgressBar>("HealthBar").Position = Position;
	} 
}
