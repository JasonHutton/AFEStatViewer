using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFEStatViewer.Decoders
{
    // This is for AFE versions prior to Season 4 or Pathogen/Hardcore. (Not fully sure, and don't exactly recall.)
    public sealed class ShiftPlusOne : ISaveDecoder
    {
        public byte[] DecodeBytes(byte[] input)
        {
            var output = new byte[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                output[i] = (byte)((input[i] + 1) % 127);
            }
            return output;
        }

        public string PostProcessString(string decodedText) => decodedText;
    }
}
