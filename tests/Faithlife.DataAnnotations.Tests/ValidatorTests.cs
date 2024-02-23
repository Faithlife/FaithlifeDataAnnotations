using System.ComponentModel.DataAnnotations;
using NUnit.Framework;

namespace Faithlife.DataAnnotations.Tests;

public sealed class ValidatorTests
{
	[Test]
	public void ValidateNull()
	{
		Assert.Throws<ArgumentNullException>(() => ValidatorUtility.GetValidationResults(null!));
	}

	[TestCase(false)]
	[TestCase(0)]
	[TestCase("")]
	public void ValidateNotValidatable(object value)
	{
		Assert.That(ValidatorUtility.IsValid(value));
		Assert.That(ValidatorUtility.GetValidationResults(value), Is.Empty);
		ValidatorUtility.Validate(value);
	}

	[Test]
	public void ValidateRequired()
	{
		var validatable = new ValidatableDto();

		var results = ValidatorUtility.GetValidationResults(validatable);
		AssertFieldIsRequired(results.Single());
		var exception = Assert.Throws<ValidationException>(() => ValidatorUtility.Validate(validatable));
		AssertFieldIsRequired(exception!.ValidationResult);

		validatable.Required = "";
		results = ValidatorUtility.GetValidationResults(validatable);
		AssertFieldIsRequired(results.Single());
		Assert.Throws<ValidationException>(() => ValidatorUtility.Validate(validatable));

		validatable.Required = " ";
		results = ValidatorUtility.GetValidationResults(validatable);
		AssertFieldIsRequired(results.Single());
		Assert.Throws<ValidationException>(() => ValidatorUtility.Validate(validatable));

		validatable.Required = "x";
		results = ValidatorUtility.GetValidationResults(validatable);
		Assert.That(results, Is.Empty);
		ValidatorUtility.Validate(validatable);

		void AssertFieldIsRequired(ValidationResult result)
		{
			Assert.That(result.MemberNames.Single(), Is.EqualTo(nameof(ValidatableDto.Required)));
			Assert.That(result.ErrorMessage, Is.EqualTo("The Required field is required."));
		}
	}

	private sealed class ValidatableDto
	{
		[Required]
		public string? Required { get; set; }
	}
}
