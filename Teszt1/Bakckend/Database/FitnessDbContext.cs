
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Calsses;

namespace Teszt1.Bakckend.Database
{
    public class FitnessDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Food> Foods { get; set; } // Összehangolva a Food.cs-vel
        public DbSet<Meal> Meals { get; set; }
        public DbSet<MealEntry> MealEntries { get; set; }
        public DbSet<WorkOut> Workouts { get; set; }
        public DbSet<WorkoutEntry> WorkoutEntries { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<UserBadge> UserBadges { get; set; }
        public DbSet<DailyLog> DailyLogs { get; set; }
        public DbSet<Weight> Weights { get; set; }
        public DbSet<Steps> Steps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- 1. ELSŐDLEGES KULCSOK (KEYS) ÉS TÁBLA NEVEK (MAPPING) ---
            // Kifejezetten kényszerítjük az Id mezőket kulcsként, hogy elkerüljük a korábbi hibát

            modelBuilder.Entity<User>(e => { e.ToTable("Dim_User"); e.HasKey(x => x.Id); });
            modelBuilder.Entity<Food>(e => { e.ToTable("Dim_Food"); e.HasKey(x => x.Id); });
            modelBuilder.Entity<Meal>(e => { e.ToTable("Dim_Meal"); e.HasKey(x => x.Id); });
            modelBuilder.Entity<MealEntry>(e => { e.ToTable("Dim_MealEntry"); e.HasKey(x => x.Id); });
            modelBuilder.Entity<WorkOut>(e => { e.ToTable("Dim_Workout"); e.HasKey(x => x.Id); });
            modelBuilder.Entity<WorkoutEntry>(e => { e.ToTable("Dim_WorkoutEntry"); e.HasKey(x => x.Id); });
            modelBuilder.Entity<Exercise>(e => { e.ToTable("Dim_Exercise"); e.HasKey(x => x.Id); });
            modelBuilder.Entity<Badge>(e => { e.ToTable("Dim_Badge"); e.HasKey(x => x.Id); });
            modelBuilder.Entity<UserBadge>(e => { e.ToTable("Dim_UserBadge"); e.HasKey(x => x.Id); });
            modelBuilder.Entity<DailyLog>(e => { e.ToTable("F_DailyLog"); e.HasKey(x => x.Id); });
            modelBuilder.Entity<Weight>(e => { e.ToTable("Dim_Weight"); e.HasKey(x => x.Id); });
            modelBuilder.Entity<Steps>(e => { e.ToTable("Dim_Steps"); e.HasKey(x => x.Id); });

            // --- 2. KAPCSOLATOK (FOREIGN KEYS) ÉS TÖRLÉSI SZABÁLYOK ---

            // User -> DailyLog (1:N)
            modelBuilder.Entity<DailyLog>()
                .HasOne(d => d.User)
                .WithMany(u => u.DailyLogs)
                .HasForeignKey(d => d.User_Id)
                .OnDelete(DeleteBehavior.Cascade);

            // User -> Workout (1:N)
            modelBuilder.Entity<WorkOut>()
                .HasOne(w => w.User)
                .WithMany(u => u.Workouts)
                .HasForeignKey(w => w.User_Id)
                .OnDelete(DeleteBehavior.Cascade);

            // User -> Meal (1:N)
            modelBuilder.Entity<Meal>()
                .HasOne(m => m.User)
                .WithMany(u => u.Meals)
                .HasForeignKey(m => m.User_Id)
                .OnDelete(DeleteBehavior.Cascade);

            // Workout -> WorkoutEntry (1:N)
            modelBuilder.Entity<WorkoutEntry>()
                .HasOne(we => we.Workout)
                .WithMany(w => w.Entries)
                .HasForeignKey(we => we.Workout_id)
                .OnDelete(DeleteBehavior.Cascade);

            // Meal -> MealEntry (1:N)
            modelBuilder.Entity<MealEntry>()
                .HasOne(me => me.Meal)
                .WithMany(m => m.Entries)
                .HasForeignKey(me => me.Meal_Id)
                .OnDelete(DeleteBehavior.Cascade);

            // User -> Weight (1:N)
            modelBuilder.Entity<Weight>()
                .HasOne(w => w.User)
                .WithMany()
                .HasForeignKey(w => w.User_Id)
                .OnDelete(DeleteBehavior.Cascade);

            // User -> Steps (1:N)
            modelBuilder.Entity<Steps>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.User_Id)
                .OnDelete(DeleteBehavior.Cascade);

            // --- 3. KÜLÖNLEGES KAPCSOLATOK (RESTRICT) ---

            modelBuilder.Entity<WorkoutEntry>()
                .HasOne(we => we.Exercise)
                .WithMany()
                .HasForeignKey(we => we.Exercise_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MealEntry>()
                .HasOne(me => me.food)
                .WithMany()
                .HasForeignKey(me => me.Food_Id)
                .OnDelete(DeleteBehavior.Restrict);

            // --- 4. USER - BADGE KAPCSOLÓTÁBLA ---

            modelBuilder.Entity<UserBadge>()
                .HasOne(ub => ub.User)
                .WithMany(u => u.UserBadges)
                .HasForeignKey(ub => ub.User_Id);

            modelBuilder.Entity<UserBadge>()
                .HasOne(ub => ub.Badge)
                .WithMany()
                .HasForeignKey(ub => ub.Badge_Id);
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // A kapcsolatleíró (Connection String) felépítése:
            // server: általában localhost vagy a géped IP címe
            // user: XAMPP-nál alapértelmezetten 'root'
            // password: XAMPP-nál alapértelmezetten üres ("")
            // database: a phpMyAdmin-ban létrehozott adatbázis neve (fitness_db)
            var connectionString = "server=localhost;user=root;password=;database=fittrck"; optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            // Ha telepítetted a Proxies csomagot, ez maradhat
            optionsBuilder.UseLazyLoadingProxies();

            base.OnConfiguring(optionsBuilder);
        }
    }
}
