using AFEStatViewer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFEStatViewer.Services.Decoders
{
    public abstract class DecoderBase : ISaveDecoder
    {
        public DecoderOptions Options { get; }

        protected DecoderBase(DecoderOptions? options = null)
        {
            Options = options ?? DecoderOptions.Default;
        }

        public byte[] DecodeBytes(byte[] input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            var output = new byte[input.Length];
            if (input.Length == 0)
                return output;

            int start = Options.SkipFirstByte ? 1 : 0;
            int endExclusive = input.Length;

            // Copy skipped bytes so output stays the same length
            if (Options.SkipFirstByte)
                output[0] = input[0];

            if (Options.SkipLastByte)
            {
                output[output.Length - 1] = input[input.Length - 1];

                // Only shrink the decode range if there is at least one byte left to decode. (this should guard against 1 byte files...which will never happen, but...)
                if (endExclusive > start)
                    endExclusive--;
            }

            // Decode only the range we intend to transform
            for (int i = start; i < endExclusive; i++)
            {
                output[i] = TransformByte(input[i]);
            }

            return output;
        }

        protected abstract byte TransformByte(byte b);
    }
}
