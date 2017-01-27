using System;
namespace TestHelloWorld
{
	public class FizzBuzz
	{
		public FizzBuzz()
		{
		}

		public string this[int index] {
			get {
				string returnValue = "";

				if (0 == (index % 5)) {
					returnValue += "Fizz";
				}
				if (0 == (index % 3)) {
					returnValue += "Buzz";
				}

				return returnValue;
			}
		}
	}
}
