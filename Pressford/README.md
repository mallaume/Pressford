# Project Title

KPMG Assessment : Pressford Consulting

## Getting Started

Open the solution in Visual Studio 2017 and run the command "Update-Database" in the Package Manager Console to init the local database.

The database will be seeded with 2 users :
-Scott Pressford (Username : "scott.pressford@gmail.com" / Password : "Password2018!")
-Alan Turing (Username : "alan.turing@gmail.com" / Password : "Password2018!")

Scott Pressford is declared as a Publisher and thus can create and edit articles whereas Alan Turing is a simple reader and is not allowed
to edit or write articles. Both users have a quota of 3 likes to use amongst articles.

The database will be seeded with 3 articles extracted from Bill Gates' blog.

### Prerequisites

You need to install AspNetCore v2.

## Authors

Marc Allaume

## Notes

This is my first attempt at using ASP.Net Core + EF Core + Identity Core so it took me longer than expected to wire
everything up... I'm especially not very proud of mixing AngularJS routing views with ASP.Net Partial Views...
While I could have done better sticking with ASP.Net 5, I choose to do the project within 1 day as instructed. 
Anyway it was really nice to work with it and it was on my 2018 learning todo list :)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details