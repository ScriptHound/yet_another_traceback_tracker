using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Yet_Another_Traceback_Tracker.Models;
using Yet_Another_Traceback_Tracker.Services;

namespace Yet_Another_Traceback_Tracker.Controllers;

[ApiController]
[Route("[controller]")]
public class LogsInfoController
{
    private readonly ILogService _logService;

    public LogsInfoController(ILogService logService)
    {
        _logService = logService;
    }

    [HttpGet]
    public IEnumerable<LogEntity> Get()
    {
        return _logService.GetLogRecords();
    }
    
    [HttpPost]
    public ActionResult<string> Post([FromBody] string logContext)
    {
        _logService.CreateLogRecord(logContext);

        return new OkResult();
    }
}