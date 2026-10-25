namespace InvestFlow.Application.Exceptions
{
    public class IdempotencyKeyConflictException : Exception
    {
        public IdempotencyKeyConflictException() : base("IdempotencyKeyConflictException")
        {

        }
    }
}
