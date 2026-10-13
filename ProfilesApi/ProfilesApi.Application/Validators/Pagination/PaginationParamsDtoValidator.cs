using FluentValidation;
using ProfilesApi.Application.DTOs.Common;
using ProfilesApi.Application.Validators.Constants;

namespace ProfilesApi.Application.Validators.Pagination;

public sealed class PaginationParamsDtoValidator : AbstractValidator<PaginationParamsDto>
{
    public PaginationParamsDtoValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.");
        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0.")
            .LessThanOrEqualTo(ValidationConstants.MaxPageSize)
            .WithMessage($"Page size cannot exceed {ValidationConstants.MaxPageSize}.");
    }
}
