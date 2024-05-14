# MingleZone

MingleZone is a social media platform developed using .NET for the backend. This project aims to provide users with a seamless and engaging social networking experience.

## Table of Contents

- [Features](#features)
- [Technologies](#technologies)
- [Installation](#installation)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

## Features

- User authentication and authorization
- Profile creation and management
- Post creation, editing, and deletion
- Commenting on posts
- Real-time notifications
- Friend requests and friend list management
- Messaging between users

## Technologies

- **Backend:** .NET Core, ASP.NET Core
- **Frontend:** HTML, CSS, JavaScript
- **Database:** MySQL
- **Other Tools:** Entity Framework, SignalR

## Installation

To get a local copy up and running follow these simple steps:

### Prerequisites

- .NET Core SDK
- MySQL

### Clone the Repository

```bash
git clone https://github.com/AhmedJguirim/dotnet_MingleZone.git
cd dotnet_MingleZone
```

### Setup Database

- Create a MySQL database.
- Update the connection string in **appsettings.json** with your database credentials.

### Run Migrations

```bash
dotnet ef database update
```

### Build and Run the Project

```bash
dotnet build
dotnet run
```

### Usage

Once the application is running, you can access it via http://localhost:5000.

### Key Functionalities

- **Sign Up / Sign In:** Create a new account or log into an existing one.
- **Profile Management:** Update your profile information and settings.
- **Posting:** Create, edit, and delete posts.
- **Interactions:** Comment on posts, send friend requests, and manage your friend list.
- Messaging: Send and receive messages in real-time.

### Contributing

Contributions are what make the open-source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

### How to Contribute

1. Fork the Project
2. Create your Feature Branch (git checkout -b feature/AmazingFeature)
3. Commit your Changes (git commit -m 'Add some AmazingFeature')
4. Push to the Branch (git push origin feature/AmazingFeature)
5. Open a Pull Request

### License

Distributed under the MIT License.

### Contact

Ahmed Jguirim - jguirimahmed111@gmail.com

Project Link: https://github.com/AhmedJguirim/dotnet_MingleZone
