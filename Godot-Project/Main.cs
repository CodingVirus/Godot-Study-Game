using Godot;
using System;

public class Main : Node
{
    [Export]
    public PackedScene Mob;

    public int Score = 0;
    public int TotalScore = 0;
    public float MobMinSpeed = 200f;
    public float MobMaxSpeed = 250f;

    private Random rand = new Random();

    public override void _Ready()
    {
       // NewGame();
    }

    private float RandRand(float min, float max)
    {
        return (float) (rand.NextDouble() * (max - min) + min);
    }

    public void GameOver()
    {
        var mobTimer = (Timer) GetNode("MobTimer");
        var scoreTimer = (Timer) GetNode("ScoreTimer");
        var GameOverMusic = (AudioStreamPlayer) GetNode("GameOver");
        var Music = (AudioStreamPlayer) GetNode("Music");

        //Mob.EmitSignal(nameof(EraseMob));
        Music.Stop();
        GameOverMusic.Play();
        scoreTimer.Stop();
        mobTimer.Stop();
        
        var hud = (HUD) GetNode("HUD");
        hud.ShowGameOver();
    }

    public void NewGame()
    {
        Score = 0;
        MobMinSpeed = 200f;
        MobMaxSpeed = 250f;

        var Music = (AudioStreamPlayer) GetNode("Music");
        var Player = (Player) GetNode("Player");
        var startTimer = (Timer) GetNode("StartTimer");
        var startPosition = (Position2D) GetNode("StartPosition");
        
        Music.Play();
        Player.Start(startPosition.Position);
        startTimer.Start();

        var hud = (HUD) GetNode("HUD");
        hud.UpdateScore(Score);
        hud.ShowMessage("Get Ready!");
    }

    public void OnMobTimerTimeout()
    {
        var mobSpawnLocation = (PathFollow2D) GetNode("MobPath/MobSpawnLocation");
        mobSpawnLocation.SetOffset(rand.Next());

        // Create a Mob instance and add it to the scene.
        var mobInstance = (RigidBody2D) Mob.Instance();
        AddChild(mobInstance);

        // Set the mob's direction perpendicular to the path direction.
        var direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

        // Set the mob's position to a random location.
        mobInstance.Position = mobSpawnLocation.Position;

        // Add some randomness to the direction.
        direction += RandRand(-Mathf.Pi / 4, Mathf.Pi / 4);
        mobInstance.Rotation = direction;

    // Choose the velocity.
        mobInstance.SetLinearVelocity(new Vector2(RandRand(MobMinSpeed, MobMaxSpeed), 0).Rotated(direction));
    }
    public async void OnScoreTimerTimeout()
    {
        Score += 1;
        var hud = (HUD) GetNode("HUD");
        hud.UpdateScore(Score);

        if (Score >= TotalScore)
        {
            TotalScore = Score;
            hud.UpdateTotalScore(TotalScore);
        }
        else
        {
            hud.UpdateTotalScore(TotalScore);
        }

        if (Score % 10 == 0)
        {
            MobMinSpeed -= 20f;
            MobMaxSpeed += 100f;
            hud.Notice("Level UP!");
            if (MobMinSpeed <= 20f)
                MobMinSpeed = 20f;
        }
        else
        {
            hud.Notice("");
        }
    }
    public void OnStartTimerTimeout()
    {
        var mobTimer = (Timer) GetNode("MobTimer");
        var scoreTimer = (Timer) GetNode("ScoreTimer");

        mobTimer.Start();
        scoreTimer.Start();
    }
}
