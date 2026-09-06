using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AFEStatViewer.Services
{
    internal class SaveGameLoader
    {
        private readonly string basePath;
        private readonly string saveFilename;
        public string SaveGamePath { get; private set; } = string.Empty;
        public SaveGameLoader(string basePath, string saveFilename)
        {
            this.basePath = basePath;
            this.saveFilename = saveFilename;
        }
        private string FindSaveGame()
        {
            string saveGamePath = string.Empty;
            List<string> possiblePaths = new List<string>();

            if (!Directory.Exists(basePath))
            {
                return string.Empty;
            }

            // Timestamp gets updated when you load game to menu, then close the game.
            possiblePaths.Add(basePath); // We want to check the basepath (Season 1 and earlier AFE behavior.)
            possiblePaths.AddRange(Directory.GetDirectories(basePath)); // And we also want to check subdirectories within that. (Season 2 AFE behavior.)

            // If a path doesn't exist, discard it
            for (int i = 0; i < possiblePaths.Count; i++)
            {
                possiblePaths[i] = System.IO.Path.Combine(possiblePaths[i], saveFilename);
                if (!File.Exists(possiblePaths[i]))
                {
                    possiblePaths.RemoveAt(i);
                    i--; // Decrement counter so that the end of loop increment indexes the correct element, as one has been removed.
                }
            }

            // Find the most recently accessed savegame from our paths.
            DateTime mostRecentFileTime = DateTime.MinValue;
            for (int i = 0; i < possiblePaths.Count; i++)
            {
                DateTime lastAccessedTime = File.GetLastWriteTimeUtc(possiblePaths[i]);
                if (lastAccessedTime > mostRecentFileTime)
                {
                    mostRecentFileTime = lastAccessedTime;
                    saveGamePath = possiblePaths[i];
                }
            }

            return saveGamePath;
        }

        private string GetSaveGamePath()
        {
            if (!string.IsNullOrEmpty(SaveGamePath))
            {
                return SaveGamePath;
            }

            SaveGamePath = FindSaveGame();

            return SaveGamePath;
        }

        private byte[] ReadSaveGame(string saveGamePath)
        {
            using (var fs = new FileStream(saveGamePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                byte[] encryptedBytes = new byte[fs.Length];

                int offset = 0;
                while (offset < encryptedBytes.Length)
                {
                    int read = fs.Read(encryptedBytes, offset, encryptedBytes.Length - offset);
                    if (read == 0) break;
                    offset += read;
                }

                if (offset != encryptedBytes.Length)
                {
                    throw new IOException($"Expected to read {encryptedBytes.Length} bytes, but only read {offset}.");
                }

                return encryptedBytes;
            }
        }

        private DecodeAttemptResult DecodeSaveGame(byte[] encryptedBytes)
        {
            // Try decoders in "most likely newest first" order
            var decoders = new List<ISaveDecoder>
            {
                new Decoders.XOR(0x42, new DecoderOptions(DecoderFlags.SkipLastByte)),
                new Decoders.ShiftModulo(1, 127),
                new Decoders.NOP(),
            };

            return SaveGameDecoding.DecodeFirstValidJson(encryptedBytes, decoders);
        }

        public SaveGameLoadResult LoadSavegame()
        {
            string saveGamePath = GetSaveGamePath();

            if (string.IsNullOrEmpty(saveGamePath))
            {
                return SaveGameLoadResult.Failed(SaveGameLoadFailure.SaveGameNotFound, $"Save game not found at: {basePath}");
            }

            byte[] encryptedBytes;

            try
            {
                encryptedBytes = ReadSaveGame(saveGamePath);
            }
            catch (IOException ex)
            {
                return SaveGameLoadResult.Failed(SaveGameLoadFailure.ReadFailed, ex.Message, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                return SaveGameLoadResult.Failed(SaveGameLoadFailure.ReadFailed, ex.Message, ex);
            }

            DecodeAttemptResult decodeResult = DecodeSaveGame(encryptedBytes);

            if (!decodeResult.Success || decodeResult.Json == null || decodeResult.DecoderUsed == null)
            {
                return SaveGameLoadResult.Failed(SaveGameLoadFailure.DecodeFailed, decodeResult.Error);
            }

#if DEBUG
            Debug.WriteLine($"Decoder selected: {decodeResult.DecoderUsed.GetType().Name}");
#endif

#if DEBUG && SAVE_JSON
            string outputPath = System.IO.Path.Combine(AppContext.BaseDirectory, Properties.Settings.Default.AFE1_SaveGame_Output_Filename);

            File.WriteAllText(outputPath, decodeResult.Json, Encoding.UTF8);
#endif

            return SaveGameLoadResult.Successful(decodeResult.Json);
        }
    }
}
