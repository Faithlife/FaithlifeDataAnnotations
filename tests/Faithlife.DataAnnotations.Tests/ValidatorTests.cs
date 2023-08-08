using System.ComponentModel.DataAnnotations;
using NUnit.Framework;

namespace Faithlife.DataAnnotations.Tests
{
	public class ValidatorTests
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
			Assert.IsTrue(ValidatorUtility.IsValid(value));
			Assert.IsEmpty(ValidatorUtility.GetValidationResults(value));
			ValidatorUtility.Validate(value);
		}

		[Test]
		public void ValidateRequired()
		{
			var validatable = new ValidatableDto();

			var results = ValidatorUtility.GetValidationResults(validatable);
			Assert.AreEqual(nameof(ValidatableDto.Required), results.Single().MemberNames.Single());
			var exception = Assert.Throws<ValidationException>(() => ValidatorUtility.Validate(validatable));
			Assert.AreEqual(nameof(ValidatableDto.Required), exception!.ValidationResult.MemberNames.Single());

			validatable.Required = "";
			results = ValidatorUtility.GetValidationResults(validatable);
			Assert.AreEqual(nameof(ValidatableDto.Required), results.Single().MemberNames.Single());
			Assert.Throws<ValidationException>(() => ValidatorUtility.Validate(validatable));

			validatable.Required = " ";
			results = ValidatorUtility.GetValidationResults(validatable);
			Assert.AreEqual(nameof(ValidatableDto.Required), results.Single().MemberNames.Single());
			Assert.Throws<ValidationException>(() => ValidatorUtility.Validate(validatable));

			validatable.Required = "x";
			results = ValidatorUtility.GetValidationResults(validatable);
			Assert.IsEmpty(results);
			ValidatorUtility.Validate(validatable);
		}

		[Test]
		public void ValidateRecursive()
		{
			var invalid = new ValidatableDto();
			var validatable = new ValidatableDto { Required = "x", Validatable = invalid };

			var results = ValidatorUtility.GetValidationResults(validatable);
			Assert.AreEqual(nameof(ValidatableDto.Validatable), results.Single().MemberNames.Single());

			validatable.Validatable = new ValidatableDto { Required = "x", Validatable = invalid };
			results = ValidatorUtility.GetValidationResults(validatable);
			Assert.AreEqual(nameof(ValidatableDto.Validatable), results.Single().MemberNames.Single());

			validatable.Validatable.Validatable = new ValidatableDto { Required = "x" };
			results = ValidatorUtility.GetValidationResults(validatable);
			Assert.IsEmpty(results);
		}

		private sealed class ValidatableDto
		{
			[Required]
			public string? Required { get; set; }

			[ValidateObject]
			public ValidatableDto? Validatable { get; set; }
		}
	}
}
