namespace Iiroki.TimeSeriesPlatform.Services.Exceptions;

public class MetadataServiceException(string msg, Exception? cause = null) : Exception(msg, cause) { }
