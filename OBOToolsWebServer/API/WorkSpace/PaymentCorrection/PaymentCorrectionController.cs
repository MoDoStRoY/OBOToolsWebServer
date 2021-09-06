using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OBOToolsWebServer.Config.Objects.PaymentCorrection;
using OBOToolsWebServer.Scripts.Logic.PaymentCorrection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OBOToolsWebServer.API.WorkSpace.PaymentCorrection
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentCorrectionController : ControllerBase
    {
        [HttpPost]
        [Route("getResults")]
        public List<string> GetResults([FromBody] PaymentData paymentData)
        {
            return PaymentCorrectionLogic.GetResults(paymentData.dataList);
        }
    }
}
