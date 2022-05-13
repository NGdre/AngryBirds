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

    public async void Write(string path)
    {
        using (StreamWriter outputFile = new StreamWriter(path))
        {
            foreach (Point p in Points)
            {
                await outputFile.WriteLineAsync($"{p.X} {p.Y}");
            }
        }
    }

    public void Read(string path)
    {
        using (StreamReader sr = new StreamReader(path))
        {
            string s;

            while ((s = sr.ReadLine()) != null)
            {
                string[] subs = s.Split();

                if (!double.TryParse(subs[0], out double X))
                {
                    return;
                }

                if (!double.TryParse(subs[1], out double Y))
                {
                    return;
                }

                Point p = new Point(X, Y);

                Points.Add(p);
            }
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

