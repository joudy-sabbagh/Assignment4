// Domain/ValueObjects/Money.cs
using System;

namespace Domain.ValueObjects
{
	public readonly struct Money
	{
		public decimal Amount { get; }
		public Money(decimal amount)
		{
			if (amount < 0)
				throw new ArgumentException(nameof(amount));
			Amount = amount;
		}
		public override string ToString() => Amount.ToString("C");
	}
}
