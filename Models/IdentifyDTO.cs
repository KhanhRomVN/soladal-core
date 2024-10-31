namespace soladal_core.Data
{
    public class IdentifyDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string Type { get; set; }
        public required int GroupId { get; set; } = -1;
        public string GroupName { get; set; } = "";
        // Personal
        public string Firstname { get; set; } = "";
        public string Lastname { get; set; } = "";
        public string DateOfBirth { get; set; } = "";
        public string Gender { get; set; } = "";
        public string Country { get; set; } = "";
        public string City { get; set; } = "";
        public string Street { get; set; } = "";
        public string Zipcode { get; set; } = "";

        // Passport
        public string PassportID { get; set; } = "";
        public string PassportIssuedBy { get; set; } = "";
        public string PassportIssuedDate { get; set; } = "";
        public string PassportExpiredDate { get; set; } = "";

        // ID Card
        public string IDCardID { get; set; } = "";
        public string IDCardIssuedBy { get; set; } = "";
        public string IDCardIssuedDate { get; set; } = "";
        public string IDCardExpiredDate { get; set; } = "";

        // Driving License
        public string DrivingLicenseID { get; set; } = "";
        public string DrivingLicenseIssuedBy { get; set; } = "";
        public string DrivingLicenseIssuedDate { get; set; } = "";
        public string DrivingLicenseExpiredDate { get; set; } = "";

        // Contact
        public string Phone { get; set; } = "";
        public string Gmail { get; set; } = "";
        public string PasswordGmail { get; set; } = "";
        public string TwoFactorGmail { get; set; } = "";

        // Job
        public string JobTitle { get; set; } = "";
        public string JobCompany { get; set; } = "";
        public string JobDescription { get; set; } = "";
        public string JobStartDate { get; set; } = "";
        public string JobEndDate { get; set; } = "";

        // Other
        public bool IsFavorite { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}