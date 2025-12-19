using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFEStatViewer
{
    [Flags]
    public enum DecoderFlags
    {
        None            = 0,
        SkipFirstByte   = 1 << 0,
        SkipLastByte    = 1 << 1,
    }

    public readonly record struct DecoderOptions(DecoderFlags Flags)
    {
        public static readonly DecoderOptions Default = new(DecoderFlags.None);

        public bool SkipFirstByte => (Flags & DecoderFlags.SkipFirstByte) != 0;
        public bool SkipLastByte => (Flags & DecoderFlags.SkipLastByte) != 0;
    }
}
