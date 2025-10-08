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
✅ Screenshot: <img width="1919" height="1016" alt="SignUp" src="https://github.com/user-attachments/assets/756bd7a7-339d-4548-814c-5af44c93bc75" />


2. 🔐 Login
Endpoint: POST /api/Auth/login
Copy the token from the response.

✅ Screenshot: <img width="1916" height="1021" alt="Login" src="https://github.com/user-attachments/assets/4c5e45d8-802f-4f27-98f6-5552185869e2" />


3. ✅ Authorize
Click Authorize button in Swagger and paste your Bearer token.

✅ Screenshot: <img width="1919" height="1023" alt="Authorize as Applicant" src="https://github.com/user-attachments/assets/7eb92330-9327-4dc7-8d14-f85f7863f173" />


4. 📤 Upload Resume (Applicant only)
Endpoint: POST /api/Applicants/uploadResume
Choose a PDF or DOCX resume file.

✅ Screenshot: <img width="1908" height="1018" alt="Resume Uploaded" src="https://github.com/user-attachments/assets/710ebf58-8955-4bf1-9394-17f260bdb50e" />

5. 📄 Get All Jobs (Any user)
Endpoint: GET /api/Jobs

✅ Screenshot: <img width="1919" height="1013" alt="Jobs" src="https://github.com/user-attachments/assets/a27dd308-e72f-4233-9c38-a1fa21c8cb2f" />


6. 📩 Apply for a Job (Applicant)
Endpoint: GET /api/Jobs/apply?job_id=1
Make sure to use a valid job ID.

✅ Screenshot: <img width="1904" height="1014" alt="Job Applied" src="https://github.com/user-attachments/assets/bbc6eac8-bfc6-4229-8056-ecbfa1114f89" />


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
✅ Screenshot: <img width="1919" height="1010" alt="Admin Sign Up" src="https://github.com/user-attachments/assets/692713f3-a30a-4659-b3b9-3e808baa092a" />
✅ Screenshot:<img width="1919" height="994" alt="Admin Login" src="https://github.com/user-attachments/assets/2f434c54-a4d6-4dd8-be4f-31ee9b8002f1" />


8. 📋 Admin - View All Applicants
Endpoint: GET /api/admin/applicants

✅ Screenshot: <img width="1918" height="1006" alt="View Applicants" src="https://github.com/user-attachments/assets/a033fc7e-a805-42f6-a965-199af9bffe96" />


9. 👤 Admin - View Applicant Details
Endpoint: GET /api/admin/applicant/{applicant_id}
Use a valid applicant_id from previous step.

✅ Screenshot: <img width="1909" height="1021" alt="Applicant Details" src="https://github.com/user-attachments/assets/d2039d27-6316-4876-a8bc-71f857171e26" />


🔄 Optional: Post Job (Admin/Employer)
Endpoint: POST /api/admin/job

json
Copy code
{
  "title": "Backend Developer",
  "description": "Experienced in .NET Core",
  "companyName": "TechCorp"
}
✅ Screenshot: <img width="1917" height="998" alt="Post Job" src="https://github.com/user-attachments/assets/23e21829-460f-4878-bd51-930d7c96f65d" />

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
