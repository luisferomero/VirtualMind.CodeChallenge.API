using System;
using System.Collections.Generic;
using System.Text;

namespace Virtualmind.CodeChallenge.Entities.Responses
{
    /// <summary>
    /// Class to be able to find out if the service was completed correctly or there was an error.
    /// </summary>
    public class ResponseHelper<T> where T : class
    {
        public bool Success { get; set; }
        public ICollection<string> Errors { get; set; } = new List<string>();
        public int? StatusCode { get; set; }
        public T Entity { get; set; }

        /// <summary>
        /// If the Service failed to run
        /// </summary>
        /// <param name="success">The execution was successful or not</param>
        /// <param name="errors">Errors list</param>
        /// <param name="statusCode">Status Code (if is needed)</param>
        public ResponseHelper(ICollection<string> errors, bool success = false, int? statusCode = null)
        {
            Success = success;
            Errors = errors;
            StatusCode = statusCode;
        }

        /// <summary>
        /// The service ran successfully
        /// </summary>
        /// <param name="success">The execution was successful or not</param>
        /// <param name="statusCode">Status Code (if is needed)</param>
        /// <param name="entity">Entity that returns the service</param>
        public ResponseHelper(T entity, bool success = true, int? statusCode = null)
        {
            Success = success;
            StatusCode = statusCode;
            Entity = entity;
        }
    }
}
