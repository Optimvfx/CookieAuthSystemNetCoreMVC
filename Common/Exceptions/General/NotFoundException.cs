using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions.General
{
    public class NotFoundException : Exception
    {
        public string NotFoundModel { get; set; } = null!;

        public override string Message => $"{NotFoundModel} is not found";
    }
}
