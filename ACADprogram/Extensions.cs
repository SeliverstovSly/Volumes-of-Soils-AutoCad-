using System;
using System.Text;

namespace ACADprogram
{
    public static class ExceptionExtensions
    {
        public static string MessageExpress(this System.Exception exception)
        {
            var sb = new StringBuilder(exception.Message); var e = exception; while (e.InnerException != null)
            {
                e = e.InnerException; sb.Append(" => "); sb.Append(e.Message);
            }
            sb.Append(Environment.NewLine); sb.Append(Environment.NewLine); sb.Append(exception.StackTrace);
            return sb.ToString();
        }
    }
}
