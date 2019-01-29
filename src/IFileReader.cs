using System;

namespace Chorizo
{
    public interface IFileReader : IDisposable
    {
        string ReadToEnd();
    }
}