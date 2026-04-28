using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using Teszt1.Bakckend.Calsses;
using Teszt1.Frontend;

namespace Teszt1
{
    public class DatabaseService
    {
        private string connectionString = "Server=localhost;Database=FitTrck;Uid=root;Pwd=;Port=3306;";

        public float GetTodayCalories(int userId)
        {
            float totalKcal = 0;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT SUM(f.kcal * (me.qty / 100)) 
                                 FROM dim_meal m
                                 JOIN dim_mealentry me ON m.id = me.meal_id
                                 JOIN dim_food f ON me.id = f.id
                                 WHERE m.user_id = @uid AND m.date = CURDATE()";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@uid", userId);
                    var result = cmd.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                        totalKcal = Convert.ToSingle(result);
                }
            }
            return totalKcal;
        }

        // KERESÉS a dim_food táblában (LIKE operátorral)
        public List<Food> SearchFoods(string searchText)
        {
            var foods = new List<Food>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT id, name, kcal FROM dim_food WHERE name LIKE @search LIMIT 20";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            foods.Add(new Food
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Kcal = reader.GetInt32(2)
                            });
                        }
                    }
                }
            }
            return foods;
        }

        public void AddMealWithFood(int userId, string mealName, int foodId, float quantity)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                // 1. Étkezés elnevezése (Reggeli, Ebéd stb.)
                string mealSql = "INSERT INTO dim_meal (user_id, date, name) VALUES (@uid, CURDATE(), @name)";
                // FIGYELJ IDE: MySqlCommand kell ide!
                var cmd1 = new MySqlCommand(mealSql, conn);
                cmd1.Parameters.AddWithValue("@uid", userId);
                cmd1.Parameters.AddWithValue("@name", mealName);
                cmd1.ExecuteNonQuery();
                long mid = cmd1.LastInsertedId;

                // 2. Étel hozzárendelése
                string entrySql = "INSERT INTO dim_mealentry (meal_id, food_id, qty) VALUES (@mid, @fid, @qty)";
                // FIGYELJ IDE: Ide is MySqlCommand kell!
                var cmd2 = new MySqlCommand(entrySql, conn);
                cmd2.Parameters.AddWithValue("@mid", mid);
                cmd2.Parameters.AddWithValue("@fid", foodId);
                cmd2.Parameters.AddWithValue("@qty", quantity);
                cmd2.ExecuteNonQuery();
            }
        }

        public List<string> GetLastThreeMeals(int userId)
        {
            List<string> result = new List<string>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT m.name, f.name, me.qty FROM dim_meal m 
                                 JOIN dim_mealentry me ON m.id = me.meal_id 
                                 JOIN dim_food f ON me.id = f.id 
                                 WHERE m.user_id = @uid ORDER BY m.id DESC LIMIT 3";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@uid", userId);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add($"{reader.GetString(0)}: {reader.GetString(1)} ({reader.GetFloat(2)}g)");
                        }
                    }
                }
            }
            return result;
        }

        public void AddWorkoutEntry(int workoutId, int exerciseId, int sets, int reps, float weight)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO dim_workoutentry (workout_id, exercise_id, sets, reps, weight) " +
                             "VALUES (@wid, @eid, @sets, @reps, @weight)";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@wid", workoutId);
                    cmd.Parameters.AddWithValue("@eid", exerciseId);
                    cmd.Parameters.AddWithValue("@sets", sets);
                    cmd.Parameters.AddWithValue("@reps", reps);
                    cmd.Parameters.AddWithValue("@weight", weight);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public long CreateWorkout(int userId, string dayName, string workoutName)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO dim_workout (user_id, date, name) VALUES (@uid, CURDATE(), @name)";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@uid", userId);
                    cmd.Parameters.AddWithValue("@name", $"{dayName} - {workoutName}");
                    cmd.ExecuteNonQuery();
                    return cmd.LastInsertedId;
                }
            }
        }

        public void SaveWorkoutPlan(int userId, string planName, List<WorkoutDay> days)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                // Végigmegyünk azokon a napokon, amiket a felhasználó kitöltött
                foreach (var day in days)
                {
                    // Minden kitöltött naphoz létrehozunk egy bejegyzést a dim_workout táblába
                    string sql = "INSERT INTO dim_workout (user_id, date, name) VALUES (@uid, CURDATE(), @name)";
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@uid", userId);

                        // A 'name' mezőbe mentjük a sablon nevét, a napot, és a beírt tervet is!
                        // Pl: "Tömegnövelő (Hétfő): Fekvenyomás 4x10"
                        cmd.Parameters.AddWithValue("@name", $"{planName} ({day.DayName}): {day.PlannedExercise}");

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        // EZZEL TÖLTJÜK BE A FŐOLDALT INDULÁSKOR!
        public async Task<List<string>> GetNapiEdzesekAsync(int felhasznaloId, string nap)
        {
            var eredmeny = new List<string>();
            using (var conn = new MySqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = "SELECT Nev FROM edzesek WHERE FelhasznaloId = @Fid AND Nap = @Nap";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Fid", felhasznaloId);
                    cmd.Parameters.AddWithValue("@Nap", nap);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            eredmeny.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return eredmeny;
        }

        // JAVÍTOTT MENTÉS (Golyóálló ID lekérés!)
        public async Task UjEdzesMenteseAsync(int felhasznaloId, string nap, string edzesNev, List<string> gyakorlatok)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                await conn.OpenAsync();

                // 1. Lépés: Elmentjük az edzést, és AZONNAL kikérjük a pontos ID-t
                string insertEdzesQuery = "INSERT INTO edzesek (FelhasznaloId, Nap, Nev) VALUES (@FelhasznaloId, @Nap, @Nev); SELECT LAST_INSERT_ID();";
                long edzesId = 0;

                using (var cmd = new MySqlCommand(insertEdzesQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@FelhasznaloId", felhasznaloId);
                    cmd.Parameters.AddWithValue("@Nap", nap);
                    cmd.Parameters.AddWithValue("@Nev", edzesNev);

                    // A Scalar garantáltan visszaadja az ID-t
                    var result = await cmd.ExecuteScalarAsync();
                    edzesId = Convert.ToInt64(result);
                }

                // 2. Lépés: Elmentjük a szetteket
                if (edzesId > 0 && gyakorlatok != null && gyakorlatok.Count > 0)
                {
                    string insertGyakorlatQuery = "INSERT INTO edzes_gyakorlatok (EdzesId, GyakorlatSzoveg) VALUES (@EdzesId, @GyakorlatSzoveg);";

                    foreach (var gyakorlat in gyakorlatok)
                    {
                        using (var cmd = new MySqlCommand(insertGyakorlatQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@EdzesId", edzesId);
                            cmd.Parameters.AddWithValue("@GyakorlatSzoveg", gyakorlat);
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
        }

        // 2. EDZÉS TÖRLÉSE (A Cascade miatt a szettek is törlődnek automatikusan!)
        public async Task EdzesTorleseAsync(int felhasznaloId, string nap, string edzesNev)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string deleteQuery = "DELETE FROM edzesek WHERE FelhasznaloId = @FelhasznaloId AND Nap = @Nap AND Nev = @Nev;";

                using (var cmd = new MySqlCommand(deleteQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@FelhasznaloId", felhasznaloId);
                    cmd.Parameters.AddWithValue("@Nap", nap);
                    cmd.Parameters.AddWithValue("@Nev", edzesNev);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // 3. MEGLÉVŐ EDZÉS LEKÉRDEZÉSE (Ez kell a szerkesztő ablaknak!)
        public async Task<List<string>> GetEdzesGyakorlatokAsync(int felhasznaloId, string nap, string edzesNev)
        {
            var eredmeny = new List<string>();
            using (var conn = new MySqlConnection(connectionString))
            {
                await conn.OpenAsync();

                string query = @"
                    SELECT eg.GyakorlatSzoveg 
                    FROM edzesek e
                    JOIN edzes_gyakorlatok eg ON e.Id = eg.EdzesId
                    WHERE e.FelhasznaloId = @Fid AND e.Nap = @Nap AND e.Nev = @Nev";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Fid", felhasznaloId);
                    cmd.Parameters.AddWithValue("@Nap", nap);
                    cmd.Parameters.AddWithValue("@Nev", edzesNev);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            eredmeny.Add(reader.GetString("GyakorlatSzoveg"));
                        }
                    }
                }
            }
            return eredmeny;
        }
    }
}