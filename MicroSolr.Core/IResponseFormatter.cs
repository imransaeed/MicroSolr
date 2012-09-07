namespace MicroSolr.Core
{
    using System;

    public interface IResponseFormatter<out TFormattedResponse>
    {
        TFormattedResponse Format(string data);
    }
}