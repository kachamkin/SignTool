using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core;

namespace WebSignTool
{
    public interface IMongo
    {
        public Task AddRecord(DateTime _Date, string? _Host, string? _Path);
        public Task<IEnumerable<LogEntry>> GetRecords();
    }

    public class Mongo : IMongo
    {
        private readonly IConfiguration Configuration;
        private readonly IMongoDatabase? db;
        public Mongo(IConfiguration _config)
        {
            Configuration = _config;
            if (Configuration.GetSection("Options").GetValue<string>("DataBaseType") == "Mongo")
                db = new MongoClient(MongoClientSettings.FromConnectionString(Configuration.GetConnectionString("Mongo"))).GetDatabase(Configuration.GetSection("Options").GetValue<string>("MongoDatabase"));
        }

        public async Task AddRecord(DateTime _Date, string? _Host, string? _Path) =>
            await db.GetCollection<LogEntry>("LogEntries").InsertOneAsync(new LogEntry(_Date, _Host, _Path));

        public async Task<IEnumerable<LogEntry>> GetRecords()
        {
            using IAsyncCursor<LogEntry> ac = await db.GetCollection<LogEntry>("LogEntries").FindAsync<LogEntry>("{}");
            return (await ac.ToListAsync<LogEntry>()).OrderBy(r => r.Date);
        }
    }
}
