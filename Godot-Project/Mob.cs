using Godot;
using System;

public class Mob : RigidBody2D
{
    Node parent;

    public override void _Ready()
    {
        parent = GetParent();

        var animSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        animSprite.Playing = true;
        string[] mobTypes = animSprite.Frames.GetAnimationNames();
        animSprite.Animation = mobTypes[GD.Randi() % mobTypes.Length];
    }

    public override void _Process(float delta)
    {
        GD.Print(Name);
        if ((bool)parent.Get("gameState") == false)
        {
            this.QueueFree();
        }
    }
    public void OnVisibilityNotifier2DScreenExited()
    {
        QueueFree();
    }
}
