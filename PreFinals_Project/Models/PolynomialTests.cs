using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FluentAssertions;
using NUnit.Framework;

namespace PreFinals_Project.Models
{
    [TestFixture]
    public class PolynomialTests
    {
        [TestCase("3.12321321m^3d^9a^1z^20", "3.12321321ad^9m^3z^20")]
        [TestCase("3.12321321m^3d^9a^1z^123y^20", "3.12321321ad^9m^3y^20z^123")]
        [TestCase("b^20a^60z^100","a^60b^20z^100")]
        public void Term_Variables_ReturnsAsSorted(string termString, string expected)
        {
            //Arrange
            var variables = new List<Variable>();
            var processedTerm = Regex.Replace(termString, "[a-z]", "x");
            var splitByLetter = processedTerm.Trim().Split('x');
            double coefficient;
            if (splitByLetter[0] == string.Empty) coefficient = 1;
            else if (Regex.IsMatch(splitByLetter[0], "[^0-9.,]")) throw new InvalidOperationException("Not a valid coefficient");
            else coefficient = Convert.ToDouble(splitByLetter[0]);
            int indexToStart = 0;
            if (splitByLetter[0].Length > 0) indexToStart = splitByLetter[0].Length;
            processedTerm = Regex.Replace(termString.Substring(indexToStart), @"\d+", $"$&=");
            processedTerm = processedTerm.Remove(processedTerm.Length - 1, 1);
            Console.WriteLine(processedTerm);
            var splitByComma = processedTerm.Split('=');
            foreach (string splitted in splitByComma)
            {
                var splitByExponent = splitted.Split('^');
                variables.Add(new Variable(Convert.ToChar(splitByExponent[0]), Convert.ToInt32(splitByExponent[1])));
            }
            var term = new Term(coefficient, variables);
            //Act
            Console.WriteLine(term.VariablesInString);
            //Assert
            term.ToString().Should().Be(expected);

        }

        [TestCase("3x^2y^3 + 5x^2w^3 - 8x^2w^3z^4 + 3", "-8w^3x^2z^4 + 5w^3x^2 + 3x^2y^3 + 3")]
        [TestCase("3x2y3 + 5x2w3 - 8x2w3z4a + 3", "-8aw^3x^2z^4 + 5w^3x^2 + 3x^2y^3 + 3")]
        public void Polynomial_TermsReturnsAsSorted(string expression, string expected)
        {
            //Arrange
            var polynomial = new Polynomial(expression);
            //Act

            //Assertd
            polynomial.ToString().Should().Be(expected);
            Console.WriteLine(polynomial);
        }

        [TestCase("3x^2y^3 + 5x^2w^3 - 8x^2w^3z^4 + 3", "-2x^2w^3 + 9y^1 - 4x^1w^1 - x^2y^3 + 8x^2w^3z^4 – 4", "3w^3x^2 + 2x^2y^3 - 4wx + 9y - 1")]
        [TestCase("3x2y3 + 5x2w3 - 8x2w3z4 + 3", "-2x2w3 + 9y - 4xw - x2y3 + 8x2w3z4 – 4", "3w^3x^2 + 2x^2y^3 - 4wx + 9y - 1")]
        [TestCase("7a3 - 6a2 + 3a + 1","a2 - a + 1","7a^3 - 5a^2 + 2a + 2")]
        [TestCase("3w2+7w","7w3-7w-4","7w^3 + 3w^2 - 4")]
        [TestCase("-3u3 - 3u2 -4u + 7","-4u3 + 2u2 + 6u - 4", "-7u^3 - u^2 + 2u + 3")]
        [TestCase("3u3 - 2u2 -4u + 4","3u3 + 2u2 - 4u - 4", "6u^3 - 8u")]
        public void Polynomial_Addition_ReturnsAsExpected(string polyA, string polyB, string expected)
        {
            //Arrange
            var polynomialA = new Polynomial(polyA);
            var polynomialb = new Polynomial(polyB);

            //Act
            var sum = polynomialA.Add(polynomialb);
            

            sum.ToString().Should().Be(expected);
        }

        [TestCase("3x^2y^3 + 5x^2w^3 - 8x^2w^3z^4 + 3", "-2x^2w^3 + 9y^1 - 4x^1w^1 - x^2y^3 + 8x^2w^3z^4 – 4", "-16w^3x^2z^4 + 7w^3x^2 + 4x^2y^3 + 4wx - 9y + 7")]
        [TestCase("3x2y3 + 5x2w3 - 8x2w3z4 + 3", "-2x2w3 + 9y - 4xw - x2y3 + 8x2w3z4 – 4", "-16w^3x^2z^4 + 7w^3x^2 + 4x^2y^3 + 4wx - 9y + 7")]
        [TestCase("7a3 - 6a2 + 3a + 1", "a2 - a + 1", "7a^3 - 7a^2 + 4a")]
        [TestCase("3w2+7w", "7w3-7w-4", "-7w^3 + 3w^2 + 14w + 4")]
        [TestCase("-3u3 - 3u2 -4u + 7", "-4u3 + 2u2 + 6u - 4", "u^3 - 5u^2 - 10u + 11")]
        [TestCase("3u3 - 2u2 -4u + 4", "3u3 + 2u2 - 4u - 4", "-4u^2 + 8")]
        public void Polynomial_Subtraction_ReturnsAsExpected(string polyA, string polyB, string expected)
        {
            //Arrange
            var polynomialA = new Polynomial(polyA);
            var polynomialb = new Polynomial(polyB);

            //Act
            var difference = polynomialA.Subtract(polynomialb);


            difference.ToString().Should().Be(expected);
        }
        [TestCase("3u3 - 2u2 -4u + 4", "3u3 + 2u2 - 4u - 4","9u^6 - 28u^4 + 32u^2 - 16")]
        [TestCase("3w2 + 7w", "7w3 - 7w - 4","21w^5 + 49w^4 - 21w^3 - 61w^2 - 28w")]
        [TestCase("3w2", "7w3","21w^5")]
        [TestCase("x2+x", "x","x^3 + x^2")]
        public void Polynomial_Multiplication_ReturnsAsExpected(string polyA, string polyB, string expected)
        {
            //Arrange
            var polynomialA = new Polynomial(polyA);
            var polynomialb = new Polynomial(polyB);

            //Act
            var product = polynomialA.Multiply(polynomialb);

            product.ToString().Should().Be(expected);

        }

        //[TestCase("3u3 - 2u2 -4u + 4", "3u3 + 2u2 - 4u - 4", "9u^6 - 28u^4 + 32u^2 - 16")]
        //[TestCase("3w2 + 7w", "7w3 - 7w - 4", "21w^5 + 49w^4 - 21w^3 - 61w^2 - 28w")]
        [TestCase("3w2", "7w3")]
        [TestCase("x2+x", "x")]
        [TestCase("3x3 – 5x2 + 10x – 3", "3x + 1")]
        [TestCase("3x4−5x2+3", "x+2")]
        [TestCase("3x4−5x2+3", "x+1")]
        [TestCase("xy2+x", "y")]
        public void Polynomial_Division_ReturnsAsExpected(string polyA, string polyB)
        {
            //Arrange
            var polynomialA = new Polynomial(polyA);
            var polynomialb = new Polynomial(polyB);

            //Act
            var quotient = polynomialA.Divide(polynomialb);

            Console.WriteLine(quotient.ToString());

        }


    }
}
