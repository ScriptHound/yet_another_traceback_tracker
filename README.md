# YATT, yet another traceback tracker
This project is intented to be used in case of personal need 
to track logs, exceptions etc. Currently to save logs data SQLite 
is used for convenience

# Deployment
```bash
dotnet run
```

# API Description
At the moment there are only two endpoints:
* HTTP GET returns all logs sorted by descending creation date
* HTTP POST creates a log, simply pass a string
