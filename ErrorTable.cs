using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comp
{
    static class ErrorTable
    {
        public static Dictionary<byte, string> Error = new Dictionary<byte, string>()
        {
                {201, "неизвестный символ"},
                {202, "ожидался символ"},
                {203, "слишком большое число"},
                {204, "не закрытая скобка"},
                {205, "неверный идентификатор"}
        };
    }
}
