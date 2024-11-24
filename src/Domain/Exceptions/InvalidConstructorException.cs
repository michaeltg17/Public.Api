namespace Domain.Exceptions
{
    internal class InvalidConstructorException : InvalidOperationException
    {
        public InvalidConstructorException() : base("Use the parameterized constructor.") { }
    }
}
