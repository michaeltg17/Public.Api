using System;

namespace Application.Exceptions
{
    public class ImageApiException : Exception
    {
        public ImageApiException(string message) : base(message) { }
    }
}
