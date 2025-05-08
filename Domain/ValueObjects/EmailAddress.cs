// Domain/ValueObjects/EmailAddress.cs
using System;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects
{
	public sealed record EmailAddress
	{
		public string Value { get; }
		public EmailAddress(string value)
		{
			if (string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
				throw new ArgumentException(nameof(value));
			Value = value;
		}
		public override string ToString() => Value;
	}
}
