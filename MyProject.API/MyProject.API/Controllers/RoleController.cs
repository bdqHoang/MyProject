using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.API.Common;
using MyProject.Application.Features.Role.Commands.Create;
using MyProject.Application.Features.Role.Commands.Delete;
using MyProject.Application.Features.Role.Commands.Update;
using MyProject.Application.Features.Role.DTO;
using MyProject.Application.Features.Role.Queries;
using MyProject.Core.Entities;

namespace MyProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{RoleName.Manager},{RoleName.Admin}")]
    public class RoleController(ISender sender) : ControllerBase
    {
        /// <summary>
        /// get all roles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<RoleDetailRes>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRolesAsync()
        {
            var query = new GetAllRoleQuery();
            var result = await sender.Send(query);
            return Ok(ApiResponse<IEnumerable<RoleDetailRes>>.SuccessResponse(result, "Roles fetched successfully"));
        }

        /// <summary>
        /// get role by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<RoleDetailRes>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRoleByIdAsync([FromRoute] Guid id)
        {
            var query = new GetRoleByIdQuery(id);
            var result = await sender.Send(query);
            if (result == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(
                    $"Role with ID {id} not found",
                    StatusCodes.Status404NotFound
                ));
            }

            return Ok(ApiResponse<RoleDetailRes>.SuccessResponse(result, "Role fetched successfully"));
        }

        /// <summary>
        /// create role
        /// </summary>
        /// <param name="userReq"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<RoleDetailRes>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddRoleAsync([FromBody] CreateRoleReq userReq)
        {
            var command = new CreateRoleCommand(userReq);
            var roleId = await sender.Send(command);

            // return full dto
            var query = new GetRoleByIdQuery(roleId);
            var result = await sender.Send(query);

            return Ok(
                ApiResponse<RoleDetailRes>.SuccessResponse(result!, "Role created successfully")
            );
        }

        /// <summary>
        /// delete role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRoleAsync([FromRoute] Guid id)
        {
            var command = new DeleteRoleCommand(id);
            var result =  await sender.Send(command);
            return Ok(ApiResponse<object>.SuccessResponse(result, "Role deleted successfully"));
        }

        /// <summary>
        /// remove range role
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRangeRoleAsync([FromBody] IEnumerable<Guid> ids)
        {
            var command = new RemoveRangeRoleCommand(ids);
            var result = await sender.Send(command);
            return Ok(ApiResponse<object>.SuccessResponse(result, "Roles deleted successfully"));
        }

        /// <summary>
        /// update role
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateRoleReq"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<RoleDetailRes>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateRoleAsync([FromRoute] Guid id, [FromBody] UpdateRoleReq updateRoleReq)
        {
            var command = new UpdateRoleCommand(id, updateRoleReq);
            var result = await sender.Send(command);
            return Ok(ApiResponse<RoleDetailRes>.SuccessResponse(result, "Role updated successfully"));
        }
    }
}
