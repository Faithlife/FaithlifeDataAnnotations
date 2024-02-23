using System.ComponentModel.DataAnnotations;
using NUnit.Framework;

namespace Faithlife.DataAnnotations.Tests;

public sealed class ValidateItemsAttributeTests
{
	[Test]
	public void ValidateItems()
	{
		var validatable = new ValidatableDto { Required = "x" };

		var results = ValidatorUtility.GetValidationResults(validatable);
		Assert.That(results, Is.Empty);

		validatable.Validatables = [null!];
		results = ValidatorUtility.GetValidationResults(validatable);
		Assert.That(results.Single().MemberNames.Single(), Is.EqualTo(nameof(ValidatableDto.Validatables)));
		Assert.That(results.Single().ErrorMessage, Is.EqualTo("The field Validatables is invalid. ([0]: The item is null.)"));

		validatable.Validatables = [new ValidatableDto(), new ValidatableDto()];
		results = ValidatorUtility.GetValidationResults(validatable);
		Assert.That(results.Single().MemberNames.Single(), Is.EqualTo(nameof(ValidatableDto.Validatables)));
		Assert.That(results.Single().ErrorMessage, Is.EqualTo("The field Validatables is invalid. ([0]: The Required field is required.)"));

		validatable.Validatables[0].Required = "x";
		results = ValidatorUtility.GetValidationResults(validatable);
		Assert.That(results.Single().MemberNames.Single(), Is.EqualTo(nameof(ValidatableDto.Validatables)));
		Assert.That(results.Single().ErrorMessage, Is.EqualTo("The field Validatables is invalid. ([1]: The Required field is required.)"));

		validatable.Validatables[1].Required = "x";
		results = ValidatorUtility.GetValidationResults(validatable);
		Assert.That(results, Is.Empty);
	}

	[Test]
	public void ValidateNullableItems()
	{
		var validatable = new ValidatableDto { Required = "x" };

		var results = ValidatorUtility.GetValidationResults(validatable);
		Assert.That(results, Is.Empty);

		validatable.NullableValidatables = [null!];
		results = ValidatorUtility.GetValidationResults(validatable);
		Assert.That(results, Is.Empty);

		validatable.Validatables = [new ValidatableDto()];
		results = ValidatorUtility.GetValidationResults(validatable);
		Assert.That(results.Single().MemberNames.Single(), Is.EqualTo(nameof(ValidatableDto.Validatables)));
		Assert.That(results.Single().ErrorMessage, Is.EqualTo("The field Validatables is invalid. ([0]: The Required field is required.)"));
	}

	private sealed class ValidatableDto
	{
		[Required]
		public string? Required { get; set; }

		[ValidateItems]
		public IReadOnlyList<ValidatableDto>? Validatables { get; set; }

		[ValidateItems(AllowNullItems = true)]
		public IReadOnlyList<ValidatableDto?>? NullableValidatables { get; set; }
	}
}
