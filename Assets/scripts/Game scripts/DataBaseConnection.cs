using System.Threading.Tasks;
using UnityEngine;
using MySqlConnector;

public class DataBaseConnection : MonoBehaviour
{

    private MySqlConnection conn;

    async void Start()
    {
         string connStr = "Server=localhost;User ID=root;Password=123456;Database='gravitiventure db';";//在这里输入数据库名称和密码
        conn = new MySqlConnection(connStr);

        try
        {
            await conn.OpenAsync();
            Debug.Log("数据库连接成功");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("数据库连接失败：" + ex.Message);
        }
    }
    
    public async Task InsertGameDataAsync(float surviveTime, int score, float scorePerSec, int gravityCount)
    {
        if (conn == null || conn.State != System.Data.ConnectionState.Open)
        {
            Debug.LogError("数据库未连接，无法插入数据");
            return;
        }

        string sql = @"
            INSERT INTO `游戏数据统计表` (`存活时间`, `得分`, `每秒得分`, `改变重力次数`)
            VALUES (@survive_time, @score, @score_per_sec, @gravity_count)";

        try
        {
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@survive_time", surviveTime);
            cmd.Parameters.AddWithValue("@score", score);
            cmd.Parameters.AddWithValue("@score_per_sec", scorePerSec);
            cmd.Parameters.AddWithValue("@gravity_count", gravityCount);

            int rowsAffected = await cmd.ExecuteNonQueryAsync();
            Debug.Log($"插入成功，影响行数: {rowsAffected}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("插入数据失败：" + ex.Message);
        }
    }
}
