using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Description;

namespace GruppoCap.Core.Api
{
    /// <inheritdoc/>
    /// <summary>
    /// Base controller
    /// </summary>
    public abstract class BaseApiController : RevoApiController
    {
        /// <summary>
        /// Create HttpResponseMessage for OK response
        /// </summary>
        /// <param name="resultObj">Object to serialize in output</param>
        /// <returns><paramref name="HttpResponseMessage"/></returns>
        /// <response code="200">OK</response>
        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage ResponseOK(object resultObj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, resultObj);
        }

        /// <summary>
        /// Create HttpResponseMessage for KO response
        /// </summary>
        /// <param name="resultObj">Object to serialize in output</param>
        /// <returns><paramref name="HttpResponseMessage"/></returns>
        /// <response code="500">Internal server error</response>
        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage ResponseKO(object resultObj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, resultObj);
        }

        /// <summary>
        /// Create HttpResponseMessage for OK response
        /// </summary>
        /// <param name="BaseApiResponse"></param>
        /// <returns><paramref name="BaseApiResponse"/></returns>
        /// <response code="200">OK</response>
        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage ResponseOK(BaseApiResponse resp)
        {
            return Request.CreateResponse(HttpStatusCode.OK, resp);
        }




        // se ResponseOK, il boolean resultVal non sarà sempre true? 
        // non può essere parameterless?

        /// <summary>
        /// Create HttpResponseMessage for OK response
        /// </summary>
        /// <param name="resultVal">Operation Result</param>
        /// <returns><paramref name="BaseApiResponse"/></returns>
        /// <response code="200">OK</response>
        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage ResponseOK(bool resultVal)
        {
            return ResponseOK(resultVal, null, null);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage ResponseOK(String IDCase, bool resultVal)
        {
            return ResponseOK(resultVal, null, null, IDCase);
        }

        /// <summary>
        /// Create HttpResponseMessage for OK response
        /// </summary>
        /// <param name="resultVal">Operation Result</param>
        /// <returns><paramref name="BaseApiResponse"/></returns>
        /// <response code="200">OK</response>
        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage ResponseOK(bool resultVal, string errorCode)
        {
            return ResponseOK(resultVal, errorCode, null);
        }

        /// <summary>
        /// Create HttpResponseMessage for OK response
        /// </summary>
        /// <param name="resultVal">Operation Result</param>
        /// <param name="errorCode">Error code of operation</param>
        /// <param name="errorMessage">Error message of operation</param>
        /// <returns><paramref name="BaseApiResponse"/></returns>
        /// <response code="200">OK</response>
        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage ResponseOK(bool resultVal, string errorCode, string errorMessage)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new BaseApiResponse() { Result = resultVal, ErrorCode = errorCode, ErrorMessage = errorMessage });
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage ResponseOK(bool resultVal, string errorCode, string errorMessage, String IDCase)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new BaseApiResponse() { Result = resultVal, ErrorCode = errorCode, ErrorMessage = errorMessage, IDCase = IDCase });
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage ResponseOK(bool resultVal, string errorCode, string errorMessage, String IDCase, String RefMail)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new BaseApiResponse() { Result = resultVal, ErrorCode = errorCode, ErrorMessage = errorMessage, IDCase = IDCase, RefMail = RefMail });
        }

        /// <summary>
        /// Create HttpResponseMessage for KO response
        /// </summary>
        /// <param name="BaseApiResponse"></param>
        /// <returns><paramref name="BaseApiResponse"/></returns>
        /// <response code="500">Internal server error</response>
        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage ResponseKO(BaseApiResponse resp)
        {
            return Request.CreateResponse(HttpStatusCode.OK, resp);
        }



        // se ResponseKO, il boolean resultVal non sarà sempre false? 
        // non può essere parameterless?

        /// <summary>
        /// Create HttpResponseMessage for KO response
        /// </summary>
        /// <param name="resultVal">Operation Result</param>
        /// <returns><paramref name="BaseApiResponse"/></returns>
        /// <response code="500">Internal server error</response>
        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage ResponseKO(bool resultVal)
        {
            return ResponseKO(resultVal, null, null);
        }



        // se ResponseKO, il boolean resultVal non sarà sempre false?

        /// <summary>
        /// Create HttpResponseMessage for KO response
        /// </summary>
        /// <param name="resultVal">Operation Result</param>
        /// <param name="errorCode">Error code of operation</param>
        /// <returns><paramref name="BaseApiResponse"/></returns>
        /// <response code="500">Internal server error</response>
        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage ResponseKO(bool resultVal, string errorCode)
        {
            return ResponseKO(resultVal, errorCode, null);
        }

        /// <summary>
        /// Create HttpResponseMessage for KO response
        /// </summary>
        /// <param name="resultVal">Operation Result</param>
        /// <param name="errorCode">Error code of operation</param>
        /// <param name="errorMessage">Error message of operation</param>
        /// <returns><paramref name="BaseApiResponse"/></returns>
        /// <response code="500">Internal server error</response>
        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage ResponseKO(bool resultVal, string errorCode, string errorMessage)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new BaseApiResponse() { Result = resultVal, ErrorCode = errorCode, ErrorMessage = errorMessage });
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage ResponseKO(bool resultVal, string errorCode, string errorMessage, String IDCase)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new BaseApiResponse() { Result = resultVal, ErrorCode = errorCode, ErrorMessage = errorMessage, IDCase = IDCase });
        }




        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage ResponseApi(BaseApiResponse resp)
        {
            if (resp.Result)
            {
                return ResponseOK(resp);
            }
            else
            {
                return ResponseKO(resp.ErrorCode, resp.ErrorMessage);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage ResponseKO(String errorCode, String errorMessage)
        {
            return ResponseKO(false, errorCode, errorMessage);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage ResponseKO(String errorCode, String errorMessage, String IDCase)
        {
            return ResponseKO(false, errorCode, errorMessage, IDCase);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage ResponseOK()
        {
            return ResponseOK(true, null, null);
        }

    }
}