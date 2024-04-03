# YATT, yet another traceback tracker
This project is intented to be used in case of personal need 
to track logs, exceptions etc. when Sentry 
or other services are considered as overkill. 
Currently to save logs data SQLite 
is used for convenience.

# Deployment
```bash
dotnet run
```

# API Description
At the moment there are only two endpoints:
* HTTP GET returns all logs sorted by descending creation date
* HTTP POST creates a log, simply pass a string


# Development plan
1. Api for logs creation and querying
2. OTP authorization
3. Refactoring: make possible to switch database easily