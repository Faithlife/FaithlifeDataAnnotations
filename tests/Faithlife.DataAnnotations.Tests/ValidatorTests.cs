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
		Assert.That(results.Single().MemberNames.Single(), Is.EqualTo(nameof(ValidatableDto.Required)));
		var exception = Assert.Throws<ValidationException>(() => ValidatorUtility.Validate(validatable));
		Assert.That(exception!.ValidationResult.MemberNames.Single(), Is.EqualTo(nameof(ValidatableDto.Required)));

		validatable.Required = "";
		results = ValidatorUtility.GetValidationResults(validatable);
		Assert.That(results.Single().MemberNames.Single(), Is.EqualTo(nameof(ValidatableDto.Required)));
		Assert.Throws<ValidationException>(() => ValidatorUtility.Validate(validatable));

		validatable.Required = " ";
		results = ValidatorUtility.GetValidationResults(validatable);
		Assert.That(results.Single().MemberNames.Single(), Is.EqualTo(nameof(ValidatableDto.Required)));
		Assert.Throws<ValidationException>(() => ValidatorUtility.Validate(validatable));

		validatable.Required = "x";
		results = ValidatorUtility.GetValidationResults(validatable);
		Assert.That(results, Is.Empty);
		ValidatorUtility.Validate(validatable);
	}

	[Test]
	public void ValidateRecursive()
	{
		var invalid = new ValidatableDto();
		var validatable = new ValidatableDto { Required = "x", Validatable = invalid };

		var results = ValidatorUtility.GetValidationResults(validatable);
		Assert.That(results.Single().MemberNames.Single(), Is.EqualTo(nameof(ValidatableDto.Validatable)));

		validatable.Validatable = new ValidatableDto { Required = "x", Validatable = invalid };
		results = ValidatorUtility.GetValidationResults(validatable);
		Assert.That(results.Single().MemberNames.Single(), Is.EqualTo(nameof(ValidatableDto.Validatable)));

		validatable.Validatable.Validatable = new ValidatableDto { Required = "x" };
		results = ValidatorUtility.GetValidationResults(validatable);
		Assert.That(results, Is.Empty);
	}

	private sealed class ValidatableDto
	{
		[Required]
		public string? Required { get; set; }

		[ValidateObject]
		public ValidatableDto? Validatable { get; set; }
	}
}
