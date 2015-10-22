using System;

namespace ContosoUniversity.Logging
{
	public interface ILogger
	{
		void Information(string message);
		void Information(string fmt, params object[] vars);
		void Information(Exception exception, string fmt, params object[] vars);

		void Warning(string messasge);
		void Warning(string fmt, params object[] vars);
		void Warning(Exception exception, string fmt, params object[] vars);

		void Error(string messasge);
		void Error(string fmt, params object[] vars);
		void Error(Exception exception, string fmt, params object[] vars);

		void TraceApi(string componentName, string method, TimeSpan timespan);
		void TraceApi(string componentName, string method, TimeSpan timespan, string properties);
		void TraceApi(string componentName, string method, TimeSpan timespan, string fmt, params object[] vars);
	}
}
