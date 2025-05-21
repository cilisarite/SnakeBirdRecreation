using System;
using System.Collections.Generic;
using Snakebird.InstanceTile;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Snakebird.Tile
{
    public partial class GameBoard : MonoBehaviour
    {
        #region Serialized.
        #endregion

        #region Structures.
        public struct BoardSaveData
        {
            public SnakebirdSaveData[] snakebirdSaveDatas;
            public FruitSaveData[] fruitSaveDatas;
        }
        #endregion

        #region Public.
        #endregion

        #region Private.
        private List<BoardSaveData> _boardSaveStates;
        #endregion

        #region Events.
        #endregion

        #region Listeners.
        #endregion

        #region Public Methods.
        public void SaveState()
        {
            BoardSaveData boardSaveData = new BoardSaveData();
            boardSaveData.snakebirdSaveDatas = new SnakebirdSaveData[_snakeBirds.Count];
            boardSaveData.fruitSaveDatas = new FruitSaveData[_fruits.Count];

            for (int i = 0; i < _snakeBirds.Count; i++)
            {
                boardSaveData.snakebirdSaveDatas[i] = _snakeBirds[i].Save();
            }
            for (int i = 0; i < _fruits.Count; i++)
            {
                boardSaveData.fruitSaveDatas[i] = _fruits[i].Save();
            }

            _boardSaveStates.Add(boardSaveData);
        }
        public void Rollback()
        {
            if (_boardSaveStates.Count == 0)
                return;

            BoardSaveData boardSaveData = _boardSaveStates[_boardSaveStates.Count - 1];

            for (int i = 0; i < _snakeBirds.Count; i++)
            {
                _snakeBirds[i].Load(boardSaveData.snakebirdSaveDatas[i]);
            }
            for (int i = 0; i < _fruits.Count; i++)
            {
                _fruits[i].Load(boardSaveData.fruitSaveDatas[i]);
            }

            _boardSaveStates.RemoveAt(_boardSaveStates.Count - 1);
        }
        #endregion

        #region Private Methods.
        #endregion
    }
}
