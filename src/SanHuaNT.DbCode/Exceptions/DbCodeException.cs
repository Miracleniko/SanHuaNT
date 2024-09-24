using System;
using System.Runtime.Serialization;
using SanHuaNT;

namespace SanHuaNT.DbCode
{
    /// <summary>DbCode异常</summary>
    [Serializable]
    public class DbCodeException : XException
    {
        #region 构造
        /// <summary>初始化</summary>
        public DbCodeException() { }

        /// <summary>初始化</summary>
        /// <param name="message"></param>
        public DbCodeException(String message) : base(message) { }

        /// <summary>初始化</summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public DbCodeException(String format, params Object[] args) : base(format, args) { }

        /// <summary>初始化</summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public DbCodeException(String message, Exception innerException) : base(message, innerException) { }

        /// <summary>初始化</summary>
        /// <param name="innerException"></param>
        public DbCodeException(Exception innerException) : base((innerException?.Message), innerException) { }
        #endregion
    }
}