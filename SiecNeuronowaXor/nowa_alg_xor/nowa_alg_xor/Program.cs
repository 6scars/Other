using System;

public class SiecNeuronowa
{
    public int iloscWejsc;
    int iloscUkrytych;
    public int iloscWyjsc;

    double[,] wagiWejUkryte;
    double[,] wagiUkryteWyjscie;
    double[] biasUkryte;
    double[] biasWyjscie;

    double[] ukryta;
    double[] wyjscie;

    Random losuj = new Random();

    public SiecNeuronowa(int wejsc, int ukrytych, int wyjsc)
    {
        iloscWejsc = wejsc;
        iloscUkrytych = ukrytych;
        iloscWyjsc = wyjsc;

        wagiWejUkryte = new double[iloscWejsc, iloscUkrytych];
        wagiUkryteWyjscie = new double[iloscUkrytych, iloscWyjsc];
        biasUkryte = new double[iloscUkrytych];
        ukryta = new double[iloscUkrytych];
        biasWyjscie = new double[iloscWyjsc];
        wyjscie = new double[iloscWyjsc]; // ← TO DODAJ

        for (int i = 0; i < iloscWejsc; i++)
            for (int j = 0; j < iloscUkrytych; j++)
                wagiWejUkryte[i, j] = losuj.NextDouble() * 2 - 1;

        for (int i = 0; i < iloscUkrytych; i++)
        {
            for (int j = 0; j < iloscWyjsc; j++)
                wagiUkryteWyjscie[i, j] = losuj.NextDouble() * 2 - 1;

            biasUkryte[i] = losuj.NextDouble() * 2 - 1;
        }




        for (int i =0; i <iloscWyjsc; i ++)
            biasWyjscie[i] = losuj.NextDouble() * 2 - 1;
    }

    double Sigmoid(double x) => 1.0 / (1.0 + Math.Exp(-x));
    double Pochodna(double x) => x * (1 - x);

    public double[] Ucz(double[] dane, double oczekiwane, double nauka = 0.1)
    {
        for (int i = 0; i < iloscUkrytych; i++)
        {
            ukryta[i] = biasUkryte[i];
            for (int j = 0; j < iloscWejsc; j++)
                ukryta[i] += dane[j] * wagiWejUkryte[j, i];
            ukryta[i] = Sigmoid(ukryta[i]);
        }
        for (int biasId = 0; biasId < biasWyjscie.Length; biasId++)
        {
            wyjscie[biasId] = biasWyjscie[biasId];
            for (int i = 0; i < iloscUkrytych; i++)
                wyjscie[biasId] += ukryta[i] * wagiUkryteWyjscie[i, biasId];
            wyjscie[biasId] = Sigmoid(wyjscie[biasId]);
        }

        double[] blad = new double[wyjscie.Length];
        double[] deltaWyjscie = new double[wyjscie.Length];
        for (int bladId = 0; bladId< wyjscie.Length; bladId++)
        {
           blad[bladId] = oczekiwane - wyjscie[bladId];
            deltaWyjscie[bladId] = blad[bladId] * Pochodna(wyjscie[bladId]);


            double[] deltaUkryte = new double[iloscUkrytych];
            for (int i = 0; i < iloscUkrytych; i++)
                deltaUkryte[i] = deltaWyjscie[bladId] * wagiUkryteWyjscie[i, bladId] * Pochodna(ukryta[i]);


            for (int i = 0; i < iloscUkrytych; i++)
                wagiUkryteWyjscie[i, bladId] += nauka * deltaWyjscie[bladId] * ukryta[i];


            biasWyjscie[bladId] += nauka * deltaWyjscie[bladId];

            for (int i = 0; i < iloscWejsc; i++)
                for (int j = 0; j < iloscUkrytych; j++)
                    wagiWejUkryte[i, j] += nauka * deltaUkryte[j] * dane[i];

            for (int i = 0; i < iloscUkrytych; i++)
                biasUkryte[i] += nauka * deltaUkryte[i];

        };

        for(int i=0; i<blad.Length; i++)
        {
            blad[i] = Math.Abs(blad[i]);
        }


        return blad;
    }

