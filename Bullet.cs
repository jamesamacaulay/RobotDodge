using System;
using SplashKitSDK;

public class Bullet
{
    private Player player;
    public Color clr = Color.Red;   
    public Point2D Position { get; private set; }
    private Vector2D _unitVelocity;
    public Vector2D Velocity { get; private set;}
    private const double SPEED = 15.0;
    private const double BULLET_RADIUS = 5.0;
    
    public Bullet(Player p, Point2D target)
    {
        player = p;
        Position = SplashKit.PointAt(player.X + (player.Width / 2), player.Y + (player.Height / 2));

        GetUnitVector(Position, target);
        Velocity = SplashKit.VectorMultiply(_unitVelocity, SPEED);

        Console.WriteLine($"Bullet: {Position.X}, {Position.Y}, fired at {target.X}, {target.Y}, unit vector: {_unitVelocity.X}, {_unitVelocity.Y}, speed: {SPEED}");
    }
    
    private void GetUnitVector(Point2D origin, Point2D target)
    {
        Vector2D v = SplashKit.VectorPointToPoint(origin, target);
        _unitVelocity = SplashKit.UnitVector(v);
    }
    public void Draw()
    {
        SplashKit.FillCircle(clr, Position.X, Position.Y, BULLET_RADIUS);
    }

    public void Update()
    {
        Position = SplashKit.PointOffsetBy(Position, Velocity);
    }
    public bool IsOutOfBounds(Window w)
    {
        return (Position.X < -BULLET_RADIUS || Position.X > w.Width || Position.Y < -BULLET_RADIUS || Position.Y > w.Height);
    }
    public bool CollidedWith(Robot r)
    {
        return ((Position.X > r.X) && (Position.X < r.X + r.Width) && (Position.Y > r.Y) && (Position.Y < r.Y + r.Height));
    }
}