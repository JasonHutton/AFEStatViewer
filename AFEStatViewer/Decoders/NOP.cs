using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFEStatViewer.Decoders
{
    // This is just a no operation mainly for debugging and assumes an already-decoded input file.
    public sealed class NOP : DecoderBase
    {
        public NOP(DecoderOptions? options = null) : base(options)
        {
        }
        protected override byte TransformByte(byte b) => b;
    }
}
