using System;
using SplashKitSDK;

public class Program
{
    public static void Main()
    {
        Window w = new Window("Robot Dodge", 800, 600);
        Player p = new Player(w);
        RobotDodge r = new RobotDodge(w, p);

        do {
            SplashKit.ProcessEvents();
            
            r.HandleInput();
            r.Update();
            
            r.Draw();
        } while ((!p.Quit) && (r.IsAlive));
        
    }
}
