using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	public class Fibonacci {
		private List<long> _numbers;

		public Fibonacci() {
			_numbers = new List<long>();
		}

		public Fibonacci(int numSteps) : this() {
			long firstNumber = 0;
			long secondNumber = 1;

			_numbers.Add(firstNumber);

			while (0 != numSteps) {
				_numbers.Add(secondNumber);

				long temp = firstNumber;
				firstNumber = secondNumber;
				secondNumber += temp;

				numSteps--;
			}
		}

		public void print() {
			foreach (long i in _numbers) {
				Console.Write(i + " ");
			}
			Console.Write("\n");
		}

		public long this[int index] {
			get {
				return _numbers[index];
			}
		}
	}
}
