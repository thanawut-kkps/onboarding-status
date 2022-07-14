using System;

namespace Phatra.Core.Exceptions
{
    public abstract class BaseBusinessException : BaseMessageException
    {
        public BaseBusinessException()
        {

        }

        public BaseBusinessException(params string[] args)
            : base(args)
        {

        }

        public BaseBusinessException(Exception innerException, params string[] args)
            : base(innerException, args)
        {

        }

    }
}
