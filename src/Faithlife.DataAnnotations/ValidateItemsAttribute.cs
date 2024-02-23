using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Faithlife.DataAnnotations;

/// <summary>
/// Used to validate a collection property whose items have their own properties with data annotations.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class ValidateItemsAttribute : ValidationAttribute
{
	/// <summary>
	/// True if null items are allowed. Defaults to false.
	/// </summary>
	public bool AllowNullItems { get; set; }

	/// <inheritdoc />
	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
	{
		if (value is not IEnumerable items)
			return null;

		var (validationResults, index) = items.Cast<object?>()
			.Where(x => !AllowNullItems || x is not null)
			.Select((x, i) => (Results: x is null ? [new ValidationResult("The item is null.")] : ValidatorUtility.GetValidationResults(x), Index: i))
			.FirstOrDefault(x => x.Results.Count != 0);
		if (validationResults is null)
			return null;

		var innerErrorMessage = string.Join(" ", validationResults.Select(x => x.ErrorMessage).Where(x => x is not null));
		return new ValidationResult(
			errorMessage: $"{FormatErrorMessage(validationContext.DisplayName)}{(innerErrorMessage.Length == 0 ? "" : $" ([{index}]: {innerErrorMessage})")}",
			memberNames: validationContext.MemberName is { } memberName ? new[] { memberName } : null);
	}
}
