using Hosbital_Project.FileHelpers;
using Hosbital_Project.Models;
using PhoneNumbers;
using System.Text.RegularExpressions;

internal class Authentication
{
    public List<User> users = FileHelper.ReadUsersFromFile();
    public Authentication(List<User> users)
    {
        this.users = users;
    }


    public void ValidateCommonFields(string password, string name, string surname, string email, string phone, string regionCode, ref List<string> errors, out string formattedPhone)
    {
        if (string.IsNullOrEmpty(password))
            errors.Add("Password cannot be empty.");
        else if (password.Length < 6 || !password.Any(char.IsDigit))
            errors.Add("Password must be at least 6 chars and contain digits.");

        if (string.IsNullOrWhiteSpace(name))
            errors.Add("Name cannot be empty.");

        if (string.IsNullOrWhiteSpace(surname))
            errors.Add("Surname cannot be empty.");
        if (string.IsNullOrWhiteSpace(email))

            errors.Add("Email cannot be empty.");
        else if (!email.EndsWith("@gmail.com") && !email.EndsWith("@yahoo.com") && !email.EndsWith("@outlook.com") && !email.EndsWith("@hotmail.com") && !email.EndsWith("@mail.ru") && !email.EndsWith("@icloud.com"))
            errors.Add("Email is wrong.");

        string first = email.Split('@').First();
        string firstPartPattern = @"^[a-zA-Z0-9_-]+$";

        if (!Regex.IsMatch(first, firstPartPattern))
            errors.Add(" Email cannot contain special characters in the first part.");


        formattedPhone = "";
        var phoneUtil = PhoneNumberUtil.GetInstance();
        try
        {
            var number = phoneUtil.Parse(phone, regionCode);
            if (!phoneUtil.IsValidNumber(number))
                errors.Add("Invalid phone number.");
            else
                formattedPhone = phoneUtil.Format(number, PhoneNumberFormat.E164);
        }
        catch (NumberParseException)
        {
            errors.Add("Phone number format is invalid.");
        }
    }


    public bool ValidateRegistration(string username, string password, string name, string surname, string email, string phone, string regionCode, out List<string> errors, out string formattedPhone)
    {
        errors = new List<string>();

        if (string.IsNullOrWhiteSpace(username))
            errors.Add("Username cannot be empty.");
        else if (string.IsNullOrWhiteSpace(username) || username.Length < 6)
            errors.Add("Username must be at least 6 characters.");
        else if (users.Any(u => u.username == username))
            errors.Add("Username already exists.");


        ValidateCommonFields(password, name, surname, email, phone, regionCode, ref errors, out formattedPhone);

        return errors.Count == 0;
    }




    public bool ValidateDoctorCandidateRegistration(string password, string name, string surname, string email, string phone, string regionCode, string reason, out List<string> errors, out string formattedPhone)
    {
        errors = new List<string>();

        int wordCount = reason
  .Split(new char[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries)
  .Length;
        if (string.IsNullOrWhiteSpace(reason))
            errors.Add("Reason cannot be empty.");
        else if (string.IsNullOrWhiteSpace(reason) || wordCount < 10)
            errors.Add("Reason must be at least 10 words.");

        ValidateCommonFields(password, name, surname, email, phone, regionCode, ref errors, out formattedPhone);

        return errors.Count == 0;
    }


    public void DoctorCandidateRegistration(Hosbital hosbital, string password, string name, string surname, string email, string phone, string regionCode, int experienceYear, Department department, string reason)
    {
        password = password.Trim();
        name = name.Trim();
        surname = surname.Trim();
        email = email.Trim();
        phone = phone.Replace(" ", "").Replace("-", "");
        var errors = new List<string>();
        string formattedPhone;
        if (!ValidateDoctorCandidateRegistration(password, name, surname, email, phone, regionCode, reason, out errors, out formattedPhone))
        {
            Console.WriteLine("Registration failed with the following errors:");
            foreach (var error in errors)
            {
                Console.WriteLine($"- {error}");
            }
            return;
        }

        var doctorCandidate = new DoctorCandidate(hosbital, name, surname, email, password, phone, experienceYear, department, reason, regionCode);
        hosbital.doctorCandidates.Add(doctorCandidate);
        FileHelper.WriteCandidateToFile(hosbital.doctorCandidates);

        Console.WriteLine($"\n ~ Thank you, Dr. {doctorCandidate.name}!\r\n\r\n Your application has been received and is currently under review.\r\n You will be contacted via email or phone after the review is complete.\r\n\r\n[Press any key to return to main menu...]\r\n");


    }
  
    public User? SignInUser(string username, string password)
    {
        //fayldan oxu
        foreach (var user in users)
        {
            if (user.username == username && user.password == password)
                return user;
        }
        return null;
    }
    public Doctor DoctorSignIn(Hosbital hosbital, string email, string password)
    {
        //fayldan oxu
        foreach (var doctor in hosbital.doctors)
        {
            if (doctor.email == email && doctor.password == password)
                return doctor;
        }
        return null;
    }
    public bool AdminSignIn(string email, string password)
    {
        if (password == "admin123" && email == "admin@gmail.com")
            return true;

        return false;
    }
}