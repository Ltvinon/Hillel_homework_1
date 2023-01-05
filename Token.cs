namespace Hillel_homework_1
{
    /// <summary>
    /// Базовый класс токена.
    /// </summary>
    public class Token
    {
        private int _precedence;

        /// <summary>
        /// Приоритет выполнения операции.
        /// </summary>
        public int Precedence
        {
            get => _precedence;
        }

        public Token(int precedence)
        {
            _precedence = precedence;
        }
    }

    /// <summary>
    /// Делегат операции вычисления для токенов операций.
    /// </summary>
    public delegate double ComputeDelegate(double x, double y);

    /// <summary>
    /// Перечисление названий (имён) операций.
    /// </summary>
    public enum OperationName
    {
        Addition,
        Substraction,
        Multiply,
        Division,
    }

    /// <summary>
    /// Токен операции.
    /// </summary>
    public class Operation : Token
    {
        private OperationName _name;

        /// <summary>
        /// Название (имя) операции.
        /// </summary>
        public OperationName Name
        {
            get => _name;
        }

        /// <summary>
        /// Делегат операции вычисления.
        /// </summary>
        public ComputeDelegate Compute;

        public Operation(int precedence, OperationName operationName, ComputeDelegate compute) : base(precedence)
        {
            _name = operationName;
            Compute = compute;
        }

    }

    /// <summary>
    /// Токен числа
    /// </summary>
    public class Number : Token
    {
        private double _value;

        /// <summary>
        /// Значение числа.
        /// </summary>
        public double Value
        {
            get => _value;
            set { _value = value; }
        }

        public Number(double value, int precedence = 0) : base(precedence)
        {
            _value = value;
        }
    }

    /// <summary>
    /// Перечисление ориентаций скобок для токенов скобок.
    /// </summary>
    public enum ParenthesisOrientation
    {
        Left, Right,
    }

    /// <summary>
    /// Токен скобок.
    /// </summary>
    public class Parenthesis : Token
    {
        private ParenthesisOrientation _orientation;

        /// <summary>
        /// Ориентация скобки.
        /// </summary>
        public ParenthesisOrientation Orientation
        {
            get => _orientation;
        }

        public Parenthesis(ParenthesisOrientation orientation, int precedence = 0) : base(precedence)
        {
            _orientation = orientation;
        }


    }

}