using System;
using SplashKitSDK;

public class Robot
{
    public double X;
    public double Y;
    public Color MainColor;
    private Vector2D Velocity { get; set;}
    private Player player;
    public Circle CollisionCircle
    {
        get
        {
            return SplashKit.CircleAt(X + (Width / 2), Y + (Height / 2), 20);
        }
    }

    public int Width
    {
        get {
            return 50;
        }
    }

    public int Height
    {
        get {
            return 50;
        }
    }

    public Robot(Window gameWindow ,Player p)
    {
        player = p;
        MainColor = SplashKit.RandomRGBColor(150);
        int SPEED = (int)(SplashKit.Rnd() * 3 + 1);

        // left/right
        if(SplashKit.Rnd() < 0.5)
        {
            X = SplashKit.Rnd(gameWindow.Width);

            if(SplashKit.Rnd() < 0.5)
            {
                Y = -Height;
            }
            else
            {
                Y = gameWindow.Height;
            }
        }
        else
        {
            Y = SplashKit.Rnd(gameWindow.Height);

            if(SplashKit.Rnd() < 0.5)
            {
                X = -Width;
            }
            else
            {
                X = gameWindow.Width;
            }
        }
        // top/bottom
        

        Point2D fromPt = new Point2D()
        {
            X = X,
            Y = Y
        };
        Point2D toPt = new Point2D()
        {
            X = player.X,
            Y = player.Y
        };

        Vector2D dir;
        dir = SplashKit.UnitVector(SplashKit.VectorPointToPoint(fromPt, toPt));
        Velocity = SplashKit.VectorMultiply(dir, SPEED);
    }

    public void Draw()
    {
        double leftX = X + 12;
        double rightX = X + 27;
        double eyeY = Y + 10;
        double mouthY = Y + 30;

        SplashKit.FillRectangle(Color.SlateGray, X, Y, Width, Height);
        SplashKit.FillRectangle(MainColor, leftX, eyeY, 10, 10);
        SplashKit.FillRectangle(MainColor, rightX, eyeY, 10, 10);
        SplashKit.FillRectangle(MainColor, leftX, mouthY, 25, 10);
        SplashKit.FillRectangle(Color.Black, leftX + 2, mouthY + 2, 21, 6);

        //SplashKit.DrawCircle(Color.Black, CollisionCircle);
    }
    public void Update()
    {
        X += Velocity.X;
        Y += Velocity.Y;
    }
    public bool IsOutOfBounds(Window w)
    {
        return (X < -Width || X > w.Width || Y < -Height || Y > w.Height);
    }

}