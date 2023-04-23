using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using P = PudelkoLib.Pudelko;

namespace PudelkoLib
{
    // enum
    public enum UnitOfMeasure
    {
        meter,
        centimeter,
        milimeter
    }
    public sealed class Pudelko : IFormattable, IEquatable<Pudelko>, IEnumerable<double>
    {
        // Zmienne
        public UnitOfMeasure unit { get; set; }
        public readonly double a, b, c;
        public double A => Math.Truncate(a * 1000) / 1000;
        public double B => Math.Truncate(b * 1000) / 1000;
        public double C => Math.Truncate(c * 1000) / 1000;
        //Konwert
        public double ConvertToM(double x, UnitOfMeasure unit)
        {
            if (unit == UnitOfMeasure.milimeter) return x / 1000;
            else if (unit == UnitOfMeasure.centimeter) return x / 100;
            else return x;
        }
        //Tworzenie
        public Pudelko(double? a = null, double? b = null, double? c = null, UnitOfMeasure unit = UnitOfMeasure.meter)
        {
            this.a = a is null ? 0.1 : ConvertToM((double)a, unit);
            this.b = b is null ? 0.1 : ConvertToM((double)b, unit);
            this.c = c is null ? 0.1 : ConvertToM((double)c, unit);
            this.unit = unit;

            if (this.a > 10)
                throw new ArgumentOutOfRangeException(nameof(A));
            else if (this.a < 0.001)
                throw new ArgumentOutOfRangeException(nameof(A));

            if (this.b > 10)
                throw new ArgumentOutOfRangeException(nameof(B));
            else if (this.b < 0.001)
                throw new ArgumentOutOfRangeException(nameof(B));

            if (this.c > 10)
                throw new ArgumentOutOfRangeException(nameof(C));
            else if (this.c < 0.001)
                throw new ArgumentOutOfRangeException(nameof(C));
        }
        //To String 
        public override string ToString() { return this.ToString("m", CultureInfo.CurrentCulture); }
        public string ToString(string unitformat) { return this.ToString(unitformat, CultureInfo.CurrentCulture); }
        public string ToString(string? unitformat, IFormatProvider? provider = null)
        {

            if (String.IsNullOrEmpty(unitformat))
            {
                unitformat = "m";
            }
            if (provider is null) { provider = CultureInfo.CurrentCulture; }
            switch (unitformat.ToLower())
            {
                case "m":
                    return $"{String.Format("{0:N3}", A)} m × {String.Format("{0:N3}", B)} m × {String.Format("{0:N3}", C)} m";
                case "mm":
                    return $"{String.Format("{0}", A * 1000)} mm × {String.Format("{0}", B * 1000)} mm × {String.Format("{0}", C * 1000)} mm";
                case "cm":
                    return $"{String.Format("{0:N1}", A * 100)} cm × {String.Format("{0:N1}", B * 100)} cm × {String.Format("{0:N1}", C * 100)} cm";
                default:
                    throw new FormatException("Wrong Format!");
            }
        }
        //Obliczenia
        public double Objetosc => Math.Round(a * b * c, 1);
        public double Pole => Math.Round(2 * a * b + 2 * a * c + 2 * b * c, 2);
        //Equals
        public bool Equals(Pudelko? obj)
        {
            if (obj is null) return false;

            double[] checkValue1 = new[] { a, b, c };
            double[] checkValue2 = new[] { obj.a, obj.b, obj.c };
            Array.Sort(checkValue1, checkValue2);
            if (a == obj.a && b == obj.b && c == obj.c) return true;
            else return false;
        }
        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            else if (obj is not Pudelko[]) return false;
            return Equals(obj);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(a, b, c);
        }
        //Przeciążenia
        public static bool operator ==(Pudelko left, Pudelko right)
         {
             return left.Equals(right);
         }
        public static bool operator !=(Pudelko left, Pudelko right)
        {
            return !left.Equals(right);
        }
        // + Konwersjas
        public static Pudelko operator +(Pudelko p1, Pudelko p2)
        {
            double[] checkValue1 = new[] { p1.a, p1.b, p1.c };
            double[] checkValue2 = new[] { p2.a, p2.b, p2.c };
                Array.Sort(checkValue1);
                Array.Sort(checkValue2);
                Array.Reverse(checkValue1);
                Array.Reverse(checkValue2);
            double a = Math.Max(checkValue1[0], checkValue2[0]);
            double b = Math.Max(checkValue1[1], checkValue2[1]);
            Pudelko result = new Pudelko(a, b, checkValue1[2] + checkValue2[2]);
            return result;
        }
        //Explicit
        public static explicit operator double[](Pudelko l)
        {
            double[] tab = new double[3];
            tab[0] = l.a;
            tab[1] = l.b;
            tab[2] = l.c;
            return tab;
        }
        //Implicit
        public static implicit operator Pudelko(ValueTuple<int, int, int> numb)
        {
            Pudelko result = new Pudelko(numb.Item1, numb.Item2, numb.Item3, UnitOfMeasure.milimeter);
                return result;
        }

        //Indekser
        public double this[int indexer]
        {
            get
            {
                switch (indexer)
                {
                    case 0: return a;
                    case 1: return b;
                    case 2: return c;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }
        //Parsowanie
        public static Pudelko Parse(string str)
        {
            // Sprawdzenie, czy podany łańcuch jest pusty
            if (string.IsNullOrWhiteSpace(str))
                 throw new ArgumentException("Unexpected format Pudelko.");
            // Rozdzielenie wartości długości, szerokości i wysokości
            string[] parts = str.Split(new char[] { '×' }, StringSplitOptions.RemoveEmptyEntries);
            // Sprawdzenie, czy udało się rozdzielić wszystkie wartości
            if (parts.Length != 3)
                throw new ArgumentException("Unexpected format Pudelko.");
            // Parsowanie wartości długości, szerokości i wysokości
            if (!double.TryParse(parts[0].Trim(), out double length) || !double.TryParse(parts[1].Trim(), out double width) || !double.TryParse(parts[2].Trim(), out double height))
            {
                throw new ArgumentException("Unexpected format Pudelko.");
            }
            // Utworzenie i zwrócenie obiektu pudełka
            return new Pudelko(length, width, height);
        }

        //Iterrator
        public IEnumerator<double> GetEnumerator()
        {
            List<double> tab = new List<double>() { a, b, c };
            return tab.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}