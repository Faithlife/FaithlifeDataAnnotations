using System.ComponentModel.DataAnnotations;

namespace Faithlife.DataAnnotations;

/// <summary>
/// Used to validate a property whose type has its own properties with data annotations.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class ValidateObjectAttribute : ValidationAttribute
{
	/// <inheritdoc />
	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
	{
		if (value is null)
			return null;

		var validationResults = ValidatorUtility.GetValidationResults(value);
		if (validationResults.Count == 0)
			return null;

		var innerErrorMessage = string.Join(" ", validationResults.Select(x => x.ErrorMessage).Where(x => x is not null));
		return new ValidationResult(
			errorMessage: $"{FormatErrorMessage(validationContext.DisplayName)}{(innerErrorMessage.Length == 0 ? "" : $" ({innerErrorMessage})")}",
			memberNames: validationContext.MemberName is { } memberName ? new[] { memberName } : null);
	}
}
