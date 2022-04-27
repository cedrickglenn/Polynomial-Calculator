using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using PreFinals_Project.DoublyLinkedList_Class;

namespace PreFinals_Project.Models
{
    public class Polynomial
    {
        public ILinkedList<Term> Terms { get; private set; }
        public IList<Term> TermsInList { get; private set; }
        public string Expression { get; private set; }
        public ILinkedList<Term> DenominatorTerms { get; private set; }
        public StringBuilder SolutionText { get; private set; } = new StringBuilder();
        public Polynomial(string expression)
        {
            Expression = expression.ToLower();
            Terms = PreProcessor(Expression);
        }

        public Polynomial(ILinkedList<Term> terms)
        {
            Terms = terms;
        }
        public Polynomial(Term term)
        {
            Terms = new DoublyLinkedList<Term>();
            Terms.AddToTail(term);
            SolutionText.Append(ToString());
        }
        public Polynomial(Term term , StringBuilder solutionText)
        {
            Terms = new DoublyLinkedList<Term>();
            Terms.AddToTail(term);
            SolutionText = solutionText;
            SolutionText.Append(ToString());
        }
        public Polynomial(ILinkedList<Term> terms, StringBuilder solutionText)
        {
            Terms = terms;
            SolutionText = solutionText;
            SolutionText.Append(ToString());
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var term in Terms)
            {
                if (term.Coefficient < 0) sb.Append("-");
                sb.Append($"{term} + ");
            }
            sb.Replace("+ -", "- ");
            if (sb.Length > 0) sb.Remove(sb.Length - 3, 3);
            return sb.ToString();
        }

