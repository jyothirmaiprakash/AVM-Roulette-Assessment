using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Roulette
{

    public static class Roulette
    {
        private static decimal _userBankRoll;

        private static decimal _currentBetAmount;

        private static decimal UserBankroll
        {
            get => _userBankRoll; set
            {
                _userBankRoll = decimal.Round(value, 2, MidpointRounding.ToZero);
            }
        }

        private static decimal CurrentBetAmount
        {
            get => _currentBetAmount; set
            {

                _currentBetAmount = decimal.Round(value, 2, MidpointRounding.AwayFromZero);
            }
        }

        public static void StartSession()
        {
            Recharge();
            PlayGame();
        }

        public static void Recharge()
        {
            Console.WriteLine("Enter Amount to start game");
            UserBankroll = Helper.GetNumericUserInput<decimal>("Please Enter Valid Amount to Start the Game", 0.1M, null);

        }

        public static void ExitGame()
        {
            Console.WriteLine("Thank you for playing");
            Environment.Exit(0);
        }

        private static void PlayGame()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("Amount in your wallet: " + string.Format("{0:0.00}", UserBankroll));

                    int selectedBetOption = GetBetOptions();
                    Console.WriteLine("Chosen Option: " + selectedBetOption);


                    switch (selectedBetOption)
                    {
                        case 1:
                            PlayRedOrBlackBet();
                            break;
                        case 2:
                            PlayOddOrEventBet();
                            break;
                        case 3:
                            PlaySingleNumberBet();
                            break;
                        case 4:
                            PlayRangeOfNumbersBet();
                            break;
                        case 5:
                            PlayLowOrHighNumbersBet();
                            break;
                        case 6:
                            ExitGame();
                            break;
                    }

                    if (UserBankroll > 0)
                    {
                        continue;
                    }

                    Console.WriteLine("You don't have sufficient funds to continue the game");
                    Console.WriteLine("Would You like to recharge Yes(Y) or No (N) ");
                    var userFeedBack = Helper.GetTextUserInput("Please enter Y or N", "Y", "N");
                    if (userFeedBack == "Y")
                    {
                        Recharge();
                        continue;
                    }

                    ExitGame();
                    break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                CurrentBetAmount = 0;
                UserBankroll = 0;
            }
        }




        /// <summary>
        /// method to let the user to enter the bet option
        /// This method shows the available bet optons to the user and asks the user to enter the bet option
        /// </summary>
        /// <returns></returns>
        private static int GetBetOptions()
        {
            Console.WriteLine("Choose any one bet from below options");
            Console.WriteLine("1. bet on red or black");
            Console.WriteLine("2. bet on odd or even");
            Console.WriteLine("3. bet on a specific number");
            Console.WriteLine("4. bet on range of numbers");
            Console.WriteLine("5. bet on low or high range numbers");
            Console.WriteLine("6. Exit");
            Console.WriteLine("Enter your option number");

            return Helper.GetNumericUserInput<int>("Please choose the valid option", 1, 6);

        }


        /// <summary>
        /// Enable user to play Red or Black bet
        /// </summary>
        /// <returns></returns>
        private static void PlayRedOrBlackBet()
        {
            Console.WriteLine("Choose Red or Black");
            Console.WriteLine("Enter 'R' for Red, 'B' for Black");
            var choseValue = Helper.GetTextUserInput("Please enter valid entry", "R", "B");
            Console.WriteLine("Chosen Value: " + choseValue);
            SetBetAmount();
            int spinValue = SpinWheel();
            ComputeUserBankrollForOddOrEvenBet(choseValue == "R" ? "E" : "O", spinValue);

        }


        /// <summary>
        /// Enable user to bet on Odd or Even Numbers
        /// </summary>
        /// <returns></returns>
        private static void PlayOddOrEventBet()
        {
            Console.WriteLine("Choose Odd or Even");
            Console.WriteLine("Enter 'O' for Odd, 'E' for Even");
            var choseValue = Helper.GetTextUserInput("Please enter valid entry", "O", "E");
            Console.WriteLine("Chosen Value: " + choseValue);
            SetBetAmount();
            int spinValue = SpinWheel();
            ComputeUserBankrollForOddOrEvenBet(choseValue, spinValue);
        }


        /// <summary>
        /// Reusable logic for Red or Black bet and Odd or Even bet
        /// </summary>
        /// <param name="chosenValue"></param>
        /// <param name="valueOnWheel"></param>
        /// <returns></returns>
        private static void ComputeUserBankrollForOddOrEvenBet(string chosenValue, int valueOnWheel)
        {

            if (valueOnWheel <= 0)
            {
                UpdateAndNotifyUserBankRollOnLose();
            }
            else
            {
                if (valueOnWheel % 2 == 0)
                {
                    if (chosenValue == "E")
                    {
                        UpdateAndNotifyUserBankRollOnWin(1);
                        return;
                    }
                }
                else
                {
                    if (chosenValue == "O")
                    {
                        UpdateAndNotifyUserBankRollOnWin(1);
                        return;
                    }
                }

                UpdateAndNotifyUserBankRollOnLose();

            }
        }


        /// <summary>
        /// method with logic for playing single number bet
        /// </summary>
        /// <returns></returns>
        private static void PlaySingleNumberBet()
        {
            Console.WriteLine("Enter number between 0 and 36 or -1 for '00'");
            var choseValue = Helper.GetNumericUserInput<int>("Please enter valid number", -1, 36);
            Console.WriteLine("Chosen Value: " + (choseValue == -1 ? "00" : choseValue.ToString()));
            SetBetAmount();
            int spinValue = SpinWheel();
            if (choseValue == spinValue)
            {
                UpdateAndNotifyUserBankRollOnWin(35);
            }
            else
            {
                UpdateAndNotifyUserBankRollOnLose();
            }
        }
        /// <summary>
        /// method with logic to play range of numbers bet
        /// </summary>
        private static void PlayRangeOfNumbersBet()
        {
            Console.WriteLine("Choose Range of Numbers");
            Console.WriteLine("Enter 'A' for 1-12, 'B' for 13-24, 'C' for 25-36");
            var choseValue = Helper.GetTextUserInput("Please enter valid entry", "A", "B", "C");
            Console.WriteLine("Chosen Value: " + choseValue);
            SetBetAmount();
            int spinValue = SpinWheel();

            if (spinValue < 0)
            {
                UpdateAndNotifyUserBankRollOnLose();
                return;
            }

            if (spinValue < 13)
            {
                if (choseValue == "A")
                {
                    UpdateAndNotifyUserBankRollOnWin(2);
                    return;
                }
            }
            else
            {
                if (spinValue < 25)
                {
                    if (choseValue == "B")
                    {
                        UpdateAndNotifyUserBankRollOnWin(2);
                        return;
                    }
                }
                else
                {
                    if (choseValue == "C")
                    {
                        UpdateAndNotifyUserBankRollOnWin(2);
                        return;
                    }
                }
            }

            UpdateAndNotifyUserBankRollOnLose();

        }
        /// <summary>
        /// method with logic for playing Low or High number range bet
        /// </summary>

        private static void PlayLowOrHighNumbersBet()
        {
            Console.WriteLine("Choose Low or High Range of Numbers");
            Console.WriteLine("Enter 'L' for 1-18, 'H' for 19-36");
            var choseValue = Helper.GetTextUserInput("Please enter valid entry", "L", "H");
            Console.WriteLine("Chosen Value: " + choseValue);
            SetBetAmount();
            int spinValue = SpinWheel();
            if (spinValue < 0)
            {
                UpdateAndNotifyUserBankRollOnLose();
                return;
            }

            if (spinValue < 19)
            {
                if (choseValue == "L")
                {
                    UpdateAndNotifyUserBankRollOnWin(1);
                    return;
                }
            }
            else
            {
                if (choseValue == "H")
                {
                    UpdateAndNotifyUserBankRollOnWin(1);
                    return;
                }
            }

            UpdateAndNotifyUserBankRollOnLose();

        }

        /// <summary>
        /// Method to execute if a user wins a bet
        /// </summary>
        /// <param name="rate"></param>
        private static void UpdateAndNotifyUserBankRollOnWin(int rate)
        {
            Console.WriteLine("Congratulations!!! you won the bet");
            UserBankroll += (rate * CurrentBetAmount);
            CurrentBetAmount = 0;
        }
        /// <summary>
        /// Method to execute if a user loses a bet
        /// </summary>
        private static void UpdateAndNotifyUserBankRollOnLose()
        {
            Console.WriteLine("Sorry!!! you lost the bet");
            UserBankroll -= CurrentBetAmount;
            CurrentBetAmount = 0;
        }
        /// <summary>
        /// method to enter valid bet amount
        /// </summary>
        private static void SetBetAmount()
        {
            Console.WriteLine("Enter Bet Amount");
            CurrentBetAmount = Helper.GetNumericUserInput<decimal>("Please enter valid bet Amount", 0.1M, UserBankroll);
            Console.WriteLine("Placed Bet:" + string.Format("{0:0.00}", CurrentBetAmount));
        }

        /// <summary>
        /// method to generate a random number
        /// </summary>
        /// <returns></returns>
        private static int SpinWheel()
        {
            Console.WriteLine("Press any key to spin");
            Console.ReadLine();
            Random r = new Random();
            int rInt = r.Next(-1, 37);
            Console.WriteLine("Value on the wheel: " + (rInt == -1 ? "00" : rInt.ToString()));
            return rInt;
        }

    }

}
