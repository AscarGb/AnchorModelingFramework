using AnchorModeling.Entities;
using AnchorModeling.QueryExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TestAnchorModel.Models;

namespace TestAnchorModel
{

    class Program
        {
            private static readonly string userName = $"Tom{Guid.NewGuid():N}";
            static async Task Main(string[] args)
            {
                using (var db = new MyDbContext())
                {
                    await db.Database.EnsureDeletedAsync();
                    await db.Database.EnsureCreatedAsync();
                }

                Console.WriteLine("Add");
                await Add();
                await SelectAllByExtensions();

                Console.WriteLine("Update");
                await Update();
                await SelectAllByExtensions();

                Console.WriteLine("Select anchor properties:");
                await SelectAnchorValues();

                Console.WriteLine("Delete");
                await Delete();
                await SelectAllByExtensions();

                Console.WriteLine("Complete");
                Console.ReadLine();

            }

            static async Task Add()
            {
                using (var db = new MyDbContext())
                {
                    var source = new Source
                    {
                        Name = "System A"
                    };

                    db.Sources.Add(source);

                    var transaction = new Transaction
                    {
                        Source = source,
                        SysTime = DateTime.UtcNow,
                        User = "Admin"
                    };

                    using (var dbTransaction = await db.BeginTransactionAsync(transaction))
                    {
                        var book1 = new Book
                        {
                            Name = "CLR via C#"
                        };

                        db.Books.Add(book1);

                        var book2 = new Book
                        {
                            Name = "The Art of Computer Programming"
                        };

                        db.Books.Add(book2);

                        var user = new User
                        {
                            ApplTime = DateTime.UtcNow,
                            BirthDate = DateTime.UtcNow.AddYears(-30),
                            Book = book1,                            
                            Name = userName,
                            Email = "Tom@testMail.test",
                            Height = 180
                        };

                        db.Users.Add(user);

                        await db.SaveChangesAsync();

                        dbTransaction.Commit();
                    }
                }
            }

            static async Task Update()
            {
                using (var db = new MyDbContext())
                {
                    var source = new Source
                    {
                        Name = "System B"
                    };

                    db.Sources.Add(source);

                    var transaction = new Transaction
                    {
                        Source = source,
                        SysTime = DateTime.UtcNow,
                        User = "Admin"
                    };

                    using (var dbTransaction = await db.BeginTransactionAsync(transaction))
                    {
                        var book1 = new Book
                        {
                            Name = "War and Peace"
                        };

                        var user = db.Users.FirstOrDefault(a => a.Name == userName);

                        user.ApplTime = DateTime.UtcNow;
                        user.Book = book1;
                        user.Email = "UpdatedMail@mail.test";
                        user.Height = 181;

                        await db.SaveChangesAsync();

                        dbTransaction.Commit();
                    }
                }
            }

            static async Task Delete()
            {
                using (var db = new MyDbContext())
                {
                    var source = new Source
                    {
                        Name = "System B"
                    };

                    db.Sources.Add(source);

                    var transaction = new Transaction
                    {
                        Source = source,
                        SysTime = DateTime.UtcNow,
                        User = "Admin"
                    };

                    using (var dbTransaction = await db.BeginTransactionAsync(transaction))
                    {
                        var user = db.Users.FirstOrDefault(a => a.Name == userName);

                        db.Users.Remove(user);

                        await db.SaveChangesAsync();

                        dbTransaction.Commit();
                    }
                }
            }

            static async Task SelectAnchorValues()
            {
                using var db = new MyDbContext();

                var user = await db.Users.FirstOrDefaultAsync(a => a.Name == userName);
                var id = user.Id;

                Console.WriteLine($"History of email {nameof(P_H_Users_Email)}:");
                (await db.P_H_Users_Email.Where(a => a.A_Id == id).ToListAsync()).ForEach(a => Console.WriteLine(a.Value));

                Console.WriteLine($"History of height {nameof(P_H_Users_Height)}:");
                (await db.P_H_Users_Height.Where(a => a.A_Id == id).ToListAsync()).ForEach(a => Console.WriteLine(a.Value));

                Console.WriteLine($"History of book1 {nameof(T_H_Users_Book_to_Books)}:");
                (await db.T_H_Users_Book_to_Books
                    .Where(a => a.A_Id == id) //find concrete user
                    .Select(a => db.P_Books_Name.OrderByDescending(d => d.Transaction.Id).First(b => b.A_Id == a.ToId).Value)
                    .ToListAsync())
                    .ForEach(a => Console.WriteLine(a));
            }

            static async Task SelectAllByExtensions()
            {
                using var db = new MyDbContext();

                var userHistory = await db.GetAnchorModelSet<User>()
                    .Where(a => a.AnchorId == 1)
                     .SelectAsync(true);

                foreach (var r in userHistory)
                    Console.WriteLine("{0,25} {1,10} {2,35} {3,20} {4,25} {5,5} {6,10}",
                        r.Transaction?.SysTime,
                        r.AnchorAttributePropertyInfo?.Name,
                        r.ToString(),
                       !r.IsTie ? r.AnchorAttributePropertyInfo?.PropertyType?.Name : r.TieValueType?.Name,
                        r.AttributeType?.Name,
                        r.ToId,
                         r.CloseTransaction?.SysTime);
            }
        }
    }