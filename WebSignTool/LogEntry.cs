using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebSignTool
{
    public class LogEntry
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string MongoId { get; set; } = "";
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Host { get; set; }
        public string? Path { get; set; }
        public override string ToString()
        {
            return Date + " " + Host + "    " + Path + "\r\n";
        }
        public LogEntry(DateTime _Date, string? _Host, string? _Path)
        {
            Date = _Date;
            Host = _Host;
            Path = _Path;
        }
        public LogEntry()
        { }
    }

    public interface ISql
    {
        public void AddRecord(DateTime _Date, string? _Host, string? _Path);
        public IEnumerable<LogEntry> GetRecords();
    }

    public class LogContext : DbContext, ISql
    {
        private readonly IConfiguration Configuration;
        public LogContext(IConfiguration configuration)
        {
            Configuration = configuration;
            if (Configuration.GetSection("Options").GetValue<string>("DataBaseType") == "SQL")
                Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (Configuration.GetSection("Options").GetValue<string>("DataBaseType") == "SQL")
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("LocalDB"));
        }
        public DbSet<LogEntry> LogEntries => Set<LogEntry>();

        public void AddRecord(DateTime _Date, string? _Host, string? _Path)
        {
            LogEntries.Add(new(_Date, _Host, _Path));
            SaveChanges();
        }

        public IEnumerable<LogEntry> GetRecords()
        {
            LogEntries.UpdateRange(LogEntries);
            return LogEntries;
        }

        public IEnumerable<LogEntry> GetRecords(DateTime from, DateTime to)
        {
            LogEntries.UpdateRange(LogEntries);
            return LogEntries.Where(e => e.Date >= from && e.Date <= to);
        }

    }

}
