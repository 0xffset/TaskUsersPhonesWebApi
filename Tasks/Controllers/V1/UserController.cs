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
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IMemoryCache _memoryCache;

        public UserController(ILogger<UserController> logger,
                              IUserService userService,
                              IMemoryCache memoryCache)
        {
            _logger = logger;
            _userService = userService;
            _memoryCache = memoryCache;
        }
        /// <summary>
        /// Deletes a user by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /v1/User/1
        ///
        /// </remarks>

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _userService.Delete(id, cancellationToken);
                ResponseViewModel response = new()
                {
                    Success = true,
                    Message = "User deleted successfully"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Not data found")
                {

                    return StatusCode(StatusCodes.Status404NotFound, new ResponseViewModel
                    {
                        Success = false,
                        Message = "User not found",
                        Error = new ErrorViewModel
                        {
                            Code = "NOT_FOUND",
                            Message = "User not found"
                        }
                    });
                }


                _logger.LogError(ex, "An error occurred while deleting the user");

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel
                {
                    Success = false,
                    Message = "Error deleting the user",
                    Error = new ErrorViewModel
                    {
                        Code = "DELETE_USER_ERROR",
                        Message = ex.Message
                    }
                });
            }

        }

        /// <summary>
        /// Updates a user by id
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /v1/User
        ///     {
        ///       {
        ///           "id": 3,
        ///           "fullName": "Joe Doe",
        ///           "userName": "joedoe",
        ///           "email": "joedoe@example.com"
        ///        }
        ///     }
        ///
        /// </remarks>

        [HttpPut]
        public async Task<IActionResult> Edit(UserUpdateViewModel model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                string message;
                // Check if the email exists
                if (!await _userService.UserExistsById(model.Id))
                {
                    message = $"The user does not exists";
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel
                    {
                        Success = false,
                        Message = message,
                        Error = new ErrorViewModel
                        {
                            Code = "EMAIL_DOES_NOT_EXISTS",
                            Message = message
                        }
                    });
                }
                // Check if the username already exists
                if (await _userService.UserUsernameExists(model.UserName))
                {
                    message = $"The usersname already exists. Please, try again.";
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel
                    {
                        Success = false,
                        Message = message,
                        Error = new ErrorViewModel
                        {
                            Code = "USERNAME_ALREADY_EXISTS",
                            Message = message
                        }
                    });
                }
                try
                {
                    _ = await _userService.Update(model, cancellationToken);

                    ResponseViewModel response = new()
                    {
                        Success = true,
                        Message = "User updated successfully"
                    };
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while updating the user");
                    message = $"An error occurred while updating the user- " + ex.Message;

                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel
                    {
                        Success = false,
                        Message = message,
                        Error = new ErrorViewModel
                        {
                            Code = "UPDATE_USER_ERROR",
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

        /// <summary>
        /// Get all users
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<UserViewModel> users = await _userService.GetAllUsers(cancellationToken);
                ResponseViewModel<IEnumerable<UserViewModel>> response = new()
                {
                    Success = true,
                    Message = "Users retrieved successfully",
                    Data = users
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving users");

                ResponseViewModel<IEnumerable<UserViewModel>> errorResponse = new()
                {
                    Success = false,
                    Message = "Error retrieving users",
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
        /// Creates a new user
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /v1/User
        ///     {
        ///          "fullName": "Joe Doe",
        ///          "userName": "joedoe",
        ///          "email": "joedoe@example.com",
        ///          "password": "J0eD3eS3cure@"
        ///
        ///      }
        ///
        /// </remarks>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(UserCreateViewModel model, CancellationToken cancellationToken)
        {

            if (ModelState.IsValid)
            {
                string message;
                try
                {
                    // Validate if the email already exist
                    if (await _userService.UserExistByEmail(model.Email))
                    {
                        message = $"The email already exists.";
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel<UserViewModel>
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


                    UserViewModel data = await _userService.Create(model, cancellationToken);
                    ResponseViewModel<UserViewModel> response = new()
                    {
                        Success = true,
                        Message = "User created successfully",
                        Data = data
                    };
                    return Ok(response);
                }
                catch (Exception ex)
                {

                    _logger.LogError(ex, $"An error occurred while adding the user");
                    message = $"An error occurred while adding the user- " + ex.Message;

                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel<UserViewModel>
                    {
                        Success = false,
                        Message = message,
                        Error = new ErrorViewModel
                        {
                            Code = "ADD_USER_ERROR",
                            Message = message
                        }
                    });

                }
            }

            return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel<UserViewModel>
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
