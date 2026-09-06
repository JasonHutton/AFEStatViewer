using System;

namespace AFEStatViewer
{
    public enum SaveGameLoadFailure
    {
        None,
        SaveGameNotFound,
        ReadFailed,
        DecodeFailed,
        ApplyFailed,
    }

    public sealed class SaveGameLoadResult
    {
        public bool Success { get; }
        public SaveGameLoadFailure Failure { get; }
        public string Error { get; }
        public Exception Exception { get; }

        private SaveGameLoadResult(bool success, SaveGameLoadFailure failure = SaveGameLoadFailure.None, string error = null, Exception exception = null)
        {
            Success = success;
            Failure = failure;
            Error = error;
            Exception = exception;
        }

        public static SaveGameLoadResult Successful()
        {
            return new SaveGameLoadResult(true);
        }

        public static SaveGameLoadResult Failed(SaveGameLoadFailure failure, string error, Exception exception = null)
        {
            return new SaveGameLoadResult(false, failure, error, exception);
        }
    }
}
