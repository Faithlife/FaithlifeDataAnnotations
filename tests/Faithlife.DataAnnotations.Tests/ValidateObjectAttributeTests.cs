using System.ComponentModel.DataAnnotations;
using NUnit.Framework;

namespace Faithlife.DataAnnotations.Tests;

public sealed class ValidateObjectAttributeTests
{
	[Test]
	public void ValidateRecursive()
	{
		var invalid = new ValidatableDto();
		var validatable = new ValidatableDto { Required = "x", Validatable = invalid };

		var results = ValidatorUtility.GetValidationResults(validatable);
		Assert.That(results.Single().MemberNames.Single(), Is.EqualTo(nameof(ValidatableDto.Validatable)));
		Assert.That(results.Single().ErrorMessage, Is.EqualTo("The field Validatable is invalid. (The Required field is required.)"));

		validatable.Validatable = new ValidatableDto { Required = "x", Validatable = invalid };
		results = ValidatorUtility.GetValidationResults(validatable);
		Assert.That(results.Single().MemberNames.Single(), Is.EqualTo(nameof(ValidatableDto.Validatable)));
		Assert.That(results.Single().ErrorMessage, Is.EqualTo("The field Validatable is invalid. (The field Validatable is invalid. (The Required field is required.))"));

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
