# Logger
A SimpleLogger for C#

# Usuage
```cs
Logger logger = new Logger(typeof(Program), ConsoleColor.Black, ConsoleColor.White);
logger.Write("Hello");
logger.WriteLine("World");

Logger.RemoveLogger(typeof(Program));
```
