using System;
using System.Collections.Generic;
using System.IO;
public class Motion
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

            Point p = cb(time, period, Points, i, k);

            if (p.Y < 0) break;

            Points.Add(p);

            time += period;
        }
    }

    public void Write(string path)
    {
        StreamWriter sw = File.CreateText(path);

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

    public string GetPoint(int i)
    {
        Point p = Points[i];

        //нужно проверить существует ли точка
        return $"{p.X} {p.Y}";
    }
}

