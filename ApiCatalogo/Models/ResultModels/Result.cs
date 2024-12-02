namespace MainBlog.Models.ResultModels
{
    public abstract class Result
    {
        public bool IsSuccess { get; }
        public Error? Error { get; }

        protected Result(bool isSuccess, Error? error = null)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success()
            => new SuccessResult();

        public static Result Failure(Error error)
            => new FailureResult(error);
    }

    public class SuccessResult : Result
    {
        public SuccessResult() : base(true) { }
    }

    public class FailureResult : Result
    {
        public FailureResult(Error error) : base(false, error) { }
    }

}
