// ***********************************************************************
// Assembly         : Simple Calculator
// Author           : Christian
// Created          : 09-23-2019
//
// Last Modified By : Christian
// Last Modified On : 09-23-2019
// ***********************************************************************
// <copyright file="CalculatorProcessor.cs" company="">
//     Copyright © 2019 Christian Shadd
// </copyright>
// <summary></summary>
// ***********************************************************************
using NCalc;
using System.Collections.Generic;

namespace Simple_Calculator
{
    /// <summary>
    /// A processor for a calculator using basic integers and other symbols
    /// to make strings that are solved in a <see cref="Problem" /> object.
    /// </summary>
    public class CalculatorProcessor
    {
        /// <summary>
        /// The <see cref="Problem" /> object currently in use.
        /// </summary>
        private Problem problem;

        /// <summary>
        /// A <see cref="Stack{T}" /> of <see cref="Problem" /> objects in
        /// history.
        /// </summary>
        /// <value>The history.</value>
        public Stack<Problem> history { get; private set; }

        /// <summary>
        /// Prevents deletion from the <see cref="Problem" />'s
        /// raw value until the Problem has been solved.
        /// </summary>
        /// <value><c>true</c> if [no delete]; otherwise, <c>false</c>.</value>
        /// <remarks>Sometimes there is an operation that changes the syntax of the
        /// Problem's Raw value. In the case where this holds true, it
        /// would be unwise to delete the new syntax as this can cause a
        /// <see cref="EvaluationException" /> in the program.</remarks>
        private bool NoDelete { get; set; }

        /// <summary>
        /// A solver for solving <see cref="Problem" />s.
        /// </summary>
        /// <seealso cref="Expression" />
        private Expression solver;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatorProcessor" />
        /// class.
        /// </summary>
        public CalculatorProcessor() : base()
        {
            this.history = new Stack<Problem>();
            this.problem = new Problem(0);
            this.NoDelete = false;
            this.solver = null;
            return;
        }

        #region Problem
        /// <summary>
        /// A problem container that has a raw value and a result value.
        /// to solve.
        /// </summary>
        public class Problem
        {
            /// <summary>
            /// Gets or sets the raw value of the problem.
            /// </summary>
            /// <value>The raw value to set.</value>
            /// <remarks>This can be an expression of any
            /// type.</remarks>
            public string Raw { get; set; }

