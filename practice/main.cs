using System;
using System.IO;

class Program {
  public static void Main (string[] args) 
  {
    Console.WriteLine("Введите число");

    string input = Console.ReadLine();
    int n = 0;
    string path = "positions.txt";
    
    try 
    {
      n = Convert.ToInt32(input);
    }
    catch (FormatException) 
    {
      Console.WriteLine("Input string is not a sequence of digits.");
    }
    catch (OverflowException)
    {
      Console.WriteLine("The number cannot fit in an Int32.");
    }
    
    Console.WriteLine(n);

    int[] positions = new int[n];

    for (int i = 0; i < positions.Length; i++) 
    {
      positions[i] = i + 1;
    }
    
    using (StreamWriter sw = File.CreateText(path))
    {
      for (int i = 0; i < positions.Length; i++) 
      {
        sw.WriteLine(positions[i]);
      }
    }
    
    using (StreamReader sr = File.OpenText(path))
    {
      string s;
      
      while ((s = sr.ReadLine()) != null)
          Console.WriteLine(s);
    }
    
  }
}
