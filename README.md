# Polynomial-Calculator
This program is designed to handle basic polynomial arithmetic – addition, subtraction, multiplication, and division. My final project for my Data Structures and Algorithms subject.

DESCRIPTION:

This program is designed to handle basic polynomial arithmetic – addition, subtraction, multiplication, and division.  Multivariable polynomials are accepted, and the order of variables does not matter. List and LinkedList are data structures used in this program. Terms in a polynomial are represented as nodes in a linked list and each of their variables are stored in a list. Addition and Subtraction is done by iterating through each node and combining like terms (same variables and exponents). Multiplication is done by iterating through each node of the second polynomial and multiplying it to every node on the first polynomial. Division is done by implementing an algorithm akin to long division.

HOW TO USE THE PROGRAM:


To use the program, one must enter polynomials represented on string on the respective fields and select the desired operation. The program is designed to handle any kind of representing polynomials, such as representing exponents with ‘^’ or directly inputting exponents, e.g. 3x2 (3x^2). The program will throw an error if the representation is not consistent on each term, or if basic polynomial rules are violated – e.g. exponents are negative.



*Initial display of the program.*
![image](https://user-images.githubusercontent.com/45276381/165645914-a775b523-0584-411a-bcf8-7e0c7d18ba84.png)



*Program can accept both forms, as long as consistency is followed.*
![image](https://user-images.githubusercontent.com/45276381/165646013-0b72bb45-f9af-41e9-87ee-95e07daa7fb3.png)
![image](https://user-images.githubusercontent.com/45276381/165646008-554a072e-5a0c-4c3b-873b-1bfe64880ef4.png)



*Pressing “Calculate” yields the following results.*
![image](https://user-images.githubusercontent.com/45276381/165646050-8c664860-c897-44cb-840f-a7e21e6d71af.png)
 

ADDITIONAL FEATURES:

- Display: results and solutions are displayed in LaTeX form, in order for better readability and consistency with other polynomial calculators, e.g. Wolfram Alpha.

LIMITATIONS:

- This program cannot handle simplification of terms.
- Simplified polynomials cannot be handled by this program.
- Only basic arithmetic can be done.
- Dividing fractional terms is represented as double.
- Cannot handle more polynomial operations.
- Solution for long division cannot be handled.
