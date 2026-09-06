using System;

namespace AFEStatViewer
{
    public enum SaveGameLoadFailure
    {
        None,
        SaveGameNotFound,
        ReadFailed,
        DecodeFailed,
    }

    public sealed class SaveGameLoadResult
    {
        public bool Success { get; }
        public SaveGameLoadFailure Failure { get; }
        public string Error { get; }
        public Exception Exception { get; }
        public string Json { get; }

        private SaveGameLoadResult(bool success, SaveGameLoadFailure failure = SaveGameLoadFailure.None, string error = null, Exception exception = null, string json = null)
        {
            Success = success;
            Failure = failure;
            Error = error;
            Exception = exception;
            Json = json;
        }

        public static SaveGameLoadResult Successful(string json)
        {
            return new SaveGameLoadResult(true, json: json);
        }

        public static SaveGameLoadResult Failed(SaveGameLoadFailure failure, string error, Exception exception = null)
        {
            return new SaveGameLoadResult(false, failure, error, exception);
        }
    }
}
