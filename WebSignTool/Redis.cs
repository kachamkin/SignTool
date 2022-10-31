using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Data;

namespace WebSignTool
{
    public interface IRedis
    {
        public Task AddRecord(DateTime _Date, string? _Host, string? _Path);
        public Task AddRecord(string Key, string Value);
        public Task<EnumerableRowCollection> GetRecords();
        public string? GetRecord(string key);


    }

    public class Redis : IRedis
    {
        private readonly IConfiguration configuration;
        private readonly IConnectionMultiplexer? cm;
        private readonly IDatabase? db;
        public Redis(IConfiguration _config)
        {
            configuration = _config;
            if (configuration.GetSection("Options").GetValue<string>("DataBaseType") == "Redis")
            {

                cm = Task<ConnectionMultiplexer>.Run(() => ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis"))).Result;
                db = cm.GetDatabase();
            }
        }

        public async Task AddRecord(DateTime _Date, string? _Host, string? _Path)
        {
            string server = configuration.GetConnectionString("Redis");
            server = server[..server.IndexOf(",")];

            IServer iServer = cm.GetServer(server);
            if (await iServer.DatabaseSizeAsync() >= configuration.GetSection("Options").GetValue<long>("MaxLogSizeRedis"))
                foreach (RedisKey key in iServer.Keys(pattern: "*"))
                    if (key != "awaitid")
                        await db.StringGetDeleteAsync(key);

            await db.StringSetAsync(DateTime.UtcNow.Ticks.ToString(), _Date + ": host " + _Host + ", path: " + _Path);
        }

        public async Task AddRecord(string Key, string Value)
        {
            string server = configuration.GetConnectionString("Redis");
            server = server[..server.IndexOf(",")];

            IServer iServer = cm.GetServer(server);
            if (await iServer.DatabaseSizeAsync() >= configuration.GetSection("Options").GetValue<long>("MaxLogSizeRedis"))
                foreach (RedisKey key in iServer.Keys(pattern: "*"))
                    if (key != "awaitid")
                        await db.StringGetDeleteAsync(key);

            await db.StringSetAsync(Key, Value);
        }

        public string? GetRecord(string key)
        {
            return db?.StringGet(key);
        }

        public async Task<EnumerableRowCollection> GetRecords()
        {
            DataTable dt = new();
            dt.Columns.Add("key", typeof(string));
            dt.Columns.Add("value", typeof(string));

            string server = configuration.GetConnectionString("Redis");
            server = server[..server.IndexOf(",")];

            foreach (RedisKey key in cm.GetServer(server).Keys(pattern: "*"))
            {
                if (key != "awaitid")
                {
                    DataRow row = dt.Rows.Add();
                    row["key"] = key;
                    row["value"] = await db.StringGetAsync(key);
                }
            }

            return dt.AsEnumerable().OrderBy(r => r["key"]);
        }
    }
}
