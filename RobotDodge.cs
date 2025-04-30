using System;
using SplashKitSDK;

public class RobotDodge
{
    private Player _Player;
    private Window _GameWindow;
    private List<Robot> Robots;
    private List<Bullet> Bullets;
    private int _Health;
    private const int MAX_HP = 5;
    private int _Score;
    private Font _Font;
    private SplashKitSDK.Timer gameTimer;

    public bool Quit
    {
        get
        {
            return _Player.Quit;
        }
    }

    public bool IsAlive
    {
        get
        {
            return _Health > 0;
        }
    }

    public RobotDodge(Window w, Player p)
    {
        _GameWindow = w;
        _Player = p;
        Robots = new List<Robot>();
        Bullets = new List<Bullet>();
        
        _Player.SetBullets(Bullets);
        
        Robots.Add(RandomRobot(_GameWindow));
        Robots.Add(RandomRobot(_GameWindow));
        Robots.Add(RandomRobot(_GameWindow));

        _Health = MAX_HP;
        _Score = 0;
        _Font = SplashKit.LoadFont("Roboto", "Roboto-Bold.ttf");

        StartTimer();
    }
    private void StartTimer()
    {
        gameTimer = SplashKit.CreateTimer("gameTime");
        SplashKit.StartTimer(gameTimer);
    }
    public void HandleInput()
    {
        _Player.HandleInput();
        _Player.StayOnWindow(_GameWindow);
    }

    public void Draw()
    {
        _GameWindow.Clear(Color.White);
        _Player.Draw();
        foreach(Robot r in Robots)
        {
            r.Draw();
        }
        foreach(Bullet b in Bullets)
        {
            b.Draw();
        }
        Update();
        DrawUI(_Health, _Score);
        _GameWindow.Refresh(60);
    }

    public Robot RandomRobot(Window gameWindow)
    {
        return new Robot(gameWindow, _Player);
    }
    public void Update()
    {
        for(int i = 0; i < Bullets.Count; i++)
        {
            Bullet b = Bullets[i];
            UpdateBullet(b);
            CheckBulletCollisions(b, i);
        }
        for(int i = 0; i < Robots.Count; i++)
        {
            Robot r = Robots[i];
            UpdateRobot(r);
            CheckCollisions(r, i);
        }
        
        _Score = (int)SplashKit.TimerTicks(gameTimer) / 1000;
    }
    private void UpdateRobot(Robot r)
    {
        r.Update();
    }
    private void UpdateBullet(Bullet b)
    {
        b.Update();
    }
    private void CheckBulletCollisions(Bullet b, int index)
    {
        for(int i = 0; i < Robots.Count; i++)
        {
            Robot r = Robots[i];
            if(b.CollidedWith(r))
            {
                Robots.RemoveAt(i);
                Bullets.RemoveAt(index);
                Robots.Add(RandomRobot(_GameWindow));                
                break;
            }
        }
        if(b.IsOutOfBounds(_GameWindow))
        {
            Bullets.RemoveAt(index);
        }
    }
    private void CheckCollisions(Robot r, int index)
    {
        if(_Player.CollidedWith(r))
        {
            _Health--;
            Robots.RemoveAt(index);
            Robots.Add(RandomRobot(_GameWindow));
        }
        if(r.IsOutOfBounds(_GameWindow))
        {
            Robots.RemoveAt(index);
            Robots.Add(RandomRobot(_GameWindow));
        }
    }

    public void DrawUI(int H, int score)
    {
        double healthBarWidth = 200;
        double healthBarHeight = 20;
        double healthBarX = (_GameWindow.Width - healthBarWidth) / 2;
        double healthBarY = _GameWindow.Height - healthBarHeight - 10;
        double scoreOffset = 10;
        int fontSize = 14;
        SplashKit.FillRectangle(Color.Red, healthBarX, healthBarY, healthBarWidth, healthBarHeight);
        SplashKit.FillRectangle(Color.Lime, healthBarX, healthBarY, (healthBarWidth * H) / MAX_HP, healthBarHeight);

        string scoreText = $"Score: {score}";
        
        SplashKit.DrawText(scoreText, Color.Black, "Roboto", fontSize, 
            healthBarX + healthBarWidth + scoreOffset,
            healthBarY + ((healthBarHeight - SplashKit.TextHeight(scoreText,"Roboto",fontSize)) / 2));
    }
}
