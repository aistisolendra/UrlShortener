using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Wrap;

namespace UrlShortener.Services.Retry
{
    public sealed class RetryService : IRetryService
    {
        public RetryService()
        {
        }

        public async Task<T> RetryAsync<T>(
            Func<Task<T>> func,
            RetryOptions retryOptions
        )
        {
            var policy = GetPolicy(
                exc => retryOptions.ExceptionsToCatch.Contains(exc.GetType()),
                retryOptions.RetryDelay,
                retryOptions.RetryCount,
                retryOptions.RetryTimeout
            );

            var result = await policy
                .ExecuteAndCaptureAsync(func)
                .ConfigureAwait(false);

            if (result.Outcome is OutcomeType.Failure)
                throw result.FinalException;

            return result.Result;
        }

        public async Task<T> RetryAsync<T>(
            Func<Task<T>> func,
            TimeSpan retryDelay,
            int retryCount,
            TimeSpan retryTimeout,
            params Type[] exceptionsToCatch)
        {
            var policy = GetPolicy(
                exc => exceptionsToCatch.Contains(exc.GetType()),
                retryDelay,
                retryCount,
                retryTimeout
            );

            var result = await policy
                .ExecuteAndCaptureAsync(func)
                .ConfigureAwait(false);

            if (result.Outcome is OutcomeType.Failure)
                throw result.FinalException;

            return result.Result;
        }

        public async Task RetryAsync(
            Func<Task> func,
            RetryOptions retryOptions
        )
        {
            var policy = GetPolicy(
                exc => retryOptions.ExceptionsToCatch.Contains(exc.GetType()),
                retryOptions.RetryDelay,
                retryOptions.RetryCount,
                retryOptions.RetryTimeout
            );

            var result = await policy
                .ExecuteAndCaptureAsync(func)
                .ConfigureAwait(false);

            if (result.Outcome is OutcomeType.Failure)
                throw result.FinalException;
        }

        public async Task RetryAsync(
            Func<Task> func,
            TimeSpan retryDelay,
            int retryCount,
            TimeSpan retryTimeout,
            params Type[] exceptionsToCatch
        )
        {
            var policy = GetPolicy(
                exc => exceptionsToCatch.Contains(exc.GetType()),
                retryDelay,
                retryCount,
                retryTimeout
            );

            var result = await policy
                .ExecuteAndCaptureAsync(func)
                .ConfigureAwait(false);

            if (result.Outcome is OutcomeType.Failure)
                throw result.FinalException;
        }

        private AsyncPolicyWrap GetPolicy(Func<Exception, bool> exceptionPredicate, TimeSpan retryDelay, int retryCount,
            TimeSpan timeout)
        {
            var handle = Policy
                .Handle(exceptionPredicate)
                .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(retryDelay, retryCount));

            var timeoutPolicy = Policy
                .TimeoutAsync(timeout, ((context, span, task) => task));

            var policy = Policy.WrapAsync(handle, timeoutPolicy);

            return policy;
        }
    }
}