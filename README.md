# ST2-2025-Team-23-PMG-s-Game-Library

## App made by Georgi Kostadinov, Peter Tenevski, Miroslav Lyugov

PMG's Game Library/Game Repo is a library/repository of games, utilizing RAWG Video Games API or just RAWG for short (https://rawg.io/apidocs).
It's supposed to be a web .NET web app, using Razer Views, akin to MyAnimeList, where you can make an account and track all the games you've played/completed so far.

The main programming languages used here are C# for all the DB connections, API Integration and most of the back end, HTML and CSS for the front end with a couple of JavaScript scripts for a couple of effects on the front end and Python for the AI integration using Mistral 7B Instruct LLM (which is located in the pythonServer/models folder).

# File Structure

| Directory  | Purpose |
| ------------- |:-------------:|
| PMG's Game Repo/PMG's Game Repo      | ASP.NET Core MVC Application     |
| pythonServer      | LLM Server (Mistral/llama cpp)     |

# FRONT END

After the fade in, you can see that there's a homepage with general information about:
- How many games there are in total in the website
- How many users are in the website in total
- and if you're logged in: however many games are in your library as well as a "community favourite" section where it shows the most favourited game as of currently

The background is also static with a slight blur filter on top, as well as darkened brightness to help the foreground pop out a bit more against the background. This is mainly done with a body:before section of code in the styles.css file.

The navbar as well is transparent and static on top, with the site's name acting as also a home button, "Games" where you can browse the games, the info for which is provided by RAWG, "My Repo" where you can browse your library and on the very right your account's profile picture, where you can go to your profile our log out, or log in if you haven't already.

In the "Games" details page for any specific game you can see a quick description and also an option to go back to the list or to add it to your "Repo" at the bottom of the details page.


# BACK END

The Back end is a fairly typical ASP.NET core with Razer Views structure, complete with Services, Migrations, DTOs and everything else needed for the web app to at least run.
Naturally, there's 2 types of controllers the users can use, depending on the authorization they have: AccountController (for all users) and AdminController (accessible only to admin users). Normal accounts can be created easily by just registering, but admin accounts are premade in the AppDbContext (for now there's only 3, one for each member).

We have DTOs for almost anything Game related, from developers to game details and genre and with those DTOs we transfer the data around so that the functionality, of course, works. Like being able to add games to your library (basically favouriting) and it being connected to your account. The search function as well works as you'd expect, with a couple of filters being at your disposal.

We also added an AI search using Mistral, you can prompt the AI about games, like maybe "Action games with a positive rating" and alike and it will show you action games with a high rating, according to the RAWG API. The code surrounding the AI was written in python.

# Used Design Patterns


| Pattern | Where | Purpose |
|--------|--------|---------|
| MVC | ASP.NET MVC portion | Separation of UI / Logic / Data |
| Repository Pattern | `Database.cs` | Wraps DB access besides Controllers |
| Singleton | DB instance + llama model | Exactly one instance in process |
| Dependency Injection | LlmClient + HttpClientFactory | Easy replacement & testablity |
| Options Pattern | LlmOptions/appsettings.json | Configured endpoint/model & timeout |
| Adapter/API Gateway | `LlmClient` | Converts local LLM to OpenAI API format |
| DTO | `LlmDto` / `QueryPlan` | Structured message formats |
| Facade | LlmClient & python server wrapper | Hides complexity of transport |
| Validation Pattern | DataAnnotations Contact.cs | Safe data entry |
