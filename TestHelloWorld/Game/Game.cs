using System;
using System.Collections.Generic;

namespace TestHelloWorld {
	namespace Games {
		// !!! In theory, some of the Rule, Move, Piece, etc.
		// !!! classes should be within this class, as there is no
		// !!! reason for the outside world to see some of them.  However,
		// !!! for now, there could be, so I'm leaving it as is for now.

		public abstract class Game {
			protected List<Rule> _rules;

			protected List<Player> _players;

			protected bool _isActive;
			protected int _maxNumPlayers;

			protected bool _requiresUserInput;

			public Game() {
				_rules = new List<Rule>();
				_players = new List<Player>();
				_isActive = false;
			}

			public Game(int maxNumPlayers) {
				_maxNumPlayers = maxNumPlayers;
				_isActive = false;

				_rules = new List<Rule>();
				_players = new List<Player>(maxNumPlayers);
			}

			public virtual void RequireInput() {
				_requiresUserInput = true;
			}

			public virtual void DoneWithInput() {
				_requiresUserInput = false;
			}

			public virtual void Begin() {
				_isActive = true;
			}

			public virtual void End() {
				_isActive = false;
			}

			public virtual void NewGame() {
				_players.Clear();
			}

			protected abstract void Reset();
			public abstract void Clear();

			protected virtual void AddPlayer(Player p) {
				_players.Add(p);
			}

			protected void AddRule(Rule r) {
				_rules.Add(r);
			}

			public abstract string name {
				get;
			}

			public bool isActive {
				get {
					return _isActive;
				}
			}
		}
	}
}
