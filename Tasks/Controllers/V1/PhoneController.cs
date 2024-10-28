using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Taks.Core.Entities.ViewModels;
using Tasks.API.Helpers;
using Tasks.Core.Entities.ViewModels;
using Tasks.Core.Interfaces.IServices;

namespace Task.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PhoneController : ControllerBase
    {
        private readonly ILogger<PhoneController> _logger;
        private readonly IPhoneService _phoneService;
        private readonly IUserService _useService;
        private readonly IMemoryCache _memoryCache;

        public PhoneController(ILogger<PhoneController> logger,
                               IPhoneService phoneService,
                               IUserService userService,
                               IMemoryCache memoryCache)
        {
            _logger = logger;
            _phoneService = phoneService;
            _useService = userService;
            _memoryCache = memoryCache;
        }
        /// <summary>
        /// Deletes a phone by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /v1/Phone/1
        ///
        /// </remarks>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _phoneService.Delete(id, cancellationToken);


                ResponseViewModel response = new()
                {
                    Success = true,
                    Message = "Phone deleted successfully"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "No data found")
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseViewModel
                    {
                        Success = false,
                        Message = "Phone not found",
                        Error = new ErrorViewModel
                        {
                            Code = "NOT_FOUND",
                            Message = "Phone not found"
                        }
                    });
                }

                _logger.LogError(ex, "An error occurred while deleting the phone");

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel
                {
                    Success = false,
                    Message = "Error deleting the phone",
                    Error = new ErrorViewModel
                    {
                        Code = "DELETE_PHONE_ERROR",
                        Message = ex.Message
                    }
                });

            }
        }
        /// <summary>
        /// Get a phone by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /v1/Phone/1
        ///
        /// </remarks>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            try
            {
                PhoneViewModel phone = new();
                phone = await _phoneService.GetById(id, cancellationToken);
                ResponseViewModel<PhoneViewModel> response = new()
                {
                    Success = true,
                    Message = "Phone retrieved successfully",
                    Data = phone
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "No data found")
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseViewModel<PhoneViewModel>
                    {
                        Success = false,
                        Message = "Phone not found",
                        Error = new ErrorViewModel
                        {
                            Code = "NOT_FOUND",
                            Message = "Phone not found"
                        }
                    });
                }

                _logger.LogError(ex, $"An error occurred while retrieving the phone");

                ResponseViewModel<PhoneViewModel> errorResponse = new()
                {
                    Success = false,
                    Message = "Error retrieving phone",
                    Error = new ErrorViewModel
                    {
                        Code = "ERROR_CODE",
                        Message = ex.Message
                    }
                };

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
        /// <summary>
        /// Creates a new phone
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /v1/Phone
        ///     {
        ///        
        ///       {
        ///         "number": "829-999-9999",
        ///         "cityCode": "829",
        ///         "countryCode": "+1",
        ///         "userId": 1
        ///     }
        ///     
        ///    }
        ///
        /// </remarks>

        [HttpPost]
        public async Task<IActionResult> Create(PhoneCreateViewModel model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                string message;
                if (!await _useService.UserExistsById(model.UserId))
                {
                    message = $"The user does not exists.";
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel<PhoneViewModel>
                    {
                        Success = false,
                        Message = message,
                        Error = new ErrorViewModel
                        {
                            Code = "INPUT_VALIDATION_ERROR",
                            Message = message
                        }
                    });
                }

                try
                {
                    PhoneViewModel data = await _phoneService.Create(model, cancellationToken);
                    ResponseViewModel<PhoneViewModel> response = new()
                    {
                        Success = true,
                        Message = "Phone crated successfully",
                        Data = data
                    };
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while adding the phone");
                    message = $"An error occurred while adding the phone- " + ex.Message;

                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel<PhoneViewModel>
                    {
                        Success = false,
                        Message = message,
                        Error = new ErrorViewModel
                        {
                            Code = "ADD_PHONE_ERROR",
                            Message = message
                        }
                    });
                }

            }

            return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel<PhoneViewModel>
            {
                Success = false,
                Message = "Invalid input",
                Error = new ErrorViewModel
                {
                    Code = "INPUT_VALIDATION_ERROR",
                    Message = ModalStateHelper.GetErrors(ModelState)
                }
            });
        }

        /// <summary>
        /// Updates a phone by id
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /v1/Phone
        ///     {
        ///         "Id": 1,
        ///         "number": "829-999-9999",
        ///         "cityCode": "829",
        ///         "countryCode": "+1",
        ///         
        ///     }
        ///
        /// </remarks>

        [HttpPut]
        public async Task<IActionResult> Edit(PhoneUpdateViewModel model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                string message;
                int phoneId = model.Id;
                if (!await _phoneService.ExistsPhoneById(phoneId, cancellationToken))
                {
                    message = $"The phone does not exists.";
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel
                    {
                        Success = false,
                        Message = message,
                        Error = new ErrorViewModel
                        {
                            Code = "NOT_EXISTS",
                            Message = message
                        }
                    });
                }

                try
                {
                    await _phoneService.Update(model, cancellationToken);

                    ResponseViewModel response = new()
                    {
                        Success = true,
                        Message = "Task updated successfully"
                    };

                    return Ok(response);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while updating the phone");
                    message = $"An error occurred while updating the phone- " + ex.Message;

                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel
                    {
                        Success = false,
                        Message = message,
                        Error = new ErrorViewModel
                        {
                            Code = "UPDATE_ROLE_ERROR",
                            Message = message
                        }
                    });
                }
            }

            return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel
            {
                Success = false,
                Message = "Invalid input",
                Error = new ErrorViewModel
                {
                    Code = "INPUT_VALIDATION_ERROR",
                    Message = ModalStateHelper.GetErrors(ModelState)
                }
            });
        }
    }
}
