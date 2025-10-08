# 🧑‍💼 Recruitment Management System (RMS)

This is a full-featured **Recruitment Management System** built using **ASP.NET Core Web API**, **Entity Framework Core**, and **SQL Server**. It allows **Admins**, **Employers**, and **Applicants** to manage job postings, applications, and profiles.

## 🔧 Tech Stack

- ASP.NET Core Web API 
- Entity Framework Core
- SQL Server
- Swagger (OpenAPI)
- JWT Authentication
- Role-based Authorization
- File Upload
- Resume Parsing (Simulated)

---

## 🚀 Features

### ✅ Admin
- View all applicants and their details
- View job postings and applicants per job

### ✅ Employer
- Post jobs
- View applications for their jobs

### ✅ Applicant
- Sign up, login
- Upload resume (PDF/DOCX)
- Apply for jobs
- Resume parsing (simulated)

---

## 🛠️ Setup Instructions

### 1. 📦 Clone the Repository
```bash
git clone https://github.com/your-username/recruitment-management-system.git
cd recruitment-management-system
2. ⚙️ Configure Database
Make sure SQL Server is installed and running

Set your connection string in appsettings.json:

json
Copy code
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=RMS_DB;Trusted_Connection=True;"
}
3. 📄 Run Migrations
bash
Copy code
dotnet ef database update
4. ▶️ Run the Application
bash
Copy code
dotnet run
The API will be hosted at:

bash
Copy code
http://localhost:5000 or http://localhost:port
📘 API Documentation (via Swagger)
Open your browser at:

bash
Copy code
http://localhost:<port>/swagger
🧪 API Testing Guide (with Swagger)
1. 🔐 Sign Up as Applicant
Endpoint: POST /api/Auth/signup
Body:

json
Copy code
{
  "name": "John Doe",
  "email": "john@example.com",
  "password": "Password123!",
  "address": "123 Main St",
  "userType": "Applicant",
  "profileHeadline": "Software Engineer"
}


2. 🔐 Login
Endpoint: POST /api/Auth/login
Copy the token from the response.


3. ✅ Authorize
Click Authorize button in Swagger and paste your Bearer token.



4. 📤 Upload Resume (Applicant only)
Endpoint: POST /api/Applicants/uploadResume
Choose a PDF or DOCX resume file.


5. 📄 Get All Jobs (Any user)
Endpoint: GET /api/Jobs



6. 📩 Apply for a Job (Applicant)
Endpoint: GET /api/Jobs/apply?job_id=1
Make sure to use a valid job ID.



7. 👑 Admin Signup and Login
Repeat signup and login with:

json
Copy code
{
  "name": "Admin",
  "email": "admin2@example.com",
  "password": "AdminPassword123!",
  "address": "Admin HQ",
  "userType": "Admin",
  "profileHeadline": "Administrator"
}


8. 📋 Admin - View All Applicants
Endpoint: GET /api/admin/applicants



9. 👤 Admin - View Applicant Details
Endpoint: GET /api/admin/applicant/{applicant_id}
Use a valid applicant_id from previous step.



🔄 Optional: Post Job (Admin/Employer)
Endpoint: POST /api/admin/job

json
Copy code
{
  "title": "Backend Developer",
  "description": "Experienced in .NET Core",
  "companyName": "TechCorp"
}

📂 Folder Structure (Important Files)
pgsql
Copy code
RecruitmentManagementSystem/
├── Controllers/
│   ├── AuthController.cs
│   ├── AdminController.cs
│   ├── JobsController.cs
│   └── ApplicantsController.cs
├── Models/
│   ├── User.cs
│   ├── Profile.cs
│   ├── Job.cs
│   └── JobApplication.cs
├── DTOs/
│   ├── LoginDto.cs
│   ├── SignupDto.cs
│   └── UserResponseDto.cs
├── Data/
│   └── AppDbContext.cs
├── Migrations/
│   └── [auto generated EF files]
├── Program.cs
├── appsettings.json
└── README.md
