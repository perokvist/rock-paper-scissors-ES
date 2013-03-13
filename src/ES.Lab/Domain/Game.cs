using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ES.Lab.Commands;
using ES.Lab.Events;
using Treefort.Events;

namespace ES.Lab.Domain
{
    public class Game
    {
        private readonly IDictionary<Choice, Choice> winnersAgainst;
        private Guid id;
        private string title;
        private string winner;
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
                               command.AggregateId,
                               command.PlayerId,
                               command.Title,
                               command.FirstTo,
                               DateTime.UtcNow) {CorrelationId = command.CorrelationId} //TODO move to appservice
                       };
        }
        
        public IEnumerable<IEvent> Handle(JoinGameCommand command)
        {
            var events = new List<IEvent>();

            if (playerTwo == null && playerOne != null) //throw ex ?
            {
                events.Add(new GameStartedEvent(id, playerOne.Email, command.PlayerId));
                events.Add(new RoundStartedEvent(id, 1));
            }

            return events;
        }

        public IEnumerable<IEvent> Handle(MakeChoiceCommand command)
        {
            var events = new List<IEvent>();

            if (playerOne == null || playerTwo == null) //TODO throw ex?
                return events;
                
            var playerOneChoice = playerOne.CurrentChoice;
            var playerTwoChoice = playerTwo.CurrentChoice;

            if (state != GameState.Started)
                return events;

            if (IsPlayerOne(command.PlayerId) && playerOneChoice == Choice.None)
                playerOneChoice = PlayerChoice(command, events);
            else if (IsPlayerTwo(command.PlayerId) && playerTwoChoice == Choice.None)
                playerTwoChoice = PlayerChoice(command, events);

            // Decide winner if both has chosen
            if (playerOneChoice != Choice.None && playerTwoChoice != Choice.None)
            {
                // decide round
                var newRound = true;
                Choice winsAgainstOne = winnersAgainst[playerOne.CurrentChoice];

                if (playerTwoChoice == winsAgainstOne)
                    newRound = RoundWonBy(playerTwo, command, events);
                else if (playerOneChoice == playerTwo.CurrentChoice)
                    events.Add(new RoundTiedEvent(command.AggregateId, round));
                else
                    newRound = RoundWonBy(playerOne, command, events);

                if (newRound)
                    events.Add(new RoundStartedEvent(command.AggregateId, round + 1));
            }
            return events;
        }

        public void Handle(GameCreatedEvent @event)
        {
            id = @event.GameId;
            title = @event.Title;
            playerOne = new GamePlayer(@event.PlayerId);
            firstTo = @event.FirstTo;
        }

        public void Handle(ChoiceMadeEvent @event)
        {
            if (IsPlayerOne(@event.PlayerId))
                playerOne.CurrentChoice = @event.Choice;
            else if (IsPlayerTwo(@event.PlayerId))
                playerTwo.CurrentChoice = @event.Choice;
        }

        public void Handle(RoundStartedEvent @event)
        {
            id = @event.GameId;
            round = @event.Round;
            playerOne.CurrentChoice = Choice.None;
            playerTwo.CurrentChoice = Choice.None;
        }

        public void Handle(RoundWonEvent @event)
        {
            if (IsPlayerOne(@event.PlayerId))
                playerOne.AddWin();
            else if (IsPlayerTwo(@event.PlayerId))
                playerTwo.AddWin();
        }

        public void Handle(GameWonEvent @event)
        {
            state = GameState.Finished;
            winner = @event.PlayerId;
        }

        public void Handle(GameStartedEvent @event)
        {
            state = GameState.Started;
            playerTwo = new GamePlayer(@event.PlayerTwoId);
        }


        private bool IsWinner(GamePlayer player)
        {
            return (player.Score + 1) >= firstTo;
        }

        private void GameWonBy(GamePlayer player, MakeChoiceCommand command, List<IEvent> events)
        {
            events.Add(new GameWonEvent(command.AggregateId, player.Email));
        }

        private bool RoundWonBy(GamePlayer player, MakeChoiceCommand command, List<IEvent> events)
        {
            events.Add(new RoundWonEvent(command.AggregateId, player.Email, round));
            if (IsWinner(playerTwo))
            {
                GameWonBy(playerTwo, command, events);
                return false;
            }
            return true;
        }

        private Choice PlayerChoice(MakeChoiceCommand command, List<IEvent> events)
        {
            events.Add(new ChoiceMadeEvent(
                         command.AggregateId,
                         this.round,
                         command.PlayerId,
                         command.Choice));
            var playerChoice = command.Choice;
            return playerChoice;
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