class main
{
   static void Main()
    {
        double FPS = 60;
        double period = 1 / FPS;
        double initSpeed = 60;
        double angle = 45;
        string path = "../../../positions.txt";

        ProjectileFlight pf = new ProjectileFlight(initSpeed, angle);

        GetPosition getP = new GetPosition(pf.GetPosition);

        Motion m = new Motion(period);

        m.Calculate(getP);
        m.Print();
        m.Write(path);
    }
}
