using System;
using System.Dynamic;
using SplashKitSDK;

public class Player {
    private Bitmap _PlayerBitmap;
    public double X { get; private set; }
    public double Y { get; private set; }
    public bool Quit { get; private set; }
    private const double MAXSPEED = 5.0;
    private const double ACC_RATE = 0.3;
    private const double DEC_RATE = 0.4;
    private const int GAP = 5;
    private double accX = 0.0;
    private double accY = 0.0;
    private List<Bullet> Bullets;

    
    public int Width
    {
        get {return _PlayerBitmap.Width; }
    }

    public int Height
    {
        get {return _PlayerBitmap.Height; }
    }
    public Player(Window gameWindow)
    {
        _PlayerBitmap = new Bitmap("Player", "Resources/images/Player.png");
        X = (gameWindow.Width - Width) / 2;
        Y = (gameWindow.Height - Height) / 2;
        Quit = false;
    }

    public void Draw()
    {
        _PlayerBitmap.Draw(X, Y);
    }

    public void SetBullets(List<Bullet> b)
    {
        Bullets = b;
    }

    public void HandleInput()
    {
        if(SplashKit.MouseClicked(MouseButton.LeftButton))
        {
            // timer logic
            // if allowed to fire, create a bullet
            Bullet b = new Bullet(this, SplashKit.MousePosition());
            Bullets.Add(b);
        }

        bool isAccX = false;
        bool isAccY = false;

        if(SplashKit.KeyDown(KeyCode.EscapeKey))
        {
            Quit = true;
        }
        //deliberately written as independent if..then statements to allow for
        // simultaneous keypress in multiple directions
        if(SplashKit.KeyDown(KeyCode.LeftKey))
        {
            isAccX = true;
            // reset acceleration if player changes directions on an axis
            // player was moving right and now presses left, set accX to 0
            if(accX > 0)
            {
                accX = 0;
            }
            // if not changing direction, speed up to -MAXSPEED
            else if(accX > (-1.0 * MAXSPEED))
            {
                accX -= ACC_RATE;
            }
            // maintain full speed
            else{
                accX = (-1.0 * MAXSPEED);
            }
        }
        if(SplashKit.KeyDown(KeyCode.RightKey))
        {
            isAccX = true;
            // reset acceleration if player changes directions on an axis
            // player was moving right and now presses left, set accX to 0
            if(accX < 0)
            {
                accX = 0;
            }
            // if not changing direction, speed up to +MAXSPEED
            else if(accX < (1.0 * MAXSPEED))
            {
                accX += ACC_RATE;
            }
            // maintain full speed
            else{
                accX = (+1.0 * MAXSPEED);
            }
        }
        if(SplashKit.KeyDown(KeyCode.UpKey))
        {
            isAccY = true;
            // reset acceleration if player changes directions on an axis
            // player was moving right and now presses left, set accX to 0
            if(accY > 0)
            {
                accY = 0;
            }
            // if not changing direction, speed up to -MAXSPEED
            else if(accY > (-1.0 * MAXSPEED))
            {
                accY -= ACC_RATE;
            }
            // maintain full speed
            else{
                accY = (-1.0 * MAXSPEED);
            }
        }
        if(SplashKit.KeyDown(KeyCode.DownKey))
        {
            isAccY = true;
            // reset acceleration if player changes directions on an axis
            // player was moving right and now presses left, set accX to 0
            if(accY < 0)
            {
                accY = 0;
            }
            // if not changing direction, speed up to +MAXSPEED
            else if(accY < (1.0 * MAXSPEED))
            {
                accY += ACC_RATE;
            }
            // maintain full speed
            else{
                accY = (+1.0 * MAXSPEED);
            }
        }
        // slow down when key is released
        if(!isAccX)
        {
            // if still moving but below the deceleration rate, stop. Prevents slow drift
            if(Math.Abs(accX) < DEC_RATE)
            {
                accX = 0;
            }
            // if moving to the right and slowing down and x speed is above the deceleration rate
            else if((accX < 0) && (accX < DEC_RATE))
            {
                accX += DEC_RATE;
            }
            // if moving to the left and slowing down and x speed is above the deceleration rate
            else if((accX > 0) && (accX > (-1.0 * DEC_RATE)))
            {
                accX -= DEC_RATE;
            }
        }
        if(!isAccY)
        {
            // if still moving but below the deceleration rate, stop. Prevents slow drift
            if(Math.Abs(accY) < DEC_RATE)
            {
                accY = 0;
            }
            // if moving to the right and slowing down and x speed is above the deceleration rate
            else if((accY < 0) && (accY < DEC_RATE))
            {
                accY += DEC_RATE;
            }
            // if moving to the left and slowing down and x speed is above the deceleration rate
            else if((accY > 0) && (accY > (-1.0 * DEC_RATE)))
            {
                accY -= DEC_RATE;
            }
        }

        X += accX;
        Y += accY;
    }

    public void StayOnWindow(Window limit)
    {
        if(X < GAP)
        {
            X = GAP;
        }
        // window width less player width and gap to keep bitmap fully on screen
        else if(X > limit.Width - Width - GAP)
        {
            X = limit.Width - Width - GAP;
        }

        if(Y < GAP)
        {
            Y = GAP;
        }
        // window height less player height and gap to keep bitmap fully on screen
        else if(Y > limit.Height - Height - GAP)
        {
            Y = limit.Height - Height - GAP;
        }
    }

    public bool CollidedWith(Robot other)
    {
        return _PlayerBitmap.CircleCollision(X, Y, other.CollisionCircle);
    }
}