using Godot;
using System;

public class Player : Area2D
{
    [Signal]
    public delegate void Hit();

    [Export]
    public int Speed = 400;
    public Vector2 ScreenSize;

    public override void _Ready()
    {
        ScreenSize = GetViewport().GetSize();
        Hide();
    }

    public override void _Process(float delta)
    {
        MovePlayer(delta);
    }

    public void Start(Vector2 pos)
    {
        Position = pos;
        Show();
        GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
    }
    public void OnPlayerBodyEntered(PhysicsBody2D body)
    {
        Hide();
        EmitSignal(nameof(Hit));
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
    }

    public void MovePlayer(float delta)
    {
        var velocity = Vector2.Zero;

        if (Input.IsActionPressed("move_right"))
        {
            velocity.x += 1;
        }

        if (Input.IsActionPressed("move_left"))
        {
            velocity.x -= 1;
        }

        if (Input.IsActionPressed("move_up"))
        {
            velocity.y -= 1;
        }

        if (Input.IsActionPressed("move_down"))
        {
            velocity.y += 1;
        }

        var animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");

        if (velocity.Length() > 0)
        {
            velocity = velocity.Normalized() * Speed;
            animatedSprite.Play();
        }
        else
        {
            animatedSprite.Stop();
        }
        
        this.Position += velocity * delta;
        this.Position = new Vector2
        (
            x:Mathf.Clamp(this.Position.x, 0, ScreenSize.x),
            y:Mathf.Clamp(this.Position.y, 0, ScreenSize.y)
        );

        if (velocity.x < 0)
		{
    			animatedSprite.FlipH = true;
		}
		else
		{	
    			animatedSprite.FlipH = false;
		}

		if (velocity.y > 0)
		{
    			animatedSprite.FlipV = true;
		}
		else
		{	
    			animatedSprite.FlipV = false;
		}
    }
}
