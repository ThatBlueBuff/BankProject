using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Bank
{
    public class Client
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public string AccountNumber { get; set; }
        public string Password { get; set; }
    }

    public class BankSystem
    {
        public List<Client> clients;

        public BankSystem()
        {
            clients = new List<Client>();
            // Baza danych klientów
            clients.Add(new Client { ID = 1, Name = "Jan Nowak", Balance = 1457.23m, AccountNumber = "001", Password = "haslo" });
            clients.Add(new Client { ID = 2, Name = "Agnieszka Kowalska", Balance = 3600.18m, AccountNumber = "002", Password = "haslo" });
            clients.Add(new Client { ID = 3, Name = "Robert Lewandowski", Balance = 2745.03m, AccountNumber = "003", Password = "haslo" });
            clients.Add(new Client { ID = 4, Name = "Zofia Płucińska", Balance = 7344.00m, AccountNumber = "004", Password = "haslo" });
            clients.Add(new Client { ID = 5, Name = "Grzegorz Braun", Balance = 455.38m, AccountNumber = "005", Password = "gasnica" });
        }

        public List<Client> GetAllClients()
        {
            return clients;
        }

        public Client Login(int clientId, string password)
        {
            foreach (var client in clients)
            {
                if (client.ID == clientId && client.Password == password)
                {
                    return client;
                }
            }
            return null; // Walidacja danych
        }

        public bool TransferMoney(Client sender, Client recipient, decimal amount)
        {
            if (sender.Balance >= amount)
            {
                sender.Balance -= amount;
                recipient.Balance += amount;
                return true;
            }
            return false; // Sprawdzanie warunku do przelewu i logika przelewow
        }
        
        
            
        

            
    }

    public static class Program
    {
        public static void Main(string[] args)
        {
            BankSystem bank = new BankSystem();

            bool isRunning = true;
            while (isRunning)
            {
                Display.DisplayStart();
                string choice = Console.ReadLine();


                switch (choice)
                {
                    case "1":

                        Display.DisplayLista(bank.GetAllClients());
                        Console.WriteLine("");
                        Console.WriteLine("");
                        break;
                    case "2":
                        Display.DisplayLogowanie();
                        int clientId = int.Parse(Console.ReadLine());
                        Console.WriteLine("Wpisz hasło:");
                        string password = Console.ReadLine();
                        Client loggedInClient = bank.Login(clientId, password);
                        if (loggedInClient != null)
                        {
                            HandleLoggedInClient(loggedInClient, bank);
                        }
                        else
                        {
                            Console.WriteLine("Nieprawidłowe ID lub hasło.");
                        }
                        break;
                    case "3":
                        System.Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Nieprawidłowy wybór.");
                        break;
                }                                           // Logowanie
            }
        }

        

        public static void HandleLoggedInClient(Client client, BankSystem bank)
        {
            Display.DisplayZalogowany(client);
            Console.WriteLine("Podaj numer konta odbiorcy:");
            string recipientAccountNumber = Console.ReadLine();
            Console.WriteLine("Wpisz kwotę przelewu:");
            decimal amount = decimal.Parse(Console.ReadLine());

           
            Client recipient = null;
            
            foreach (var c in bank.GetAllClients())
            {
                 if (c.AccountNumber == recipientAccountNumber) //tutaj byl srednik przez ktory caly program walil fikolka i ogarniecie tego zajelo nam to 30 minut, prosimy o 3 
                {
                    recipient = c;
                    break;
                }
            }

            if (recipient != null) 
            {
                bool success = bank.TransferMoney(client, recipient, amount);
                if (success)
                {

                    Display.DisplayWykonany(bank.GetAllClients());
                }
                else
                {
                    Display.DisplayNieudany();
                }
            }
            else
            {
                Console.WriteLine("Nie znaleziono takiego odbiorcy.");
            }
        } // Wybor klienta do przelewu
    }

    public static class Display
    {
        public static void DisplayStart()
        {
            Console.WriteLine(" Wybierz opcję:");
            Console.WriteLine();
            Console.WriteLine(" 1. Lista wszystkich klientów banku");
            Console.WriteLine(" 2. Logowanie do konta");
            Console.WriteLine(" 3. Wyłącz");
            Console.WriteLine(" Wybierz 1, 2 lub 3:");
            Console.WriteLine();
        }

        public static void DisplayLista(List<Client> listaKlientow)
        {
            Console.Clear();
            Console.WriteLine();
            foreach (var klient in listaKlientow)
            {
                Console.WriteLine($"{klient.ID} | {klient.Name} | {klient.Balance} | {klient.AccountNumber}");
            }

        }

        public static void DisplayLogowanie()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(" Zaloguj się podając wybrane ID klienta");
            Console.WriteLine();
        }

        public static void DisplayZalogowany(Client klient)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(" Jesteś już zalogowany");
            Console.WriteLine($" ID: {klient.ID}");
            Console.WriteLine($" Imię i nazwisko: {klient.Name}");
            Console.WriteLine($" Saldo: {klient.Balance}");
            Console.WriteLine($" Numer konta: {klient.AccountNumber}");
            Console.WriteLine(" Wpisz numer konta, na który chcesz wykonać przelew");
            Console.WriteLine();
        }

        public static void DisplayPrzelew()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(" Podaj kwotę przelewu:");
            Console.WriteLine();
        }

        public static void DisplayNieudany()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(" Za mało środków na koncie,");
            Console.WriteLine(" Naciśnij ENTER, aby powrócić do strony konta");
            Console.WriteLine();
        }

        public static void DisplayWykonany(List<Client> listaKlientow)
        {
            Console.Clear();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" Przelew został poprawnie wykonany");
            Console.ResetColor();
            Console.WriteLine();
            foreach (var klient in listaKlientow)
            {
                Console.WriteLine($"{klient.ID} | {klient.Name} | {klient.Balance} | {klient.AccountNumber}");
            }
            Console.WriteLine(" Naciśnij ENTER, aby powrócić do ekranu logowania");           
            Console.ReadLine();
        }
    }
}
