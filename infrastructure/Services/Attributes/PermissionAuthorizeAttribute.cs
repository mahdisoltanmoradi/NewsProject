using Common;
using DataLayer.Contracts;
using DataLayer.Entities.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace infrastructure.Services.Attributes
{
    public class PermissionAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly UserManager<User> _userManager;

        public PermissionAuthorizeAttribute(IPermissionRepository permissionRepository,UserManager<User> userManager)
        {
            _permissionRepository = permissionRepository;
            this._userManager = userManager;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var actionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
            if (SkipAuthorization(actionDescriptor))
            {
                await base.OnActionExecutionAsync(context, next);
                return;
            }

            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectResult("/Home/AccessDenied");
                return;
            }

            var action = actionDescriptor.ActionName;
            var controller = actionDescriptor.ControllerTypeInfo.FullName;
            var actionFullName = controller + "." + action;

            var gottenUser = await _userManager.FindByNameAsync(context.HttpContext.User.Identity.Name);
            var hasPermission = await _permissionRepository.UserHasPermissionAsync(gottenUser.Id, actionFullName);
            if (!hasPermission)
            {
                context.Result = new RedirectResult("/Home/AccessDenied");
                return;
            }

            await base.OnActionExecutionAsync(context, next);
        }

        private static bool SkipAuthorization(ControllerActionDescriptor actionDescriptor)
        {
            return actionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        }
    }
}