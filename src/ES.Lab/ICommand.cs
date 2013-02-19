using System;

namespace ES.Lab
{
    public interface ICommand
    {
        Guid EntityId { get; }
    }
}