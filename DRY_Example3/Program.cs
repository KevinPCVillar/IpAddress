using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRY_Example3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Write a Valid IP Address: ");
            string input = Console.ReadLine();
            var splittedIp = input.Split('.');

            if(Validator.IsNumberOctantsCorrect(splittedIp) && Validator.AreAllNumbers(splittedIp))
            {
                var address = Converter.StringIntoInt(splittedIp);
                IpAddress ip = new IpAddress(address[0], address[1], address[2], address[3]);
                var classification = Classificator.GetAddressClassification(ip);
                try
                {
                    Console.WriteLine("The IP Address: {0}, and is a class {1}",Validator.IP(ip),classification); 
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            else
            {
                Console.WriteLine("Not Valid IP Address");
            }
        }
    }

    public class Validator
    {
        public static string IP(IpAddress address)
        {
            foreach (var value in GetOctantvalue(address))
            {
                if (value > 255 || value < 0)
                    throw new Exception("Not Valid IP Address");
            }

            return String.Concat(address.FirstOctant, ".", address.SecondOctant, ".", address.ThirdOctant, ".", address.FourthOctant);
        }

        public static bool IsNumberOctantsCorrect(string[] octants)
        {
            if (octants.Count() != 4)
                return false;
            else
                return true;
        }

        public static bool AreAllNumbers(string[] octants)
        {
            foreach (var octant in octants)
            {
                try
                {
                    var type = Int32.Parse(octant).GetType();
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return true;
        }

        private static IEnumerable<int> GetOctantvalue(IpAddress address)
        {
            yield return address.FirstOctant;
            yield return address.SecondOctant;
            yield return address.ThirdOctant;
            yield return address.FourthOctant;
        }
    }

    public class Converter
    {
        public static List<int> StringIntoInt(string[] StringArray)
        {
            List<int> list = new List<int>();
            foreach (var data in StringArray)
            {
                list.Add(Int32.Parse(data));
            }
            return list;
        }
    }

    public class Classificator
    {
        public static IpAddressClassifications GetAddressClassification(IpAddress address)
        {
            if(address.FirstOctant == 127)
            {
                return IpAddressClassifications.Localhost;
            }
            foreach (var range in GetRange())
            {
                if(address.FirstOctant >= range.Begin && address.FirstOctant <= range.End)
                {
                    return range.Class;
                }
            }
            return 0;
        }

        private static IEnumerable<IpRange> GetRange()
        {
            yield return new IpRange { Begin = 0, End = 126 , Class = IpAddressClassifications.A};
            yield return new IpRange { Begin = 128, End = 191, Class = IpAddressClassifications.B };
            yield return new IpRange { Begin = 192, End = 223, Class = IpAddressClassifications.C};
            yield return new IpRange { Begin = 223, End = 255, Class = IpAddressClassifications.D };
        }
    }

    public class IpRange
    {
        public int Begin { get; set; }
        public int End { get; set; }
        public IpAddressClassifications Class { get; set; }
    }

    public enum IpAddressClassifications
    {
        A = 1,
        B = 2,
        C = 3,
        D = 4,
        Localhost = 5
    }

    public class IpAddress
    {
        public int FirstOctant;
        public int SecondOctant;
        public int ThirdOctant;
        public int FourthOctant;

        public IpAddress(int firstOctant, int secondOctant, int thirdOctant, int fourthOctant)
        {
            FirstOctant = firstOctant;
            SecondOctant = secondOctant;
            ThirdOctant = thirdOctant;
            FourthOctant = fourthOctant;
        }
    }
}
