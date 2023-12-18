using System;
using System.Collections.Generic;
using Core.Game;
using Core.Lobby;
using Core.Service;
using Core.Startup;
using Cysharp.Threading.Tasks;

namespace Core.StateMachine
{
    public class ApplicationStateMachine
    {
        private readonly Dictionary<Type, IApplicationState> _states;
        private IApplicationState _activeApplicationState;

        public ApplicationStateMachine(
            ISceneLoadService sceneLoadService,
            IStartupFactory startupFactory,
            ILobbyFactory lobbyFactory,
            IGameFactory gameFactory
        )
        {
            var startupState = new StartupApplicationState(
                this,
                sceneLoadService,
                startupFactory);

            var lobbyState = new LobbyApplicationState(
                lobbyFactory,
                sceneLoadService);
            
            var gameState = new GameApplicationState(
                gameFactory,
                sceneLoadService
            );

            _states = new Dictionary<Type, IApplicationState>
            {
                [typeof(StartupApplicationState)] = startupState,
                [typeof(LobbyApplicationState)] = lobbyState,
                [typeof(GameApplicationState)] = gameState
             };
        }
        
        public async UniTaskVoid Enter<TState>() where TState : class, IApplicationState
        {
            var state = ChangeState<TState>();
            await state.Enter();
        }
        
        private TState ChangeState<TState>() where TState : class, IApplicationState
        {
            _activeApplicationState?.Exit();

            var state = GetState<TState>();
            _activeApplicationState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IApplicationState =>
            _states[typeof(TState)] as TState;
    }
}