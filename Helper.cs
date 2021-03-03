using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roulette
{
    public static class Helper
    {
        #region Extension Methods
        /// <summary>
        /// This method takes the string input and converts to required output type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public static bool TryParse<T>(this string input, out T output)
        {
            output = default(T);
            try
            {

                output = (T)Convert.ChangeType(input, output.GetType());

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// returns true if input matches atleast one in lookup
        /// </summary>
        /// <param name="input"></param>
        /// <param name="lookup"></param>
        /// <returns></returns>
        public static bool MatchAny(this string input, params string[] lookup)
        {
            return lookup.Any(value => value == input);
        }

        #endregion


        /// <summary>
        ///  method to let the user to enter the amount
        ///  This method is used to enter the intial amount and to enter the bet amount
        /// </summary>
        /// <param name="displayMessageForInvalidEntry"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static T GetNumericUserInput<T>(string displayMessageForInvalidEntry , Nullable<T> min, Nullable<T> max) where T : struct
        {
            if (!(Console.ReadLine().TryParse<T>(out T enteredAmount)
                  && (min is null || Nullable.Compare(enteredAmount, min) >= 0)
                  && (max is null || Nullable.Compare(enteredAmount, max) <= 0)))
            {
                Console.WriteLine(displayMessageForInvalidEntry);
                enteredAmount = GetNumericUserInput(displayMessageForInvalidEntry, min, max);
            }
            return enteredAmount;
        }
        /// <summary>
        /// This method takes errormessage and valid entries and checks the user entered value present in the valid entries 
        /// </summary>
        /// <param name="displayMessageForInvalidEntry"></param>
        /// <param name="validEntries"></param>
        /// <returns></returns>
        public static string GetTextUserInput(string displayMessageForInvalidEntry, params string[] validEntries)
        {
            string userInput = Console.ReadLine();
            if (!(userInput.MatchAny(validEntries)))
            {
                Console.WriteLine(displayMessageForInvalidEntry);
                userInput = GetTextUserInput(displayMessageForInvalidEntry, validEntries);
            }

            return userInput;
        }
    }
}
