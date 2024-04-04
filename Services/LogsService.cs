using System.Collections;
using Yet_Another_Traceback_Tracker.Models;

namespace Yet_Another_Traceback_Tracker.Services;

public interface ILogService
{
    public void CreateLogRecord(string logContent);
    public IEnumerable<LogEntity> GetLogRecords();
    public IEnumerable<LogEntity> GetLogRecords(DateTime dateSince, DateTime dateUntil);
}

public class LogService : ILogService
{
    public void CreateLogRecord(string logContent)
    {
        using (var context = new LogsContext())
        {
            context.MyEntities.Add(new LogEntity
            {
                LogContent = logContent
            });
            context.SaveChanges();
        }
    }
    
    public IEnumerable<LogEntity> GetLogRecords()
    {
        using (var context = new LogsContext())
        {
            return context.MyEntities.OrderByDescending(log => log.CreatedDate).ToArray();
        }
    }

    public IEnumerable<LogEntity> GetLogRecords(DateTime dateSince, DateTime dateUntil)
    {
        using (var context = new LogsContext())
        {
            return context.MyEntities.Where(
                    log => dateSince <= log.CreatedDate && dateUntil >= log.CreatedDate)
                .ToArray();
        }
    }
}