    public double[] Przewidz(double[] dane)
    {
        for (int i = 0; i < iloscUkrytych; i++)
        {
            ukryta[i] = biasUkryte[i];
            for (int j = 0; j < iloscWejsc; j++)
                ukryta[i] += dane[j] * wagiWejUkryte[j, i];
            ukryta[i] = Sigmoid(ukryta[i]);
        }

        Array.Copy(biasWyjscie, wyjscie, iloscWyjsc);

        for (int i = 0; i < iloscUkrytych; i++)
        {
            for (int idWyjscia = 0; idWyjscia < iloscWyjsc; idWyjscia++)
            {
                wyjscie[idWyjscia] = biasWyjscie[idWyjscia]; // Zacznij od biasu
                for (int j = 0; j < iloscUkrytych; j++)
                {
                    wyjscie[idWyjscia] += ukryta[j] * wagiUkryteWyjscie[j, idWyjscia];
                }
                wyjscie[idWyjscia] = Sigmoid(wyjscie[idWyjscia]);
            }
        }


        double[] wynik = new double[wyjscie.Length];
        for (int i = 0; i < wyjscie.Length; i++)
            wynik[i] = wyjscie[i];
        return wynik;
    }

    public double[][] StworzWejsciowe()
    {
        List<double[]> dane = new List<double[]>();
        int ilosc = (int)Math.Pow(2, iloscWejsc); 

        for (int i = 0; i < ilosc; i++)
        {
            string bin = Convert.ToString(i, 2).PadLeft(iloscWejsc, '0');
            double[] bity = bin.Select(c => (double)(c - '0')).ToArray();
            dane.Add(bity);
        }

        return dane.ToArray();
    }

    public double[] StworzWyjscia(double[][] daneWej)
    {
        double[] daneWyj = new double[daneWej.Length];

        for (int i = 0; i < daneWej.Length; i++)
        {
            int suma = 0;
            for (int j = 0; j < daneWej[i].Length; j++)
            {
                suma += (int)daneWej[i][j];
            }

            daneWyj[i] = suma % 2; 
        }

        return daneWyj;
    }


    public double sredniaBledow(double[] sumaBledow)
    {
        double sredniaBledow = 0;
        for (int i = 0; i < sumaBledow.Length; i++)
        {
            sredniaBledow += sumaBledow[i];
        }
        sredniaBledow = sredniaBledow / sumaBledow.Length;
        return sredniaBledow;
    }


    public void ZapiszWagi(string sciezka)
    {
        using (StreamWriter sw = new StreamWriter(sciezka))
        {
            sw.WriteLine($"{iloscWejsc} {iloscUkrytych} {iloscWyjsc}"); 

            for (int i = 0; i < iloscWejsc; i++)
                for (int j = 0; j < iloscUkrytych; j++)
                    sw.WriteLine(wagiWejUkryte[i, j]);

            for (int i = 0; i < iloscUkrytych; i++)
                sw.WriteLine(biasUkryte[i]);

            for (int i = 0; i < iloscUkrytych; i++)
                for (int j = 0; j < iloscWyjsc; j++)
                    sw.WriteLine(wagiUkryteWyjscie[i, j]);

            for (int i = 0; i < iloscWyjsc; i++)
                sw.WriteLine(biasWyjscie[i]);
        }
    }

    public static SiecNeuronowa WczytajWagi(string sciezka)
    {
        using (StreamReader sr = new StreamReader(sciezka))
        {
            string[] linie = File.ReadAllLines(sciezka);
            int index = 0;

            
            string[] rozmiary = linie[index++].Split(' ');
            int wej = int.Parse(rozmiary[0]);
            int ukr = int.Parse(rozmiary[1]);
            int wyj = int.Parse(rozmiary[2]);

            SiecNeuronowa siec = new SiecNeuronowa(wej, ukr, wyj);

            
            for (int i = 0; i < wej; i++)
                for (int j = 0; j < ukr; j++)
                    siec.wagiWejUkryte[i, j] = double.Parse(linie[index++]);

            for (int i = 0; i < ukr; i++)
                siec.biasUkryte[i] = double.Parse(linie[index++]);

            
            for (int i = 0; i < ukr; i++)
                for (int j = 0; j < wyj; j++)
                    siec.wagiUkryteWyjscie[i, j] = double.Parse(linie[index++]);

            for (int i = 0; i < wyj; i++)
                siec.biasWyjscie[i] = double.Parse(linie[index++]);

            return siec;
        }
    }

    private int[] PobierzIndeksy(string tekst)
    {
        int start = tekst.IndexOf('[') + 1;
        int end = tekst.IndexOf(']');
        string[] indeksy = tekst.Substring(start, end - start).Split(',');
        return new int[] { int.Parse(indeksy[0]), int.Parse(indeksy[1]) };
    }

