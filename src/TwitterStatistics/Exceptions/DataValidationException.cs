using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace TwitterStatistics.Exceptions
{
    public class DataValidationException : Exception
    {
        public Dictionary<string, List<string>> ValidationDescriptions { get; set; }

        /// <summary>
        /// Creates the exception without any additional descriptive information.
        /// </summary>
        public DataValidationException()
            : base()
        {
        }

        /// <summary>
        /// Creates the exception with a description.
        /// </summary>
        /// <param name="message">The description of the exception.</param>
        public DataValidationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates the exception with a description.
        /// </summary>
        /// <param name="message">The description of the exception.</param>
        /// <param name="validationDescriptions">List of specific validation errors.</param>
        public DataValidationException(string message, Dictionary<string, List<string>> validationDescriptions)
            : base(message)
        {
            ValidationDescriptions = new Dictionary<string, List<string>>() { };
            foreach (var desc in validationDescriptions)
            {
                if (ValidationDescriptions.ContainsKey(desc.Key))
                {
                    ValidationDescriptions[desc.Key].AddRange(desc.Value);
                }
                else
                {
                    ValidationDescriptions.Add(desc.Key, desc.Value);
                }
            }
        }

        /// <summary>
        /// Creates the exception with a description and inner cause.
        /// </summary>
        /// <param name="message">The description of the exception.</param>
        /// <param name="innerEx">The inner cause of the exception.</param>
        public DataValidationException(string message, Exception innerEx)
            : base(message, innerEx)
        {
        }

        /// <summary>
        /// Creates the exception with a description and inner cause.
        /// </summary>
        /// <param name="message">The description of the exception.</param>
        /// <param name="innerEx">The inner cause of the exception.</param>
        /// <param name="validationDescriptions">List of specific validation errors.</param>
        public DataValidationException(string message, Exception innerEx, Dictionary<string, List<string>> validationDescriptions)
            : base(message, innerEx)
        {
            ValidationDescriptions = new Dictionary<string, List<string>>() { };
            foreach (var desc in validationDescriptions)
            {
                if (ValidationDescriptions.ContainsKey(desc.Key))
                {
                    ValidationDescriptions[desc.Key].AddRange(desc.Value);
                }
                else
                {
                    ValidationDescriptions.Add(desc.Key, desc.Value);
                }
            }
        }

        /// <summary>
        /// Creates the exception from serialized data. Usual scenario is when exception is occurred somewhere on the remote workstation
        /// and we have to re-create/re-throw the exception on the local machine
        /// </summary>
        /// <param name="info">Serialization information.</param>
        /// <param name="context">Serialization context.</param>
        public DataValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

            ValidationDescriptions = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(info.GetString("ValidationDescriptions"));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(typeof(SerializationInfo).Name);
            info.AddValue("ValidationDescriptions", JsonConvert.SerializeObject(ValidationDescriptions));
            base.GetObjectData(info, context);
        }
    }
}
