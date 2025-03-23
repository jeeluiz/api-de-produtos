namespace Produto.Application;

using FluentValidation.Results;

public static class Extensions
{
    public static AppResult<T> ToAppResult<T>(this ValidationResult validationResult)
    {
        return new AppResult<T>(
            message: string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)),
            success: false,
            statusCode: AppResultStatus.Erro
        );
    }

    public static AppResult ToAppResult(this ValidationResult validationResult)
    {
        return validationResult.ToAppResult<object>();
    }
}