    private int PobierzIndeks(string tekst)
    {
        int start = tekst.IndexOf('[') + 1;
        int end = tekst.IndexOf(']');
        return int.Parse(tekst.Substring(start, end - start));
    }


}

class Program
{


    static void TrenujSiec(int wwe =2 , int wh = 2, int wy = 1)
    {
        var siec = new SiecNeuronowa(wwe, wh, wy);

        double[][] daneWej = siec.StworzWejsciowe();
        double[] daneWyj = siec.StworzWyjscia(daneWej);

        for (int epoka = 0; epoka < 100000; epoka++)
        {
            double[] sumaBledow = new double[siec.iloscWyjsc];
            for (int i = 0; i < daneWej.Length; i++)
                for (int bladId = 0; bladId < sumaBledow.Length; bladId++)
                    sumaBledow[bladId] += siec.Ucz(daneWej[i], daneWyj[i])[bladId];

            if (epoka % 1000 == 0)
                Console.WriteLine("Epoka " + epoka + ", blad: " + sumaBledow[0].ToString("F4"));

            double sredniaBledow = siec.sredniaBledow(sumaBledow);
            if (sredniaBledow < 0.1)
                break;
        }

        Console.WriteLine("\nWyniki:");
        foreach (var dane in daneWej)
        {
            double[] wynik = siec.Przewidz(dane);

            Console.Write("Wejście: ");
            foreach (var i in dane)
                Console.Write($" {i}");

            Console.Write(" => Wyjście:");
            foreach (var w in wynik)
                Console.Write($" {w:F4}");

            Console.WriteLine();
        }

        siec.ZapiszWagi("wagi.txt");
        Console.WriteLine("Wagi zapisane do pliku wagi.txt");
    }



    static void Main()
    {

        var x = true;
        while (x)
        {
            Console.Write("\nUWAGA: sieć zakłada ,że w pliku z wagami jest już wytrenowana sieć \n o konkretnej ilości neuronów. Jeśli chcesz zmienić to, użyj opcji nr.2 \n do uczenia sieci neuronowej");
            Console.WriteLine("\nCzy chcesz:\n podać próbke i zobaczyć co wyjdzie: 1 \n Uczyć algorytm: 2 \n Aby zakończyć: 3");
            var key = Console.ReadLine();


            switch (key)
            {
                case "1":
                    string sciezkaWag = "wagi.txt";

                    if (!File.Exists(sciezkaWag))
                    {
                        Console.WriteLine("Nie znaleziono pliku z wagami. Najpierw wytrenuj sieć (opcja 2).");
                        x = false;
                        break;
                    }

                    Console.WriteLine("Wybrałeś 1 – test z próbki.");

                    SiecNeuronowa siecTestowa = SiecNeuronowa.WczytajWagi(sciezkaWag);

                    Console.Write($"Wprowadź próbkę ({siecTestowa.iloscWejsc} bitów): ");
                    string tekstProbki = Console.ReadLine().Trim();

                    double[] probka = new double[tekstProbki.Length];
                    for (int i = 0; i < tekstProbki.Length; i++)
                    {
                        probka[i] = tekstProbki[i] == '1' ? 1.0 : 0.0;
                    }

                    if (probka.Length != siecTestowa.iloscWejsc)
                    {
                        Console.WriteLine("Nieprawidłowa długość próbki!");
                        break;
                    }

                    var wynik = siecTestowa.Przewidz(probka);

                    Console.Write("Wynik sieci: ");
                    foreach (var w in wynik)
                        Console.Write($"{w:F4} ");
                    Console.WriteLine();

                    break;

                case "2":
                    Console.WriteLine("Wybrałeś 2.");

                    Console.Write("Podaj ilość neuronów w warstwie wejściowej: ");
                    int.TryParse(Console.ReadLine(), out int IloscWe);

                    Console.Write("Podaj ilość neuronów w warstwie ukrytej: ");
                    int.TryParse(Console.ReadLine(), out int IloscH);

                    Console.Write("Podaj ilość neuronów w warstwie wyjściowej: ");
                    int.TryParse(Console.ReadLine(), out int IloscWy);

                    TrenujSiec(IloscWe, IloscH, IloscWy);
                    break;
                case "3":
                    x = false;
                    break;
            }

        }



    }
}





