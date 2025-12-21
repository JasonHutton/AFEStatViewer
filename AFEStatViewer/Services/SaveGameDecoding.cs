using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace AFEStatViewer.Services
{
    public sealed record DecodeAttemptResult(bool Success, string Json, ISaveDecoder DecoderUsed, string Error);

    public static class SaveGameDecoding
    {
        public static DecodeAttemptResult DecodeFirstValidJson(byte[] encryptedBytes, IReadOnlyList<ISaveDecoder> decodersInPreferenceOrder)
        {
            if (encryptedBytes == null)
                throw new ArgumentNullException(nameof(encryptedBytes));
            if (decodersInPreferenceOrder == null || decodersInPreferenceOrder.Count == 0)
                throw new ArgumentException("At least one decoder must be provided.", nameof(decodersInPreferenceOrder));

            string lastError = null;

            foreach (var decoder in decodersInPreferenceOrder)
            {
                try
                {
                    byte[] decodedBytes = decoder.DecodeBytes(encryptedBytes);

                    // Trim trailing nulls
                    int length = decodedBytes.Length;
                    while (length > 0 && decodedBytes[length - 1] == 0x00)
                        length--;

                    if (length == 0)
                    {
                        lastError = $"{decoder.GetType().Name}: Decoded output was empty.";
                        continue;
                    }

                    if (IsValidJsonUtf8(decodedBytes, length))
                    {
                        string json = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true).GetString(decodedBytes, 0, length);

                        return new DecodeAttemptResult(true, json, decoder, null);
                    }

                    lastError = $"{decoder.GetType().Name}: Decoded output was not valid JSON (UTF-8).";
                }
                catch (Exception ex)
                {
                    lastError = $"{decoder.GetType().Name}: {ex.GetType().Name}: {ex.Message}";
                }
            }

            return new DecodeAttemptResult(false, null, null, lastError ?? "No decoder produced valid JSON.");
        }

        private static bool IsValidJsonUtf8(byte[] utf8Bytes, int length)
        {
            try
            {
                var reader = new Utf8JsonReader(new ReadOnlySpan<byte>(utf8Bytes, 0, length), isFinalBlock: true, state: default);
                while (reader.Read())
                {
                    // Just iterate; invalid JSON throws JsonException (or ArgumentException for invalid UTF-8).
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
