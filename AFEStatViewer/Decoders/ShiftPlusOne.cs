using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFEStatViewer.Decoders
{
    // This is for AFE versions prior to Season 4 or Pathogen/Hardcore. (Not fully sure, and don't exactly recall.)
    public sealed class ShiftPlusOne : DecoderBase
    {
        public ShiftPlusOne(DecoderOptions? options = null) : base(options)
        {
        }

        protected override byte TransformByte(byte b) => (byte)((b + 1) % 127);
    }
}
