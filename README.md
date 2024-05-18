# EduNex

Welcome to EduNex, an innovative education platform designed to streamline the learning process for students, teachers, and parents. This project leverages modern technologies to create a robust and scalable educational environment.

## Table of Contents
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Architecture](#architecture)
- [Usage](#usage)
- [License](#license)

## Features
- **Teacher Functionality**:
  - Create courses consisting of multiple lectures.
  - Attach video files to lectures using Cloudinary integration.
  - Add and manage exams with a comprehensive examination system.

- **Student Functionality**:
  - Register and manage their accounts.
  - Charge their wallets using PayMob integration.
  - Enroll in courses if they have sufficient funds in their wallet.

- **Parent Functionality**:
  - Monitor their child's progress and records using their national ID.

- **Admin Functionality**:
  - Dashboard to monitor system entities and transactions.
  - Approve new teacher registrations.

## Technologies Used
- **Backend**:
  - ASP.NET Core API
  - SQL Server
  - JWT for authentication
- **Cloud Services**:
  - Cloudinary for media management
  - PayMob for payment processing
- **Design Patterns**:
  - 3-Tier Architecture
  - Generic Repository Design Pattern
  - Unit of Work
  - AutoMapper for object-object mapping

## Architecture
The project is structured following the 3-tier architecture pattern, comprising:

1. **Presentation Layer**: Handles the UI and user interactions (Assumed to be a web client).
2. **Business Logic Layer**: Contains business logic and application workflow, implemented in ASP.NET Core.
3. **Data Access Layer**: Manages data persistence using SQL Server, with repositories and unit of work pattern for data operations.

### Key Components
- **Generic Repositories**: Simplify data access and management.
- **Unit of Work**: Ensures a single transaction scope for multiple operations.
- **AutoMapper**: Facilitates easy mapping between DTOs and entities.



## Usage
1. **Admin Portal**: Monitor system entities, transactions, and approve new teachers.
2. **Teacher Portal**: Create and manage courses, lectures, and exams.
3. **Student Portal**: Register, charge wallet, and enroll in courses.
4. **Parent Portal**: Monitor student progress using national ID.


## License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more information.

---

Feel free to reach out if you have any questions or need further assistance. Happy learning with EduNex!
