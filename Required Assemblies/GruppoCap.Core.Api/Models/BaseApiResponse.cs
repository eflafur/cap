using System.Text;

namespace GruppoCap.Core.Api
{
    /// <summary>
    /// Base class of Api response
    /// </summary>
    public class BaseApiResponse
    {
        /// <summary>
        /// ID Case
        /// </summary>
        public string IDCase { get; set; }
        /// <summary>
        /// Api result
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// Error Code
        /// </summary>
        public string ErrorCode { get; set; }
        /// <summary>
        /// Error message
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// RefMail
        /// </summary>
        public string RefMail { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Result: {0}", Result);
            if (!ErrorCode.IsNullOrEmpty())
                sb.AppendFormat(" - ErrorCode: {0}", ErrorCode);
            if (!ErrorMessage.IsNullOrEmpty())
                sb.AppendFormat(" - ErrorMessage: {0}", ErrorMessage);
            if (!IDCase.IsNullOrEmpty())
                sb.AppendFormat(" - IDCase: {0}", IDCase);

            return sb.ToString();
        }
    }
}