using System;
namespace TestHelloWorld {
	namespace Games {
		public class Move {
			private readonly int _xDisplacement;
			private readonly int _yDisplacement;

			private readonly bool _isInfinite;

			public Move(int xd, int yd, bool isInfinite) {
				_xDisplacement = xd;
				_yDisplacement = yd;
				_isInfinite = isInfinite;
			}

			public bool IsLegal(int xd, int yd) {
				if (xd == _xDisplacement &&
					yd == _yDisplacement) {
					return true;
				} else {
					if (false == _isInfinite) {
						return false;
					} else {
						// This is a bit silly, but it's more accurate
						// than trying to use dot products with imprecise equivalence to 0 or 1.

						if (0 == _xDisplacement ||
							0 == _yDisplacement) {
							if (0 == _xDisplacement) {
								if (0 != xd) {
									return false;
								} else {
									if (Math.Sign(yDisplacement) == Math.Sign(yd)) {
										return true;
									} else {
										return false;
									}
								}
							}
							if (0 == _yDisplacement) {
								if (0 != yd) {
									return false;
								} else {
									if (Math.Sign(xDisplacement) == Math.Sign(xd)) {
										return true;
									} else {
										return false;
									}
								}
							}

							// Should never get here
							return false;
						} else {
							if (xd / _xDisplacement == yd / _yDisplacement) {
								if (Math.Sign(xd) == Math.Sign(_xDisplacement) &&
									Math.Sign(yd) == Math.Sign(_yDisplacement)) {
									return true;
								} else {
									return false;
								}
							} else {
								return false;
							}
						}
					}
				}
			}

			public bool IsLegal(Move m) {
				return IsLegal(m.xDisplacement, m.yDisplacement);
			}

			public Coordinate AddToCoordinate(Coordinate c) {
				return new Coordinate(c.x + _xDisplacement, c.y + _yDisplacement);
			}

			public bool TakeStep(int stepIndex, ref Coordinate c) {
				if (stepIndex == stepCount) {
					return false;
				}

				if (_xDisplacement != 0 &&
					_yDisplacement != 0) {
					if (Math.Abs(_xDisplacement) == Math.Abs(_yDisplacement)) {
						c += new Coordinate(Math.Sign(xDisplacement), Math.Sign(yDisplacement));
					} else {
						bool isIncrementingY = false;
						int numDominantSteps = 0;
						if (_xDisplacement > _yDisplacement) {
							numDominantSteps = Math.Abs(_xDisplacement);

							if (stepIndex < numDominantSteps) {
								isIncrementingY = false;
							} else {
								isIncrementingY = true;
							}
						} else {
							numDominantSteps = Math.Abs(_yDisplacement);

							if (stepIndex < numDominantSteps) {
								isIncrementingY = true;
							} else {
								isIncrementingY = false;
							}							
						}

						c += true == isIncrementingY ? new Coordinate(0, Math.Sign(_yDisplacement)) :
							new Coordinate(Math.Sign(_xDisplacement), 0);
					}
				} else {
					c += new Coordinate(Math.Sign(_xDisplacement), Math.Sign(_yDisplacement));
				}

				return true;
			}

			public int stepCount {
				get {
					// If it's a diagonal move, it's just the same steps as
					// one of the steps.

					// Otherwise, we just add the displacements together.

					if (Math.Abs(_xDisplacement) == Math.Abs(_yDisplacement)) {
						return Math.Abs(_xDisplacement);
					} else {
						return Math.Abs(_xDisplacement) + Math.Abs(_yDisplacement);
					}
				}
			}

			public int xDisplacement {
				get {
					return _xDisplacement;
				}
			}

			public int yDisplacement {
				get {
					return _yDisplacement;
				}
			}
		}
	}
}
