using Microsoft.AspNetCore.Mvc;

namespace DatabaseEncryption.Controllers
{
    [ApiController]
    [Route("api/encrypt")]
    public class EncryptionController : ControllerBase
    {
        private readonly ILogger<EncryptionController> _logger;
        private readonly ISqlCipherHelper cipherHelper;

        public EncryptionController(ILogger<EncryptionController> logger, ISqlCipherHelper cipherHelper)
        {
            _logger = logger;
            this.cipherHelper = cipherHelper;
        }

        [HttpGet(Name = "encrypt")]
        public IActionResult Get(string path)
        {
            try
            {
                this.cipherHelper.DoEncription(path);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}