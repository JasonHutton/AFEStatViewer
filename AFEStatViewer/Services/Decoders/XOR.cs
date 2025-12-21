using AFEStatViewer.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFEStatViewer.Services.Decoders
{
    // This is for AFE versions after Season 4 or Pathogen/Hardcore. (Not fully sure, and don't exactly recall.)
    public sealed class XOR : DecoderBase
    {
        private readonly byte _key;

        public XOR(byte key, DecoderOptions? options = null) : base(options)
        {
            _key = key;
        }

        protected override byte TransformByte(byte b) => (byte)(b ^ _key);
    }
}
