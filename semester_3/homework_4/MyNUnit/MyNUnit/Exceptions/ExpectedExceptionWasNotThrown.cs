using System;

namespace MyNUnit.Exceptions
{
    public class ExpectedExceptionWasNotThrown : Exception
    {
        public override string Message { get; } = "ожидаемое исключение не было брошено";
    }
}
