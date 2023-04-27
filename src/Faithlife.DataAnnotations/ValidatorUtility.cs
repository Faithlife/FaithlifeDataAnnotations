using System.ComponentModel.DataAnnotations;

namespace Faithlife.DataAnnotations
{
	/// <summary>
	/// Helper methods for using <see cref="Validator"/>.
	/// </summary>
	public static class ValidatorUtility
	{
		/// <summary>
		/// Returns true if the specified object and its properties are valid.
		/// </summary>
		/// <exception cref="ArgumentNullException">The value is null.</exception>
		public static bool IsValid(object value)
		{
			_ = value ?? throw new ArgumentNullException(nameof(value));
			return Validator.TryValidateObject(value, new ValidationContext(value), validationResults: null, validateAllProperties: true);
		}

		/// <summary>
		/// Validates the specified object and its properties, throwing <see cref="ValidationException"/> if invalid.
		/// </summary>
		/// <exception cref="ArgumentNullException">The value is null.</exception>
		/// <exception cref="ValidationException">The value is invalid.</exception>
		public static void Validate(object value)
		{
			_ = value ?? throw new ArgumentNullException(nameof(value));
			Validator.ValidateObject(value, new ValidationContext(value), validateAllProperties: true);
		}

		/// <summary>
		/// Gets the validation results from validating the specified object and its properties.
		/// </summary>
		/// <exception cref="ArgumentNullException">The value is null.</exception>
		public static IReadOnlyList<ValidationResult> GetValidationResults(object value)
		{
			_ = value ?? throw new ArgumentNullException(nameof(value));
			var validationResults = new List<ValidationResult>();
			Validator.TryValidateObject(value, new ValidationContext(value), validationResults, validateAllProperties: true);
			return validationResults;
		}
	}
}
