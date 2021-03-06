# The Exercise

This repository contains code to meet the following exercise:

- Create a 2 page website using asp.net core where the second page is secured and can only be accessed once the user is logged in via the first page
- Store credentials securely
- Second page must utilise a JS front-end language framework such as Angular or React and to be creative with what functionality it has
- Show any design patterns etc.

# Trying It Out

- Clone the GIT repo or download the code via GitHub
- Open the solution file in Visual Studio 2019
- Build and Run

There are 2 users to log in with (case sensitive!)
- Chris with password "secure"
- Test with password "test"

# Solution

## To meet these goals, I:

- Created an ASP.NET MVC website as well as using bootstrap 4 for layout/UI
- Set up Entity Framework Core to store a list of Users
  - Passwords are hashed with a cryptographically generated salt
  - In Memory database is used for this example, however it can easily be changed to use SQL Server
  - The database is seeded with 2 users (Chris and Test) on start up for demo purposes
- Configured Cookie authentication middleware
  - Unauthenticated users are redirected to the AuthenticationController for login
  - On login, the user is looked up in the database, if found, the given password is salted with the salt from the database, hashed and compared to the stored hashed password
  - On successful login, the user is logged in via cookie authentication
- The second page hosts React using ReactJS.net as i only wanted to use it for the home page to meet the objective
- The second page functionality is a simple tile puzzle game that shows the target image, once the start level button is pressed, 
  the tiles are shuffled and the user has to put the image back together again. Once they are back a congratulations message is shown. 
  The user can also reset during a level to show the original image and start over.
- The logout link logs the user out returning to the login page

## Design Patterns

- MVC is used for the presentation layer for the main website
- The mediator design pattern is used to separate the controllers from the database login
  - The MediatR library is used
  - Sign In and Sign Out are turned into commands (SignInCommand and SignOutCommand)
  - Handlers for each take these commands and perform the sign in and out functionality. The controllers are only used for routing, model binding and returning the appropriate views.

# TODO

- Timer for levels
- Multiple difficulties
- Multiple levels (images)
- Api to store leaderboard of times and usernames
- Additional styling to remove bootstrap feel