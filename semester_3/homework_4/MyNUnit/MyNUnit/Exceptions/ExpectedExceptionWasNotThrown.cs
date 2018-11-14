﻿using System;

namespace MyNUnit.Exceptions
{
    /// <summary>
    /// Исключение, ожидаемое при выполнении метода, не было брошено.
    /// </summary>
    public class ExpectedExceptionWasNotThrown : Exception
    {
        public override string Message { get; } = "ожидаемое исключение не было брошено";
    }
}
