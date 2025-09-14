using System;

namespace ORMish
{
    public interface IRecord
    {
        Guid Id { get;}
        void Delete();
        void Put();
    }
}


