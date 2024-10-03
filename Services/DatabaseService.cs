using ADO.NET_HW_09_10.Data;
using ADO.NET_HW_09_10.Helpers;
using ADO.NET_HW_09_10.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET_HW_09_10.Services
{
    internal class DatabaseService
    {
        public bool Register(string username, string password)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (db.Users.Any(u => u.Name == username))
                {
                    return false;
                }

                var user = new User
                {
                    Name = username,
                    Password = PasswordHelper.SHA512(password)
                };

                db.Users.Add(user);
                db.SaveChanges();
                return true;
            }
        }

        public bool Login(string username, string password)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Name == username);

                if (user != null && PasswordHelper.VerifyPassword(password, user.Password))
                {
                    user.CountLoginAttempts = 0;
                    db.SaveChanges();
                    return true;
                }
                else if (user == null || user.IsBannedForLogin)
                {
                    Console.WriteLine("Пользователь не существует или заблокирован.");
                    return false;
                }
                else
                {
                    user.CountLoginAttempts++;
                    if (user.CountLoginAttempts >= 3)
                    {
                        user.IsBannedForLogin = true;
                        Console.WriteLine("Аккаунт заблокирован после 3 неудачных попыток входа.");
                    }
                    db.SaveChanges();
                    return false;
                }
            }
        }
        public void ViewBooks()
        {
            Console.Clear();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var books = db.Books.ToList();
                foreach (var book in books)
                {
                    Console.WriteLine($"Название: {book.Title} | Автор: {book.Author} | Жанр: {book.Genre} | Год: {book.Year}");
                }
            }
            Console.WriteLine("Для продолжения нажмите любую клавишу...");
            Console.ReadKey();
        }
        public void SearchBookByTitle(string title)
        {
            Console.Clear();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var books = db.Books.Where(b => b.Title.Contains(title)).ToList();

                if (books.Any())
                {
                    foreach (var book in books)
                    {
                        Console.WriteLine($"Название: {book.Title} | Автор: {book.Author} | Жанр: {book.Genre} | Год: {book.Year}");
                    }
                }
                else
                {
                    Console.WriteLine("Книги не найдены.");
                }
            }
            Console.WriteLine("Для продолжения нажмите любую клавишу...");
            Console.ReadKey();
        }
        public void ViewBooksPaginated(int pageNumber, int pageSize = 5)
        {
            Console.Clear();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                int totalBooks = db.Books.Count();
                int totalPages = (int)Math.Ceiling(totalBooks / (double)pageSize);

                if (pageNumber < 1 || pageNumber > totalPages)
                {
                    Console.WriteLine("Такой страницы не существует.");
                    return;
                }

                var books = db.Books
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                foreach (var book in books)
                {
                    Console.WriteLine($"Название: {book.Title} | Автор: {book.Author} | Жанр: {book.Genre} | Год: {book.Year}");
                }

                Console.WriteLine($"\nСтраница {pageNumber} из {totalPages}");

                Console.WriteLine("\nВведите 'вперед', 'назад' или 'выход'");
            }
        }
        public bool CanGoForward(int pageNumber, int pageSize = 5)
        {
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                int totalBooks = db.Books.Count();
                int totalPages = (int)Math.Ceiling(totalBooks / (double)pageSize);

                return pageNumber < totalPages;
            }
        }
    }
}