        public Polynomial Add(Polynomial otherPolynomial)
        {
            var newPolynomialLinkedList = new DoublyLinkedList<Term>();
            var polyATerms = Terms;
            var polyBTerms = otherPolynomial.Terms;
            var polyATemp = polyATerms.Head;
            var polyBTemp = polyBTerms.Head;

            while (polyATemp != null && polyBTemp != null)
            {
                if (polyATemp.Data.VariablesInString == polyBTemp.Data.VariablesInString)
                {
                    var newCoefficient = polyATemp.Data.Coefficient + polyBTemp.Data.Coefficient;
                    if (newCoefficient != 0 )
                    {
                        newPolynomialLinkedList.AddToTail(new Term(newCoefficient, polyATemp.Data.Variables));
                        polyATemp = polyATemp.Next;
                        polyBTemp = polyBTemp.Next;
                        continue;
                    }
                    polyATemp = polyATemp.Next;
                    polyBTemp = polyBTemp.Next;
                }
                else if (polyATemp.Data.TotalDegree > polyBTemp.Data.TotalDegree)
                {
                    newPolynomialLinkedList.AddToTail(polyATemp.Data);
                    polyATemp = polyATemp.Next;
                }
                else if(polyATemp.Data.TotalDegree < polyBTemp.Data.TotalDegree)
                {
                    newPolynomialLinkedList.AddToTail(polyBTemp.Data);
                    polyBTemp = polyBTemp.Next;
                }
                else
                {
                    newPolynomialLinkedList.AddToTail(polyATemp.Data);
                    newPolynomialLinkedList.AddToTail(polyBTemp.Data);
                    polyATemp = polyATemp.Next;
                    polyBTemp = polyBTemp.Next;
                }
            }
            var polyTemp = polyATemp ?? polyBTemp;
            while (polyTemp != null)
            {
                newPolynomialLinkedList.AddToTail(polyTemp.Data);
                polyTemp = polyTemp.Next;
            }
            SolutionText.Append($"({ToString()}) ");
            SolutionText.Append($"+ ({otherPolynomial}) = ");
            return new Polynomial(CombineLikeTerms(newPolynomialLinkedList), SolutionText);
        }
        public Polynomial Subtract(Polynomial otherPolynomial)
        {
            var newPolynomialLinkedList = new DoublyLinkedList<Term>();
            var polyATerms = Terms;
            var polyBTerms = otherPolynomial.Terms;
            var polyATemp = polyATerms.Head;
            var polyBTemp = polyBTerms.Head;
            while (polyATemp != null && polyBTemp != null)
            {
                if (polyATemp.Data.VariablesInString == polyBTemp.Data.VariablesInString)
                {
                    var newCoefficient = polyATemp.Data.Coefficient - polyBTemp.Data.Coefficient;
                    if (newCoefficient != 0)
                    {
                        newPolynomialLinkedList.AddToTail(new Term(newCoefficient, polyATemp.Data.Variables));
                        polyATemp = polyATemp.Next;
                        polyBTemp = polyBTemp.Next;
                        continue;
                    }
                    polyATemp = polyATemp.Next;
                    polyBTemp = polyBTemp.Next;
                }
                else if (polyATemp.Data.TotalDegree > polyBTemp.Data.TotalDegree)
                {
                    newPolynomialLinkedList.AddToTail(polyATemp.Data);
                    polyATemp = polyATemp.Next;
                }
                else if (polyATemp.Data.TotalDegree < polyBTemp.Data.TotalDegree)
                {
                    polyBTemp.Data.Coefficient = 0 - polyBTemp.Data.Coefficient;
                    newPolynomialLinkedList.AddToTail(polyBTemp.Data);
                    polyBTemp = polyBTemp.Next;
                }
                else
                {
                    polyBTemp.Data.Coefficient = 0 - polyBTemp.Data.Coefficient;
                    newPolynomialLinkedList.AddToTail(polyATemp.Data);
                    newPolynomialLinkedList.AddToTail(polyBTemp.Data);
                    polyATemp = polyATemp.Next;
                    polyBTemp = polyBTemp.Next;
                }
            }
            var polyTemp = polyATemp;
            var isPolyB = false;
            if (polyBTemp != null)
            {
                polyTemp = polyBTemp;
                isPolyB = true;
            }

            while (polyTemp != null)
            {
                if (isPolyB) polyTemp.Data.Coefficient = 0 - polyTemp.Data.Coefficient;
                newPolynomialLinkedList.AddToTail(polyTemp.Data);
                polyTemp = polyTemp.Next;
            }
            SolutionText.Append($"({ToString()}) ");
            SolutionText.Append($"- ({otherPolynomial}) = ");
            return new Polynomial(CombineLikeTerms(newPolynomialLinkedList),SolutionText);
        }
        public Polynomial Multiply(Polynomial otherPolynomial)
        {
            var termsToMultiplyA = otherPolynomial.Terms;
            var termsToMultiplyB = Terms;
            var product = new DoublyLinkedList<Term>();
            var tempA = termsToMultiplyA.Tail;
            var tempB = termsToMultiplyB.Tail;

            while (tempA != null)
            {
                while (tempB != null)
                {
                    var variablesA = tempA.Data.Variables;
                    var variablesB = tempB.Data.Variables;
                    var newVariables = new List<Variable>();
                    foreach(var variable in variablesA) newVariables.Add(new Variable(variable.Character,variable.Exponent));
                    foreach (var variable in variablesB) newVariables.Add(new Variable(variable.Character, variable.Exponent));
                    var newCoefficient = tempA.Data.Coefficient * tempB.Data.Coefficient;
                    product.AddToTail(new Term(newCoefficient, newVariables));
                    tempB = tempB.Prev;
                }
                tempA = tempA.Prev;
                tempB = termsToMultiplyB.Tail;
            }

            SolutionText.Append($"({ToString()})");
            SolutionText.Append($"({otherPolynomial}) = ");
            return new Polynomial(CombineLikeTerms(product),SolutionText);
        }
        public Polynomial Divide(Polynomial otherPolynomial, StringBuilder solutionText = null, ILinkedList<Term> quotient = null )
        {
            if (solutionText != null) SolutionText = solutionText;
            if (quotient == null) quotient = new DoublyLinkedList<Term>();
            var quotientPoly = new Polynomial(quotient);
            if (quotientPoly.Terms.Count != 0) SolutionText.Append($"{quotientPoly} + ");
            SolutionText.Append(@"\frac{");
            SolutionText.Append(ToString());
            SolutionText.Append("}{");
            SolutionText.Append(otherPolynomial);
            SolutionText.Append("} = ");
            if (ToString() == otherPolynomial.ToString())
            {
                if (ToString() == "0") throw new InvalidOperationException();
                return new Polynomial(new Term(1, new List<Variable>()), SolutionText);
            }
            if (otherPolynomial.Terms.Count == 1)
            {
                if (otherPolynomial.Terms.Head.Data.Coefficient == 0) throw new InvalidOperationException();
                return DivideWithOnlyOneTerm(otherPolynomial);
            }
            if (Terms.Head.Data.TotalDegree < otherPolynomial.Terms.Head.Data.TotalDegree)
            {
                foreach (var term in Terms)
                {
                    term.DenominatorTerms = otherPolynomial.Terms;
                    quotient.AddToTail(term);
                }
                return new Polynomial(CombineLikeTerms(quotient), SolutionText); 
            }
            var dividend = Terms;
            var divisor = otherPolynomial.Terms;
            var variablesA = dividend.Head.Data.Variables;
            var variablesB = divisor.Head.Data.Variables;
            var newCoefficient = dividend.Head.Data.Coefficient / divisor.Head.Data.Coefficient;
            quotient.AddToTail(new Term(newCoefficient, variablesA , variablesB));
            quotientPoly = new Polynomial(quotient.Tail.Data);
            var dividendPoly = new Polynomial(Terms);
            var multipliedPoly = quotientPoly.Multiply(new Polynomial(divisor));
            var termsToSubtract = dividendPoly.Subtract(multipliedPoly).Terms;
            dividend = termsToSubtract;
            dividendPoly = new Polynomial(dividend);
            return dividendPoly.Divide(new Polynomial(divisor),SolutionText,quotient);
        }
        private Polynomial DivideWithOnlyOneTerm(Polynomial otherPolynomial)
        {
            SolutionText.Append(@"\frac{");
            SolutionText.Append(ToString());
            SolutionText.Append("}{");
            SolutionText.Append(otherPolynomial);
            SolutionText.Append("} = ");
            var dividend = Terms;
            var divisor = otherPolynomial.Terms;
            var quotient = new DoublyLinkedList<Term>();
            var tempA = dividend.Head;
            var tempB = divisor.Head;

            while (tempA != null)
            {
                while (tempB != null)
                {
                    var variablesA = tempA.Data.Variables;
                    var variablesB = tempB.Data.Variables;
                    if (!tempA.Data.VariablesInString.Contains(tempB.Data.VariablesInString))
                    {
                        quotient.AddToTail(new Term(tempA.Data.Coefficient, tempA.Data.Variables, otherPolynomial.Terms));
                    }
                    else
                    {
                        var newCoefficient = tempA.Data.Coefficient / tempB.Data.Coefficient;
                        quotient.AddToTail(new Term(newCoefficient, variablesA, variablesB));
                    }
                    tempB = tempB.Next;
                }
                tempA = tempA.Next;
                tempB = divisor.Head;
            }
            return new Polynomial(CombineLikeTerms(quotient), SolutionText);
        }
        private ILinkedList<Term> PreProcessor(string expression)
        {
            if (!expression.Contains("^"))
            {
                expression = Regex.Replace(expression, @"(?![a-z]\d)[a-z]", "$&1");
                expression = Regex.Replace(expression, "[a-z]", "$&^|");
            }
            expression = expression.Replace(" ", string.Empty);
            expression = expression.Replace("-", "+-");
            expression = expression.Replace("–", "+-");
            expression = expression.Replace("−", "+-");
            if (expression.StartsWith("+")) expression = expression.Remove(0, 1);
            var splitByTerms = expression.Split('+');
            var termList = new List<Term>();
            foreach (var term in splitByTerms)
            {
                Term newTerm;
                var newTermHasEqual = false;
                if (term == "") continue;
                if (int.TryParse(term, out var result)) newTerm = new Term(result, new List<Variable>());
                else
                {
                    var treatedTerm = term.Trim();
                    var processedTerm = Regex.Replace(treatedTerm, "[a-z]", "x");
                    var splitByLetter = processedTerm.Trim().Split('x');
                    double coefficient;
                    var indexToStart = 0;
                    if (splitByLetter[0] == string.Empty) coefficient = 1;
                    else if (splitByLetter[0] == "-") coefficient = -1;
                    else if (Regex.IsMatch(splitByLetter[0], "[^0-9.,-]")) throw new InvalidOperationException("Not a valid coefficient");
                    else coefficient = Convert.ToDouble(splitByLetter[0]);
                    if (splitByLetter[0].Length > 0) indexToStart = splitByLetter[0].Length;
                    processedTerm = Regex.Replace(treatedTerm.Substring(indexToStart), @"\d+", $"$&?");
                    if (processedTerm.Length >0) processedTerm = processedTerm.Remove(processedTerm.Length - 1, 1);
                    var splitByComma = processedTerm.Split('?');
                    var variables = splitByComma.Select(splitted => splitted.Split('^')).Select(splitByExponent => new Variable(Convert.ToChar(splitByExponent[0]), ProcessExponentValue(splitByExponent[1]))).ToList();
                    newTerm = new Term(coefficient, variables);
                }
                foreach (var node in termList)
                {
                    if (node.VariablesInString != newTerm.VariablesInString) continue;
                    node.Coefficient += newTerm.Coefficient;
                    if (node.Coefficient == 0)
                    {
                        termList.Remove(node);
                        newTermHasEqual = true;
                        break;
                    }
                    newTermHasEqual = true;
                }
                if (!newTermHasEqual) termList.Add(newTerm);
            }

            var termLinkedList = SortTermsAndConvertToLinkedList(termList);
            return termLinkedList;
        }
        private int ProcessExponentValue(string exponent)
        {
            exponent = exponent.Replace("|", string.Empty);
            if (exponent.Trim() == string.Empty || exponent.Trim() == "") return 1;
            return Convert.ToInt32(exponent.Trim());
        }
        private ILinkedList<Term> SortTermsAndConvertToLinkedList(List<Term> termList)
        {
            var termLinkedList = new DoublyLinkedList<Term>();
            termList.Sort();
            TermsInList = termList;
            foreach (var term in termList)
            {
                termLinkedList.AddToTail(term);
            }

            return termLinkedList;
        }
        private ILinkedList<Term> CombineLikeTerms(ILinkedList<Term> terms)
        {
            var termsForCombining = terms;
            var newList = new List<Term>();
            var iteratorA = termsForCombining.Head;
            var iteratorB = termsForCombining.Head;
            
            while (iteratorA != null)
            {
                var newCoefficient = 0d;
                var isFirstOccurrence = true;
                while (iteratorB != null)
                {
                    
                    if (iteratorA == iteratorB)
                    {
                        iteratorB = iteratorB.Next;
                        continue;
                    }
                    if ((iteratorA.Data.VariablesInString == iteratorB.Data.VariablesInString) && iteratorB.Data.NotAdded && (iteratorA.Data.DenominatorTerms == iteratorB.Data.DenominatorTerms))
                    {
                        if(isFirstOccurrence) newCoefficient += iteratorA.Data.Coefficient + iteratorB.Data.Coefficient;
                        else newCoefficient += iteratorB.Data.Coefficient;
                        iteratorA.Data.NotAdded = false;
                        iteratorB.Data.NotAdded = false;
                        isFirstOccurrence = false;
                    }
                    iteratorB = iteratorB.Next;
                }
                if(!iteratorA.Data.NotAdded && (newCoefficient != 0)) newList.Add(new Term(newCoefficient, iteratorA.Data.Variables, iteratorA.Data.DenominatorTerms));
                iteratorA = iteratorA.Next;
                iteratorB = termsForCombining.Head;
            }
            foreach (var term in termsForCombining)
                if (term.NotAdded && (term.Coefficient != 0))
                    newList.Add(new Term(term.Coefficient,term.Variables,term.DenominatorTerms));
            return SortTermsAndConvertToLinkedList(newList);
        }
    }
}
