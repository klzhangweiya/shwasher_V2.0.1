using System;
using System.Runtime.Serialization;
using Abp;
using Abp.Logging;

namespace IwbZero.Authorization.AuthorizeFilter
{
    /// <summary>
    /// This exception is thrown on an unauthorized request.
    /// </summary>
    [Serializable]
    public class IwbSessionExpireException : AbpException, IHasLogSeverity
    {
        /// <summary>
        /// Severity of the exception.
        /// Default: Warn.
        /// </summary>
        public LogSeverity Severity { get; set; }

        /// <summary>
        /// Creates a new <see cref="IwbSessionExpireException"/> object.
        /// </summary>
        public IwbSessionExpireException()
        {
            Severity = LogSeverity.Warn;
        }

        /// <summary>
        /// Creates a new <see cref="IwbSessionExpireException"/> object.
        /// </summary>
        public IwbSessionExpireException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="IwbSessionExpireException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public IwbSessionExpireException(string message)
            : base(message)
        {
            Severity = LogSeverity.Warn;
        }

        /// <summary>
        /// Creates a new <see cref="IwbSessionExpireException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public IwbSessionExpireException(string message, Exception innerException)
            : base(message, innerException)
        {
            Severity = LogSeverity.Warn;
        }
    }
}
