using System;
namespace Shared
{

    public interface ILogCreator
    {
        ILog Create(string fileName);
    }
}
