using System;

namespace PowerSQL.Exceptions
{
    public interface IPrintExceptions
    {
        void printException(Exception ex);
    }
}