using System;
using System.Collections.Generic;

public delegate Point GetPosition(double time, double Period, List<Point> Points, int i, double k);
public class ProjectileFlight
{
    readonly double initSpeed;
    readonly double angle;
    readonly double weight;
    readonly double g = 9.8;
    readonly int precision = 2;

    public ProjectileFlight(double initSpeed, double angle, double weight)
    {
        this.initSpeed = initSpeed;
        this.angle = angle * Math.PI / 180;
        this.weight = weight;
    }

    public Point GetPosition(double time)
    {
        double x = this.initSpeed * time * Math.Cos(this.angle);
        double y = this.initSpeed * time * Math.Sin(this.angle) - 0.5 * this.g * time * time;

        double xRounded = Math.Round(x, precision, MidpointRounding.AwayFromZero);
        double yRounded = Math.Round(y, precision, MidpointRounding.AwayFromZero);

        return new Point(xRounded, yRounded);
    }

    public Point GetPosition(double time, double Period, List<Point> Points, int i, double k)
    {
        if (time == 0) return new Point(0, 0, initSpeed * Math.Cos(angle), initSpeed * Math.Sin(angle));

        double dt = Period;

        double x = Points[i - 1].X + dt * Points[i - 1].Vx;
        double y = Points[i - 1].Y + dt * Points[i - 1].Vy;

        double Vx = Points[i - 1].Vx - dt * k * Points[i - 1].Vx / weight;
        double Vy = Points[i - 1].Vy - dt * (g + k * Points[i - 1].Vy / weight);

        double xRounded = Math.Round(x, precision, MidpointRounding.AwayFromZero);
        double yRounded = Math.Round(y, precision, MidpointRounding.AwayFromZero);
        double VxRounded = Math.Round(Vx, precision, MidpointRounding.AwayFromZero);
        double VyRounded = Math.Round(Vy, precision, MidpointRounding.AwayFromZero);

        return new Point(xRounded, yRounded, VxRounded, VyRounded);
    }
}