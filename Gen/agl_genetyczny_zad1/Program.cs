using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.IO;



class Osobnik
{
    public string Bit1 = "";
    public string Bit2 = "";
    public float Number;
    public float Number2;
    public float Value;


    public Osobnik() {}


}

class Program
{


    static Random rand = new Random();



    static List<Osobnik> stworzPopulacje(int wielPopulacji)
    {
        List<Osobnik> populacja = new List<Osobnik>();
        for (int i = 0; i < wielPopulacji; i++)
        {
            int randomNumber = rand.Next(0, 1 << 4);
            int randomNumber2 = rand.Next(0, 1 << 4);
            string bity = Convert.ToString(randomNumber, 2).PadLeft(4, '0');
            string bity2 = Convert.ToString(randomNumber2, 2).PadLeft(4, '0');
            populacja.Add(new Osobnik()
            {
                Bit1 = bity,
                Bit2 = bity2
            });
        }
        return populacja;
    }


    public static Dictionary<string, float> tablicaKodowania(int min, int max, float liczbaChro)
    {
        int minMax = min + max;
        float ilKrokow = (float)(Math.Pow(2, liczbaChro) - 1);
        float jedKrokVal = (float)minMax / ilKrokow;
        float temp = min;
        Dictionary<string, float> slownik = new Dictionary<string, float>();
        for (int i = 0; i <= ilKrokow; i++)
        {
            string binary = Convert.ToString(i, 2).PadLeft(4, '0');
            slownik[binary] = temp;
            temp += jedKrokVal;

        }

        return slownik;
    }

    public static void dekodowanie(List<Osobnik> populacja, Dictionary<string,float> slownik)
    {
        
        foreach (var osoba in populacja)
        {
            string bit1osoba = osoba.Bit1;
            string bit2osoba = osoba.Bit2;

            osoba.Number = slownik[bit1osoba];
            osoba.Number2 = slownik[bit2osoba];
        }
    }

    public static float funkcjaPrzysto(float number, float number2)
    {
        double wynik = 0;
        wynik += Math.Sin(number * 0.05) + Math.Sin(number2 * 0.05) + 0.4 * Math.Sin(number * 0.15) * Math.Sin(number2 * 0.15);
        return (float)wynik;
    }

    public static void addValue(List<Osobnik> populacja)
    {

        foreach (var item in populacja)
        {
            float value = funkcjaPrzysto(item.Number, item.Number2);
            item.Value = value;
        }
    }

    public static List<Osobnik> turniej(List<Osobnik> populacja, int wielkoscPopulacji)
    {
        List<Osobnik> zapPopulacja = new List<Osobnik>(populacja);
        List<Osobnik> newPopulation = new List<Osobnik>();

        for (int i = 0; i < wielkoscPopulacji-1; i++)
        {
            int random = rand.Next(0, zapPopulacja.Count);
            Osobnik Wojownik1 = zapPopulacja[random];

            
            int random2 = rand.Next(0, zapPopulacja.Count);
            //jeżeli wojownik jest taki sam to zmień
            while (random == random2)
            {
                random2 = rand.Next(0, zapPopulacja.Count);
            }
            Osobnik Wojownik2 = zapPopulacja[random2];

            if (Wojownik1.Value > Wojownik2.Value)
            {
                newPopulation.Add(Wojownik1);
            }
            else
            {
                newPopulation.Add(Wojownik2);
            }
    }


        return newPopulation;
    }

    public static Osobnik najVal(List<Osobnik> populacja)
    {
        Osobnik osoba = populacja.MaxBy(item => item.Value);
        // Osobnik kopOsoba = new Osobnik (osoba);

        return osoba;
    }

    public static void sredVal(List<Osobnik> populacja)
    {
        int count = populacja.Count;
        float licznikVal = 0;
        foreach (var item in populacja)
        {
            licznikVal += item.Value;
        }

        Console.WriteLine($"    srednia wartosc: {licznikVal / count}");
        
    }


    public static List<Osobnik> opeMutacji(List<Osobnik> winners, int liczbaChro)
    {
        List<Osobnik> newList = new List<Osobnik>();

        foreach(var items in winners)
        {
            for (int i =0; i<2; i++)
            {
                int random = rand.Next(0, liczbaChro);
                char[] bity = items.Bit1.ToCharArray();
                bity[random] = (bity[random] == '0' ? '1' : '0');

                int random2 = rand.Next(0, liczbaChro);
                char[] bity2 = items.Bit2.ToCharArray();
                bity2[random2] = (bity2[random2] == '0' ? '1' : '0');


                newList.Add(new Osobnik()
                {
                    Bit1 = String.Join("", bity),
                    Bit2 = String.Join("", bity2),
                });
            }



        }


        return newList;
    }



    public static void Main()
    {
        int liczbaChro = 4;
        int liczbaPara = 2; // nie używane
        int min = 0;
        int max = 100;
        int wielPopulacji = 23;
        Dictionary<string, float> slownik = tablicaKodowania(min, max, liczbaChro);

        //PIERWSZA POPULACJA
        List<Osobnik> populacja =  stworzPopulacje(wielPopulacji);
        dekodowanie(populacja, slownik);
        addValue(populacja);
        Osobnik najsilniejszy = najVal(populacja);
        Console.WriteLine($"najwyzsze value pierwszej puli pulacji :\n                    {najsilniejszy.Value}");

        //średnia pierwszej populacji
        sredVal(populacja);
        Console.WriteLine("");
        //przed pętlą

        for (int i=0; i<20; i++)
        {
            //pętla
            //turniej
            List<Osobnik> winners = turniej(populacja,wielPopulacji);

            //najwyzsze value
            Osobnik winner = najVal(winners);

            //tworzenie potomków
            List<Osobnik> potomkowie = opeMutacji(winners, liczbaChro);

            //dekodowanie i funkcja przystosowania
            dekodowanie(potomkowie, slownik);
            addValue(potomkowie);
            Console.WriteLine(winner.Value);

            potomkowie.Add(winner);

            //wyświetlanie najlepszego value i średniego value
            Osobnik potomekHighVal = najVal(potomkowie);
            Console.WriteLine($"[{i}] nawyzsza wartosc potomka: {potomekHighVal.Value}");
            sredVal(potomkowie);
            Console.WriteLine("");
            populacja = potomkowie;


        }

    }
}
