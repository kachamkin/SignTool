using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace WebSignTool
{
    public class LogEntry
    {
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

    public class LogContext : DbContext
    {
        private readonly IConfiguration Configuration;
        public LogContext(IConfiguration configuration)
        {
            Configuration = configuration;
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
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
    }

}
