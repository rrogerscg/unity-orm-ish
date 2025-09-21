using System;

namespace ORMish
{
    public interface IRecord
    {
        Guid Id { get;}
        string Type { get; }
        void Delete();
        void Put();
    }
}


