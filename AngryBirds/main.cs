using System;
using System.Collections.Generic;

delegate Point GetPosition(double time);

class Point
{
    public double X;
    public double Y;
    public Point(double x, double y)
    {
        this.X = x;
        this.Y = y;
    }
}

class ProjectileFlight
{
    readonly double initSpeed;
    readonly double angle;
    readonly double g = 9.8;

    public ProjectileFlight(double initSpeed, double angle)
    {
        this.initSpeed = initSpeed;
        this.angle = angle * Math.PI / 180;
    }
  
    public Point GetPosition(double time)
    {
        double x = this.initSpeed * time * Math.Cos(this.angle);
        double y = this.initSpeed * time * Math.Sin(this.angle) - 0.5 * this.g * time * time;

        int precision = 2;

        double xRounded = Math.Round(x, precision, MidpointRounding.AwayFromZero);
        double yRounded = Math.Round(y, precision, MidpointRounding.AwayFromZero);

        return new Point(xRounded, yRounded);
    }
}

class Motion
{
    public List<Point> Points = new List<Point>();
    double period = 1;

    public Motion(double Period)
    {
        this.period = Period;
    }

    public void calculate(GetPosition cb)
    {
        double time = 0;
        
        for (;;) 
        {
          Point p = cb(time); 
          
          if (p.Y < 0) break;

          Points.Add(p);

          time += period;
        }
    }
}

class Program
{
  static void Main(string[] args)
  {
      double FPS = 60;
      double period = 1 / FPS;
      double initSpeed = 31;
      double angle = 60;
    
      ProjectileFlight pf = new ProjectileFlight(initSpeed, angle);
    
      GetPosition getP = new GetPosition(pf.GetPosition);
    
      Motion m = new Motion(period);

      m.calculate(getP);

      Console.WriteLine("x  y");
    
      foreach(Point p in m.Points)
      {
        Console.WriteLine($"{p.X} {p.Y}");
      }
  }
}
