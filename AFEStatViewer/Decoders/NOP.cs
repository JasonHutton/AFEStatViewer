using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFEStatViewer.Decoders
{
    // This is just a no operation mainly for debugging and assumes an already-decoded input file.
    public sealed class NOP : ISaveDecoder
    {
        public byte[] DecodeBytes(byte[] input) => input;

        public string PostProcessString(string decodedText) => decodedText;
    }
}
