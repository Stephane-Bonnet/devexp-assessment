namespace DevexpAssessment.Exceptions
{
    // TODO: Add more specific exception types as needed: ApiConnectionException, ApiTimeoutException, ApiUnauthorizedException, ApiSerializationException, etc
    // For HTTP exceptions, some sort of factory: ApiExceptionFactory.Create(response)

    public class DevexpAssessmentException : Exception
    {
        public DevexpAssessmentException(string message) : base(message) { }
    }

    public class DevexpAssessmentUnexpectedException : Exception
    {
        public DevexpAssessmentUnexpectedException(string message) : base(message) { }
        public DevexpAssessmentUnexpectedException(Exception innerException) : base("An unexpected error occured", innerException) { }
    }
}
