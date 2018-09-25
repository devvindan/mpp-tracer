using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Tracer
{
    public class ConsoleWriter : IWriter
    {
        public void WriteResult(TraceResult result, ISerializer serializer)
        {
            using (Stream stream= Console.OpenStandardOutput())
            {
                serializer.Serialize(result, stream);
            }
        }
    }
}