            /// <summary>
            /// Gets or sets the result value of the
            /// problem.
            /// </summary>
            /// <value>The result value to set.</value>
            public string Result { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Problem" />
            /// class.
            /// </summary>
            public Problem() : this("")
            {
                return;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Problem" />
            /// class.
            /// </summary>
            /// <param name="raw">The raw value to set.</param>
            public Problem(double raw) : this(raw + "")
            {
                return;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Problem" />
            /// class.
            /// </summary>
            /// <param name="raw">The raw value to set.</param>
            public Problem(string raw) : base()
            {
                this.Raw = raw;
                this.Result = null;
            }

            /// <summary>
            /// Checks for negatives in the <see cref="Raw" /> value.
            /// </summary>
            /// <returns><c>true</c> if negative exists, <c>false</c>
            /// otherwise.</returns>
            public bool NegativeExists()
            {
                return this.Raw.Contains("n");
            }

            /// <summary>
            /// Returns a "proper" expression with any <c>n</c>
            /// replaced with a <c>-</c> for negative
            /// numbers.
            /// </summary>
            /// <returns>A <see cref="string" /> value.</returns>
            public string ProperExpression()
            {
                return this.Raw.Replace("n", "-");
            }

            /// <summary>
            /// Returns a "proper" formatted problem with any <c>n</c>
            /// replaced with a <c>-1*</c> for negative
            /// numbers.
            /// </summary>
            /// <returns>A <see cref="string" /> value.</returns>
            public string ProperFormat()
            {
                return this.Raw.Replace("n", "-1*");
            }

            /// <summary>
            /// Sets the <see cref="Raw" /> value with number.
            /// </summary>
            /// <param name="number">The number to set as the raw
            /// value.</param>
            public void SetRawWithNumber(double number)
            {
                this.Raw = number + "";
                if (number < 0)
                {
                    this.Raw = "n" + this.Raw;
                }
                return;
            }
        }
        #endregion

        /// <summary>
        /// Appends a decimal to the <see cref="problem" />.
        /// </summary>
        public void AppendDecimal()
        {
            this.problem.Raw += ".";
            return;
        }

        /// <summary>
        /// Appends a number to the <see cref="problem" />.
        /// </summary>
        /// <param name="number">The number to append.</param>
        public void AppendNumber(int number)
        {
            // If the raw value is just "0"...
            if (this.problem.Raw.Equals("0"))
            {
                // Set problem with number.
                this.problem.SetRawWithNumber(number);
            }
            else
            {
                // Append number to raw value.
                this.problem.Raw += number;
            }
            return;
        }

        /// <summary>
        /// Commits an addition statement to the <see cref="problem" />.
        /// </summary>
        public void Add()
        {
            this.problem.Raw += " + ";
            return;
        }

        /// <summary>
        /// Clears the <see cref="problem" /> by setting it to 0.
        /// </summary>
        public void C()
        {
            this.problem.SetRawWithNumber(0);
            return;
        }

        /// <summary>
        /// Clears last number entry of the <see cref="problem" />.
        /// </summary>
        public void CE()
        {
            // Split the problem by tokens.
            var components = this.problem.Raw.Split();
            // Get last token.
            var lastToken = components[components.Length - 1];
            // Check to see if last token is a number.
            var canRemove = double.TryParse(lastToken, out double result);
            // If last token is a number...
            if (canRemove)
            {
                // Get last index of last token.
                var endOf = this.problem.Raw.LastIndexOf(lastToken);
                // Remove last token via the last index.
                this.problem.Raw = this.problem.Raw.Substring(0, endOf);
            }

            // If there is only one token...
            if (components.Length == 1)
            {
                // Set problem back to 0.
                this.problem.SetRawWithNumber(0);
            }
            return;
        }

        /// <summary>
        /// Clears the <see cref="history" /> of <see cref="Problem" />s.
        /// </summary>
        public void ClearHistory()
        {
            this.history.Clear();
            return;
        }

        /// <summary>
        /// Deletes last value of <see cref="problem" />'s raw value.
        /// </summary>
        /// <remarks>This handles especially well for negatives.</remarks>
        public void Delete()
        {
            // If we can delete...
            if (!this.NoDelete)
            {
                // If there is a negative and the problem has more than two
                // values, such that the first one is a negative with
                // values after it...
                if (this.problem.NegativeExists()
                    && this.problem.Raw.Length > 2)
                {
                    // Remove last value.
                    this.problem.Raw = this.problem.Raw.Substring(0,
                        this.problem.Raw.Length - 1);
                }
                // If there is a negative and the problem has two values,
                // such that the first one is a negative with values
                // after it...
                else if (this.problem.NegativeExists()
                    && this.problem.Raw.Length == 2)
                {
                    // Clear last entry.
                    this.CE();
                }
                // If the problem has more than one value...
                else if (this.problem.Raw.Length > 1)
                {
                    // Remove last value.
                    this.problem.Raw = this.problem.Raw.Substring(0,
                        this.problem.Raw.Length - 1);
                }
                else
                {
                    // Clear last entry.
                    this.CE();
                }
            }
            return;
        }

        /// <summary>
        /// Commits an division statement to the <see cref="problem" />.
        /// </summary>
        public void Div()
        {
            this.problem.Raw += " / ";
            return;
        }

        /// <summary>
        /// Commits an 1/x statement to the <see cref="problem" />.
        /// </summary>
        public void OneOverX()
        {
            this.problem.Raw = "1/(" + this.problem.Raw + ")";
            this.NoDelete = true;
            return;
        }

        /// <summary>
        /// Commits an multiplication statement to the <see cref="problem" />.
        /// </summary>
        public void Mul()
        {
            this.problem.Raw += " x ";
            return;
        }

        /// <summary>
        /// Commits an % statement to the <see cref="problem" />.
        /// </summary>
        public void Percent()
        {
            this.problem.Raw = this.problem.Raw + "%";
            return;
        }

        /// <summary>
        /// Reverses <see cref="problem" /> from negative to positive.
        /// </summary>
        public void ReverseOperationNumber()
        {
            // If the problem is "0"...
            if (!this.problem.Raw.Equals("0"))
            {
                // If the problem has a negative...
                if (this.problem.NegativeExists())
                {
                    // Remove the first value, in this case a negative.
                    this.problem.Raw = this.problem.Raw.Substring(1);
                }
                else
                {
                    // Prepend a negative value.
                    this.problem.Raw = "n" + this.problem.Raw;
                }
            }
            return;
        }

        /// <summary>
        /// Solves the <see cref="problem" /> via evaluation.
        /// </summary>
        /// <returns><see cref="string" /> of the result.</returns>
        /// <exception cref="EvaluationException">Problem cannot
        /// be solved due to malformed tokens.</exception>
        public string Solve()
        {
            // Make the problem into a proper expression.
            var proper = this.problem.ProperExpression();
            // Solve the proper expression. 
            this.solver = new Expression(proper);
            // Turn result from solver into a string.
            var result = this.solver.Evaluate() + "";
            // Set the result of the solved problem.
            this.problem.Result = result;
            // Put solved problem into history.
            this.history.Push(this.problem);
            // Make a new problem with the result.
            this.problem = new Problem(result);
            // Characters can be deleted now.
            this.NoDelete = false;
            // Return our result.
            return result;
        }

        /// <summary>
        /// Commits an x^2 statement to the <see cref="problem" />.
        /// </summary>
        public void Sqr()
        {
            this.problem.Raw = "(" + this.problem.Raw + "*" + this.problem.Raw + ")";
            this.NoDelete = true;
            return;
        }

        /// <summary>
        /// Commits an sqrt(x) statement to the <see cref="problem" />.
        /// </summary>
        public void Sqrt()
        {
            this.problem.Raw = "Sqrt(" + this.problem.Raw + ")";
            this.NoDelete = true;
            return;
        }

        /// <summary>
        /// Commits an subtraction statement to the <see cref="problem" />.
        /// </summary>
        public void Sub()
        {
            this.problem.Raw += " - ";
            return;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance. The
        /// result is a proper format of the <see cref="problem" />.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this
        /// instance.</returns>
        /// <seealso cref="object" />
        public override string ToString()
        {
            base.ToString();
            return this.problem.ProperFormat();
        }
    }
}
