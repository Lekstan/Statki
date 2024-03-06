using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace statki
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Witaj w grze w statki dla dwóch graczy!");

            
            char[,] planszaStatkiGracz1 = InicjalizujPlansze();
            char[,] planszaCelowaniaGracz1 = InicjalizujPlansze();

            
            char[,] planszaStatkiGracz2 = InicjalizujPlansze();
            char[,] planszaCelowaniaGracz2 = InicjalizujPlansze();

            
            Console.WriteLine("Gracz 1, rozpocznij ustawianie statków.");
            UstawStatki(planszaStatkiGracz1);

            Console.Clear();

            
            Console.WriteLine("Gracz 2, rozpocznij ustawianie statków.");
            UstawStatki(planszaStatkiGracz2);

            Console.Clear();

            
            Gra(planszaStatkiGracz1, planszaCelowaniaGracz1, planszaStatkiGracz2, planszaCelowaniaGracz2);
        }

        static char[,] InicjalizujPlansze()
        {
            char[,] plansza = new char[10, 10];

            
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    plansza[i, j] = '-';
                }
            }

            return plansza;
        }

        static void UstawStatki(char[,] plansza)
        {
            var iloscStatkow = new Dictionary<int, int> { { 1, 4 }, { 2, 3 }, { 3, 2 }, { 4, 1 } };
            foreach (var kvp in iloscStatkow)
            {
                int dlugoscStatku = kvp.Key;
                int iloscStatkowDanegoRodzaju = kvp.Value;

                for (int i = 0; i < iloscStatkowDanegoRodzaju; i++)
                {
                    bool ustawionoStatek = false;
                    while (!ustawionoStatek)
                    {
                        Console.WriteLine($"Ustaw {dlugoscStatku}-masztowiec numer {i + 1}");

                        Console.Write("Podaj wiersz początku (0-9): ");
                        int wierszPoczatkowy = Convert.ToInt32(Console.ReadLine());

                        Console.Write("Podaj kolumnę początku (0-9): ");
                        int kolumnaPoczatkowa = Convert.ToInt32(Console.ReadLine());

                        Console.Write("Wybierz orientację (h - pozioma, v - pionowa): ");
                        char orientacja = Convert.ToChar(Console.ReadLine());

                        if (SprawdzMiejsce(plansza, wierszPoczatkowy, kolumnaPoczatkowa, dlugoscStatku, orientacja))
                        {
                            UstawStatek(plansza, wierszPoczatkowy, kolumnaPoczatkowa, dlugoscStatku, orientacja);
                            ustawionoStatek = true;
                        }
                        else
                        {
                            Console.WriteLine("Nie można ustawić statku w tym miejscu. Spróbuj ponownie.");
                        }
                    }
                }
            }
        }

        static bool SprawdzMiejsce(char[,] plansza, int wiersz, int kolumna, int dlugoscStatku, char orientacja)
        {
            if (orientacja == 'h')
            {
                if (kolumna + dlugoscStatku > 10)
                    return false;

                for (int i = kolumna; i < kolumna + dlugoscStatku; i++)
                {
                    if (plansza[wiersz, i] != '-')
                        return false;
                }
            }
            else if (orientacja == 'v')
            {
                if (wiersz + dlugoscStatku > 10)
                    return false;

                for (int i = wiersz; i < wiersz + dlugoscStatku; i++)
                {
                    if (plansza[i, kolumna] != '-')
                        return false;
                }
            }

            return true;
        }

        static void UstawStatek(char[,] plansza, int wiersz, int kolumna, int dlugoscStatku, char orientacja)
        {
            if (orientacja == 'h')
            {
                for (int i = kolumna; i < kolumna + dlugoscStatku; i++)
                {
                    plansza[wiersz, i] = 'S';
                }
            }
            else if (orientacja == 'v')
            {
                for (int i = wiersz; i < wiersz + dlugoscStatku; i++)
                {
                    plansza[i, kolumna] = 'S';
                }
            }
        }

        static void Gra(char[,] planszaStatkiGracz1, char[,] planszaCelowaniaGracz1, char[,] planszaStatkiGracz2, char[,] planszaCelowaniaGracz2)
        {
            int trafieniaGracz1 = 0;
            int trafieniaGracz2 = 0;
            bool kolejGracza1 = true; 

            while (trafieniaGracz1 < 20 && trafieniaGracz2 < 20)
            {
                if (kolejGracza1)
                {
                    Console.WriteLine("Gracz 1, twój ruch:");
                    RuchGracza(planszaCelowaniaGracz2, planszaStatkiGracz2, ref trafieniaGracz1, planszaStatkiGracz1);
                    kolejGracza1 = false;
                }
                else
                {
                    Console.WriteLine("Gracz 2, twój ruch:");
                    RuchGracza(planszaCelowaniaGracz1, planszaStatkiGracz1, ref trafieniaGracz2, planszaStatkiGracz2);
                    kolejGracza1 = true;
                }
            }

            Console.WriteLine("Koniec gry.");
        }

        static void RuchGracza(char[,] planszaCelowania, char[,] planszaStatki, ref int trafienia, char[,] planszaStatkiPrzeciwnika)
        {
            WyswietlPlansze(planszaStatkiPrzeciwnika, planszaCelowania);

            Console.Write("Podaj wiersz strzału (0-9): ");
            int wierszStrzalu = Convert.ToInt32(Console.ReadLine());

            Console.Write("Podaj kolumnę strzału (0-9): ");
            int kolumnaStrzalu = Convert.ToInt32(Console.ReadLine());

            if (planszaCelowania[wierszStrzalu, kolumnaStrzalu] != '-' && planszaCelowania[wierszStrzalu, kolumnaStrzalu] != 'S')
            {
                Console.WriteLine("Już tu strzelałeś!");
                return;
            }

            if (planszaStatki[wierszStrzalu, kolumnaStrzalu] == 'S')
            {
                Console.WriteLine("Trafiony!");
                planszaCelowania[wierszStrzalu, kolumnaStrzalu] = 'X';
                trafienia++;
            }
            else
            {
                Console.WriteLine("Pudło!");
                planszaCelowania[wierszStrzalu, kolumnaStrzalu] = 'O';
            }

            Console.Clear(); 
        }

        static void WyswietlPlansze(char[,] planszaStatki, char[,] planszaCelowania)
        {
            Console.WriteLine("Plansza gracza (celowanie):");
            WyswietlPlansze(planszaCelowania);

            Console.WriteLine("\nPlansza gracza (statki):");
            WyswietlPlansze(planszaStatki);
        }

        static void WyswietlPlansze(char[,] plansza)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Console.Write(plansza[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}