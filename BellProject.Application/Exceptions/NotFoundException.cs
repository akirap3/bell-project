using System;

namespace BellProject.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public string? ResourceName { get; }
        public object? ResourceKey { get; }

        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public NotFoundException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
            ResourceName = name;
            ResourceKey = key;
        }
    }
}
