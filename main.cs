using System;
using System.Collections.Generic;
using System.IO;

delegate Point GetPosition(double time, double Period, List<Point> Points, int i, double m, double k);

class Point
{
    public double X;
    public double Y;
    public double Vx;
    public double Vy;
    public Point(double x, double y)
    {
        this.X = x;
        this.Y = y;
    }
    public Point(double x, double y, double Vx, double Vy)
    {
        this.X = x;
        this.Y = y;
        this.Vx = Vx;
        this.Vy = Vy;
    }
}

class ProjectileFlight
{
    readonly double initSpeed;
    readonly double angle;
    readonly double g = 9.8;
    readonly int precision = 2;

    public ProjectileFlight(double initSpeed, double angle)
    {
        this.initSpeed = initSpeed;
        this.angle = angle * Math.PI / 180;
    }

    public Point GetPosition(double time)
    {
        double x = this.initSpeed * time * Math.Cos(this.angle);
        double y = this.initSpeed * time * Math.Sin(this.angle) - 0.5 * this.g * time * time;

        double xRounded = Math.Round(x, precision, MidpointRounding.AwayFromZero);
        double yRounded = Math.Round(y, precision, MidpointRounding.AwayFromZero);

        return new Point(xRounded, yRounded);
    }

    public Point GetPosition(double time, double Period, List<Point> Points, int i, double m, double k)
    {
        if (time == 0) return new Point(0, 0, initSpeed * Math.Cos(angle), initSpeed * Math.Sin(angle));

        double dt = Period;

        double x = Points[i - 1].X + dt * Points[i - 1].Vx;
        double y = Points[i - 1].Y + dt * Points[i - 1].Vy;

        double Vx = Points[i - 1].Vx - dt * k * Points[i - 1].Vx / m;
        double Vy = Points[i - 1].Vy - dt * (g + k * Points[i - 1].Vy / m);

        double xRounded = Math.Round(x, precision, MidpointRounding.AwayFromZero);
        double yRounded = Math.Round(y, precision, MidpointRounding.AwayFromZero);
        double VxRounded = Math.Round(Vx, precision, MidpointRounding.AwayFromZero);
        double VyRounded = Math.Round(Vy, precision, MidpointRounding.AwayFromZero);

        return new Point(xRounded, yRounded, VxRounded, VyRounded);
    }
}

class Motion
{
    public List<Point> Points = new List<Point>();
    readonly double period = 1;

    public Motion(double Period)
    {
        this.period = Period;
    }

    public void Calculate(GetPosition cb)
    {
        double time = 0;
        double k;

        for (int i = 0; ; i++)
        {
            k = time / 2;
            
            Point p = cb(time, period, Points, i, 50, k);

            if (p.Y < 0) break;

            Points.Add(p);

            time += period;
        }
    }

    public void Write(string path)
    {
        using StreamWriter sw = File.CreateText(path);

        sw.WriteLine("x  y");

        foreach (Point p in Points)
        {
            sw.WriteLine($"{p.X} {p.Y}");
        }
    }

    public void Print()
    {
        if (Points.Count == 0) Console.WriteLine("list of coordinates is empty");

        foreach (Point p in Points)
        {
            Console.WriteLine($"{p.X} {p.Y}");
        }
    }
}


class main
{
   static void Main(string[] args)
    {
        double FPS = 60;
        double period = 1 / FPS;
        double initSpeed = 60;
        double angle = 30;
        string path = "../../../positions.txt";

        ProjectileFlight pf = new ProjectileFlight(initSpeed, angle);

        GetPosition getP = new GetPosition(pf.GetPosition);

        Motion m = new Motion(period);

        m.Calculate(getP);
        m.Print();
        m.Write(path);
    }
}
