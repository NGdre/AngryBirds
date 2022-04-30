﻿using System;
using System.Collections.Generic;
using System.IO;
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
