// See https://aka.ms/new-console-template for more information
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

class Osobnik
{
    public string Bit = "";
    public float Number;
    public float Value;

    public Osobnik() { }
    public Osobnik(string bit) { Bit = bit; }

    public void przypisz(string bit, float number, float value)
    {
        Bit = bit;
        Number = number;
        Value = value;
    }
}

class Program
{
    static Random rand = new Random();

    static Dictionary<string, float> stworzDictonary(int min, int max, int liczbaChro)
    {
        float dMinMax = max - min;
        float liczbaKrokow = (float)(Math.Pow(2, liczbaChro) - 1);
        float step = dMinMax / liczbaKrokow;
        float xd = min;
        Dictionary<string, float> binDictionary = new Dictionary<string, float>();

        for (int i = 0; i <= liczbaKrokow; i++)
        {
            string binary = Convert.ToString(i, 2).PadLeft(liczbaChro, '0');
            binDictionary[binary] = xd;
            xd += step;
        }
        return binDictionary;
    }

    static string Kodowanie(float x, int min, int max, int liczbaChro)
    {
        Dictionary<string, float> binDictionary = stworzDictonary(min, max, liczbaChro);
        float difference = float.MaxValue;
        string klucz = "";

        foreach (var item in binDictionary)
        {
            float tempItem = Math.Abs(x - item.Value);
            if (tempItem < difference)
            {
                klucz = item.Key;
                difference = tempItem;
            }
        }
        return klucz;
    }

    static float Dekodowanie(string x, int min, int max, int liczbaChro)
    {
        Dictionary<string, float> binDictionary = stworzDictonary(min, max, liczbaChro);
        return binDictionary.ContainsKey(x) ? binDictionary[x] : -1;
    }

    //zwraca liste osobników z bitami i ich odzwierciedloną wartoscią
    public static List<Osobnik> InitializePopulation(int populationSize, int liczbaChro, int min, int max)
    {
        List<Osobnik> population = new List<Osobnik>();
        for (int i = 0; i < populationSize; i++)
        {
            int randomNum = rand.Next(0, (1 << liczbaChro));
            string Bits = Convert.ToString(randomNum, 2).PadLeft(liczbaChro, '0');
            float number = Dekodowanie(Bits, min, max, liczbaChro);
            population.Add(new Osobnik(Bits) { Number = number });
        }
        return population;
    }

    public static void funkcjaPrzystosowania(List<Osobnik> Lista)
    {
        foreach (var item in Lista)
        {
            item.Value = (float)(Math.Sin(5 * item.Number) * item.Number);
        }
    }

    //jeżeli ostatni zwycięzca nie mają pary to jego strata
    public static List<Osobnik> selekcjaTurniejowa(List<Osobnik> population, int rozmiarTurnieju)
    {
        List<Osobnik> population2 = population.ToList();
        List<Osobnik> zwyciezcy = new List<Osobnik>();
        while (population2.Count >= rozmiarTurnieju)
        {
            List<Osobnik> Wojownicy = new List<Osobnik>();
            for (int i = 0; i < rozmiarTurnieju; i++)
            {
                int randNum = rand.Next(0, population2.Count);
                Wojownicy.Add(population2[randNum]);
                population2.RemoveAt(randNum);

            }
            Osobnik zwyciezca = Wojownicy.OrderByDescending(o => o.Value).First();
            
            zwyciezcy.Add(zwyciezca);
        }

        
        if (population2.Count > 0)
        {
            List<Osobnik> Wojownicy = new List<Osobnik>();
            for (int i =0; i< population2.Count; i++)
            {
                Wojownicy.Add(population2[i]);
            }
            Osobnik zwyciezca = Wojownicy.OrderByDescending(o => o.Value).First();
            zwyciezcy.Add(zwyciezca);

        }
        return zwyciezcy;
    }

    public static List<Osobnik> operatorKrzyzowania(List<Osobnik> rodzice, int liczbaPotomkow )
    {
        
        int zakresDo = rodzice[0].Bit.Length;
       List <Osobnik> potomkowie = new List<Osobnik>();
       
        for(int i =0; i<rodzice.Count-1; i += 2)
        {
            for(int x =0; x<liczbaPotomkow; x++)
            {
                int punktPrzeciecia = rand.Next(0,zakresDo-1);
                Osobnik potomek = new Osobnik()
                {
                    Bit = rodzice[i].Bit.Substring(0, punktPrzeciecia) + rodzice[i + 1].Bit.Substring(punktPrzeciecia)
                };
                potomkowie.Add(potomek);
            }
        }
        return potomkowie;
    }

