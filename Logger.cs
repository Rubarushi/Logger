using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Logger
{

	internal class ConsoleContents
	{
		public string Message = string.Empty;
		public Type mType = null;
		public ConsoleColor ForeGroundColor = default;
		public ConsoleColor BackGroundColor = default;
		public bool WriteLine = true;
	}

	public class Logger
	{
		public static void StartLogger()
		{
			Task.Factory.StartNew(() =>
			{
				foreach (ConsoleContents CC in Collections.GetConsumingEnumerable())
				{
					if (Console.BackgroundColor != CC.BackGroundColor)
					{
						Console.BackgroundColor = CC.BackGroundColor;
					}

					if (Console.ForegroundColor != CC.ForeGroundColor)
					{
						Console.ForegroundColor = CC.ForeGroundColor;
					}

					if (CC.WriteLine)
					{
						CC.Message += "\r\n";
					}

					Console.Write(CC.Message);
				}
			});
		}

		private ConsoleColor BackColor = default;
		private ConsoleColor ForeColor = default;
		private Type type;

		public Logger(Type type, ConsoleColor BackColor = default, ConsoleColor ForeColor = default)
        {
            this.BackColor = BackColor;
            this.ForeColor = ForeColor;
			this.type = type;

			LoggerDic.TryAdd(type, this);
		}

		public static void RemoveLogger(Type type)
        {
			LoggerDic.TryRemove(type, out _);
        }

		public static Logger GetLogger(Type type)
        {
			return LoggerDic[type];
        }

		private static BlockingCollection<ConsoleContents> Collections = new BlockingCollection<ConsoleContents>();
		private static ConcurrentDictionary<Type, Logger> LoggerDic = new ConcurrentDictionary<Type, Logger>();

		public void WriteLine(string msg, params object[] args)
		{
			WriteLine(string.Format(msg, args));
		}

		public void WriteLine(string msg)
		{
            ConsoleContents CC = new ConsoleContents
            {
                Message = msg,
                mType = type,
                WriteLine = true,
                ForeGroundColor = ForeColor,
                BackGroundColor = BackColor
            };

            Collections.Add(CC);
		}

		public void Write(string msg, params object[] args)
		{
			Write(string.Format(msg, args));
		}

		public void Write(string msg)
		{
            ConsoleContents CC = new ConsoleContents
            {
                Message = msg,
                mType = type,
                WriteLine = false,
                ForeGroundColor = ForeColor,
                BackGroundColor = BackColor
            };
            Collections.Add(CC);
		}
	}
}