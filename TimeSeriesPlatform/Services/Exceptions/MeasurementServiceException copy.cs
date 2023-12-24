namespace Iiroki.TimeSeriesPlatform.Services.Exceptions;

public class MeasurementServiceException(string msg, Exception? cause = null) : Exception(msg, cause) { }
