using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	public class Fibonacci {
		private List<ulong> _numbers;

		public Fibonacci() {
			_numbers = new List<ulong>();
		}

		public Fibonacci(int numSteps) : this() {
			ulong firstNumber = 0;
			ulong secondNumber = 1;

			_numbers.Add(firstNumber);

			while (0 != numSteps) {
				_numbers.Add(secondNumber);

				ulong temp = firstNumber;
				firstNumber = secondNumber;
				secondNumber += temp;

				numSteps--;
			}
		}

		public void print() {
			foreach (ulong i in _numbers) {
				Console.Write(i + " ");
			}
			Console.Write("\n");
		}

		public ulong this[int index] {
			get {
				return _numbers[index];
			}
		}
	}
}
