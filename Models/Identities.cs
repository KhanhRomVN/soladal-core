namespace soladal_core.Data
{
    public class Indentify
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required int GroupId { get; set; } = -1;
        public required string Type { get; set; }
        // Personal
        public required string Firstname { get; set; } = "";
        public required string Lastname { get; set; } = "";
        public DateTime DateOfBirth { get; set; }
        public required string Gender { get; set; } = "";
        public required string Country { get; set; } = "";
        public required string City { get; set; } = "";
        public required string Street { get; set; } = "";
        public string Zipcode { get; set; } = "";

        // Passport
        public string PassportID { get; set; } = ""; 
        public string PassportIssuedBy { get; set; } = "";
        public DateTime PassportIssuedDate { get; set; }
        public DateTime PassportExpiredDate { get; set; }

        // ID Card
        public string IDCardID { get; set; } = "";
        public string IDCardIssuedBy { get; set; } = "";
        public DateTime IDCardIssuedDate { get; set; }
        public DateTime IDCardExpiredDate { get; set; }

        // Driving License
        public string DrivingLicenseID { get; set; } = "";
        public string DrivingLicenseIssuedBy { get; set; } = "";
        public DateTime DrivingLicenseIssuedDate { get; set; }
        public DateTime DrivingLicenseExpiredDate { get; set; }

        // Contact
        public string Phone { get; set; } = "";
        public required string Gmail { get; set; } = "";
        public required string PasswordGmail { get; set; } = "";
        public string TwoFactorGmail { get; set; } = "";

        // Job
        public string JobTitle { get; set; } = "";
        public string JobCompany { get; set; } = "";
        public string JobDescription { get; set; } = "";
        public DateTime JobStartDate { get; set; }
        public DateTime JobEndDate { get; set; }

        // Other
        public required bool IsFavorite { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}