using Godot;
using System;

public class HUD : CanvasLayer
{
    [Signal]
    public delegate void StartGame();

    public void ShowMessage(string text)
    {
        var messageTimer = (Timer) GetNode("MessageTimer");
        var messageLabel = (Label) GetNode("MessageLabel");

        messageLabel.Text = text;
        messageLabel.Show();
        messageTimer.Start();
    }

    async public void ShowGameOver()
    {
        var startButton = (Button) GetNode("StartButton");
        var messageTimer = (Timer) GetNode("MessageTimer");
        var messageLabel = (Label) GetNode("MessageLabel");

        ShowMessage("Game Over");
        await ToSignal(messageTimer, "timeout");
        Notice("");
        messageLabel.Text = "Dodge the\nCreeps!";
        messageLabel.Show();
        startButton.Show();
    }

    public void Notice(string text)
    {
        var noticeLabel = (Label) GetNode("NoticeLabel");
        noticeLabel.Text = text;
    }

    public void UpdateScore(int score)
    {
        var scoreLabel = (Label) GetNode("ScoreLabel");
        scoreLabel.Text = score.ToString();
    }

    public void UpdateTotalScore(int score)
    {
        var totalScore = (Label) GetNode("TotalScoreLabel");
        totalScore.Text = score.ToString();
    }
    public void OnStartButtonPressed()
    {
        var startButton = (Button) GetNode("StartButton");
        startButton.Hide();

        EmitSignal("StartGame");
    }

    public void OnMessageTimerTimeout()
    {
        var messageLabel = (Label) GetNode("MessageLabel");
        messageLabel.Hide();
    }
}
