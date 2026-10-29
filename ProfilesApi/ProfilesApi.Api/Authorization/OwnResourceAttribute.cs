using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProfilesApi.Domain.Enum;
using ProfilesApi.Domain.Interfaces;

namespace ProfilesApi.Api.Authorization;

[AttributeUsage(AttributeTargets.Method)]
public sealed class OwnResourceAttribute : TypeFilterAttribute
{
    public OwnResourceAttribute(ProfileSubject subject, params AccountRole[] allowedRoles)
        : base(typeof(OwnResourceFilter))
    {
        Arguments = [subject, allowedRoles];
    }

    private sealed class OwnResourceFilter(ProfileSubject subject, AccountRole[] allowedRoles, IUnitOfWork unitOfWork) : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = context.HttpContext.User;
            if (user.IsInRole(AccountRole.Admin.ToString()) || allowedRoles.Any(role => user.IsInRole(role.ToString())))
            {
                await next();
                return;
            }

            if (CurrentUser.TryGetAccountId(user, out var userId)
                && TryGetResourceId(context, out var resourceId)
                && await OwnsAsync(userId, resourceId, context.HttpContext.RequestAborted))
            {
                await next();
                return;
            }

            context.Result = new ForbidResult();
        }

        private async Task<bool> OwnsAsync(Guid userId, Guid resourceId, CancellationToken cancellationToken)
        {
            var accountId = await FindAccountIdAsync(resourceId, cancellationToken);
            return accountId is null || accountId == userId;
        }

        private async Task<Guid?> FindAccountIdAsync(Guid resourceId, CancellationToken cancellationToken)
        {
            return subject switch
            {
                ProfileSubject.Account => (await unitOfWork.Accounts.GetAsync(account => account.Id == resourceId, cancellationToken: cancellationToken))?.Id,
                ProfileSubject.Patient => (await unitOfWork.Patients.GetAsync(patient => patient.Id == resourceId, cancellationToken: cancellationToken))?.AccountId,
                ProfileSubject.Doctor => (await unitOfWork.Doctors.GetAsync(doctor => doctor.Id == resourceId, cancellationToken: cancellationToken))?.AccountId,
                ProfileSubject.Receptionist => (await unitOfWork.Receptionists.GetAsync(receptionist => receptionist.Id == resourceId, cancellationToken: cancellationToken))?.AccountId,
                _ => throw new ArgumentOutOfRangeException(nameof(subject), subject, "Unsupported profile subject.")
            };
        }

        private static bool TryGetResourceId(ActionExecutingContext context, out Guid resourceId)
        {
            if (context.RouteData.Values.TryGetValue("id", out var routeValue)
                && Guid.TryParse(routeValue?.ToString(), out resourceId))
            {
                return true;
            }

            foreach (var argument in context.ActionArguments.Values)
            {
                var idProperty = argument?.GetType().GetProperty("Id");
                if (idProperty?.PropertyType == typeof(Guid) && idProperty.GetValue(argument) is Guid id)
                {
                    resourceId = id;
                    return true;
                }
            }

            resourceId = Guid.Empty;
            return false;
        }
    }
}

public enum ProfileSubject
{
    Account,
    Patient,
    Doctor,
    Receptionist
}
