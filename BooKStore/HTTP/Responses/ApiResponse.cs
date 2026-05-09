using BooKStore.HTTP;
using Microsoft.AspNetCore.Mvc;

namespace BooKStore.HTTP.Responses
{
    public static class ApiResponse
    {
        public static IActionResult ToResponse<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                return new OkObjectResult(result);
            }

            if (result.Errors.Any(e => e.Contains("not found", StringComparison.OrdinalIgnoreCase)))
            {
                return new NotFoundObjectResult(result);
            }

            return new BadRequestObjectResult(result);
        }

        public static IActionResult ToCreatedResponse<T>(
            Result<T> result,
            string actionName,
            object routeValues,
            ControllerBase controller)
        {
            if (!result.IsSuccess)
            {
                return ToResponse(result);
            }

            return controller.CreatedAtAction(
                actionName,
                routeValues,
                result
            );
        }
    }
}