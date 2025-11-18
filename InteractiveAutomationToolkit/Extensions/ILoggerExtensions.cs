namespace Skyline.DataMiner.Utils.InteractiveAutomationScript.Extensions
{
	using System;
	using Microsoft.Extensions.Logging;

	internal static class ILoggerExtensions
	{
		public static void Trace(this ILogger logger, string className, string methodName, string message)
		{
			if (logger == null)
				return;

			logger.LogTrace("{0}.{1}|{2}", className, methodName, message);
		}

		public static void Debug(this ILogger logger, string className, string methodName, string message)
		{
			if (logger == null)
				return;

			logger.LogDebug("{0}.{1}|{2}", className, methodName, message);
		}

		public static void Information(this ILogger logger, string className, string methodName, string message)
		{
			if (logger == null)
				return;

			logger.LogInformation("{0}.{1}|{2}", className, methodName, message);
		}

		public static void Error(this ILogger logger, string className, string methodName, string message)
		{
			if (logger == null)
				return;

			logger.LogError("{0}.{1}|{2}", className, methodName, message);
		}
	}
}
