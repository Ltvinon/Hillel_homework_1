public class CalcInvalidInputException : Exception
{
    public CalcInvalidInputException() : base("Invalid characters in input string.") { }
}

public class OperationSignAtLineStartException : Exception
{
    public OperationSignAtLineStartException() : base("Operation sign at the start of the line.") { }
}

public class OperationSignAtLineEndException : Exception
{
    public OperationSignAtLineEndException() : base("Operation sign at the end of the line.") { }
}

public class SeveralOperationInARowException : Exception
{
    public SeveralOperationInARowException() : base("Several operation signs in a row.") { }
}

public class ParenthesisException : Exception
{
    public ParenthesisException(string message) : base(message) { }
}