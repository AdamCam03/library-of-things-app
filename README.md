---
title: "StarterApp readme"
parent: StarterApp
grand_parent: C# practice
nav_order: 5
mermaid: true
---

# Library of Things - Peer-to-Peer Rental Marketplace

A .NET MAUI mobile application for renting everyday items within your local community, built as part of SET09102 coursework.

- Student Name - Adam Cameron
- Student Number - 40537233
- Github Link - https://github.com/AdamCam03/library-of-things-app

## Overview

Library of Things connects people who want to rent out items they own with people who need them. Users can list items, browse available items in their area, and submit and manage rental requests. The app connects to a shared REST API at https://set09102-api.b-davison.workers.dev/ and uses JWT authentication.

The app provides the following features:

* User authentication via shared REST API (JWT tokens)
* Browse and search available items
* Create, edit and manage item listings
* Submit and manage rental requests
* View incoming and outgoing rental requests

This app is built using .NET MAUI targeting Android, with a PostgreSQL local database via Entity Framework Core. It follows the MVVM pattern using CommunityToolkit.Mvvm and implements the Repository Pattern with a generic IRepository<T> interface.

To fully understand how it works, you should follow an appropriate set of tutorials such as [this one](https://edinburgh-napier.github.io/SET09102/tutorials/csharp/) which covers all of the main concepts and techniques used here. The code uses structured comments throughout.

You can use any development environment with this project including

* [Rider](https://www.jetbrains.com/rider/)
* [Visual Studio](https://visualstudio.microsoft.com/)
* [Visual Studio Code](https://code.visualstudio.com/)

## Compatibility

| Name | Version |
|------|---------|
| [.NET](https://dotnet.microsoft.com/en-us/) | 10.0 |
| [PostgreSQL Docker image](https://hub.docker.com/_/postgres) | 16 |
| Android SDK | 36 |

## Getting Started

### Prerequisites

Before using this app, ensure you have:

1. **.NET SDK 10.0** installed
2. **Docker** installed and running
3. **Android Emulator** configured
4. **ADB** installed and accessible

### Database Setup

Start the PostgreSQL container:
```bash
docker-compose up -d db
```

Apply migrations:
```bash
dotnet run --project StarterApp.Migrations/StarterApp.Migrations.csproj
```

### Configuration

1. Copy `StarterApp.Database/appsettings.json.template` to `StarterApp.Database/appsettings.json`
2. Update the connection string with your PostgreSQL credentials:
```json
{
  "ConnectionStrings": {
    "DevelopmentConnection": "Host=localhost;Username=app_user;Password=app_password;Database=appdb"
  }
}
```

### Building and Running

Build the app:
```bash
dotnet build -c Debug
```

Install to Android emulator:
```bash
adb install -r StarterApp/bin/Debug/net10.0-android/com.companyname.starterapp-Signed.apk
```

### Running Tests

```bash
cd StarterApp.Test
dotnet test
```

### Tutorial

For a comprehensive guide on using this app and understanding its architecture, see the
[MAUI + MVVM + Database Tutorial](https://edinburgh-napier.github.io/SET09102/tutorials/csharp/maui-mvvm-database/).