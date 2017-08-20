using System;

namespace ProcessPlayer.Data.Expressions
{
    public class PegException : Exception
    {
        public PegException(string fileName, string errorType, string msg, int line, int column)
            : base(msg)
        {
            FileName = fileName;
            ErrorType = errorType;
            Line = line;
            Column = column;
        }

        public string FileName { get; private set; }

        public string ErrorType { get; private set; }

        public int Line { get; private set; }

        public int Column { get; private set; }

        public override string ToString()
        {
            return FormatError(FileName, ErrorType, Message, Line, Column);
        }

        public static string FormatError(string fileName, string errorType, string msg, int lineNo, int colNo)
        {
            return string.Format("{0}({3},{4}): {1}: {2}", fileName, errorType, msg, lineNo, colNo);
        }
    }
}
