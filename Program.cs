using PudelkoLib;

namespace PudelkoApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Pudelko p = new Pudelko(6.1448, 3.4, 2.6, UnitOfMeasure.milimeter);
            Console.WriteLine("Wymiary: "+ p.ToString("m"));
            Console.WriteLine("Wymiary: " + p.ToString("cm"));
            Console.WriteLine("Wymiary: " + p.ToString("mm"));
            Console.WriteLine("Objętość: "+ p.Objetosc);
            Console.WriteLine("Pole: "+ p.Pole);
            Console.WriteLine();

            Pudelko p2 = new Pudelko(2, 3, 3, UnitOfMeasure.centimeter);
            Console.WriteLine("Wymiary: " + p2.ToString("m"));
            Console.WriteLine("Wymiary: " + p2.ToString("cm"));
            Console.WriteLine("Wymiary: " + p2.ToString("mm"));
            Console.WriteLine("Objętość: "+ p2.Objetosc);
            Console.WriteLine("Pole: " + p2.Pole);
            Console.WriteLine();

            Pudelko p3 = new Pudelko(3.5, 5.221, 0.1);
            Console.WriteLine("Wymiary: " + p3.ToString("m"));
            Console.WriteLine("Wymiary: " + p3.ToString("cm"));
            Console.WriteLine("Wymiary: " + p3.ToString("mm"));
            Console.WriteLine("Objętość: "+ p3.Objetosc);
            Console.WriteLine("Pole: " + p3.Pole);
            Console.WriteLine();
        }
    }
}