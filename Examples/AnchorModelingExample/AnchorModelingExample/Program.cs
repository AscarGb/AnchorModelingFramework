using AnchorModeling.Entities;
using AnchorModelingExample.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AnchorModelingExample
{
    class Program
    {
        private const string TomName = "Tom";
        private const string GaryName = "Gary";

        static async Task Main(string[] args)
        {
            using (var db = new MyDbContext())
            {
                await db.Database.EnsureDeletedAsync();
                await db.Database.EnsureCreatedAsync();
            }

            await GiveTomWorkingComputer();
            await UpdateTomsWorkingComputer();

            await GiveGaryWorkingComputer();
            await UpdateGarysWorkingComputer();

            Console.WriteLine("Tom's video card history:");
            await WriteChangesTomsWorkingGraphicsCard();
            Console.WriteLine("Garys's video card history:");
            await WriteChangesGarysWorkingGraphicsCard();
        }

        static async Task GiveTomWorkingComputer()
        {
            using var db = new MyDbContext();
            var source = new Source
            {
                Name = "Company A"
            };

            db.Sources.Add(source);

            var transaction = new Transaction
            {
                Source = source,
                SysTime = DateTime.UtcNow,
                User = "Admin"
            };

            using var dbTransaction = await db.BeginTransactionAsync(transaction);
            var processor = new Processor { Name = "AMD Athlon X4 840 OEM" };
            var motherBoard = new MotherBoard { Name = "MSI A68HM-E33 V2" };
            var videoCard = new VideoCard { Name = "Palit GeForce GT 710 Silent LP [NEAT7100HD46-2080H]" };
            var ram1 = new RAM { Name = "AMD Radeon R5 Entertainment Series [R532G1601U1S-U]" };
            var ram2 = new RAM { Name = "Patriot Signature [PSD32G16002]" };
            var soundCard = new SoundCard { Name = "ORIENT AU-01N" };

            db.Processors.Add(processor);
            db.MotherBoards.Add(motherBoard);
            db.VideoCards.Add(videoCard);
            db.RAMs.Add(ram1);
            db.RAMs.Add(ram2);
            db.SoundCards.Add(soundCard);
            await db.SaveChangesAsync();

            var workComputer = new Computer
            {              
                Processor = processor,
                MotherBoard = motherBoard,
                VideoCard = videoCard,
                RAM1 = ram1,
                RAM2 = ram2,
                SoundCard = soundCard
            };
            db.Computers.Add(workComputer);
            await db.SaveChangesAsync();

            var tom = new User
            {              
                Name = TomName,
                WorkComputer = workComputer             
            };
            db.Users.Add(tom);

            await db.SaveChangesAsync();
            dbTransaction.Commit();
        }

        static async Task UpdateTomsWorkingComputer()
        {
            using var db = new MyDbContext();
            var source = new Source
            {
                Name = "Tom"
            };

            db.Sources.Add(source);

            var transaction = new Transaction
            {
                Source = source,
                SysTime = DateTime.UtcNow,
                User = "Tom"
            };

            using var dbTransaction = await db.BeginTransactionAsync(transaction);
            var processor = new Processor { Name = "AMD A8-7680 OEM" };
            var motherBoard = new MotherBoard { Name = "ASRock FM2A68M-HD+" };
            var videoCard = new VideoCard { Name = "MSI GeForce GT 730 [N730-2GD3V2]" };
            var ram1 = new RAM { Name = "Kingston ValueRAM [KVR16N11S8/4]" };
            var ram2 = new RAM { Name = "Kingston ValueRAM [KVR16N11S8/4]" };
            var soundCard = new SoundCard { Name = "ORICO SC2-BK" };

            db.Processors.Add(processor);
            db.MotherBoards.Add(motherBoard);
            db.VideoCards.Add(videoCard);
            db.RAMs.Add(ram1);
            db.RAMs.Add(ram2);
            db.SoundCards.Add(soundCard);
            await db.SaveChangesAsync();

            var workComputer = (await db.Users.Include(a => a.WorkComputer).FirstAsync(a => a.Name == TomName)).WorkComputer;
            workComputer.Processor = processor;
            workComputer.MotherBoard = motherBoard;
            workComputer.VideoCard = videoCard;
            workComputer.RAM1 = ram1;
            workComputer.RAM2 = ram2;
            workComputer.SoundCard = soundCard;

            await db.SaveChangesAsync();
            dbTransaction.Commit();
        }

        static async Task WriteChangesTomsWorkingGraphicsCard()
        {
            using (var db = new MyDbContext())
            {
                var tom = await db.Users.FirstOrDefaultAsync(a => a.Name == TomName);                

                (await (from t_H_Users_WorkComputer_to_Computer in db.T_H_Users_WorkComputer_to_Computers
                        join t_H_Computer_VideoCard_to_VideoCards in db.T_H_Computers_VideoCard_to_VideoCards on t_H_Users_WorkComputer_to_Computer.ToId equals t_H_Computer_VideoCard_to_VideoCards.A_Id
                        join p_VideoCards_Name in db.P_VideoCards_Name on t_H_Computer_VideoCard_to_VideoCards.ToId equals p_VideoCards_Name.A_Id
                        where t_H_Users_WorkComputer_to_Computer.A_Id == tom.Id
                        select p_VideoCards_Name.Value).ToListAsync())
                 .ForEach(a => Console.WriteLine(a));
            }
        }

        static async Task GiveGaryWorkingComputer()
        {
            using var db = new MyDbContext();
            var source = new Source
            {
                Name = "Company A"
            };

            db.Sources.Add(source);

            var transaction = new Transaction
            {
                Source = source,
                SysTime = DateTime.UtcNow,
                User = "Admin"
            };

            using var dbTransaction = await db.BeginTransactionAsync(transaction);
            var processor = new Processor { Name = "AMD Athlon X4 840 OEM" };
            var motherBoard = new MotherBoard { Name = "MSI A68HM-E33 V2" };
            var videoCard = new VideoCard { Name = "GIGABYTE GeForce GT 710 [GV-N710D5-1GL Rev2.0]" };
            var ram1 = new RAM { Name = "AMD Radeon R5 Entertainment Series [R532G1601U1S-U]" };
            var ram2 = new RAM { Name = "Patriot Signature [PSD32G16002]" };
            var soundCard = new SoundCard { Name = "ORIENT AU-01N" };

            db.Processors.Add(processor);
            db.MotherBoards.Add(motherBoard);
            db.VideoCards.Add(videoCard);
            db.RAMs.Add(ram1);
            db.RAMs.Add(ram2);
            db.SoundCards.Add(soundCard);
            await db.SaveChangesAsync();

            var workComputer = new Computer
            {
                Processor = processor,
                MotherBoard = motherBoard,
                VideoCard = videoCard,
                RAM1 = ram1,
                RAM2 = ram2,
                SoundCard = soundCard
            };
            db.Computers.Add(workComputer);
            await db.SaveChangesAsync();

            var tom = new User
            {
                Name = GaryName,
                WorkComputer = workComputer
            };
            db.Users.Add(tom);

            await db.SaveChangesAsync();
            dbTransaction.Commit();
        }

        static async Task UpdateGarysWorkingComputer()
        {
            using var db = new MyDbContext();
            var source = new Source
            {
                Name = "Tom"
            };

            db.Sources.Add(source);

            var transaction = new Transaction
            {
                Source = source,
                SysTime = DateTime.UtcNow,
                User = "Tom"
            };

            using var dbTransaction = await db.BeginTransactionAsync(transaction);
            var processor = new Processor { Name = "AMD A8-7680 OEM" };
            var motherBoard = new MotherBoard { Name = "ASRock FM2A68M-HD+" };
            var videoCard = new VideoCard { Name = "MSI AMD Radeon R7 240 LP [R7 240 2GD3 64bit LP]" };
            var ram1 = new RAM { Name = "Kingston ValueRAM [KVR16N11S8/4]" };
            var ram2 = new RAM { Name = "Kingston ValueRAM [KVR16N11S8/4]" };
            var soundCard = new SoundCard { Name = "ORICO SC2-BK" };

            db.Processors.Add(processor);
            db.MotherBoards.Add(motherBoard);
            db.VideoCards.Add(videoCard);
            db.RAMs.Add(ram1);
            db.RAMs.Add(ram2);
            db.SoundCards.Add(soundCard);
            await db.SaveChangesAsync();

            var workComputer = (await db.Users.Include(a => a.WorkComputer).FirstAsync(a => a.Name == GaryName)).WorkComputer;
            workComputer.Processor = processor;
            workComputer.MotherBoard = motherBoard;
            workComputer.VideoCard = videoCard;
            workComputer.RAM1 = ram1;
            workComputer.RAM2 = ram2;
            workComputer.SoundCard = soundCard;

            await db.SaveChangesAsync();
            dbTransaction.Commit();
        }

        static async Task WriteChangesGarysWorkingGraphicsCard()
        {
            using (var db = new MyDbContext())
            {
                var tom = await db.Users.FirstOrDefaultAsync(a => a.Name == GaryName);

                (await (from t_H_Users_WorkComputer_to_Computer in db.T_H_Users_WorkComputer_to_Computers
                        join t_H_Computer_VideoCard_to_VideoCards in db.T_H_Computers_VideoCard_to_VideoCards on t_H_Users_WorkComputer_to_Computer.ToId equals t_H_Computer_VideoCard_to_VideoCards.A_Id
                        join p_VideoCards_Name in db.P_VideoCards_Name on t_H_Computer_VideoCard_to_VideoCards.ToId equals p_VideoCards_Name.A_Id
                        where t_H_Users_WorkComputer_to_Computer.A_Id == tom.Id
                        select p_VideoCards_Name.Value).ToListAsync())
                 .ForEach(a => Console.WriteLine(a));
            }
        }
    }
}