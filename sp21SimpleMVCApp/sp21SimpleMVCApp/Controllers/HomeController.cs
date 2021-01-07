//these statements give our application access to code in other libraries
//by the end of the semester, it is not uncommon to have 8-10 using statements
using System;
using Microsoft.AspNetCore.Mvc;

//the namespace is like a folder for your files
//make sure the first part matches the name of your project
namespace sp21SimpleMVCApp.Controllers
{
    //your controller class will inherit from Microsoft's controller class
    public class HomeController : Controller
    {
        // GET: /Home/Index is the home page for your application
        // this action just serves up the index view
        public IActionResult Index()
        {
            //when you call the View() method with no parameters,
            //the MVC engine looks for a view with the same name as
            //the method returning it in the folder with the same name
            //as the controller.  Thus, this line will return the
            // Views/Home/Index.cshtml view.
            return View();
        }


        //This action method reads in the values from the form and
        //performs the validation and calculations
        //the two parameters are connected to textboxes on the Home/Index view
        public IActionResult Calculate(string strName, string strGrade)
        {
            //validate the name is correct by calling the CheckName method
            //declared below. You will pass this method the strName parameter
            //that came from the Index view
            Boolean bolCheckName = CheckName(strName);

            //display an error message if there is a problem
            if (bolCheckName == false)
            {
                //set the ViewBag's error message
                ViewBag.ErrorMessage = "Student name must be at least 2 letters";

                //send the user back to the Index view to try again
                //this overload of the View method takes a parameter
                //for the name of the view.  If we just did View() here,
                //the MVC engine would look for Views/Home/Calculate,
                //which doesn't exist
                return View("Index");
            }

            //declare a decimal value for the grade
            Decimal decGrade;

            //check the value of the grade
            try
            {
                //call the CheckDecimal method to validate the grade
                //you will pass this method the parameter that came
                //from the Index view
                decGrade = CheckDecimal(strGrade);
            }
            catch (Exception ex)  //only happens when CheckGrade throws an exception
            {
                //set the ViewBag's error message
                ViewBag.ErrorMessage = ex.Message;

                //send the user back to the Index view
                return View("Index");
            }

            //if code gets this far, we have a valid grade

            //declare a string for the letter grade
            string strLetter;

            //convert the number grade to a letter grade
            if (decGrade >= 90)
                strLetter = "A";
            else if (decGrade >= 80)
                strLetter = "B";
            else if (decGrade >= 70)
                strLetter = "C";
            else
                strLetter = "F";

            //populate the outputs on the ViewBag
            ViewBag.StudentName = "Student Name: " + strName;
            ViewBag.LetterGrade = "Student Grade: " + strLetter;

            //send the user to the results view
            return View("Results");
        }

        //this method checks to make sure the student's name follows
        //our business rules: all names must be at least 2 characters
        //If the business name is valid, the method returns true
        //If the name is not valid, the method returns false
        private Boolean CheckName(string strInput)
        {
            //make sure the string has a value
            if (String.IsNullOrEmpty(strInput))
                return false;

            //make sure the string is at least 2 characters
            if (strInput.Length < 2)
                return false;
            else //everything is okay, return true
                return true;         
        }


        //this method makes sure the grade follows our business rules
        //If the grade is entered correctly, the method returns the grade as
        //a decimal.  If there is an error, the method will throw an exception
        private Decimal CheckDecimal(string strInput)
        {
            //make sure the string has a value
            if (String.IsNullOrEmpty(strInput))
            {
                //throw exception so the calling method knows what went wrong
                throw new Exception("Please enter a grade!");
            }

            //create a variable to hold the decimal
            Decimal decResult;

            //create a try/catch block for converting the string to a decimal
            try
            {
                //attempt to convert the string to a decimal
                //if the string is not a number, then this line
                //will throw an exception
                decResult = Convert.ToDecimal(strInput);
            }
            catch (Exception ex)
            {
                //throw an exception so that the calling method knows what
                //went wrong.  You pass the inner exception up to the
                //calling method to preserve this information
                throw new Exception(strInput + "is not a valid number!", ex);
            }

            //now check to see if decResult is in the valid range (0-100)
            //this must be an 'if' statement, because violations to busines
            //rules do not cause exceptions.  For example, -985 is a perfectly
            //valid decimal, but not a valid grade
            if (decResult < 0 || decResult > 100)
            {
                throw new Exception(strInput + "is not in the required range!");
            }

            //if the code gets this far, the input has passed all the tests
            //return the decimal value
            return decResult;
        } //end of method
    } //end of class
}//end of namespace
