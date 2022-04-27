# Polynomial-Calculator
This program is designed to handle basic polynomial arithmetic – addition, subtraction, multiplication, and division. My final project for my Data Structures and Algorithms subject.

DESCRIPTION:

This program is designed to handle basic polynomial arithmetic – addition, subtraction, multiplication, and division.  Multivariable polynomials are accepted, and the order of variables does not matter. List and LinkedList are data structures used in this program. Terms in a polynomial are represented as nodes in a linked list and each of their variables are stored in a list. Addition and Subtraction is done by iterating through each node and combining like terms (same variables and exponents). Multiplication is done by iterating through each node of the second polynomial and multiplying it to every node on the first polynomial. Division is done by implementing an algorithm akin to long division.

HOW TO USE THE PROGRAM:


![](https://user-images.githubusercontent.com/45276381/165644492-84223f8b-9685-4e71-b281-66b3029fcbc6.png)
  
To use the program, one must enter polynomials represented on string on the respective fields and select the desired operation. The program is designed to handle any kind of representing polynomials, such as representing exponents with ‘^’ or directly inputting exponents, e.g. 3x2 (3x^2). The program will throw an error if the representation is not consistent on each term, or if basic polynomial rules are violated – e.g. exponents are negative.



*Initial display of the program.*
  

![](https://user-images.githubusercontent.com/45276381/165644437-cbae9173-091e-4bfc-9c0f-851ea78742cd.png)



*Program can accept both forms, as long as consistency is followed.*
  

![](https://user-images.githubusercontent.com/45276381/165644585-cbaee45b-785c-438e-a839-eabb31b653f6.png)



*Pressing “Calculate” yields the following results.*
  

![](https://user-images.githubusercontent.com/45276381/165644547-2d9b210e-6e91-4f97-be94-585096e4dc7a.png)



ADDITIONAL FEATURES:

- Display: results and solutions are displayed in LaTeX form, in order for better readability and consistency with other polynomial calculators, e.g. Wolfram Alpha.

LIMITATIONS:

- This program cannot handle simplification of terms.
- Simplified polynomials cannot be handled by this program.
- Only basic arithmetic can be done.
- Dividing fractional terms is represented as double.
- Cannot handle more polynomial operations.
- Solution for long division cannot be handled.