    public static List<Osobnik> operatorMutacji(List<Osobnik> populacja)
    {
        //funkcja zmienia jeden bit w genotypie
        for(int i =0; i<populacja.Count-1; i++)
        {
            char[] bity = populacja[i].Bit.ToCharArray();
            int random = rand.Next(0, bity.Length - 1);

            bity[random] = (bity[random] == '0') ? '1' : '0';
            populacja[i].Bit = string.Join("", bity);
        }

        return populacja;
    }

    public static List<Osobnik> nadajWartosc(List<Osobnik> populacja, int min, int max, int liczbaChro)
    {
        List<Osobnik> popWypelniona = populacja.ToList();
        foreach(var x in popWypelniona)
        {
            x.Number = Dekodowanie(x.Bit, min, max, liczbaChro);
        }

        return popWypelniona;
    }

    static void alg_genetyczny(float x, int min, int max, int liczbaChro, int populationSize, int generacje, int rozmiarTurnieju, int liczbaPotomkow)
    {   
        //
        List<Osobnik> population = InitializePopulation(populationSize, liczbaChro, min, max);


        for(int i =0; i<generacje; i++)
        {


            funkcjaPrzystosowania(population);
            foreach (var item in population)
            {
                Console.WriteLine($"{item.Bit.PadRight(10)} {item.Number.ToString("0.00000").PadRight(10)} {item.Value.ToString("0.00000").PadRight(10)}");

            }
            //
            List<Osobnik> zwyciezcy = selekcjaTurniejowa(population, rozmiarTurnieju);
            List<Osobnik> krzyzowanie = operatorKrzyzowania(zwyciezcy, liczbaPotomkow);

            List<Osobnik> mutowanie = operatorMutacji(krzyzowanie);
            List<Osobnik> AAAA = nadajWartosc(mutowanie, min, max, liczbaChro);
            population = AAAA.ToList();


        }
        funkcjaPrzystosowania(population);

        /*
                funkcjaPrzystosowania(population);
                //
                List<Osobnik> zwyciezcy = selekcjaTurniejowa(population, rozmiarTurnieju);
                Console.WriteLine("Zwycięzcy turnieju:");
                foreach (var item in zwyciezcy)
                {
                    Console.WriteLine($"{item.Bit}  {item.Number} {item.Value}");
                }
                //
                Console.WriteLine("potomkowie Rodzicow:");
                List<Osobnik> potomkowie =  operatorKrzyzowania(zwyciezcy, liczbaPotomkow);
                foreach (var item in potomkowie)
                {
                    Console.WriteLine($"{item.Bit}  {item.Number} {item.Value}");
                }
                //
                Console.WriteLine("mutacja:");
                List<Osobnik> potomMutac =  operatorMutacji(potomkowie);
                foreach (var item in potomkowie)
                {
                    Console.WriteLine($"{item.Bit}  {item.Number} {item.Value}");
                }
                //
                Console.WriteLine("potomkowie:");
                List<Osobnik> AAAA = nadajWartosc(potomMutac, min, max, liczbaChro);
                foreach (var item in AAAA)
                {
                    Console.WriteLine($"{item.Bit}  {item.Number} {item.Value}");
                }







                funkcjaPrzystosowania(AAAA);
                foreach (var item in AAAA)
                {
                    Console.WriteLine($"{item.Bit}  {item.Number} {item.Value}");
                };
                List<Osobnik> zwyciezcy2 = selekcjaTurniejowa(AAAA, rozmiarTurnieju);
                Console.WriteLine("Zwycięzcy turnieju:");
                foreach (var item in zwyciezcy2)
                {
                    Console.WriteLine($"{item.Bit}  {item.Number} {item.Value}");
                }
        */


        foreach (var item in population)
        {
            Console.WriteLine($"{item.Bit.PadRight(10)} {item.Number.ToString("0.00000").PadRight(10)} {item.Value.ToString("0.00000").PadRight(10)}");
        }

    }

    static void Main()
    {
        float x = 2.5f;
        int min = 0;
        int max = 7;
        int liczbaChro = 10;
        int populationSize = 50;
        int generacje = 20; //ilerazy ma rozmnożyć 
        int rozmiarTurnieju = 4;
        int liczbaPotomkow = 10;
        alg_genetyczny(x, min, max, liczbaChro, populationSize, generacje, rozmiarTurnieju, liczbaPotomkow);
    }
}
