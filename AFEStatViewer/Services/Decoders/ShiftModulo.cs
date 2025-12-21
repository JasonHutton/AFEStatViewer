using AFEStatViewer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFEStatViewer.Services.Decoders
{
    // This is for AFE versions prior to Season 4 or Pathogen/Hardcore. (Not fully sure, and don't exactly recall.)
    public sealed class ShiftModulo : DecoderBase
    {
        private readonly int _shift;
        private readonly int _modulo;

        public ShiftModulo(int shift, int modulo, DecoderOptions? options = null) : base(options)
        {
            _shift = shift;
            _modulo = modulo;
        }

        protected override byte TransformByte(byte b) => (byte)((b + _shift) % _modulo);
    }
}
