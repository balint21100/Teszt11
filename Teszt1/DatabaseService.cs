using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using Teszt1.Bakckend.Calsses;

namespace Teszt1
{
    

    public class DatabaseService
    {
        //private string connectionString = "Server=localhost;Database=FitTrck;Uid=root;Pwd=;Port=3306;";

        //public float GetTodayCalories(int userId)
        //{
        //    float totalKcal = 0;
        //    using (MySqlConnection conn = new MySqlConnection(connectionString))
        //    {
        //        conn.Open();
        //        string query = @"SELECT SUM(f.kcal * (me.qty / 100)) 
        //                         FROM dim_meal m
        //                         JOIN dim_mealentry me ON m.id = me.meal_id
        //                         JOIN dim_food f ON me.id = f.id
        //                         WHERE m.user_id = @uid AND m.date = CURDATE()";

        //        using (MySqlCommand cmd = new MySqlCommand(query, conn))
        //        {
        //            cmd.Parameters.AddWithValue("@uid", userId);
        //            var result = cmd.ExecuteScalar();
        //            if (result != DBNull.Value && result != null)
        //                totalKcal = Convert.ToSingle(result);
        //        }
        //    }
        //    return totalKcal;
        //}

        //// KERESÉS a dim_food táblában (LIKE operátorral)
        //public List<Food> SearchFoods(string searchText)
        //{
        //    var foods = new List<Food>();
        //    using (var conn = new MySqlConnection(connectionString))
        //    {
        //        conn.Open();
        //        string query = "SELECT id, name, kcal FROM dim_food WHERE name LIKE @search LIMIT 20";
        //        using (var cmd = new MySqlCommand(query, conn))
        //        {
        //            cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");
        //            using (var reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    foods.Add(new Food
        //                    {
        //                        Id = reader.GetInt32(0),
        //                        Name = reader.GetString(1),
        //                        Kcal = reader.GetInt32(2)
        //                    });
        //                }
        //            }
        //        }
        //    }
        //    return foods;
        //}

        //public void AddMealWithFood(int userId, string mealName, int foodId, float quantity)
        //{
        //    using (var conn = new MySqlConnection(connectionString))
        //    {
        //        conn.Open();
        //        // 1. Étkezés elnevezése (Reggeli, Ebéd stb.)
        //        string mealSql = "INSERT INTO dim_meal (user_id, date, name) VALUES (@uid, CURDATE(), @name)";
        //        var cmd1 = new MySqlConnector.MySqlCommand(mealSql, conn);
        //        cmd1.Parameters.AddWithValue("@uid", userId);
        //        cmd1.Parameters.AddWithValue("@name", mealName);
        //        cmd1.ExecuteNonQuery();
        //        long mid = cmd1.LastInsertedId;

        //        // 2. Étel hozzárendelése
        //        string entrySql = "INSERT INTO dim_mealentry (meal_id, food_id, qty) VALUES (@mid, @fid, @qty)";
        //        var cmd2 = new MySqlConnector.MySqlCommand(entrySql, conn);
        //        cmd2.Parameters.AddWithValue("@mid", mid);
        //        cmd2.Parameters.AddWithValue("@fid", foodId);
        //        cmd2.Parameters.AddWithValue("@qty", quantity);
        //        cmd2.ExecuteNonQuery();
        //    }
        //}

        //public List<string> GetLastThreeMeals(int userId)
        //{
        //    List<string> result = new List<string>();
        //    using (MySqlConnection conn = new MySqlConnection(connectionString))
        //    {
        //        conn.Open();
        //        string query = @"SELECT m.name, f.name, me.qty FROM dim_meal m 
        //                         JOIN dim_mealentry me ON m.id = me.meal_id 
        //                         JOIN dim_food f ON me.id = f.id 
        //                         WHERE m.user_id = @uid ORDER BY m.id DESC LIMIT 3";
        //        using (MySqlCommand cmd = new MySqlCommand(query, conn))
        //        {
        //            cmd.Parameters.AddWithValue("@uid", userId);
        //            using (MySqlDataReader reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    result.Add($"{reader.GetString(0)}: {reader.GetString(1)} ({reader.GetFloat(2)}g)");
        //                }
        //            }
        //        }
        //    }
        //    return result;
        //}
    }
}