using ADO.NET_HW_09_10.Data;
using ADO.NET_HW_09_10.Services;
using System;

class Program
{
    static void Main(string[] args)
    {
        DatabaseService ds = new DatabaseService();

        bool isLoggedIn = false;

        while (!isLoggedIn)
        {
            Console.WriteLine("Добро пожаловать! Выберите действие:");
            Console.WriteLine("1. Войти");
            Console.WriteLine("2. Зарегистрироваться");
            Console.WriteLine("3. Выйти");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    isLoggedIn = Login(ds);
                    break;
                case "2":
                    Register(ds);
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Некорректный ввод.");
                    break;
            }
        }

        ShowMainMenu(ds);
    }

    private static void Register(DatabaseService databaseService)
    {
        Console.Clear();
        Console.Write("Введите имя пользователя: ");
        string username = Console.ReadLine();
        Console.Write("Введите пароль: ");
        string password = Console.ReadLine();
        if (password.Length > 16)
        {
            Console.WriteLine("Пароль не должен превышать 16 символов.");
            return;
        }


        if (databaseService.Register(username, password))
        {
            Console.WriteLine("Регистрация прошла успешно!");
        }
        else
        {
            Console.WriteLine("Пользователь с таким именем уже существует!");
        }
    }

    private static bool Login(DatabaseService databaseService)
    {
        Console.Clear();
        Console.Write("Введите имя пользователя: ");
        string username = Console.ReadLine();
        Console.Write("Введите пароль: ");
        string password = Console.ReadLine();
        if (password.Length > 16)
        {
            Console.WriteLine("Пароль не должен превышать 16 символов.");
            return false;
        }


        if (databaseService.Login(username, password))
        {
            Console.WriteLine("Вход выполнен успешно!");
            return true;
        }
        else
        {
            Console.WriteLine("Пользователь с такими данными не найден!");
            return false;
        }
    }

    private static void ShowMainMenu(DatabaseService ds)
    {
        bool exit = false;
        int currentPage = 1;

        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("\nГлавное меню:");
            Console.WriteLine("1. Посмотреть все книги");
            Console.WriteLine("2. Найти книгу");
            Console.WriteLine("3. Пагинация по книгам");
            Console.WriteLine("4. Выйти");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ds.ViewBooks();
                    break;
                case "2":
                    Console.Write("Введите название книги для поиска: ");
                    string title = Console.ReadLine();
                    ds.SearchBookByTitle(title);
                    break;
                case "3":
                    bool paginate = true;
                    while (paginate)
                    {
                        ds.ViewBooksPaginated(currentPage);

                        string paginationChoice = Console.ReadLine();
                        if (paginationChoice.Equals("вперед") && ds.CanGoForward(currentPage))
                        {
                            currentPage++;
                        }
                        else if (paginationChoice.Equals("назад") && currentPage > 1)
                        {
                            currentPage--;
                        }
                        else if (paginationChoice.Equals("выход"))
                        {
                            paginate = false;
                        }
                        else
                        {
                            Console.WriteLine("Неверный ввод или нельзя перейти на эту страницу.");
                        }
                    }
                    break;
                case "4":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Неверный ввод.");
                    break;
            }
        }
    }
}