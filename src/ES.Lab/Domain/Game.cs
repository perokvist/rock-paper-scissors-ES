using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ES.Lab;
using ES.Lab.Commands;
using ES.Lab.Events;

namespace ES.Lab.Domain
{
    public class Game
    {
        private readonly IDictionary<Choice, Choice> winnersAgainst;
        private Guid id;
        private string title;
        private GamePlayer playerOne;
        private GamePlayer playerTwo;
        private int firstTo;
        private GameState state;
        private int round;

        public Game()
        {
            winnersAgainst = new ConcurrentDictionary<Choice, Choice>();
            winnersAgainst.Add(Choice.Rock, Choice.Paper);
            winnersAgainst.Add(Choice.Scissors, Choice.Rock);
            winnersAgainst.Add(Choice.Paper, Choice.Scissors);
        }

        public IEnumerable<IEvent> Handle(CreateGameCommand command)
        {
            return new List<IEvent>
                       {
                           new GameCreatedEvent(
                               command.EntityId,
                               command.PlayerId,
                               command.Title,
                               command.FirstTo,
                               DateTime.UtcNow)
                       };
        }

        public IEnumerable<IEvent> Handle(JoinGameCommand command)
        {
            var events = new List<IEvent>();

            if (playerTwo == null)
            {
                events.Add(new GameStartedEvent(id, playerOne.Email, command.PlayerId));
                events.Add(new RoundStartedEvent(id, 1));
            }

            return events;
        }

        public IEnumerable<IEvent> Handle(MakeChoiceCommand command)
        {
            var events = new List<IEvent>();
            var playerOneChoice = playerOne.CurrentChoice;
            var playerTwoChoice = playerTwo.CurrentChoice;

            if (state != GameState.Started)
            {
                return events;
            }

            if (IsPlayerOne(command.PlayerId) && playerOneChoice == null)
            {

                events.Add(new ChoiceMadeEvent(
                               command.EntityId,
                               this.round,
                               command.PlayerId,
                               command.Choice));
                playerOneChoice = command.Choice;
            }
            else if (IsPlayerTwo(command.PlayerId) && playerTwoChoice == null)
            {
                events.Add(new ChoiceMadeEvent(
                               command.EntityId,
                               round,
                               command.PlayerId,
                               command.Choice));
                playerTwoChoice = command.Choice;
            }

            // Decide winner if both has chosen
            if (playerOneChoice != null && playerTwoChoice != null)
            {
                // decide round
                var newRound = true;
                Choice winsAgainstOne = winnersAgainst[playerOne.CurrentChoice];

                if (playerTwoChoice == winsAgainstOne)
                {
                    events.Add(new RoundWonEvent(command.EntityId, playerTwo.Email, round));
                    if ((playerTwo.Score + 1) >= firstTo)
                    {
                        events.Add(new GameWonEvent(command.EntityId, playerTwo.Email));
                        newRound = false;
                    }
                }
                else if (playerOneChoice == playerTwo.CurrentChoice)
                {
                    events.Add(new RoundTiedEvent(command.EntityId, round));
                }
                else
                {
                    events.Add(new RoundWonEvent(command.EntityId, playerOne.Email, round));

                    if ((playerOne.Score + 1) >= firstTo)
                    {
                        events.Add(new GameWonEvent(command.EntityId, playerOne.Email));
                        newRound = false;
                    }
                }

                if (newRound)
                {
                    events.Add(new RoundStartedEvent(command.EntityId, round + 1));
                }
            }
            return events;
        }

        public void Handle(GameCreatedEvent @event)
        {
            this.id = @event.GameId;
            this.title = @event.Title;
            this.playerOne = new GamePlayer(@event.PlayerId);
            this.firstTo = @event.FirstTo;
        }

        private bool IsPlayerOne(String email)
        {
            return email == playerOne.Email;
        }

        private bool IsPlayerTwo(String email)
        {
            return email == playerTwo.Email;
        }

    }
}

    public class RoundTiedEvent : IEvent
    {
        public RoundTiedEvent(Guid entityId, int round)
        {
            throw new NotImplementedException();
        }
    }

    public class GameWonEvent : IEvent
    {
        public GameWonEvent(Guid entityId, string email)
        {
            throw new NotImplementedException();
        }
    }

    public class RoundWonEvent : IEvent
    {
        public RoundWonEvent(Guid entityId, string email, int round)
        {
            throw new NotImplementedException();
        }
    }
