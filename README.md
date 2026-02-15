This is simple API that exposes endpoint to retreive data about books from https://wolnelektury.pl/api/.

## Running API

To run the app:

### Run following command in repository root:

### dotnet run --project "BooksApi/BooksApi.Api.csproj" --launch-profile "https"


You can also lunch app in Visual Studio (Steps below):

- Clone this repository on your local machine (git clone https://github.com/polemizatorr/BooksApi)
- Lunch the project in Visual Studio (application is written in .net 10 and required this version of .net to run)
- Select project BooksApi as startup project
- Run the application from Visual Studio




## Testing 

Lunch 'https://localhost:7167/swagger/index.html' in new browser tab.

You can test the app using provided swagger interface. To use enpoint select specific endpoint, fill request parameters (pagination, filtering, sorting).
