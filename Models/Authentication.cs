using Hosbital_Project.Models;
using PhoneNumbers;

internal class Authentication
{
    public List<User> users { get; set; }

    public Authentication(List<User> users)
    {
        this.users = users;
    }


    public void ValidateCommonFields(string password, string name, string surname, string email, string phone, string regionCode, ref List<string> errors, out string formattedPhone)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 8 || !password.Any(char.IsDigit))
            errors.Add("Password must be at least 8 chars and contain digits.");

        if (string.IsNullOrWhiteSpace(name))
            errors.Add("Name cannot be empty.");

        if (string.IsNullOrWhiteSpace(surname))
            errors.Add("Surname cannot be empty.");

        if (!email.EndsWith("@gmail.com"))
            errors.Add("Email is wrong.");

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

        if (string.IsNullOrWhiteSpace(username) || username.Length < 6)
            errors.Add("Username must be at least 6 characters.");
        if (users.Any(u => u.username == username))
            errors.Add("Username already exists.");

        ValidateCommonFields(password, name, surname, email, phone, regionCode, ref errors, out formattedPhone);

        return errors.Count == 0;
    }




    public bool ValidateDoctorCandidateRegistration(string password, string name, string surname, string email, string phone, string regionCode, out List<string> errors, out string formattedPhone)
    {
        errors = new List<string>();

        ValidateCommonFields(password, name, surname, email, phone, regionCode, ref errors, out formattedPhone);

        return errors.Count == 0;
    }


    public void DoctorCandidateRegistration(Hosbital hosbital, string password, string name, string surname, string email, string phone, string regionCode,int experienceYear,Department department)
    {
        var errors = new List<string>();
        string formattedPhone;
        if (!ValidateDoctorCandidateRegistration(password, name, surname, email, phone, regionCode, out errors, out formattedPhone))
        {
            Console.WriteLine("Registration failed with the following errors:");
            foreach (var error in errors)
            {
                Console.WriteLine($"- {error}");
            }
            return;
        }
        var doctorCandidate = new DoctorCandidate(hosbital,name,surname,email ,password,phone,experienceYear,department);
        hosbital.doctorCandidates.Add(doctorCandidate);

    }
    public User? Registration(string username, string password, string name, string surname, string email, string phone, string regionCode, out List<string> errors)
    {
        username = username.Trim();
        password = password.Trim();
        name = name.Trim();
        surname = surname.Trim();
        email = email.Trim();
        phone = phone.Replace(" ", "").Replace("-", "");

        errors = new List<string>();
        string formattedPhone;

        if (!ValidateRegistration(username, password, name, surname, email, phone, regionCode, out errors, out formattedPhone))
            return null;

        var newUser = new User(username, password, name, surname, email, formattedPhone);
        users.Add(newUser);
        return newUser;
    }
    
    public User? SignInUser(string username, string password)
    {
        foreach (var user in users)
        {
            if (user.username == username && user.password == password)
                return user;
        }
        return null;
    }

    public bool AdminSignIn( string email, string password)
    {
            if (password == "admin123" && email == "admin@gmail.com")
            return true;

        return false;
    }
}
