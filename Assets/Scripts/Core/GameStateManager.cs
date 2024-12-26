using UnityEngine;
using UnityEngine.Events;
namespace Game.Core
{
    public class GameStateManager : MonoSingelton<GameStateManager>
    {
        [SerializeField]private GameInput gameInput;
        private GameState currentGameState = GameState.None;
        public UnityEvent<GameState> OnGameStateChanged;


        public void PassGame()
        {
            SetState(GameState.GamePassed);
        }
        public void EndGame()
        {
            SetState(GameState.GameOver);
        }

        public void ResumeGame()
        {
            SetState(GameState.UnPaused);
        }

        public bool IsGamePaused()
        {
            return currentGameState == GameState.Paused;
        }

        public bool IsGameOver()
        {
            return currentGameState == GameState.GameOver;
        }

        public bool IsGamePassed()
        {
            return currentGameState == GameState.GamePassed;
        }

        protected override void InternalInit()
        {
            currentGameState = GameState.None;    
        }

        protected override void InternalOnStart()
        {
            SetState(GameState.UnPaused);
            //Debug.Log("Added HandlePauseLogic");
            gameInput.OnPausePressed += HandlePauseLogic;
        }


        protected override void InternalOnDestroy()
        {
            gameInput.OnPausePressed -= HandlePauseLogic;
        }

        private void SetState(GameState newGameState)
        {
            if(newGameState == currentGameState)
            {
                return;
            }

            currentGameState = newGameState;
            OnGameStateChanged?.Invoke(newGameState);
        }

        private void HandlePauseLogic()
        {
            if(currentGameState == GameState.Paused)
            {
                SetState(GameState.UnPaused);
            }
            else if(currentGameState == GameState.UnPaused)
            {
                SetState(GameState.Paused);
            }
        }
    }
}
