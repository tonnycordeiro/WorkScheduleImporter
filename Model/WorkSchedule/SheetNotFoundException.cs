using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sisgraph.Ips.Samu.AddIn.Models.UnitForceMap
{
    public class SheetNotFoundException : Exception
    {
        public SheetNotFoundException()
        {
        }

        public SheetNotFoundException(string message) : base(message)
        {
        }

        public SheetNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SheetNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
