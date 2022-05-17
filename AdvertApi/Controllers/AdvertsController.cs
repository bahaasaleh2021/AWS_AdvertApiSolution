using AdvertApi.Models;
using AdvertApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Amazon.SimpleNotificationService;
using AdvertApi.Models.Messages;

namespace AdvertApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class AdvertsController : ControllerBase
    {
        private readonly IAdvertStorage _advertStorageSerive;
        private readonly IConfiguration _config;

        public AdvertsController(IAdvertStorage advertStorageSerive,IConfiguration config)
        {
            _advertStorageSerive = advertStorageSerive;
            _config = config;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(AdvertModel advert)
        {
            try
            {
                 var id=await _advertStorageSerive.Add(advert);
               
                return Created("",new CreateAdvertResponseModel { Id = id });

            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        [Route("Confirm")]
        public async Task<IActionResult> Confirm(ConfirmAdvertModel confirm)
        {
            try
            {
                await _advertStorageSerive.Confirm(confirm);
                await RaiseAdvertConfiremdEvent(confirm);
                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private async Task RaiseAdvertConfiremdEvent(ConfirmAdvertModel model)
        {
            var advert = await _advertStorageSerive.GetById(model.Id);
            var snsTopic = _config["AWS:AdverApiSNStopicARN"];
            using (var client = new AmazonSimpleNotificationServiceClient())
            {
                var message = new AdvertConfirmedMessage
                {
                    Id = model.Id,
                    Title = advert.Title
                };

                await client.PublishAsync(snsTopic, JsonSerializer.Serialize(message));
            }
        }
    }
}
