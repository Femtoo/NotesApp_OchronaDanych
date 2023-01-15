# NotesApp

### Aplikacja na przedmiot Ochrona Danych
Webowa apka do przechowywania stylowanych notatek. Notatki można stylizować za pomocą markdown.
Aplikacja przechowuje notatki zaszyfrowane.

## Użytkowanie
Najpierw trzeba sie zarejestrować. Po zalogowaniu wyswietlana jest lista notatek. Można stworzyć nową notatkę zwykłą lub z hasłem.
W notatkach można zaznaczyć czy chce się, żeby była ona publiczna czy nie. Można także zobaczyć listę publicznych notatek. 
Notatki swoje można usuwać i edytować, za to notatki publiczne można tylko obejrzeć. 

## Technologie
Aplikacja została napisana w technologii ASP .NET Core MVC, baza danych to SQL Server. Aplikacja jest skonteneryzowana w kontenerze linuxowym (Docker).
W bazie są przechowywane zahaszowane hasła i notatki. Hasła są zahaszowane za pomocą passwordhashera ([opcje](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.passwordhasheroptions?view=aspnetcore-7.0)
ASP.NET Core Identity Version 3: PBKDF2 with HMAC-SHA256, 128-bit salt, 256-bit subkey, 10000 iterations
Za to notatki są szyfrowane za pomocą AES-GCM, po uprzednim stworzeniu klucza na podstawie hasła do notatki (stworzone za pomocą [Rfc2898DeriveBytes](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.rfc2898derivebytes?view=net-6.0))
Autoryzacja i autentykacja odbywa się za pomocą JWT. Komunikacja pomiędzy klientem a serwerem, odbywa się za pomocą protokołu 
https. Do komunikacji z bazą danych wykorzystywany jest framework Entity Framework Core oraz technologia LINQ. 

## Uruchomienie
Po ściągnieciu repo, przejść do folderu repo NotesApp/

docker compose build

docker compose up

Adres serwera:
https://localhost/
