using System;
using System.IO;

namespace Phatra.Core.Exceptions
{
    public class SqlServerBusinessException : BaseBusinessException
    {
        public SqlServerBusinessException(string message)
            : base(message)
        {

        }


        public SqlServerBusinessException(Exception innerException, string message)
            : base(innerException, message)
        {

        }

        protected override string XmlPath
        {
            get
            {
                return Path.Combine(ExecutingPath, @"Exceptions\BusinessException.xml");
            }
        }
    }
}
