# ST2-2025-Team-23-PMG-s-Game-Library

PMG's Game Library/Game Repo is a library/repository of games, utilizing RAWG Video Games API or just RAWG for short (https://rawg.io/apidocs).
It's supposed to be a web .NET web app, using Razer Views, akin to MyAnimeList, where you can make an account and track all the games you've played/completed so far.

The main programming languages used here are C# for all the DB connections, API Integration and most of the back end, HTML and CSS for the front end with a couple of JavaScript scripts for a couple of effects on the front end and Python for the AI integration using Mistral 7B Instruct LLM (which is located in the pythonServer folder).


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
Naturally, there's 2 types of controllers the users can use, depending on the authorization they have: AccountController (for all users) and AdminController (accessible only to admin users). Normal accounts can be created easily by just registering, but admin accounts are premade in the AppDbContext.


