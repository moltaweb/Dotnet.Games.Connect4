using Conecta4.Wpf.Models;
using System;
using System.Collections.Generic;

namespace Conecta4.Wpf.Core
{
    public class Game
    {
        public int BoardRows { get; } = 6;
        public int BoardColumns { get; } = 7;

        private CellModel[,] board; 
        public bool IsPlayerTurn;
        

        public Game()
        {
            board = new CellModel[BoardRows, BoardColumns];
            CreateBoard();
        }

        public (int row, int column) RunComputerTurn()
        {
            int rowPlaced = -1;
            int columnPlaced = -1;

            do
            {
                // Randomnly pick a column
                Random random = new Random();
                columnPlaced = random.Next(0, 7);
                rowPlaced = TryDropChip(columnPlaced);

            } while (rowPlaced < 0);

            IsPlayerTurn = true;

            return (rowPlaced, columnPlaced);
        }

        private int TryDropChip(int col)
        {
            // returns rowPlaced for the passed in column. If -1, it means the column is not valid
            int rowPlaced = -1;

            CellType chipDropped = IsPlayerTurn ? CellType.PlayerChip : CellType.ComputerChip;

            // check if the column is not full, otherwise back to do/while loop to pick another column
            if (board[BoardRows-1, col].CellType == CellType.Empty)
            {
                // place the chip in the first empty row
                for (int row = 0; row < BoardRows; row++)
                {
                    if (board[row, col].CellType == CellType.Empty)
                    {
                        board[row, col].CellType = chipDropped;
                        rowPlaced = row;
                        break;
                    }
                }
            }

            return rowPlaced;
        }

        public int RunPlayerTurn(int column)
        {
            int rowPlaced = TryDropChip(column);
            return rowPlaced;
        }

        public (bool isWinner, List<CellPositionModel>? winner4) CheckWinner()
        {
            List<CellPositionModel> winner4 = new List<CellPositionModel>();

            // check 4 in row
            for (int row = 0; row < BoardRows; row++)
            {
                winner4.Clear();
                winner4.Add(new CellPositionModel(row, 0));

                for (int col = 1; col < BoardColumns; col++)
                {
                    if(board[row, col].CellType != CellType.Empty && board[row, col].CellType == board[row, col - 1].CellType)
                    {
                        winner4.Add(new CellPositionModel(row, col));
                    }
                }

                if (winner4.Count == 4)
                {
                    return (true, winner4);
                }
            }

            // check 4 in column
            for (int col = 0; col < BoardColumns; col++)
            {
                winner4.Clear();
                winner4.Add(new CellPositionModel(0, col));

                for (int row = 1; row < BoardRows; row++)
                {
                    if (board[row, col].CellType != CellType.Empty && board[row, col].CellType == board[row - 1, col].CellType)
                    {
                        winner4.Add(new CellPositionModel(row, col));
                    }
                }

                if (winner4.Count == 4)
                {
                    return (true, winner4);
                }
            }

            // check 4 in diagonal Ascending
            for (int row = 1; row < BoardRows; row++)
            {
                winner4.Clear();
                winner4.Add(new CellPositionModel(row, 1));

                for (int col = 1; col < BoardColumns; col++)
                {
                    if (board[row, col].CellType != CellType.Empty && board[row, col].CellType == board[row - 1, col - 1].CellType)
                    {
                        winner4.Add(new CellPositionModel(row, col)); 
                    }
                }

                if (winner4.Count == 4)
                {
                    return (true, winner4);
                }
            }

            // check 4 in diagonal Descending
            for (int row = BoardRows - 2; row >= 0; row--)
            {
                winner4.Clear();
                winner4.Add(new CellPositionModel(row, 1));

                for (int col = 1; col < BoardColumns; col++)
                {
                    if (board[row, col].CellType != CellType.Empty && board[row, col].CellType == board[row + 1, col - 1].CellType)
                    {
                        winner4.Add(new CellPositionModel(row, col));
                    }
                }

                if (winner4.Count == 4)
                {
                    return (true, winner4);
                }
            }

            return (false, null);
        }

        private void CreateBoard()
        {            

            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    board[row, col] = new CellModel { CellType = CellType.Empty };
                }
            }
        }

    }
}
