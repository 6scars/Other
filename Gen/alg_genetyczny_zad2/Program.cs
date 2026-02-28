using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;



class Program
{
    static Random rand = new Random();


    public class Osobnik
    {
        public string Pa = "";
        public string Pb = "";
        public string Pc = "";
        public float Number;
        public float Number2;
        public float Number3;
        public float Value;


        public Osobnik() { }


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
            string binary = Convert.ToString(i, 2).PadLeft((int)liczbaChro, '0');
            slownik[binary] = temp;
            temp += jedKrokVal;

        }

        return slownik;
    }


   public static List<Osobnik> stworzPopulacje(int wielPopulacji, int liczbaChro)
    {
        List<Osobnik> populacja = new List<Osobnik>();
        for (int i = 0; i < wielPopulacji; i++)
        {
            int randomNumber = rand.Next(0, 1 << 4);
            int randomNumber2 = rand.Next(0, 1 << 4);
            int randomNumber3 = rand.Next(0, 1 << 4);
            string bity1 = Convert.ToString(randomNumber, 2).PadLeft(liczbaChro, '0');
            string bity2 = Convert.ToString(randomNumber2, 2).PadLeft(liczbaChro, '0');
            string bity3 = Convert.ToString(randomNumber2, 2).PadLeft(liczbaChro, '0');
            populacja.Add(new Osobnik()
            {
                Pa = bity1,
                Pb = bity2,
                Pc = bity3,
            });
        }
        return populacja;
    }

    public static void dekodowanie(List<Osobnik> populacja, Dictionary<string, float> slownik)
    {

        foreach (var osoba in populacja)
        {
            string bit1osoba = osoba.Pa;
            string bit2osoba = osoba.Pb;
            string bit3osoba = osoba.Pc;

            osoba.Number = slownik[bit1osoba];
            osoba.Number2 = slownik[bit2osoba];
            osoba.Number3 = slownik[bit3osoba];
        }
    }



    public static float funkcjaPrzysto(float number, float number2, float number3, float ix)
    {
        float szum = ((float)(rand.Next(-15, 16)))/100;
        double wynik = 0;
        wynik += number * Math.Sin(number2 * ix + number3)+szum;
        return (float)wynik;
    }

    public static void addValue(List<Osobnik> populacja, List<float> ixy, List<float>iyki)
    {

        foreach (var item in populacja)
        {
            float f = 0;
            for (int i =0; i<ixy.Count; i++)
            {
                
                float value = funkcjaPrzysto(item.Number, item.Number2, item.Number3, ixy[i]);
                f += (float)Math.Pow((iyki[i] - value), 2);
               
            }
            item.Value= f;

        }
    }


    public static (List<float> ixy, List<float> iyki) ladowanieP(string pathN)
    {
        List<float> ixy = new List<float>();
        List<float> iyki = new List<float>();

        foreach (string line in File.ReadLines($"../../../{pathN}"))
        {
            string[] czesc = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (czesc.Length == 2 && float.TryParse(czesc[0], out float x) && float.TryParse(czesc[1], out float y))
            {
                ixy.Add(x);
                iyki.Add(y);
            }
        }
        return (ixy,iyki);

    }













    public static List<Osobnik> turniej(List<Osobnik> populacja, int wielPopulacji)
    {
        List<Osobnik> zapPopulacja = new List<Osobnik>(populacja);
        List<Osobnik> newPopulation = new List<Osobnik>();

        for (int i = 0; i < wielPopulacji-1; i++)
        {
            int random = rand.Next(0, zapPopulacja.Count);
            int random2 = rand.Next(0, zapPopulacja.Count);
            int random3 = rand.Next(0, zapPopulacja.Count);
            while(random != random2 && random != random3 && random2 != random3)
            {
                random = rand.Next(0, zapPopulacja.Count);
                random2 = rand.Next(0, zapPopulacja.Count);
                random3 = rand.Next(0, zapPopulacja.Count);
            }


            Osobnik Wojownik1 = zapPopulacja[random];
            Osobnik Wojownik2 = zapPopulacja[random2];
            Osobnik Wojownik3 = zapPopulacja[random3];


            if (Wojownik1.Value >= Wojownik2.Value && Wojownik1.Value >= Wojownik3.Value)
            {
                newPopulation.Add(Wojownik1);
            }
            else if (Wojownik2.Value >= Wojownik1.Value && Wojownik2.Value >= Wojownik3.Value)
            {
                newPopulation.Add(Wojownik2);
            }
            else
            {
                newPopulation.Add(Wojownik3);
            }
        }

        return newPopulation;

    }

    public static List<Osobnik> krzyzowanie(List<Osobnik> Populacja)
    {
        List<Osobnik> ukrzyzowane = new List<Osobnik>();
        List<int> cyfry = new List<int>() { 0,2,8,Populacja.Count-2};

        foreach (var i in cyfry) {
            string bityOsobnikaPa1 = Populacja[i].Pa;
            string bityOsobnikaPa2 = Populacja[i+1].Pa;
            string OstateczneBityPa1 = bityOsobnikaPa1.Substring(0, 2) + bityOsobnikaPa2.Substring(2, 2);

            string bityOsobnikaPb1 = Populacja[i].Pb;
            string bityOsobnikaPb2 = Populacja[i + 1].Pb;
            string OstateczneBityPb2 = bityOsobnikaPb1.Substring(0, 2) + bityOsobnikaPb2.Substring(2, 2);


            string bityOsobnikaPc1  = Populacja[i].Pc;
            string bityOsobnikaPc2 = Populacja[i + 1].Pc;
            string OstateczneBityPc2 = bityOsobnikaPc1.Substring(0, 2) + bityOsobnikaPc2.Substring(2, 2);

            ukrzyzowane.Add(new Osobnik()
            {
                Pa = OstateczneBityPa1,
                Pb = OstateczneBityPb2,
                Pc = OstateczneBityPc2
            });

        }
        

       

        return ukrzyzowane;
    }



    public static List<Osobnik> opeMutacji(List<Osobnik> winners, int liczbaChro)
    {
        List<Osobnik> newList = new List<Osobnik>();

        for(int i =5; i<winners.Count; i++)
        {
            int random = rand.Next(0, liczbaChro);
            char[] bitychar1 = winners[i].Pa.ToCharArray();
            bitychar1[random] = (bitychar1[random] == '0' ? '1' : '0');

            random = rand.Next(0, liczbaChro);
            char[] bitychar2 = winners[i].Pb.ToCharArray();
            bitychar2[random] = (bitychar2[random] == '0' ? '1' : '0');

            random = rand.Next(0, liczbaChro);
            char[] bitychar3 = winners[i].Pc.ToCharArray();
            bitychar3[random] = (bitychar3[random] == '0' ? '1' : '0');

            newList.Add(new Osobnik()
            {
                Pa = string.Join("", bitychar1),
                Pb = string.Join("", bitychar2),
                Pc = string.Join("", bitychar3),
            });



        }
        return newList;
    }


    public static Osobnik najVal(List<Osobnik> populacja)
    {
        Osobnik osoba = populacja.MaxBy(item => item.Value);
        // Osobnik kopOsoba = new Osobnik (osoba);

        return osoba;
    }


    public static void sredVal(List<Osobnik> populacja, int iteracja)
    {
        int count = populacja.Count;
        float licznikVal = 0;
        foreach (var item in populacja)
        {
            licznikVal += item.Value;
        }

        Console.WriteLine($"[{iteracja}]srednia wartosc: {licznikVal / count} \n");

    }

    static void Main()
    {
        string pathN = "sinusik.txt";
        var (ixy, iyki) = ladowanieP(pathN);
        int liczbaChro = 4;
       // int liczbaPara = 2; // nie używane
        int min = 0;
        int max = 3;
        int wielPopulacji = 13;
        Dictionary<string, float> slownik = tablicaKodowania(min, max, liczbaChro);

        //PIERWSZA POPULACJA
        List<Osobnik> populacja = stworzPopulacje(wielPopulacji, liczbaChro);

        dekodowanie(populacja, slownik);
        addValue(populacja, ixy, iyki);


        Console.WriteLine($"najwyzsze value: {najVal(populacja).Value}");
        sredVal(populacja,-1);
        //petla

        for(int i =0; i<100; i++)
        {
            List<Osobnik> selekcja = turniej(populacja, wielPopulacji); //13 osobnikow
            List<Osobnik> newPopulacja = new List<Osobnik>();// tu bede wkladac po cztery osobniki
            newPopulacja.AddRange(krzyzowanie(selekcja)); //cztery osobniki

            newPopulacja.AddRange(opeMutacji(selekcja, liczbaChro)); // od piątego do ostatniego to wychodzi 7 osobników

            newPopulacja.Add(najVal(selekcja));// hotdeck

            dekodowanie(newPopulacja, slownik);
            addValue(newPopulacja, ixy, iyki);

            Console.WriteLine($"[{i}] najwyzsze value: {najVal(newPopulacja).Value}");
            sredVal(newPopulacja, i);
            populacja = newPopulacja;
        }


    }
}