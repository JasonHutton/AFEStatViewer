using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFEStatViewer.Services
{
    public interface ISaveDecoder
    {
        public DecoderOptions Options { get; }
        public byte[] DecodeBytes(byte[] input);
    }
}
